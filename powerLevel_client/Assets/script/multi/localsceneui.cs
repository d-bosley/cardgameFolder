using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class localsceneui : MonoBehaviour
{
    [SerializeField] private List<ButtonComponent> m_Buttons = new List<ButtonComponent>();
    [SerializeField] private List<InputComponent> m_Inputs = new List<InputComponent>();
    private Dictionary<string, uicomponent> m_components = new Dictionary<string, uicomponent>();
    public Dictionary<string, uicomponent> Components => m_components;
    
    private void Start()
    {
        Init();
    }

    private void Init()
    {
        uimanager.Instance.LocalUI = this;
        foreach(var btn in m_Buttons)
            m_components.Add(btn.Key, btn);
        foreach(var input in m_Inputs)
            m_components.Add(input.Key, input);
    }
}
