using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignUpControll : MonoBehaviour
{
    public InputField ID_input;
    public InputField Password_input;
    public InputField Phone_input;

    [SerializeField] private Text Log;
    [SerializeField] private GameObject Login;
    public void Signup_Btn()
    {
        if (ID_input.text.Equals(string.Empty))
        {
            Log.text = "아이디를 입력하세요";
            return;
        }
        if (Password_input.Equals(string.Empty))
        {
            Log.text = "비밀번호를 입력하세요";
            return;
        }
        if (Phone_input.text.Equals(string.Empty))
        {
            Log.text = "전화번호를 입력하세요";
            return;
        }
        if (SQL_Manager.instance.SignUp(ID_input.text, Password_input.text, Phone_input.text))
        {
            gameObject.SetActive(false);
            Login.SetActive(true);
        }
    }
}
