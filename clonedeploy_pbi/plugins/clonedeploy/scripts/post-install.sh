#!/bin/sh
#########################################
clonedeploy_pbi_path=/usr/pbi/clonedeploy-$(uname -m)

echo mysql_enable=yes >> /etc/rc.conf
mv ${clonedeploy_pbi_path}/www/clonedeploy ${clonedeploy_pbi_path}/
mv ${clonedeploy_pbi_path}/clonedeploy/web ${clonedeploy_pbi_path}/www/clonedeploy
cp -R ${clonedeploy_pbi_path}/1.0.1p1/ ${clonedeploy_pbi_path}/www/clonedeploy/

mv ${clonedeploy_pbi_path}/udp-sender ${clonedeploy_pbi_path}/sbin/udp-sender
chmod +x ${clonedeploy_pbi_path}/sbin/udp-sender
/bin/ln -s ${clonedeploy_pbi_path}/sbin/udp-sender /usr/local/bin/udp-sender
/bin/ln -s ${clonedeploy_pbi_path}/bin/mono /usr/local/bin/mono
/bin/ln -s ${clonedeploy_pbi_path}/bin/mod-mono-server4 /usr/local/bin/mod-mono-server4
/bin/ln -s ${clonedeploy_pbi_path}/bin/mysql /usr/local/bin/mysql
/bin/ln -s ${clonedeploy_pbi_path}/bin/mysqladmin /usr/local/bin/mysqladmin
/bin/ln -s ${clonedeploy_pbi_path}/lib/mono/4.0/mod-mono-server4.exe ${clonedeploy_pbi_path}/lib/mono/4.5/mod-mono-server4.exe
/bin/ln -s ${clonedeploy_pbi_path}/bin/lz4c /usr/local/bin/lz4
/bin/ln -s ${clonedeploy_pbi_path}/bin/mcs /usr/local/bin/mcs

/bin/cp ${clonedeploy_pbi_path}/etc/rc.d/apache22 /usr/local/etc/rc.d/
/bin/cp ${clonedeploy_pbi_path}/etc/rc.d/mysql-server /usr/local/etc/rc.d/

sed -i.bak s/4.0/4.5/g ${clonedeploy_pbi_path}/bin/mod-mono-server4

${clonedeploy_pbi_path}/bin/python2.7 ${clonedeploy_pbi_path}/clonedeployUI/manage.py syncdb --migrate --noinput

cat << __EOF__ > ${clonedeploy_pbi_path}/etc/apache22/Includes/clonedeploy.conf
<VirtualHost *:80>
	ServerAdmin webmaster@localhost

	DocumentRoot /usr/pbi/clonedeploy-amd64/www/clonedeploy

	AddMonoApplications clonedeploy "/clonedeploy:/usr/pbi/clonedeploy-amd64/www/clonedeploy"
	MonoServerPath clonedeploy "/usr/local/bin/mod-mono-server4"
	
	<Directory /usr/pbi/clonedeploy-amd64/www/clonedeploy/>
    		MonoSetServerAlias clonedeploy
    		SetHandler mono
    		AddHandler mod_mono .aspx .ascx .asax .ashx .config .cs .asmx
     		<FilesMatch "\.(gif|jp?g|png|css|ico|xsl|wmv|zip)$">
        		SetHandler None
    		</FilesMatch>
		Options FollowSymLinks MultiViews
    		AllowOverride All
    		Order allow,deny
    		Allow from all
    		SetHandler mono
    		DirectoryIndex default.aspx
	</Directory>	
</VirtualHost>
__EOF__

mkdir ${clonedeploy_pbi_path}/www/.mono
chown -R www:www ${clonedeploy_pbi_path}/www/.mono

mv ${clonedeploy_pbi_path}/clonedeploy/tftpboot ${clonedeploy_pbi_path}/

mkdir ${clonedeploy_pbi_path}/cd_dp
mkdir ${clonedeploy_pbi_path}/cd_dp/images
mkdir ${clonedeploy_pbi_path}/cd_dp/resources

mkdir ${clonedeploy_pbi_path}/tftpboot/pxelinux.cfg
mkdir ${clonedeploy_pbi_path}/tftpboot/proxy/bios/pxelinux.cfg
mkdir ${clonedeploy_pbi_path}/tftpboot/proxy/efi32/pxelinux.cfg
mkdir ${clonedeploy_pbi_path}/tftpboot/proxy/efi64/pxelinux.cfg

mkdir ${clonedeploy_pbi_path}/www/clonedeploy/public
mkdir ${clonedeploy_pbi_path}/www/clonedeploy/private/client_iso
mkdir ${clonedeploy_pbi_path}/www/clonedeploy/private/exports
mkdir ${clonedeploy_pbi_path}/www/clonedeploy/private/imports
mkdir ${clonedeploy_pbi_path}/www/clonedeploy/private/logs

chown -R www:www ${clonedeploy_pbi_path}/www/clonedeploy
chown -R www:www ${clonedeploy_pbi_path}/cd_dp
chown -R www:www ${clonedeploy_pbi_path}/tftpboot
chmod -R 755 ${clonedeploy_pbi_path}/tftpboot

/bin/ln -s ${clonedeploy_pbi_path}/tftpboot/images ${clonedeploy_pbi_path}/tftpboot/proxy/bios/images 
/bin/ln -s ${clonedeploy_pbi_path}/tftpboot/kernels ${clonedeploy_pbi_path}/tftpboot/proxy/bios/kernels
/bin/ln -s ${clonedeploy_pbi_path}/tftpboot/images ${clonedeploy_pbi_path}/tftpboot/proxy/efi32/images 
/bin/ln -s ${clonedeploy_pbi_path}/tftpboot/kernels ${clonedeploy_pbi_path}/tftpboot/proxy/efi32/kernels 
/bin/ln -s ${clonedeploy_pbi_path}/tftpboot/images ${clonedeploy_pbi_path}/tftpboot/proxy/efi64/images 
/bin/ln -s ${clonedeploy_pbi_path}/tftpboot/kernels ${clonedeploy_pbi_path}/tftpboot/proxy/efi64/kernels


service mysql-server restart
cd ${clonedeploy_pbi_path}/bin

rand_pass=$(head /dev/urandom | tr -dc A-Za-z0-9 | head -c 8)
rand_key=$(head /dev/urandom | tr -dc A-Za-z0-9 | head -c 8)

sed -i '' "s/xx_marker1_xx/$rand_pass/" ${clonedeploy_pbi_path}/www/clonedeploy/web.config
sed -i '' "s/xx_marker2_xx/$rand_key/" ${clonedeploy_pbi_path}/www/clonedeploy/web.config

./mysqladmin create clonedeploy
./mysql clonedeploy < ${clonedeploy_pbi_path}/clonedeploy/cd.sql
./mysqladmin -u root password $rand_pass
