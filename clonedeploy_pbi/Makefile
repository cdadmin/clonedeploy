#
#	Makefile - wrap around create_plugin to make things simple
#
.include <bsd.own.mk>

TOP=	${.CURDIR}
PLUGINSDIR=	${TOP}/plugins
PLUGINS!=	ls ${PLUGINSDIR}
TARGETS=
CREATE_PLUGIN=	${TOP}/create_plugin

GIT_REPO_SETTING=.git-repo-setting
.if exists(${GIT_REPO_SETTING})
GIT_LOCATION!=cat ${GIT_REPO_SETTING}
.endif

ENV_SETUP=env GIT_LOCATION=${GIT_LOCATION}

.for _p in ${PLUGINS}
TARGETS+=	${_p}
.endfor

.include <bsd.prog.mk>

.for _p in ${PLUGINS}
${_p}: git-verify
	@cd ${TOP}; ${ENV_SETUP} ${CREATE_PLUGIN} ${.TARGET}
.endfor

list:
	@echo ${TARGETS}

help:
	@echo "------------------------------------------------------------"
	@echo "                   Available Targets                        "
	@echo "------------------------------------------------------------"
.for _p in ${PLUGINS}
	@printf "%-25s- build ${_p} plugin\n" "${_p}"
.endfor
	@printf "%-25s- list all targets\n" list
	@printf "%-25s- build all targets\n" all
	@echo "------------------------------------------------------------"

clean: git-verify
	rm -rf ${TOP}/build

git-verify:
	@if [ ! -f ${GIT_REPO_SETTING} ]; then \
		echo "No git repo choice is set.  Please use \"make git-external\" to build as an"; \
		echo "external developer or \"make git-internal\" to build as an iXsystems"; \
		echo "internal developer.  You only need to do this once."; \
		exit 1; \
	fi
	@echo "NOTICE: You are building from the ${GIT_LOCATION} git repo."

git-internal:
	@echo "INTERNAL" > ${GIT_REPO_SETTING}
	@echo "You are set up for internal (iXsystems) development.  You can use"
	@echo "the standard make targets now."

git-external:
	@echo "EXTERNAL" > ${GIT_REPO_SETTING}
	@echo "You are set up for external (github) development.  You can use"
	@echo "the standard make targets now."

all: ${PLUGINS}
