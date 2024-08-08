using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeInfoControll : MonoBehaviour
{
    public Text ID_Text;
    public InputField Password_input;
    public InputField Phone_input;

    [SerializeField] private Text Log;
    public void ChangeInfo_Btn()
    {
        User_info info = SQL_Manager.instance.info;
        ID_Text.text = info.User_Name;
        gameObject.SetActive(true);
    }
    public void Change_Btn()
    {
        if(SQL_Manager.instance.UpdateUserData(ID_Text.text, Password_input.text, Phone_input.text))
        {
            gameObject.SetActive(false);
        }
        else
        {
            Log.text = "수정 실패";
        }
    }
    public void Close_Btn()
    {
        gameObject.SetActive(false);
    }
}
