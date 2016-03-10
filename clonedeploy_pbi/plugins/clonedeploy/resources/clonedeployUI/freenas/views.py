from subprocess import Popen, PIPE
import json
import time
import urllib2
import ssl

from django.core.urlresolvers import reverse
from django.http import HttpResponse
from django.shortcuts import render
from django.template import RequestContext
from django.template.loader import render_to_string
from django.utils import simplejson

import jsonrpclib
import oauth2 as oauth
from clonedeployUI.freenas import forms, models, utils


class OAuthTransport(jsonrpclib.jsonrpc.SafeTransport):
    def __init__(self, host, verbose=None, use_datetime=0, key=None,
            secret=None):
        jsonrpclib.jsonrpc.SafeTransport.__init__(self)
        self.verbose = verbose
        self._use_datetime = use_datetime
        self.host = host
        self.key = key
        self.secret = secret

    def oauth_request(self, url, moreparams={}, body=''):
        params = {
            'oauth_version': "1.0",
            'oauth_nonce': oauth.generate_nonce(),
            'oauth_timestamp': int(time.time())
        }
        consumer = oauth.Consumer(key=self.key, secret=self.secret)
        params['oauth_consumer_key'] = consumer.key
        params.update(moreparams)

        req = oauth.Request(method='POST',
            url=url,
            parameters=params,
            body=body)
        signature_method = oauth.SignatureMethod_HMAC_SHA1()
        req.sign_request(signature_method, consumer, None)
        return req

    def request(self, host, handler, request_body, verbose=0):
        request = self.oauth_request(url=self.host, body=request_body)
        req = urllib2.Request(request.to_url())
        req.add_header('Content-Type', 'text/json')
        req.add_data(request_body)
        ssl._create_default_https_context = ssl._create_unverified_context
        f = urllib2.urlopen(req)
        return(self.parse_response(f))


class JsonResponse(HttpResponse):
    """
    This is a response class which implements FreeNAS GUI API

    It is not required, the user can implement its own
    or even open/code an entire new UI just for the plugin
    """

    error = False
    type = 'page'
    force_json = False
    message = ''
    events = []

    def __init__(self, request, *args, **kwargs):

        self.error = kwargs.pop('error', False)
        self.message = kwargs.pop('message', '')
        self.events = kwargs.pop('events', [])
        self.force_json = kwargs.pop('force_json', False)
        self.type = kwargs.pop('type', None)
        self.template = kwargs.pop('tpl', None)
        self.form = kwargs.pop('form', None)
        self.node = kwargs.pop('node', None)
        self.formsets = kwargs.pop('formsets', {})
        self.request = request

        if self.form:
            self.type = 'form'
        elif self.message:
            self.type = 'message'
        if not self.type:
            self.type = 'page'

        data = dict()

        if self.type == 'page':
            if self.node:
                data['node'] = self.node
            ctx = RequestContext(request, kwargs.pop('ctx', {}))
            content = render_to_string(self.template, ctx)
            data.update({
                'type': self.type,
                'error': self.error,
                'content': content,
            })
        elif self.type == 'form':
            data.update({
                'type': 'form',
                'formid': request.POST.get("__form_id"),
                'form_auto_id': self.form.auto_id,
                })
            error = False
            errors = {}
            if self.form.errors:
                for key, val in self.form.errors.items():
                    if key == '__all__':
                        field = self.__class__.form_field_all(self.form)
                        errors[field] = [unicode(v) for v in val]
                    else:
                        errors[key] = [unicode(v) for v in val]
                error = True

            for name, fs in self.formsets.items():
                for i, form in enumerate(fs.forms):
                    if form.errors:
                        error = True
                        for key, val in form.errors.items():
                            if key == '__all__':
                                field = self.__class__.form_field_all(form)
                                errors[field] = [unicode(v) for v in val]
                            else:
                                errors["%s-%s" % (
                                    form.prefix,
                                    key,
                                    )] = [unicode(v) for v in val]
            data.update({
                'error': error,
                'errors': errors,
                'message': self.message,
            })
        elif self.type == 'message':
            data.update({
                'error': self.error,
                'message': self.message,
            })
        else:
            raise NotImplementedError

        data.update({
            'events': self.events,
        })
        if request.is_ajax() or self.force_json:
            kwargs['content'] = json.dumps(data)
            kwargs['content_type'] = 'application/json'
        else:
            kwargs['content'] = (
                "<html><body><textarea>"
                + json.dumps(data) +
                "</textarea></body></html>"
                )
        super(JsonResponse, self).__init__(*args, **kwargs)

    @staticmethod
    def form_field_all(form):
        if form.prefix:
            field = form.prefix + "-__all__"
        else:
            field = "__all__"
        return field


def start(request, plugin_id):
    (clonedeploy_key,
    clonedeploy_secret) = utils.get_clonedeploy_oauth_creds()

    url = utils.get_rpc_url(request)
    trans = OAuthTransport(url, key=clonedeploy_key,
        secret=clonedeploy_secret)

    server = jsonrpclib.Server(url, transport=trans)
    auth = server.plugins.is_authenticated(
        request.COOKIES.get("sessionid", "")
        )
    jail_path = server.plugins.jail.path(plugin_id)
    assert auth

    try:
        clonedeploy = models.Clonedeploy.objects.order_by('-id')[0]
        clonedeploy.enable = True
        clonedeploy.save()
    except IndexError:
        clonedeploy = models.Clonedeploy.objects.create(enable=True)

    try:
        form = forms.ClonedeployForm(clonedeploy.__dict__,
            instance=clonedeploy,
            jail_path=jail_path)
        form.is_valid()
        form.save()
    except ValueError:
        return HttpResponse(simplejson.dumps({
            'error': True,
            'message': ('Clonedeploy data did not validate, configure '
                'it first.'),
            }), content_type='application/json')

    cmd = "%s onestart" % utils.clonedeploy_control
    pipe = Popen(cmd, stdin=PIPE, stdout=PIPE, stderr=PIPE,
        shell=True, close_fds=True)

    out = pipe.communicate()[0]
    return HttpResponse(simplejson.dumps({
        'error': False,
        'message': out,
        }), content_type='application/json')


def stop(request, plugin_id):
    (clonedeploy_key,
    clonedeploy_secret) = utils.get_clonedeploy_oauth_creds()
    url = utils.get_rpc_url(request)
    trans = OAuthTransport(url, key=clonedeploy_key,
        secret=clonedeploy_secret)

    server = jsonrpclib.Server(url, transport=trans)
    auth = server.plugins.is_authenticated(
        request.COOKIES.get("sessionid", "")
        )
    jail_path = server.plugins.jail.path(plugin_id)
    assert auth

    try:
        clonedeploy = models.Clonedeploy.objects.order_by('-id')[0]
        clonedeploy.enable = False
        clonedeploy.save()
    except IndexError:
        clonedeploy = models.Clonedeploy.objects.create(enable=False)

    try:
        form = forms.ClonedeployForm(clonedeploy.__dict__,
            instance=clonedeploy,
            jail_path=jail_path)
        form.is_valid()
        form.save()
    except ValueError:
        pass

    cmd = "%s onestop" % utils.clonedeploy_control
    pipe = Popen(cmd, stdin=PIPE, stdout=PIPE, stderr=PIPE,
        shell=True, close_fds=True)

    out = pipe.communicate()[0]
    return HttpResponse(simplejson.dumps({
        'error': False,
        'message': out,
        }), content_type='application/json')


def open_view(request, plugin_id):
    (clonedeploy_key,
    clonedeploy_secret) = utils.get_clonedeploy_oauth_creds()
    url = utils.get_rpc_url(request)
    trans = OAuthTransport(url, key=clonedeploy_key,
        secret=clonedeploy_secret)
    server = jsonrpclib.Server(url, transport=trans)
    jail = json.loads(server.plugins.jail.info(plugin_id))[0]
    jail_ipv4 = jail['fields']['jail_ipv4'].split('/', 1)[0]

    return render(request, "open.html", {
        'ipv4': jail_ipv4,
    })


def treemenu(request, plugin_id):
    """
    This is how we inject nodes to the Tree Menu

    The FreeNAS GUI will access this view, expecting for a JSON
    that describes a node and possible some children.
    """

    (clonedeploy_key,
    clonedeploy_secret) = utils.get_clonedeploy_oauth_creds()
    url = utils.get_rpc_url(request)
    trans = OAuthTransport(url, key=clonedeploy_key,
        secret=clonedeploy_secret)
    server = jsonrpclib.Server(url, transport=trans)
    jail = json.loads(server.plugins.jail.info(plugin_id))[0]
    jail_name = jail['fields']['jail_host']
    number = jail_name.rsplit('_', 1)
    name = "clonedeploy"
    if len(number) == 2:
        try:
            number = int(number)
            if number > 1:
                name = "Clonedeploy (%d)" % number
        except:
            pass

    plugin = {
        'name': name,
        'append_to': 'plugins',
        'icon': reverse('treemenu_icon', kwargs={'plugin_id': plugin_id}),
        'type': 'pluginsfcgi',
        'url': reverse('clonedeploy_open', kwargs={'plugin_id': plugin_id}),
        'kwargs': {'plugin_name': 'clonedeploy', 'plugin_id': plugin_id },
    }

    return HttpResponse(json.dumps([plugin]), content_type='application/json')


def status(request, plugin_id):
    """
    Returns a dict containing the current status of the services

    status can be one of:
        - STARTING
        - RUNNING
        - STOPPING
        - STOPPED
    """
    pid = None

    proc = Popen([utils.clonedeploy_control, "onestatus"],
        stdout=PIPE,
        stderr=PIPE)

    stdout = proc.communicate()[0]

    if proc.returncode == 0:
        status = 'RUNNING'
        pid = stdout.split('\n')[0]
    else:
        status = 'STOPPED'

    return HttpResponse(json.dumps({
            'status': status,
            'pid': pid,
        }),
        content_type='application/json')


def treemenu_icon(request, plugin_id):

    with open(utils.clonedeploy_icon, 'rb') as f:
        icon = f.read()

    return HttpResponse(icon, content_type='image/png')
