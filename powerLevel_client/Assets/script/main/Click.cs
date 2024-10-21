using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Click : MonoBehaviour
{
    float height;
    float width;
    Vector2 sourceDestination;
    Vector2 sourceCollider;
    Vector2 sourcePosition;
    Vector2 sourceScale;
    public Collider2D triggerBox;
    public GameObject text;
    public GameObject box;
    public Target target;
    [HideInInspector] public bool isBig;
    [HideInInspector] public bool onHover;


    // Start is called before the first frame update
    void Start()
    {
        sourceDestination = target.targetPoint;
        sourceCollider = triggerBox.offset;
        sourceScale = transform.lossyScale;
        height = Camera.main.orthographicSize * 2;
        width = height * Screen.width/Screen.height;
        text.SetActive(false);
        box.SetActive(false);
        isBig = false;
        onHover = false;
    }

    // Update is called once per frame
    void Update()
    {
        target.StartSpawnTarget();
        ScreenScale(isBig);
        PositionOffset(onHover);
        UpdateSourceDestination();
        text.SetActive(isBig);
        box.SetActive(isBig);
    }

    void ScreenScale(bool isScaled)
    {
        Vector2 targetPosition = Vector2.zero;
        Vector2 offsetVector = new Vector2(-16, 4);
        transform.position = isScaled ? Vector2.Lerp(transform.position, targetPosition + offsetVector, .15f) : Vector2.Lerp(transform.position, sourceDestination, .15f);
        transform.localScale = isScaled ? Vector2.Lerp(transform.localScale, Vector2.one * height / 24, .15f) : Vector2.Lerp(transform.localScale, sourceScale, .15f);
        //transform.position = isScaled ? targetPosition : sourceDestination;
    }
    void PositionOffset(bool mouseOver)
    {
        Vector2 offsetVector = new Vector2(0, 2);
        transform.position = mouseOver ? Vector2.Lerp(transform.position, sourceDestination + offsetVector, .15f) : Vector2.Lerp(transform.position, sourceDestination, .15f);
        //triggerBox.offset = mouseOver ? sourceCollider - offsetTrigger : sourceCollider;
    }

    void UpdateSourceDestination()
    {
        if(!isBig)
        {
            sourceDestination = target.targetPoint;
        }
    }
    
    void OnMouseOver()
    {
        //If your mouse hovers over the GameObject with the script attached, output this message
        if (Input.GetMouseButtonDown(0) && !isBig)
        {
            isBig = true;
        }
        else if (Input.GetMouseButtonDown(0) && isBig)
        {
            isBig = false;
        }

    }

    void OnMouseEnter()
    {
        //If your mouse hovers over the GameObject with the script attached, output this message
        onHover = isBig ? false : true;
    }

    void OnMouseExit()
    {
        //The mouse is no longer hovering over the GameObject so output this message each frame
        onHover = false;
    }

}
