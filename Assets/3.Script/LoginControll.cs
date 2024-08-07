using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginControll : MonoBehaviour
{
    public InputField ID_input;
    public InputField Password_input;

    [SerializeField] private Text Log;
    [SerializeField] private GameObject SignUp;
    public void Login_Btn()
    {
        if(ID_input.text.Equals(string.Empty) || Password_input.Equals(string.Empty))
        {
            Log.text = "���̵� Ȥ�� ��й�ȣ�� �Է��ϼ���";
            return;
        }
        else
        {
            if(SQL_Manager.instance.Login(ID_input.text, Password_input.text))
            {
                User_info info = SQL_Manager.instance.info;
                Debug.Log(info.User_Name + " | " + info.User_Password + " | " + info.User_PhoneNum);
                gameObject.SetActive(false);
            }
            else
            {
                Log.text = "���̵�� ��й�ȣ�� Ȯ���� �ּ���.";
            }
        }
    }
    public void Signup_Btn()
    {
        gameObject.SetActive(false);
        SignUp.SetActive(true);
    }
}
