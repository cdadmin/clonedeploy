import os
import platform

clonedeploy_pbi_path = "/usr/pbi/clonedeploy-" + platform.machine()
clonedeploy_fcgi_pidfile = "/var/run/clonedeploy_fcgi_server.pid"
clonedeploy_control = "/usr/local/etc/rc.d/apache24"
clonedeploy_icon = os.path.join(clonedeploy_pbi_path, "default.png")
clonedeploy_oauth_file = os.path.join(clonedeploy_pbi_path, ".oauth")


def get_rpc_url(request):
    addr = request.META.get("SERVER_ADDR")
    # IPv6
    if ':' in addr:
        addr = '[%s]' % addr
    return 'http%s://%s:%s/plugins/json-rpc/v1/' % (
        's' if request.is_secure() else '',
        addr,
        request.META.get("SERVER_PORT"),
    )


def get_clonedeploy_oauth_creds():
    f = open(clonedeploy_oauth_file)
    lines = f.readlines()
    f.close()

    key = secret = None
    for l in lines:
        l = l.strip()

        if l.startswith("key"):
            pair = l.split("=")
            if len(pair) > 1:
                key = pair[1].strip()

        elif l.startswith("secret"):
            pair = l.split("=")
            if len(pair) > 1:
                secret = pair[1].strip()

    return key, secret
