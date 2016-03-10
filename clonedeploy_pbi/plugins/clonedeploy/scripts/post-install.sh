#!/bin/sh
#########################################

clonedeploy_pbi_path=/usr/pbi/clonedeploy-$(uname -m)

${clonedeploy_pbi_path}/bin/python2.7 ${clonedeploy_pbi_path}/clonedeployUI/manage.py syncdb --migrate --noinput

# Create Apache alias for clonedeploy
cat << __EOF__ > ${clonedeploy_pbi_path}/etc/apache24/Includes/clonedeploy.conf
 Alias /clonedeploy "${clonedeploy_pbi_path}/www/clonedeploy"
  MonoServerPath clonedeploy "/usr/bin/mod-mono-server4"
  MonoDebug clonedeploy true
  MonoApplications clonedeploy "/clonedeploy:${clonedeploy_pbi_path}/www/clonedeploy"
  AddType text/plain .asmx
  
<Location "/clonedeploy">
    Allow from all
    Order allow,deny
    MonoSetServerAlias clonedeploy
    SetHandler mono
  </Location>
  
__EOF__

# Add paths to Apache
cat << __EOF__ > ${clonedeploy_pbi_path}/etc/apache24/envvars.d/path.env
export PATH=${clonedeploy_pbi_path}/bin:/usr/local/bin:\$PATH
export LD_LIBRARY_PATH=/usr/local/lib:\$LD_LIBRARY_PATH
__EOF__

# Optimize Apache on ZFS
sed -i '' -e 's/^#\(EnableMMAP[[:space:]]\).*$/\1Off/' ${clonedeploy_pbi_path}/etc/apache24/httpd.conf

# Enable SSL
sed -i '' -e 's|^#\(Include[[:space:]].*/httpd-ssl.conf$\)|\1|' ${clonedeploy_pbi_path}/etc/apache24/httpd.conf
sed -i '' -e 's/^#\(LoadModule[[:space:]]*ssl_module[[:space:]].*$\)/\1/' ${clonedeploy_pbi_path}/etc/apache24/httpd.conf
sed -i '' -e 's/^#\(LoadModule[[:space:]]*socache_shmcb_module[[:space:]].*$\)/\1/' ${clonedeploy_pbi_path}/etc/apache24/httpd.conf

# Make sure SSL config exists
if [ ! -f "${clonedeploy_pbi_path}/openssl/openssl.cnf" ];
        ln -s openssl.cnf.sample ${clonedeploy_pbi_path}/openssl/openssl.cnf
fi

tmp=$(mktemp /tmp/tmp.XXXXXX)
# Generate SSL certificate
if [ ! -f "${clonedeploy_pbi_path}/etc/apache24/server.crt" ]; then

	if ! grep -e '^commonName_default[[:space:]]*=' /etc/ssl/openssl.cnf; then
		sed -i '' -e '/^commonName_max[[:space:]]*=/ a\
commonName_default = clonedeploy\
' /etc/ssl/openssl.cnf
	fi
	dd if=/dev/urandom count=16 bs=1 2> /dev/null | uuencode -|head -2 |tail -1 > "${tmp}"
	openssl req -batch -passout file:"${tmp}" -new -x509 -keyout ${clonedeploy_pbi_path}/etc/apache24/server.key.out -out ${clonedeploy_pbi_path}/etc/apache24/server.crt
	openssl rsa -passin file:"${tmp}" -in ${clonedeploy_pbi_path}/etc/apache24/server.key.out -out ${clonedeploy_pbi_path}/etc/apache24/server.key
fi

echo mysql_enable=yes >> /etc/rc.conf
/bin/ln -s ${clonedeploy_pbi_path}/sbin/udp-sender /usr/local/bin/udp-sender
/bin/ln -s ${clonedeploy_pbi_path}/sbin/udp-receiver /usr/local/bin/udp-receiver
/bin/ln -s ${clonedeploy_pbi_path}/bin/lz4c /usr/local/bin/lz4
/bin/ln -s ${clonedeploy_pbi_path}/bin/mcs /usr/local/bin/mcs
mkdir ${clonedeploy_pbi_path}/cd_dp
mkdir ${clonedeploy_pbi_path}/cd_dp/images
mkdir ${clonedeploy_pbi_path}/cd_dp/resources
chown -R www:www ${clonedeploy_pbi_path}/www/clonedeploy
chown -R www:www ${clonedeploy_pbi_path}/cd_dp
chown -R www:www ${clonedeploy_pbi_path}/tftpboot
mkdir ${clonedeploy_pbi_path}/www/.mono
chown -R www:www ${clonedeploy_pbi_path}/www/.mono
/bin/ln -s ${clonedeploy_pbi_path}/tftpboot/images ${clonedeploy_pbi_path}/tftpboot/proxy/bios/images 
/bin/ln -s ${clonedeploy_pbi_path}/tftpboot/kernels ${clonedeploy_pbi_path}/tftpboot/proxy/bios/kernels
/bin/ln -s ${clonedeploy_pbi_path}/tftpboot/images ${clonedeploy_pbi_path}/tftpboot/proxy/efi32/images 
/bin/ln -s ${clonedeploy_pbi_path}/tftpboot/kernels ${clonedeploy_pbi_path}/tftpboot/proxy/efi32/kernels 
/bin/ln -s ${clonedeploy_pbi_path}/tftpboot/images ${clonedeploy_pbi_path}/tftpboot/proxy/efi64/images 
/bin/ln -s ${clonedeploy_pbi_path}/tftpboot/kernels ${clonedeploy_pbi_path}/tftpboot/proxy/efi64/kernels 
