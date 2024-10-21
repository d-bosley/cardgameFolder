using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerLevelNetwork.Core;
using Riptide;
using Riptide.Utils;

public enum ServerToClientMsg : ushort
{
    ApproveLogin,
}

public enum ClientToServerMsg : ushort
{
    RequestLogin,
}
public class networkmanager : singleton<networkmanager>
{
    protected override void Awake()
    {
        base.Awake();
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, true);
    }

    public Client Client;
    [SerializeField] private ushort rt_port = 7777;
    [SerializeField] private string rt_IP = "127.0.0.1";
    private static string s_LocalUsername;

    private void Start()
    {
        Client = new Client();
        Client.Connected += OnClientConnected;
    }
    private void OnClientConnected(object sender, EventArgs e)
    {
        playermanager.Instance.SpawnInitialPlayer(s_LocalUsername);
    }
    public void Connect(string username)
    {
        s_LocalUsername = string.IsNullOrEmpty(username) ? $"Guest" : username;
        Client.Connect($"{rt_IP}:{rt_port}");
    }
    private void FixedUpdate()
    {
        Client.Update();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        Client.Connected -= OnClientConnected;
    }

}
