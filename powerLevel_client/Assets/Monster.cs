using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Monster : MonoBehaviour
{
    [HideInInspector] public bool onHover;
    [HideInInspector] public bool clicked;
    public bool phaseOK;
    Color lerpedColor = Color.white;
    SpriteRenderer renderer;
    int health;
    int bleed;
    public TextMeshProUGUI healthDisplay;
    string healthText;
    public TextMeshProUGUI bleedDisplay;
    string bleedText;
    GamePhase phase;

    // Start is called before the first frame update
    void Start()
    {
        phase = GameObject.Find("GamePhaseManager").GetComponent<GamePhase>();
        renderer = GetComponent<SpriteRenderer>();
        onHover = false;
        clicked = false;
        phaseOK = false;
        health = 30;
    }

    // Update is called once per frame
    void Update()
    {
        if(phase.currentphase == "MainPhase1")
        {
            phaseOK = true;
        }
        else if(phase.currentphase != "MainPhase1")
        {
            phaseOK = false;
        }

        Highlight(phaseOK);

        if(clicked)
        {
            PlayerAttack();
            MonsterAttack();
        }
        
        healthText = "BOSS HEALTH: \n" + health;
        healthDisplay.text = healthText;
        bleedText = "PLAYER BLEED: \n" + bleed;
        bleedDisplay.text = bleedText;
    }

    void Highlight(bool mouseOver)
    {
        renderer.color = mouseOver ? Color.Lerp(Color.white, new Color(1f, .15f, 1f, 1f), Mathf.PingPong(Time.time, 1f)) : Color.Lerp(renderer.color, Color.white, .15f);
    }

    void PlayerAttack()
    {
        GameObject store = GameObject.Find("StoreArea");
        int childCount = store.transform.childCount;
        if(childCount > 0)
        {
            for (int i = childCount; i > 0; i--)
            {
                Attack childCard = store.transform.GetChild(i - 1).gameObject.GetComponent<Attack>();
                if(childCard.cardType == "ATTACK")
                {
                    health -= childCard.damage;
                    Destroy(store.transform.GetChild(i - 1).gameObject);
                }
            }
        }
    }

    void MonsterAttack()
    {
        GameObject store = GameObject.Find("StoreArea");
        int childCount = store.transform.childCount;
        bool defended = false;
        int hitPoint = 1;
        
        if(childCount > 0)
        {
            for (int i = childCount; i > 0; i--)
            {
                Attack childCard = store.transform.GetChild(i - 1).gameObject.GetComponent<Attack>();
                if(childCard.ability == "Buffing" || childCard.ability == "Protecting")
                {
                    defended = true;
                    break;
                }
                
                else if(childCard.ability == "Healing" || childCard.ability == "Effecting")
                {
                    bleed = 0;
                    //break;
                }

                else
                {
                    defended = false;
                }
            }
        }
        
        if(!defended)
        {
            bleed += hitPoint;
            hitPoint = 0;
            return;
        }
    }

    public void ResetStats()
    {
        bleed = 0;
        health = 30;
    }

    void OnMouseOver()
    {
        //If your mouse hovers over the GameObject with the script attached, output this message
        if (Input.GetMouseButtonDown(0) && phase.currentphase == "MainPhase1")
        {
            clicked = true;
        }

        else if(!Input.GetMouseButton(0))
        {
            clicked = false;
        }

    }

    void OnMouseEnter()
    {
        //If your mouse hovers over the GameObject with the script attached, output this message
        onHover = true;
    }

    void OnMouseExit()
    {
        //The mouse is no longer hovering over the GameObject so output this message each frame
        onHover = false;
    }
}
