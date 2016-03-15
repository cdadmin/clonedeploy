#/bin/sh
mysqladmin create clonedeploy
mysql clonedeploy < ${clonedeploy_pbi_path}/clonedeploy/cd.sql
mysqladmin -u root password xx_marker1_xx

