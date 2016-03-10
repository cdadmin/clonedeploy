--- module/utils.py.orig	2014-01-18 15:28:37.000000000 +0100
+++ module/utils.py		2014-02-07 15:27:44.256166988 +0100
@@ -110,7 +110,7 @@
         from os import statvfs
 
         s = statvfs(folder)
-        return s.f_bsize * s.f_bavail
+        return s.f_frsize * s.f_bavail
 
 
 def uniqify(seq, idfun=None):
