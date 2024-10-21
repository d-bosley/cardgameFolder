using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Riptide;

public class player : MonoBehaviour
{
public ushort Id {get; private set; }
public string Username {get; private set; }
public bool IsLocal {get; private set; }

public void Init(ushort id, string username, bool isLocal)
    {
        Id = id;
        Username = username;
        IsLocal = isLocal;
    }

private void OnDestroy()
    {
        playermanager.RemovePlayer(Id);
    }

    #region Messages
    /* == SPACE TO SEND MESSAGE ==*/
    public void RequestInit()
    {
        Message msg = Message.Create(MessageSendMode.Reliable, ClientToServerMsg.RequestLogin);
        msg.AddString(Username);
        networkmanager.Instance.Client.Send(msg);
    }
    #endregion
}
