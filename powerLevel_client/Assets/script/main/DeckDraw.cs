using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckDraw : MonoBehaviour
{
    public GameObject cardPrefab;
    [HideInInspector] public GameObject spawnPrefab;
    public string targetString;
    [HideInInspector] public bool onHover;
    [HideInInspector] public bool clicked;
    public bool phaseOK;
    Color lerpedColor = Color.white;
    SpriteRenderer renderer;
    Target target;
    GamePhase phase;

    // Start is called before the first frame update
    void Start()
    {
        phase = GameObject.Find("GamePhaseManager").GetComponent<GamePhase>();
        renderer = GetComponent<SpriteRenderer>();
        onHover = false;
        clicked = false;
        phaseOK = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(phase.currentphase == "DrawPhase")
        {
            phaseOK = true;
        }
        else if(phase.currentphase != "DrawPhase")
        {
            phaseOK = false;
        }

        Highlight(phaseOK);
    }

    void Highlight(bool mouseOver)
    {
        renderer.color = mouseOver ? Color.Lerp(Color.white, new Color(1f, .15f, 1f, 1f), Mathf.PingPong(Time.time, 1f)) : Color.Lerp(renderer.color, Color.white, .15f);
    }

    void OnMouseOver()
    {
        //If your mouse hovers over the GameObject with the script attached, output this message
        if (Input.GetMouseButtonDown(0) && phase.currentphase == "DrawPhase")
        {
            spawnPrefab = GameObject.Find(targetString);
            Instantiate(cardPrefab, transform.position, Quaternion.identity, spawnPrefab.transform);
            target = cardPrefab.GetComponent<Target>();
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
