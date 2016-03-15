import hashlib
import json
import os
import pwd
import urllib

from django.utils.translation import ugettext_lazy as _

from dojango import forms
from clonedeployUI.freenas import models, utils


class ClonedeployForm(forms.ModelForm):

    class Meta:
        model = models.Clonedeploy
        exclude = (
            'enable',
        )

    def __init__(self, *args, **kwargs):
        self.jail_path = kwargs.pop('jail_path')
        super(ClonedeployForm, self).__init__(*args, **kwargs)

    def save(self, *args, **kwargs):
        obj = super(ClonedeployForm, self).save(*args, **kwargs)

        rcconf = os.path.join(utils.clonedeploy_etc_path, "rc.conf")
        with open(rcconf, "w") as f:
            if obj.enable:
                f.write('apache22_enable="YES"\n')

        os.system(os.path.join(utils.clonedeploy_pbi_path, "tweak-rcconf"))
