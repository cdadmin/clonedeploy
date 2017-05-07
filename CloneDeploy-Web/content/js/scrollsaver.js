// scrollsaver.js
// Copyright (C) 2009-2014 M. Mahdi Hasheminezhad http://hasheminezhad.com
// Maintain scroll position of every element on postbacks and partial updates
// This source is licensed under Common Public License Version 1.0 (CPL)
// You can reach me at hasheminezhad at gmail dot com
// History:
// 2014-08-04 Third Release
// - Add loadScroll and saveScroll to the window object
// - Code cleanup
// 2011-08-24 Second Release
// - Support for localStorage
// - Consider window.location
// - Other minor tweaks
// 2009-08-21 First Public Release

(function(window, undefined) {
    var document = window.document,
        location = window.location,
        key = window.escape("scrollPosition|" + location.pathname + location.search);

    window.loadScroll = function() {


        var positions;
        //load scroll positions
        try {
            positions = (localStorage.getItem(key) || "").split("|");
        } catch (ex) {
            var cookieList = document.cookie.split(";");
            for (var i = cookieList.length - 1; i >= 0 && !positions; i--) {
                var cookieParts = cookieList[i].split("=");
                if (cookieParts[0] == key) {
                    positions = window.unescape(cookieParts[1]).split("|");
                }
            }
        }
        positions = positions || [];

        //set scroll positions
        for (var j = positions.length - 1; j >= 0; j--) {
            var currentValue = positions[j].split(",");
            try {
                if ("" == currentValue[0]) { //no id for window
                    window.scrollTo(currentValue[1], currentValue[2]);
                } else if (currentValue[0]) {
                    var elm = document.getElementById(currentValue[0]);
                    elm.scrollLeft = currentValue[1];
                    elm.scrollTop = currentValue[2];
                }
            } catch (ex) {
            }
        }
    };

    window.saveScroll = function() {
        var positions = [];
        //windows scroll position
        var wl, wt;
        if (undefined !== window.pageXOffset) {
            wl = window.pageXOffset;
            wt = window.pageYOffset;
        } else if (undefined !== document.documentElement && document.documentElement.scrollLeft) {
            wl = document.documentElement.scrollLeft;
            wt = document.documentElement.scrollTop;
        } else {
            wl = document.body.scrollLeft;
            wt = document.body.scrollTop;
        }
        if (wl || wt) positions.push(["", wl, wt].join(",")); //no id for window

        //other elements
        var elements = document.all || document.getElementsByTagName("*");
        for (var i = 0; i < elements.length; i++) {
            var e = elements[i];
            if (e.id && (e.scrollLeft || e.scrollTop)) {
                positions.push([e.id, e.scrollLeft, e.scrollTop].join(","));
            }
        }

        //save scroll positions
        try {
            localStorage.setItem(key, positions.join("|"));
        } catch (ex) {
            document.cookie = key + "=" + positions.join("|") + ";";
        }
    };
})(window);

// Attach to page load and unload
(function(window) {
    var addEvent, eventPrefix;
    if (window.attachEvent) {
        addEvent = window.attachEvent;
        eventPrefix = "on";
    } else {
        addEvent = window.addEventListener;
        eventPrefix = "";
    }
    addEvent(eventPrefix + "load", window.loadScroll, false);
    addEvent(eventPrefix + "unload", window.saveScroll, false);
})(window);

// Only for Partial PostBacks (UpdatePanel in ASP.NET)
setTimeout(function() {
    if ("undefined" != typeof Sys && "undefined" != typeof Sys.WebForms) {
        var instance = Sys.WebForms.PageRequestManager.getInstance();
        instance.add_beginRequest(window.saveScroll);
        instance.add_endRequest(window.loadScroll);
    }
}, 0);