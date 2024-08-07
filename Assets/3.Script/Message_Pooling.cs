using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Message_Pooling : MonoBehaviour
{
    [SerializeField] private Text[] message_Box;

    private string current_me = string.Empty;
    private string past_me;

    public Action<string> Message;
    public void Adding_Message(string me)
    {
        current_me = me;
    }

    private void Start()
    {
        message_Box = transform.GetComponentsInChildren<Text>();
        Message = Adding_Message;
        past_me = current_me;
    }

    private void Update()
    {
        if (past_me.Equals(current_me))
            return;

        ReadText(current_me);
        past_me = current_me;
    }
    private void ReadText(string me)
    {
        bool isInput = false;
        for(int i = 0; i < message_Box.Length; i++)
        {
            if(message_Box[i].text.Equals(""))
            {
                message_Box[i].text = me;
                isInput = true;
                break;
            }
        }
        if(!isInput)
        {
            for(int i = 1; i<message_Box.Length; i++)
            {
                message_Box[i - 1].text = message_Box[i].text;
                //메세지를 미는 작업
            }
            message_Box[message_Box.Length - 1].text = me;
        }
    }
}
