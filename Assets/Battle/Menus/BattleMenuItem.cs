using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public delegate IEnumerator YieldableDelegate();

public class BattleMenuItem : MonoBehaviour
{
    public string ButtonTextLabel
    {
        get
        {
            return _buttonTextLabel;
        }
        set
        {
            if (ButtonText)
            {
                ButtonText.text = value;
            }
            
            _buttonTextLabel = value;
        }
    }
    [SerializeField]
    private string _buttonTextLabel;

    public TMP_Text ButtonText;
    public Button ButtonInstance;

    private void Awake()
    {
        if (ButtonText)
        {
            ButtonText.text = ButtonTextLabel;
        }
    }

    private void OnValidate()
    {
        if (ButtonText)
        {
            ButtonText.text = ButtonTextLabel;
        }
    }
}
