from django.conf.urls import patterns, include, url

urlpatterns = patterns('',
     url(r'^plugins/clonedeploy/(?P<plugin_id>\d+)/', include('clonedeployUI.freenas.urls')),
)
