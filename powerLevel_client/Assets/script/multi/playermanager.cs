using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerLevelNetwork.Core;
using System;

public class playermanager : singleton<playermanager>
{
    [SerializeField] private GameObject m_PlayerPrefab;
    private static GameObject s_PlayerPrefab;
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
            stat_Players.Remove(id);
            return false;
        }
        return false;
    }

    public static player LocalPlayer => GetPlayer(networkmanager.Instance.Client.Id);
    public static bool IsLocalPlayer(ushort id) => id == LocalPlayer.Id;
    protected override void Awake()
    {
        base.Awake();
        s_PlayerPrefab = m_PlayerPrefab;
    }
    public void SpawnInitialPlayer(string username)
    {
        player playscript = Instantiate(m_PlayerPrefab, Vector3.zero, Quaternion.identity).GetComponent<player>();
        playscript.name = $"{username} -- LOCAL PLAYER WAITING FOR SERVER";
        ushort id = networkmanager.Instance.Client.Id;
        playscript.Init(id, username, true);
        stat_Players.Add(id, playscript);
        playscript.RequestInit();
    }
}
