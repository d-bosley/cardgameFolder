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

    public Server Server;
    [SerializeField] private ushort rt_port = 7777;
    [SerializeField] private ushort rt_MaxPlayers = 10;

    private void Start()
        {
            Server = new Server();
            Server.Start(rt_port, rt_MaxPlayers);
        }

    private void FixedUpdate()
        {
            Server.Update();
        }

}
