using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Message_Pooling : MonoBehaviour
{
    [SerializeField] private Text[] messageBox;

    private string current_m = string.Empty;
    private string past_m;

    public Action<string> Message;
    public void AddingMessage(string message)
    {
        current_m = message;
    }

    private void Start()
    {
        messageBox = transform.GetComponentsInChildren<Text>();
        Message = AddingMessage;
        past_m = current_m;
    }

    private void Update()
    {
        if (past_m.Equals(current_m))
            return;

        ReadText(current_m);
        past_m = current_m;
    }
    private void ReadText(string message)
    {
        bool isInput = false;
        for(int i = 0; i < messageBox.Length; i++)
        {
            if(messageBox[i].text.Equals(""))
            {
                messageBox[i].text = message;
                isInput = true;
                break;
            }
        }
        if(!isInput)
        {
            for(int i = 1; i<messageBox.Length; i++)
            {
                messageBox[i - 1].text = messageBox[i].text;
                //메세지를 미는 작업
            }
            messageBox[messageBox.Length - 1].text = message;
        }
    }
}
