from django.db import models


class Clonedeploy(models.Model):
    """
    Django model describing every tunable setting for clonedeploy
    """

    enable = models.BooleanField(default=False)
