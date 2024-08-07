using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeManager : MonoBehaviour
{
    public void ServerBtn()
    {
        SceneManager.LoadScene("ServerScene");
    }
    public void ClientBtn()
    {
        SceneManager.LoadScene("ClientScene");
    }
}
