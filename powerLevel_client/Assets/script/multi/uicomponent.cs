using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class uicomponent
{
    public string Key;
}

[Serializable]
public class ButtonComponent : uicomponent
{
    public ButtonComponent Button;
}

[Serializable]
public class InputComponent : uicomponent
{
    public TMP_InputField input;
}
