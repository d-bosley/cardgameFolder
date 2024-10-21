using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerLevelNetwork.Core;

public class uimanager : singleton<uimanager>
{
    public localsceneui LocalUI;
    public void Connect()
    {
        if(LocalUI == null)
        {
            Debug.LogError("No local UI on this scene");
            return;
        }
        string connectInput = "ConnectInput";
        if(!LocalUI.Components.TryGetValue(connectInput, out uicomponent component))
        {
            Debug.LogError($"No input component found: {connectInput}");
            return;
        }
        InputComponent input = (InputComponent)component;
        string username = input.input.text;
        networkmanager.Instance.Connect(username);
    }
}
