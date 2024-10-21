using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelChildren : MonoBehaviour
{
    int childCount;
    Transform panelObject;

    // Start is called before the first frame update
    void Start()
    {
        panelObject = GetComponent<Transform>();
        childCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DestroyCards()
    {
        childCount = transform.childCount;

        for (int i = 0; i < childCount; i++)
            {
                Destroy(panelObject.transform.GetChild(0).gameObject);
            }
    }
}
