using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public void EXIT()
    {
        Application.Quit();
    }
}
