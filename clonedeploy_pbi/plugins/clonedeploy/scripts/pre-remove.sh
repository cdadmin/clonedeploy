#!/bin/sh
#########################################

clonedeploy_pbi_path=/usr/pbi/clonedeploy-$(uname -m)

${clonedeploy_pbi_path}/etc/rc.d/apache24 forcestop 2>/dev/null || true
