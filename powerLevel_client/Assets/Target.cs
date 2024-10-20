using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [HideInInspector] public Vector2 targetPoint;
    [HideInInspector] public GameObject spawnTarget;
    public string targetString;
    Click click;
    bool targetReached;

    // Start is called before the first frame update
    void Start()
    {
        click = GetComponent<Click>();
        targetReached = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartSpawnTarget()
    {
        if(!targetReached)
        {
        spawnTarget = GameObject.Find(targetString);
        targetPoint = new Vector2(spawnTarget.transform.position.x, spawnTarget.transform.position.y);
        //transform.position = targetPoint;
        transform.position = Vector2.Lerp(transform.position, targetPoint, .15f);
        //Vector2.Lerp(transform.position, targetPoint, .15f);
        targetReached = true;
        }
    }
    public void StoreSpawnTarget()
    {
        spawnTarget = GameObject.Find("StoreArea");
        if(spawnTarget.transform.childCount < 2)
        {
            this.transform.parent = spawnTarget.transform;
            targetPoint = new Vector2(spawnTarget.transform.position.x, spawnTarget.transform.position.y);
            transform.position = targetPoint;
            click.isBig = false;
        }
    }
}
