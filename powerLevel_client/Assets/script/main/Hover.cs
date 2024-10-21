using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour
{
    Vector2 sourceDestination;
    Vector2 sourceCollider;
    public Collider2D triggerBox;
    public Click click;
    bool onHover;
    bool isClicked;

    // Start is called before the first frame update
    void Start()
    {
        sourceDestination = transform.position;
        sourceCollider = triggerBox.offset;
        onHover = false;
    }

    // Update is called once per frame
    void Update()
    {
        PositionOffset(onHover);     
        isClicked = click.isBig;
    }

    void PositionOffset(bool mouseOver)
    {
        Vector2 offsetVector = new Vector2(0, 2);
        //transform.position = mouseOver ? Vector2.Lerp(transform.position, sourceDestination + offsetVector, .15f) : Vector2.Lerp(transform.position, sourceDestination, .15f);
        //triggerBox.offset = mouseOver ? sourceCollider - offsetTrigger : sourceCollider;
    }

    void OnMouseEnter()
    {
        //If your mouse hovers over the GameObject with the script attached, output this message
        onHover = isClicked ? false : true;
    }

    void OnMouseExit()
    {
        //The mouse is no longer hovering over the GameObject so output this message each frame
        onHover = false;
    }
}
