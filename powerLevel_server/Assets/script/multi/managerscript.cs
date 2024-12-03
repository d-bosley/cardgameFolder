using System.Collections;
using System.Collections.Generic;
using Riptide;
using Riptide.Utils;
using UnityEngine;
using PowerLevelNetwork.Core;
using PowerLevelNetwork.Utils;
using System.Linq;
using System;
using System.Reflection;

public class managerscript : MonoBehaviour
{
    [HideInInspector] private Transform m_PlayerRoom;
    [HideInInspector] private Transform m_ServerRoom;
    [HideInInspector] public static bool managerupdate;
    [HideInInspector] public static bool canStart;
    [HideInInspector] public static bool canIterate;
    [HideInInspector] public int maxPlayers = 2;
    [HideInInspector] public bool hasStarted;
    [HideInInspector] public int signal;
    [HideInInspector] public int playerTurn;
    [HideInInspector] public player player_1;
    [HideInInspector] public player player_2;
    [HideInInspector] private List<int> roomDeck;
    [HideInInspector] private List<int> activeDeck;
    [HideInInspector] private List<int> discardDeck;


    private void Awake()
    {
        m_ServerRoom = transform;
        m_PlayerRoom = m_ServerRoom.GetChild(0);
    }

    private void Start()
    {
        managerupdate = false;
        canStart = true;
        canIterate = false;
        hasStarted = false;
        playerTurn = 0;
        signal = 0;
        activeDeck = new List<int>();
        discardDeck = new List<int>();
        player_1 = null;
        player_2 = null;
    }

    private void Update()
    {
        //FindTheWinner();
        //TryIterating();
    }


    public void SendNetworkRequest(string methodType)
    {
        Debug.Log("Received Network Request");
        Debug.Log($"Running Method: {methodType}");
        Type thisT = this.GetType();
        MethodInfo toRun = thisT.GetMethod(methodType);
        toRun.Invoke(this, null);
    }

    public void FindPlayersInRoom()
    {
        if(player_1 == null || player_2 == null)
        {
            player currentPlayer = player_1;
            player_2 = currentPlayer;
            RetrieveID<player>(m_PlayerRoom, out currentPlayer);
            player_1 = currentPlayer;
            DecideRoomOrder(player_1, player_2);
        }
    }


    #region GENERAL
    /*== METHODS USED IN OTHER METHODS ==*/

    //Receive pushes from players to start functions
    public void CatchAllPlayerSignals(string name, int push, ushort id)
    {
        //Check to see if we've gone through all the turns
        if (playerTurn == 5)
        {
            playerTurn = 0;
        }

        //Send a push to the manager so we know that player is ready
        signal += push;

        Debug.Log($"Player--{id} has sent a push");
        Debug.Log($"We currently have {signal} push(es)");
        //Each Client has to update before Beginning
        if(signal == m_PlayerRoom.childCount)
        {
            playerTurn++;
            BeginTheRound(m_PlayerRoom);
        }

        //Each Client has to submit cards before Battling
        if(signal == m_PlayerRoom.childCount * 2)
        {
            signal = m_PlayerRoom.childCount; //Reset the signals so we can run this again
            SendPlayerStorage(m_PlayerRoom);
        }
    }

    public void DecideRoomOrder(player a, player b)
    {
        Debug.Log("In Here");
        if(a == null || b == null)
        {
            return;
        }

        if(a.Id > b.Id)
        {   
            Debug.Log("We're Here");
            b.transform.SetAsFirstSibling();
            StartTheGame();
        }
        if(a.Id < b.Id)
        {   
            Debug.Log("We're Here");
            b.transform.SetAsLastSibling();
            StartTheGame();
        }
        if (a.Id == b.Id)
        {
            return;
        }

    }

    //Multi-use iteration script to apply to various methods
    public void CatchAllPlayerLogs(bool status, Transform playerRoom)
    {
        int counted = playerRoom.childCount;
        foreach(Transform child in playerRoom)
        {
            player playscript = child.GetComponent<player>();
            if(!status)
            {
                Debug.Log($"Player--{playscript.Id} not ready");
                break;
            }
        }
    }

    public void GetRoomCount()
    {
        if(hasStarted)
        {
            StartCoroutine(CloseTheRoom());
        }
    }

    IEnumerator CloseTheRoom()
    {
        //Reset The Room
        ResetTheRoomLite();

        //Then, send the new scene
        SendNewSceneToPlayers(0); //The scene index will be "0"

        //Wait
        yield return new WaitForSeconds(2);

        //Find the remaining player
        RetrieveID<player>(m_PlayerRoom, out player currentPlayer);
        player_1 = currentPlayer;
        player_2 = null;

        playermanager.RemovePlayer(player_1.Id);
        Destroy(player_1.gameObject);
        player_1 = null;
    }

    public void RequestingRematch(string name, int push, ushort id)
    {
        signal += push;

        Debug.Log($"Player--{id} has sent a push");
        Debug.Log($"We currently have {signal} push(es)");

        //Each Client has to agree before a Rematch
        if(signal == m_PlayerRoom.childCount)
        {
            //First, reset the room
            ResetTheRoomLite();

            //Then, send the new scene
            SendNewSceneToPlayers(1); //The scene index will be "1"
        }
    }

    public void ResetTheRoom()
    {
            managerupdate = false;
            canStart = true;
            canIterate = false;
            hasStarted = false;
            playerTurn = 0;
            signal = 0;
            activeDeck = new List<int>();
            discardDeck = new List<int>();
            player_1 = null;
            player_2 = null;
            Debug.Log($"Resetting {m_PlayerRoom.gameObject.name}!");
    }

    public void ResetTheRoomLite()
    {
            managerupdate = false;
            canStart = true;
            canIterate = false;
            hasStarted = false;
            playerTurn = 0;
            signal = 0;
            activeDeck = new List<int>();
            discardDeck = new List<int>();
            Debug.Log($"Resetting {m_PlayerRoom.gameObject.name}!");
    }

    //Iterate through all of the player's data once all players have sent a state of "updated"
    public void IterateUserInfo()
    {
        int counted = m_PlayerRoom.childCount;
        for(int i = 0; i < counted; i++)
        {
            player playscript = m_PlayerRoom.transform.GetChild(i).gameObject.GetComponent<player>();
            //ClientInfo data = playscript.u_ClientData;
            int id = i + 1;
            string name = playscript.Username;
            //data.OutLands(out List<int> lands);
            //Run a method to send this information out to each client
            ReturnInformation(name, id);

        }
    }

    //Get all player total and value comparisons from the player script
    //Then compile all of the results for a final comparison
    public void BeginFinalCalc(out string winner)
    {
        Dictionary<string, ClientResults> userResults = new Dictionary<string, ClientResults>();
        foreach(Transform child in m_PlayerRoom)
            userResults[child.gameObject.GetComponent<player>().Username] = child.gameObject.GetComponent<player>().u_ResultData;
        int entries = userResults.Count;
        int highestTotal = 0;
        string leader = null;
        foreach(var entry in userResults)
        {
            if (entry.Value.result_T > highestTotal)
            {
                highestTotal = entry.Value.result_T;
                leader = entry.Key;
            }
        }
        winner = leader;
    }

    public static T FindPlayerID<T>(string name, ushort id, out T component) where T : Component
    {
        T i_player = GameObject.Find($"{name} -- {id}").TryGetComponent<T>(out T player) ? player : null;
        Debug.Log($"Message from: {name} -- {id} confirmed");
        component = i_player;
        return component;
    }

    public static T RetrieveID<T>(Transform room, out T component) where T : Component
    {
        T i_player = room.GetChild(room.childCount - 1).TryGetComponent<T>(out T player) ? player : null;
        component = i_player;
        return component;
    }

    #endregion





    #region GAMEPLAY
    /*== METHODS USED TO RUN THROUGH THE GAME ==*/

    //Start the round
    public void BeginTheRound(Transform playerRoom)
    {
        //Get the turn order by updating the turn value and moving through the users in the room
        // player playscript = playerRoom.GetChild(playerTurn).GetComponent<player>();

        //Get the current player's id and send them a land to start the turn
        // ushort id = playscript.Id;
        // string username = playscript.Username;

        SendTheLand();

//        if (playerTurn == playerRoom.childCount)
//            {playerTurn = 0;}
    }

    public void SendPlayerStorage(Transform playerRoom)
    {
        int count = 1;
        foreach(Transform child in playerRoom)
        {
            Debug.Log($"Count is {count}");
            List<int> list;
            ushort id;
            int number;
            player playscript = child.TryGetComponent<player>(out player script) ? script : null;

            if(playscript != null)
            {
                Debug.Log("Found Player");
                list = playscript.u_StorageList;
                player rival = playerRoom.GetChild(count).TryGetComponent<player>(out player other) ? other : null;
                id = rival.Id;
                number = count + 1;
                SendPlayerCards(list, number, id);
                count -= 1;
                Debug.Log($"New Count is {count}");
            }
        }
    }

    #endregion
    





    #region MESSAGES
    /*== SPACE TO SEND MESSAGE ==*/

    
    //We check the player parent to see if everyone is in, and if they are tell the client to change scenes
    public void StartTheGame()
    {
        Debug.Log("we're in start");
        if(!hasStarted) //m_PlayerRoom.childCount == maxPlayers
        {
        Debug.Log($"Starting {m_PlayerRoom.gameObject.name}!");
        Message msg = Message.Create(MessageSendMode.Reliable, ServerToClientMsg.ChangeTheScene);
        msg.AddInt(1);
        networkmanager.Instance.Server.Send(msg, player_1.Id, true);
        if(player_2 != null)
            networkmanager.Instance.Server.Send(msg, player_2.Id, true);
        hasStarted = true;
        }
    }

    public void SendTheLand()
    {
        //Debug.Log($"Getting Land Card from Player--{id}");
        //When we've set up our LANDS we'll pass the current card as part of the message
        //For now, just get confirmation that the client received the message
        Debug.Log($"Sending Out The Land Card");
        int newLand = UnityEngine.Random.Range(5, 9);
        Message msg = Message.Create(MessageSendMode.Reliable, ServerToClientMsg.StartPlayerTurn);
        //msg.AddInt((int)id);
        msg.AddInt(newLand);
        msg.AddInt(playerTurn);
        networkmanager.Instance.Server.Send(msg, player_1.Id, true);
        if(player_2 != null)
        {
            Debug.Log("Why are we in here");
            networkmanager.Instance.Server.Send(msg, player_2.Id, true);
        }
    }

    public void SendPlayerCards(List<int> cards, int number, ushort id)
    {
        Debug.Log($"Sending Cards to Player--{id}...");
        Message msg = Message.Create(MessageSendMode.Reliable, ServerToClientMsg.SendPlayerCards);
        msg.AddInts(cards.ToArray());
        msg.AddInt(number);
        networkmanager.Instance.Server.Send(msg, id, true);
    }

    public void SendNewSceneToPlayers(int index)
    {
        Debug.Log("Sending New Scene");
        Message msg = Message.Create(MessageSendMode.Reliable, ServerToClientMsg.ChangeTheScene);
        msg.AddInt(index);
        networkmanager.Instance.Server.SendToAll(msg);
    }

    public void CompleteRound()
    {
        //managerupdate = nextTurn;
        Debug.Log("Sending Round Results...");
        BeginFinalCalc(out string winner);
        //currentwinner
        //we use this to send info during the user update to give the winner a LAND card
        canIterate = true;
        Message msg = Message.Create(MessageSendMode.Reliable, ServerToClientMsg.ReadyNextRound);
        //msg.AddBool(round);
        msg.AddString(winner);
        networkmanager.Instance.Server.SendToAll(msg);
    }

    public void ReturnInformation(string name, int id)
    {
        Debug.Log("Returning User Information...");
        canStart = true;
        ushort uid = (ushort)id;
        //NOTE:
        // In the future the client ID will need to be passed as data from the playerscript
        Message msg = Message.Create(MessageSendMode.Reliable, ServerToClientMsg.UpdateGameData);
        msg.AddString(name);
        networkmanager.Instance.Server.Send(msg, uid);
    }

    #endregion



    #region OLD CODE
    /*

    //Run through the room's deck and send cards to each user client
    public void DistributeCards()
    {
        foreach(Transform child in m_PlayerRoom)
        {
            if (roomDeck == null)
                roomDeck = m_ServerRoom.transform.Find("SkillDeck").GetComponent<SkillDeck>().deck.ToList();
            player playscript = child.GetComponent<player>();
            int hand = playscript.Handsize;
            ushort id = playscript.Id;
            List<int> cards = new List<int>();
            for (int i = hand; i < 5; i++)
            {
                Debug.Log($"The next card is {roomDeck.First()}");
                cards.Add(roomDeck.First());
                activeDeck.Add(roomDeck.First());
                roomDeck.RemoveAt(0);
                Debug.Log($"Our count is {cards.Count}");
            }
            Debug.Log($"We got {cards.Count} cards");
            SendPlayerCards(cards, id);
        }
    }

    //When all signals are received, begin calculating the results
    public void FindTheWinner()
    {
            Debug.Log("Ready To Start");
            CompleteRound();
            playerTurn++;
    }

    //This will be used to see if the Update method is initiated on all of the clients at the same time
    public void TryIterating()
    {
        if(canIterate)
        {
            int counted = m_PlayerRoom.childCount;
            for(int i = counted; i > 0; i--)
            {
                player playscript = m_PlayerRoom.transform.GetChild(i - 1).gameObject.GetComponent<player>();
                if(!playscript.isUpdated)
                {
                    break;
                }

                if(i == 1)
                {
                    Debug.Log("Ready To Iterate");
                    IterateUserInfo();
                    managerupdate = false;
                    canIterate = false;
                    break;
                }
            }
        }
    }
    
    */
    #endregion
}