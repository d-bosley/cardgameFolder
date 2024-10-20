using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Vector2 mainPosition;
    public float floatingTime;

    // Start is called before the first frame update
    void Start()
    {
        mainPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        CardMovement();
    }

    void CardMovement()
    {
        float transx = mainPosition.x;
        float transy = mainPosition.y;
        float valuex = transx + Mathf.PingPong(floatingTime * Time.time, .01f);
        float valuey = transy + Mathf.PingPong(floatingTime * Time.time, .075f);
        Vector2 moveValue = new Vector2(valuex, valuey);
        transform.localPosition = Vector2.Lerp(transform.localPosition, moveValue, .15f);
        //transform.position = moveValue;
    }
}
