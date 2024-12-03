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
    UpdateGameData,
    ReadyNextRound,
    StartPlayerTurn,
    SendPlayerCards,
    ChangeTheScene
}

public enum ClientToServerMsg : ushort
{
    RequestLogin,
    SendGameData,
    PushPlayerStatus,
    GetPlayerTurn,
    GetPlayerCards,
    RequestSceneChange
}
public class networkmanager : singleton<networkmanager>
{
    [SerializeField] private GameObject s_RoomPrefab;
    [SerializeField] private Transform s_RoomParent;
    public Server Server;
    [SerializeField] private ushort rt_port = 7777;
    [SerializeField] private ushort rt_MaxPlayers;
    public int rt_playerPerRoom {get; private set; }

    protected override void Awake()
    {
        base.Awake();
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, true);
    }

    private void Start()
    {
        Server = new Server();
        Server.Start(rt_port, rt_MaxPlayers);
        //Create Rooms
        rt_playerPerRoom = 2;
        int rt_maxRooms = rt_MaxPlayers/rt_playerPerRoom;
        for(int i = 0; i < rt_maxRooms; i++)
        {
          GameObject newRoom = Instantiate(s_RoomPrefab, s_RoomParent);
          newRoom.name = $"ServerRoom_{i + 1}";
          newRoom.transform.GetChild(0).gameObject.name = $"PlayerRoom_{i + 1}";
        }
    }

    private void FixedUpdate()
    {
        Server.Update();
    }

}
