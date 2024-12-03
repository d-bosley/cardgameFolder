using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Riptide;
using UnityEngine;
using PowerLevelNetwork.Utils;
using PowerLevelNetwork.Core;

public class communicator : singleton<communicator>
{
    [HideInInspector] private static string username;
    [HideInInspector] private static int userid;
    [HideInInspector] private static int userbleed;
    [HideInInspector] private static int userdamage;
    [HideInInspector] private static int handsize;
    [HideInInspector] private static List<int> usercards;
    [HideInInspector] private static List<int> userstorage;
    [HideInInspector] private static List<int> usertotals;
    [HideInInspector] private static player m_playscript;
    [HideInInspector] public static Dictionary<ushort, player> localPlayers;



    #region Messages
    /*== SPACE TO RECEIVE MESSAGE ==*/
    [MessageHandler((ushort)ClientToServerMsg.PushPlayerStatus)]
    private static void ReceiveStatusRequest(ushort fromid, Message msg)
    {
        Debug.Log("Status Received!");
        username = msg.GetString();
        userstorage = msg.GetInts().ToList<int>();
        userid = msg.GetUShort();
        ushort msgId = fromid;
        FindFromDictionary(fromid).SetClientValues(username, userstorage, userid);
    }

    [MessageHandler((ushort)ClientToServerMsg.SendGameData)]
    private static void StartCheckingData(ushort fromid, Message msg)
    {
        Debug.Log("Update Received!");
        username = msg.GetString();
        handsize = msg.GetInt();
        userid = msg.GetUShort();
        ushort msgId = fromid;
        //userlands = msg.GetInts().ToList<int>();
        FindFromDictionary(fromid).SetClientData(username, handsize, userid); //add argument for userlands
    }

    [MessageHandler((ushort)ClientToServerMsg.RequestSceneChange)]
    private static void ReceiveSceneChange(ushort fromid, Message msg)
    {
        //We've gotten a request for a rematch from the player
        Debug.Log("Scene Change Received!");
        //Send that request to the corresponding player ID
        //Then, we can update the scene and reset the game
        FindFromDictionary(fromid).SendNetworkRequest(msg.GetString());
    }
    #endregion

    public void UpdateDictionary(Dictionary<ushort, player> newDefinition)
    {
        localPlayers = newDefinition;
    }

    public static player FindFromDictionary(ushort fromid)
    {
        if(localPlayers.TryGetValue(fromid, out player i_player))
        {
            return i_player;
        }
        else
        {
            return null;
        }
    }



    public static T FindPlayerID<T>(string name, ushort id) where T : Component
    {
        T i_player = GameObject.Find($"{name} -- {id}").TryGetComponent<T>(out T player) ? player : null;
        Debug.Log($"Message from: {name} -- {id} confirmed");
        return i_player;
    }

    #region OLD CODE
    /*

    [MessageHandler((ushort)ClientToServerMsg.GetPlayerTurn)]
    private static void SendDistributionSignal(ushort fromid, Message msg)
    {
        //When we have sent over our LAND card we have to tell the manager to start distributing cards
        Debug.Log("Seletion Received!");
        //We'll send the card we selected over in the real script
        //For now, we just need to set our status to "hasSelected"
        m_playscript.SendDistributionSignal();
    }


    [MessageHandler((ushort)ClientToServerMsg.RequestSceneChange)]
    private static void ReceiveSceneChange(ushort fromid, Message msg)
    {
        //We've gotten a request for a rematch from the player
        Debug.Log("Scene Change Received!");
        //Send that request to the corresponding player ID
        //Then, we can update the scene and reset the game
        FindFromDictionary(fromid).SendManagerSignal();
    }

    */
    #endregion
}
