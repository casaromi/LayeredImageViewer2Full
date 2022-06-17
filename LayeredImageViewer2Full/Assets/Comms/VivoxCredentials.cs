using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VivoxUnity;

public class VivoxCredentials
{
    public VivoxUnity.Client client;
    public Uri server = new Uri("https://mt1s.www.vivox.com/api2");
    public string issuer = "michae0067-vi50-dev";
    public string domain = "mt1s.vivox.com";
    public string tokenKey = "chap306";
    public TimeSpan timeSpan = TimeSpan.FromSeconds(90);


    public ILoginSession loginSession;
    public IChannelSession channelSession;
}
