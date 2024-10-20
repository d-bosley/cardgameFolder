using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerLevelNetwork.Core;
using Riptide;
using Riptide.Utils;

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

    private void Start()
        {
            Client = new Client();
            Connect();
        }
    public void Connect()
    {
        Client.Connect($"{rt_IP}:{rt_port}");
    }
    private void FixedUpdate()
        {
            Client.Update();
        }

}
