using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageBoxHandler : MonoBehaviour
{
    public GameObject MessageBoxPanel;
    public TMP_Text MessageText;
    Action currentFinishedCallback { get; set; }

    public void ShowMessage(string message, Action finishedCallback)
    {
        MessageBoxPanel.SetActive(true);
        MessageText.text = message;
        currentFinishedCallback = finishedCallback;
    }

    public void Close()
    {
        MessageBoxPanel.SetActive(false);
    }

    public void Progress()
    {
        if (currentFinishedCallback == null)
        {
            Debug.LogWarning("No finished callback currently set on MessageBoxHandler. May have double submitted Progress.");
            return;
        }

        MessageBoxPanel.SetActive(false);
        currentFinishedCallback();
        currentFinishedCallback = null;
    }
}
