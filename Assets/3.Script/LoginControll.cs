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
            Log.text = "아이디 혹은 비밀번호를 입력하세요";
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
                Log.text = "아이디와 비밀번호를 확인해 주세요.";
            }
        }
    }
    public void Signup_Btn()
    {
        gameObject.SetActive(false);
        SignUp.SetActive(true);
    }
}
