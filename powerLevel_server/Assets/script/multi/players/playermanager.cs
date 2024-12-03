using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerLevelNetwork.Core;
using System;
using Riptide;
using Riptide.Utils;

public class playermanager : singleton<playermanager>
{
    [SerializeField] private GameObject m_PlayerPrefab;
    [SerializeField] private Transform m_RoomParent;
    private static GameObject s_PlayerPrefab;
    private static Transform s_RoomParent;
    private static Dictionary<ushort, player> stat_Players = new Dictionary<ushort, player>();
    public static player GetPlayer(ushort id)
    {
        stat_Players.TryGetValue(id, out player playscript);
        return playscript;
    }
    public static bool RemovePlayer(ushort id)
    {
        if(stat_Players.TryGetValue(id, out player playscript))
        {
            Debug.Log($"Removing Client--{id}");
            stat_Players.Remove(id);
            communicator.Instance.UpdateDictionary(stat_Players);
            return false;
        }
        return false;
    }
    protected override void Awake()
    {
        base.Awake();
        s_PlayerPrefab = m_PlayerPrefab;
        s_RoomParent = m_RoomParent;
    }
    private static void SpawnPlayer(ushort id, string username, Transform room, Transform parent)
    {
        player playscript = Instantiate(s_PlayerPrefab, room).GetComponent<player>();
        playscript.name = $"{username} -- {id}";
        playscript.Init(id, username);
        stat_Players.Add(id, playscript);
        bool shouldApprove = true;
        playscript.ApproveLogin(shouldApprove);
        communicator.Instance.UpdateDictionary(stat_Players);
        Debug.Log(parent.name);
        managerscript manager = parent.GetComponent<managerscript>();
        manager.SendNetworkRequest("FindPlayersInRoom");
    }

    #region Messages
    /* == SPACE TO RECEIVE MESSAGE ==*/
    [MessageHandler((ushort)ClientToServerMsg.RequestLogin)]
    private static void ReceiveLoginRequest(ushort fromid, Message msg)
    {
        string username = msg.GetString();
        Transform openRoom = null;
        Transform roomParent = null;
        foreach(Transform child in s_RoomParent)
        {
            roomParent = child.transform;
            Transform currentRoom = roomParent.GetChild(0);
            if(currentRoom.childCount == 2) //networkmanager.Instance.rt_playerPerRoom
            {
                Debug.Log("This Room is Full...\nTrying Next Room...");
                continue;
            }
            else
            {
                Debug.Log($"{currentRoom.name} of {roomParent.name} is Open!\nAdding Player to Room...");
                openRoom = currentRoom;
                break;
            }
        }

        if(openRoom == null)
        {
            Debug.Log("No Rooms are Open.\nTry again later.");
        }
        else
        {
            SpawnPlayer(fromid, username, openRoom, roomParent);
        }
    }
    #endregion
}
