using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GamePhase : MonoBehaviour
{
    string[] phases = {"DrawPhase", "MainPhase1", "BattlePhase", "MainPhase2"};
    [HideInInspector] public string currentphase;
    [HideInInspector] public bool turnStart;
    [HideInInspector] public bool turnEnd;
    public TextMeshProUGUI phaseText;

    // Start is called before the first frame update
    void Start()
    {
        turnStart = true;
        turnEnd = false;
        currentphase = phases[0];
        phaseText.text = currentphase;
    }

    // Update is called once per frame
    void Update()
    {
        TurnOrder();
    }

    public void TurnOrder()
    {
        if(turnStart){currentphase = phases[0];}
        DeckDraw deck = GameObject.Find("ActionDeck").GetComponent<DeckDraw>();
        if(deck.clicked && turnStart){currentphase = phases[1]; turnStart = false;}
        Monster monster = GameObject.Find("MonsterCard").GetComponent<Monster>();
        if(monster.clicked && !turnStart){currentphase = phases[2]; turnEnd = true;}
        if(turnEnd){currentphase = phases[3];}
        phaseText.text = currentphase;
    }

    public void StartTurn()
    {
        turnStart = true;
    }
}