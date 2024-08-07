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
            Log.text = "���̵� �Է��ϼ���";
            return;
        }
        if (Password_input.Equals(string.Empty))
        {
            Log.text = "��й�ȣ�� �Է��ϼ���";
            return;
        }
        if (Phone_input.text.Equals(string.Empty))
        {
            Log.text = "��ȭ��ȣ�� �Է��ϼ���";
            return;
        }
        if (SQL_Manager.instance.SignUp(ID_input.text, Password_input.text, Phone_input.text))
        {
            gameObject.SetActive(false);
            Login.SetActive(true);
        }
    }
}
