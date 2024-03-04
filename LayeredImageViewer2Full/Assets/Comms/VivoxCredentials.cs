using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VivoxUnity;

public class VivoxCredentials
{
    public VivoxUnity.Client client;
    public Uri server = new Uri("https://mtu1xp-mad.vivox.com");
    public string issuer = "24743-my_pr-73806-udash";
    public string domain = "mtu1xp.vivox.com";
    public string tokenKey = "vKK93FcXEu2ZcUOarqFJk3bkjDpNUZeB";
    public TimeSpan timeSpan = TimeSpan.FromSeconds(90);


    public ILoginSession loginSession;
    public IChannelSession channelSession;
}
