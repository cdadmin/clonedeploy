# $FreeBSD$

PORTNAME=	pyload
PORTVERSION=	0.4.9
CATEGORIES=	www
MASTER_SITES=	http://download.pyload.org/
DISTNAME=	${PORTNAME}-src-v${PORTVERSION}

MAINTAINER=	mrhanky@unterschicht.tv
COMMENT=	A lightweight one-click download manager written in python

LICENSE=	GPLv3

RUN_DEPENDS=	${PYTHON_PKGNAMEPREFIX}sqlite3>0:${PORTSDIR}/databases/py-sqlite3 \
		${PYTHON_PKGNAMEPREFIX}curl>0:${PORTSDIR}/ftp/py-curl \
		${PYTHON_PKGNAMEPREFIX}pycrypto>0:${PORTSDIR}/security/py-pycrypto

USES=		zip python:2.5-2.7 gettext

NO_BUILD=	yes
PLIST_SUB=	PORTNAME=${PORTNAME} USER=${USERS} GROUP=${GROUPS}
SUB_LIST=	PORTNAME=${PORTNAME} USER=${USERS} GROUP=${GROUPS} PYTHON_CMD=${PYTHON_CMD}
SUB_FILES=	pkg-message
USE_RC_SUBR=	${PORTNAME}

USERS=		media
GROUPS=		${USERS}

OPTIONS_DEFINE=	JSENGINE OPENSSL TESSERACT LIGHTWEIGHT UNPACK
OPTIONS_DEFAULT=JSENGINE OPENSSL TESSERACT LIGHTWEIGHT UNPACK
JSENGINE_DESC=	Install spidermonkey JS engine for Click'n'load
TESSERACT_DESC=	Image recognition provided by tesseract
LIGHTWEIGHT_DESC=Use bjoern as the webserver for pyLoad
UNPACK_DESC=	Install binaries for extracting .rar and .zip files

.include <bsd.port.options.mk>

.if ${PORT_OPTIONS:MJSENGINE}
RUN_DEPENDS+=	spidermonkey17>0:${PORTSDIR}/lang/spidermonkey17
.endif

.if ${PORT_OPTIONS:MOPENSSL}
RUN_DEPENDS+=	${PYTHON_PKGNAMEPREFIX}openssl>0:${PORTSDIR}/security/py-openssl
.endif

.if ${PORT_OPTIONS:MTESSERACT}
RUN_DEPENDS+=	tesseract>0:${PORTSDIR}/graphics/tesseract \
		${PYTHON_PKGNAMEPREFIX}imaging>0:${PORTSDIR}/graphics/py-imaging
.endif

.if ${PORT_OPTIONS:MLIGHTWEIGHT}
RUN_DEPENDS+=   ${PYTHON_PKGNAMEPREFIX}bjoern>0:${PORTSDIR}/www/py-bjoern
.endif

.if ${PORT_OPTIONS:MUNPACK}
RUN_DEPENDS+=	unrar>0:${PORTSDIR}/archivers/unrar \
		unzip>0:${PORTSDIR}/archivers/unzip
.endif

post-extract:
	${MV} ${WRKDIR}/pyload ${WRKSRC}

do-install:
	${INSTALL} -d ${STAGEDIR}${DATADIR}/pyload
	cd ${WRKSRC} && ${COPYTREE_SHARE} . ${STAGEDIR}${DATADIR}/pyload

.include <bsd.port.mk>
