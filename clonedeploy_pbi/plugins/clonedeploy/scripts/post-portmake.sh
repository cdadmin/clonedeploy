#!/bin/sh
# PBI building script
# This will run after your port build is complete
##############################################################################

clonedeploy_pbi_path=/usr/pbi/clonedeploy-$(uname -m)/

find ${clonedeploy_pbi_path}/lib -iname "*.a" -delete
rm -rf ${clonedeploy_pbi_path}/share/doc
rm -rf ${clonedeploy_pbi_path}/share/emacs
rm -rf ${clonedeploy_pbi_path}/share/examples
rm -rf ${clonedeploy_pbi_path}/share/gettext
