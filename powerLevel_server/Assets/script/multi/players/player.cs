using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Reflection;
using Riptide;
using UnityEngine;
using PowerLevelNetwork.Utils;

public class player : MonoBehaviour
{
public ushort Id {get; private set; }
public string Username {get; private set; }
public int Handsize {get; private set; }
public bool isReady;
public bool isUpdated;
[HideInInspector] public bool userupdate;
[HideInInspector] public bool hasSelected;
[HideInInspector] public List<int> u_StorageList;
[HideInInspector] public ClientInfo u_ClientData;
[HideInInspector] public ClientStorage u_ClientValues;
[HideInInspector] public ClientResults u_ResultData;
[HideInInspector] private managerscript manager;
[HideInInspector] private bool signalSent;


public void SetClientData(string a, int b, int c) //add argument for userlands
{
    Handsize = b;
    Debug.Log($"Sending Signal.a from {Username}");
    manager.CatchAllPlayerSignals(Username, 1, (ushort)c);
}

public void SetClientValues(string a, List<int> b, int c)
{
    u_StorageList = b;
    Debug.Log($"Sending Signal.b from {Username}");
    manager.CatchAllPlayerSignals(Username, 1, (ushort)c);
}

public void SendManagerSignal()
{
    //When we agree to rematch send a request
    Debug.Log("Confirming With Manager...");
    manager.RequestingRematch(Username, 1, Id);  
}

public void SendNetworkRequest(string methodType)
{
    Debug.Log("Received Network Request");
    Debug.Log($"Running Method: {methodType}");
    Type thisT = this.GetType();
    MethodInfo toRun = thisT.GetMethod(methodType);
    toRun.Invoke(this, null);
}

public void Start()
{
    StartCoroutine(FindManager());
}

public void Update()
{
    if(!networkmanager.Instance.Server.TryGetClient(Id, out Connection client))
    {
        OnDestroy();
    }
}

public void Init(ushort id, string username)
{
    Id = id;
    Username = username;
}

IEnumerator FindManager()
{
    //Wait
    yield return new WaitForSeconds(1);

    //Find the GameManager Script
    while(manager == null)
    {
        manager = transform.parent.GetComponentInParent<managerscript>();
    }
}

private void OnDestroy()
    {
        if(signalSent)
        {
            return;
        }
        else
        {
            playermanager.RemovePlayer(Id);
            manager.SendNetworkRequest("GetRoomCount");
            signalSent = true;
            Destroy(this.gameObject, 0);
        }
    }

    #region Messages
    /* == SPACE TO SEND MESSAGE ==*/
    public void ApproveLogin(bool approve)
    {
        Message msg = Message.Create(MessageSendMode.Reliable, ServerToClientMsg.ApproveLogin);
        msg.AddBool(approve);
        networkmanager.Instance.Server.Send(msg, Id);
    }
    #endregion





    #region OLD CODE
    /*

    public void SendDistributionSignal()
    {
        Debug.Log("Confirming With Manager...");
        managerscript manager = transform.parent.GetComponentInParent<managerscript>();
        manager.DistributeCards();
    }


    public void SetClientValues(List<int> a, List<int> b)
    {
        u_ClientValues = new ClientStorage(a, b);
        int totalResult = ClientCalculate.TotalCalc(u_ClientValues);
        int countResult = ClientCalculate.CountCalc(u_ClientValues);
        int specialResult = ClientCalculate.SpecialCalc(u_ClientValues);
        bool matchResult = ClientCalculate.MatchCalc(u_ClientValues);
        u_ResultData = new ClientResults(totalResult, countResult, specialResult, matchResult);
        Debug.Log($"Sending Signal.b from {Username}");
        managerscript manager = transform.parent.GetComponentInParent<managerscript>();
        manager.CatchAllPlayerSignals(1, Id);

    }

    */
    #endregion
}
