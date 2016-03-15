# New ports collection makefile for:	udpcast
# Date created:		Mon Jul 19 19:10:56 UTC 2004
# Whom:			Mario Sergio Fujikawa Ferreira <lioux@FreeBSD.org>
#
# $FreeBSD$
#

PORTNAME=	udpcast
PORTVERSION=	0.${RELEASE_DATE}
CATEGORIES=	net
MASTER_SITES=	http://udpcast.linux.lu/download/
DISTNAME=	${PORTNAME}-${RELEASE_DATE}

MAINTAINER=	ports@FreeBSD.org
COMMENT=	A file transfer tool using ethernet's multicast

USES=		gmake perl5
WRKSRC=	${WRKDIR}/${PORTNAME}-${RELEASE_DATE}

RELEASE_DATE=	20120424

PROGRAM_SBIN_FILES=	udp-receiver udp-sender

PLIST_FILES=	#udp-receiver 

GNU_CONFIGURE=	yes
CONFIGURE_ENV=	CPPFLAGS="${CPPFLAGS} -I${LOCALBASE}/include" \
		LIBS="-L${LOCALBASE}/lib"

do-install:
.for file in ${PROGRAM_SBIN_FILES}
	@${INSTALL_PROGRAM} ${WRKDIR}/${PORTNAME}-${RELEASE_DATE}/${file} ${PREFIX}/sbin/
.endfor

.include <bsd.port.mk>
