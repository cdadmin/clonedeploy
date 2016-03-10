#PREPARATION
* The ports in ./ports should be copied into your ports tree
    - <code>cp -R ports/* /usr/ports/</code>

* Add 'media' user and group to '/usr/ports/UIDs' and '/usr/ports/GIDs'.
    - /usr/ports/UIDs
        - <code>media:*:816:816::0:0:Media Plugins Daemon:/nonexistent:/usr/sbin/nologin</code>

    - /usr/ports/GIDs
        - <code>media:*:816:</code>

* Compile
    - <code>pbi_makeport -c ./plugins/**NAME** -o ./pbi --pkgdir ./pkg/**NAME** **CATEGORY/NAME**</code>
