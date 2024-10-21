using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardLimits : MonoBehaviour
{
    int childCount;
    Transform handObject;

    // Start is called before the first frame update
    void Start()
    {
        handObject = GetComponent<Transform>();
        childCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        childCount = transform.childCount;

        if (childCount > 0)
        {
            for (int i = 0; i < childCount; i++)
            {
                Transform childObject = handObject.transform.GetChild(i);
            }

            if (childCount > 1)
            {
                Destroy(handObject.transform.GetChild(0).gameObject);
            }
        }
    }

    public void DestroyHand()
    {
        childCount = transform.childCount;

        for (int i = 0; i < childCount; i++)
            {
                Destroy(handObject.transform.GetChild(0).gameObject);
            }
    }
}
