var hexcase = 0;

function syslinux_sha512(j) {
    var f = Math.random().toString(36).substring(6);
    f += Math.random().toString(36).substring(6);
    f = f.substring(0, 4).toUpperCase() + f.substring(4, 16);
    f = f.substring(0, 8);
    if (j == "") {
        return "Error: Empty password"
    }
    if (/[\x00-\x1F]/.test(str2rstr_utf8(j))) {
        return "Error: Non-printable ASCII char/s"
    }
    if (/[\x7F-\xFF]/.test(str2rstr_utf8(j))) {
        return "Error: Non-ASCII char/s"
    }
    var a = j;
    a += f;
    var d = rstr_sha512(j + f + j);
    for (var b = j.length; b > 64; b -= 64) {
        a += d.substr(0, 64)
    }
    a += d.substr(0, b);
    for (b = j.length; b > 0; b >>= 1) {
        if ((b & 1) != 0) {
            a += d.substr(0, 64)
        } else {
            a += j
        }
    }
    d = rstr_sha512(a);
    a = "";
    for (b = 0; b < j.length; ++b) {
        a += j
    }
    var i = rstr_sha512(a);
    var k = "";
    for (b = j.length; b >= 64; b -= 64) {
        k += i.substr(0, 64)
    }
    k += i.substr(0, b);
    a = "";
    for (b = 0; b < 16 + d.charCodeAt(0); ++b) {
        a += f
    }
    i = rstr_sha512(a);
    var e = "";
    for (b = f.length; b >= 64; b -= 64) {
        e += i.substr(0, 64)
    }
    e += i.substr(0, b);
    var g;
    var h = 5000;
    for (b = 0; b < h; ++b) {
        g = "";
        if ((b & 1) != 0) {
            g += k.substr(0, j.length)
        } else {
            g += d.substr(0, 64)
        } if (b % 3 != 0) {
            g += e.substr(0, f.length)
        }
        if (b % 7 != 0) {
            g += k.substr(0, j.length)
        }
        if ((b & 1) != 0) {
            g += d.substr(0, 64)
        } else {
            g += k.substr(0, j.length)
        }
        d = rstr_sha512(g)
    }
    d = stringToArray(d);
    var c = "";
    c = "$6$" + f + "$";
    c += ap_to64((d[0] << 16) | (d[21] << 8) | d[42], 4);
    c += ap_to64((d[22] << 16) | (d[43] << 8) | d[1], 4);
    c += ap_to64((d[44] << 16) | (d[2] << 8) | d[23], 4);
    c += ap_to64((d[3] << 16) | (d[24] << 8) | d[45], 4);
    c += ap_to64((d[25] << 16) | (d[46] << 8) | d[4], 4);
    c += ap_to64((d[47] << 16) | (d[5] << 8) | d[26], 4);
    c += ap_to64((d[6] << 16) | (d[27] << 8) | d[48], 4);
    c += ap_to64((d[28] << 16) | (d[49] << 8) | d[7], 4);
    c += ap_to64((d[50] << 16) | (d[8] << 8) | d[29], 4);
    c += ap_to64((d[9] << 16) | (d[30] << 8) | d[51], 4);
    c += ap_to64((d[31] << 16) | (d[52] << 8) | d[10], 4);
    c += ap_to64((d[53] << 16) | (d[11] << 8) | d[32], 4);
    c += ap_to64((d[12] << 16) | (d[33] << 8) | d[54], 4);
    c += ap_to64((d[34] << 16) | (d[55] << 8) | d[13], 4);
    c += ap_to64((d[56] << 16) | (d[14] << 8) | d[35], 4);
    c += ap_to64((d[15] << 16) | (d[36] << 8) | d[57], 4);
    c += ap_to64((d[37] << 16) | (d[58] << 8) | d[16], 4);
    c += ap_to64((d[59] << 16) | (d[17] << 8) | d[38], 4);
    c += ap_to64((d[18] << 16) | (d[39] << 8) | d[60], 4);
    c += ap_to64((d[40] << 16) | (d[61] << 8) | d[19], 4);
    c += ap_to64((d[62] << 16) | (d[20] << 8) | d[41], 4);
    c += ap_to64(d[63], 2);
    c += "$";
    return c
}

function stringToArray(d) {
    var b = [];
    for (var c = 0; c < d.length; c++) {
        b.push(d.charCodeAt(c))
    }
    return b
}
var itoa64 = "./0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

function ap_to64(a, c) {
    var b = "";
    while (--c >= 0) {
        b += itoa64.charAt(a & 63);
        a >>>= 6
    }
    return b
}

function hex_sha512(b) {
    return rstr2hex(rstr_sha512(str2rstr_utf8(b)))
}

function hex_hmac_sha512(d, c) {
    return rstr2hex(rstr_hmac_sha512(str2rstr_utf8(d), str2rstr_utf8(c)))
}

function sha512_vm_test() {
    return hex_sha512("abc").toLowerCase() == "ddaf35a193617abacc417349ae20413112e6fa4e89a97ea20a9eeee64b55d39a2192992a274fc1a836ba3c23a3feebbd454d4423643ce80e2a9ac94fa54ca49f"
}

function rstr_sha512(b) {
    return binb2rstr(binb_sha512(rstr2binb(b), b.length * 8))
}

function rstr_hmac_sha512(n, k) {
    var l = rstr2binb(n);
    if (l.length > 32) {
        l = binb_sha512(l, n.length * 8)
    }
    var i = Array(32),
        m = Array(32);
    for (var h = 0; h < 32; h++) {
        i[h] = l[h] ^ 909522486;
        m[h] = l[h] ^ 1549556828
    }
    var j = binb_sha512(i.concat(rstr2binb(k)), 1024 + k.length * 8);
    return binb2rstr(binb_sha512(m.concat(j), 1024 + 512))
}

function rstr2hex(l) {
    try {
        hexcase
    } catch (i) {
        hexcase = 0
    }
    var j = hexcase ? "0123456789ABCDEF" : "0123456789abcdef";
    var e = "";
    var h;
    for (var k = 0; k < l.length; k++) {
        h = l.charCodeAt(k);
        e += j.charAt((h >>> 4) & 15) + j.charAt(h & 15)
    }
    return e
}

function str2rstr_utf8(j) {
    var f = "";
    var i = -1;
    var g, h;
    while (++i < j.length) {
        g = j.charCodeAt(i);
        h = i + 1 < j.length ? j.charCodeAt(i + 1) : 0;
        if (55296 <= g && g <= 56319 && 56320 <= h && h <= 57343) {
            g = 65536 + ((g & 1023) << 10) + (h & 1023);
            i++
        }
        if (g <= 127) {
            f += String.fromCharCode(g)
        } else {
            if (g <= 2047) {
                f += String.fromCharCode(192 | ((g >>> 6) & 31), 128 | (g & 63))
            } else {
                if (g <= 65535) {
                    f += String.fromCharCode(224 | ((g >>> 12) & 15), 128 | ((g >>> 6) & 63), 128 | (g & 63))
                } else {
                    if (g <= 2097151) {
                        f += String.fromCharCode(240 | ((g >>> 18) & 7), 128 | ((g >>> 12) & 63), 128 | ((g >>> 6) & 63), 128 | (g & 63))
                    }
                }
            }
        }
    }
    return f
}

function rstr2binb(d) {
    var e = Array(d.length >> 2);
    for (var f = 0; f < e.length; f++) {
        e[f] = 0
    }
    for (var f = 0; f < d.length * 8; f += 8) {
        e[f >> 5] |= (d.charCodeAt(f / 8) & 255) << (24 - f % 32)
    }
    return e
}

function binb2rstr(d) {
    var e = "";
    for (var f = 0; f < d.length * 32; f += 8) {
        e += String.fromCharCode((d[f >> 5] >>> (24 - f % 32)) & 255)
    }
    return e
}
var sha512_k;

function binb_sha512(Q, P) {
    if (sha512_k == undefined) {
        sha512_k = new Array(new int64(1116352408, -685199838), new int64(1899447441, 602891725), new int64(-1245643825, -330482897), new int64(-373957723, -2121671748), new int64(961987163, -213338824), new int64(1508970993, -1241133031), new int64(-1841331548, -1357295717), new int64(-1424204075, -630357736), new int64(-670586216, -1560083902), new int64(310598401, 1164996542), new int64(607225278, 1323610764), new int64(1426881987, -704662302), new int64(1925078388, -226784913), new int64(-2132889090, 991336113), new int64(-1680079193, 633803317), new int64(-1046744716, -815192428), new int64(-459576895, -1628353838), new int64(-272742522, 944711139), new int64(264347078, -1953704523), new int64(604807628, 2007800933), new int64(770255983, 1495990901), new int64(1249150122, 1856431235), new int64(1555081692, -1119749164), new int64(1996064986, -2096016459), new int64(-1740746414, -295247957), new int64(-1473132947, 766784016), new int64(-1341970488, -1728372417), new int64(-1084653625, -1091629340), new int64(-958395405, 1034457026), new int64(-710438585, -1828018395), new int64(113926993, -536640913), new int64(338241895, 168717936), new int64(666307205, 1188179964), new int64(773529912, 1546045734), new int64(1294757372, 1522805485), new int64(1396182291, -1651133473), new int64(1695183700, -1951439906), new int64(1986661051, 1014477480), new int64(-2117940946, 1206759142), new int64(-1838011259, 344077627), new int64(-1564481375, 1290863460), new int64(-1474664885, -1136513023), new int64(-1035236496, -789014639), new int64(-949202525, 106217008), new int64(-778901479, -688958952), new int64(-694614492, 1432725776), new int64(-200395387, 1467031594), new int64(275423344, 851169720), new int64(430227734, -1194143544), new int64(506948616, 1363258195), new int64(659060556, -544281703), new int64(883997877, -509917016), new int64(958139571, -976659869), new int64(1322822218, -482243893), new int64(1537002063, 2003034995), new int64(1747873779, -692930397), new int64(1955562222, 1575990012), new int64(2024104815, 1125592928), new int64(-2067236844, -1578062990), new int64(-1933114872, 442776044), new int64(-1866530822, 593698344), new int64(-1538233109, -561857047), new int64(-1090935817, -1295615723), new int64(-965641998, -479046869), new int64(-903397682, -366583396), new int64(-779700025, 566280711), new int64(-354779690, -840897762), new int64(-176337025, -294727304), new int64(116418474, 1914138554), new int64(174292421, -1563912026), new int64(289380356, -1090974290), new int64(460393269, 320620315), new int64(685471733, 587496836), new int64(852142971, 1086792851), new int64(1017036298, 365543100), new int64(1126000580, -1676669620), new int64(1288033470, -885112138), new int64(1501505948, -60457430), new int64(1607167915, 987167468), new int64(1816402316, 1246189591))
    }
    var O = new Array(new int64(1779033703, -205731576), new int64(-1150833019, -2067093701), new int64(1013904242, -23791573), new int64(-1521486534, 1595750129), new int64(1359893119, -1377402159), new int64(-1694144372, 725511199), new int64(528734635, -79577749), new int64(1541459225, 327033209));
    var H = new int64(0, 0),
        M = new int64(0, 0),
        c = new int64(0, 0),
        d = new int64(0, 0),
        g = new int64(0, 0),
        i = new int64(0, 0),
        j = new int64(0, 0),
        K = new int64(0, 0),
        L = new int64(0, 0),
        N = new int64(0, 0),
        T = new int64(0, 0),
        U = new int64(0, 0),
        x = new int64(0, 0),
        R = new int64(0, 0),
        a = new int64(0, 0),
        e = new int64(0, 0),
        h = new int64(0, 0);
    var f, b;
    var S = new Array(80);
    for (b = 0; b < 80; b++) {
        S[b] = new int64(0, 0)
    }
    Q[P >> 5] |= 128 << (24 - (P & 31));
    Q[((P + 128 >> 10) << 5) + 31] = P;
    for (b = 0; b < Q.length; b += 32) {
        int64copy(c, O[0]);
        int64copy(d, O[1]);
        int64copy(g, O[2]);
        int64copy(i, O[3]);
        int64copy(j, O[4]);
        int64copy(K, O[5]);
        int64copy(L, O[6]);
        int64copy(N, O[7]);
        for (f = 0; f < 16; f++) {
            S[f].h = Q[b + 2 * f];
            S[f].l = Q[b + 2 * f + 1]
        }
        for (f = 16; f < 80; f++) {
            int64rrot(a, S[f - 2], 19);
            int64revrrot(e, S[f - 2], 29);
            int64shr(h, S[f - 2], 6);
            U.l = a.l ^ e.l ^ h.l;
            U.h = a.h ^ e.h ^ h.h;
            int64rrot(a, S[f - 15], 1);
            int64rrot(e, S[f - 15], 8);
            int64shr(h, S[f - 15], 7);
            T.l = a.l ^ e.l ^ h.l;
            T.h = a.h ^ e.h ^ h.h;
            int64add4(S[f], U, S[f - 7], T, S[f - 16])
        }
        for (f = 0; f < 80; f++) {
            x.l = (j.l & K.l) ^ (~j.l & L.l);
            x.h = (j.h & K.h) ^ (~j.h & L.h);
            int64rrot(a, j, 14);
            int64rrot(e, j, 18);
            int64revrrot(h, j, 9);
            U.l = a.l ^ e.l ^ h.l;
            U.h = a.h ^ e.h ^ h.h;
            int64rrot(a, c, 28);
            int64revrrot(e, c, 2);
            int64revrrot(h, c, 7);
            T.l = a.l ^ e.l ^ h.l;
            T.h = a.h ^ e.h ^ h.h;
            R.l = (c.l & d.l) ^ (c.l & g.l) ^ (d.l & g.l);
            R.h = (c.h & d.h) ^ (c.h & g.h) ^ (d.h & g.h);
            int64add5(H, N, U, x, sha512_k[f], S[f]);
            int64add(M, T, R);
            int64copy(N, L);
            int64copy(L, K);
            int64copy(K, j);
            int64add(j, i, H);
            int64copy(i, g);
            int64copy(g, d);
            int64copy(d, c);
            int64add(c, H, M)
        }
        int64add(O[0], O[0], c);
        int64add(O[1], O[1], d);
        int64add(O[2], O[2], g);
        int64add(O[3], O[3], i);
        int64add(O[4], O[4], j);
        int64add(O[5], O[5], K);
        int64add(O[6], O[6], L);
        int64add(O[7], O[7], N)
    }
    var V = new Array(16);
    for (b = 0; b < 8; b++) {
        V[2 * b] = O[b].h;
        V[2 * b + 1] = O[b].l
    }
    return V
}

function int64(c, d) {
    this.h = c;
    this.l = d
}

function int64copy(c, d) {
    c.h = d.h;
    c.l = d.l
}

function int64rrot(f, e, d) {
    f.l = (e.l >>> d) | (e.h << (32 - d));
    f.h = (e.h >>> d) | (e.l << (32 - d))
}

function int64revrrot(f, e, d) {
    f.l = (e.h >>> d) | (e.l << (32 - d));
    f.h = (e.l >>> d) | (e.h << (32 - d))
}

function int64shr(f, e, d) {
    f.l = (e.l >>> d) | (e.h << (32 - d));
    f.h = (e.h >>> d)
}

function int64add(j, h, k) {
    var m = (h.l & 65535) + (k.l & 65535);
    var n = (h.l >>> 16) + (k.l >>> 16) + (m >>> 16);
    var i = (h.h & 65535) + (k.h & 65535) + (n >>> 16);
    var l = (h.h >>> 16) + (k.h >>> 16) + (i >>> 16);
    j.l = (m & 65535) | (n << 16);
    j.h = (i & 65535) | (l << 16)
}

function int64add4(d, a, b, c, n) {
    var o = (a.l & 65535) + (b.l & 65535) + (c.l & 65535) + (n.l & 65535);
    var p = (a.l >>> 16) + (b.l >>> 16) + (c.l >>> 16) + (n.l >>> 16) + (o >>> 16);
    var q = (a.h & 65535) + (b.h & 65535) + (c.h & 65535) + (n.h & 65535) + (p >>> 16);
    var r = (a.h >>> 16) + (b.h >>> 16) + (c.h >>> 16) + (n.h >>> 16) + (q >>> 16);
    d.l = (o & 65535) | (p << 16);
    d.h = (q & 65535) | (r << 16)
}

function int64add5(d, a, b, c, e, p) {
    var q = (a.l & 65535) + (b.l & 65535) + (c.l & 65535) + (e.l & 65535) + (p.l & 65535);
    var r = (a.l >>> 16) + (b.l >>> 16) + (c.l >>> 16) + (e.l >>> 16) + (p.l >>> 16) + (q >>> 16);
    var s = (a.h & 65535) + (b.h & 65535) + (c.h & 65535) + (e.h & 65535) + (p.h & 65535) + (r >>> 16);
    var t = (a.h >>> 16) + (b.h >>> 16) + (c.h >>> 16) + (e.h >>> 16) + (p.h >>> 16) + (s >>> 16);
    d.l = (q & 65535) | (r << 16);
    d.h = (s & 65535) | (t << 16)
};