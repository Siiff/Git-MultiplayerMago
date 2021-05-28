using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class ScriptDosMenus : MonoBehaviour
{
    public void MENUSTART()
    {
        SceneManager.LoadScene("MENUSTART");
    }
    public void SELECTMAGE()
    {
        SceneManager.LoadScene("PlayerSelected");
    }
    public void RESTART()
    {
        PhotonNetwork.Disconnect();
        Time.timeScale = 1;
        SceneManager.LoadScene("MENUSTART");
    }    
    public void EXIT()
    {
        Application.Quit();
    }

}
