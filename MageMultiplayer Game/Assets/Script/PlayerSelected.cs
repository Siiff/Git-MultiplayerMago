using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSelected : MonoBehaviour
{
    void LoadPlayerScene(int value)
    {
        PlayerPrefs.SetInt("MAGO", value);
        SceneManager.LoadScene("SampleScene");
    }
    public void Btn1()
    {
        LoadPlayerScene(0);
    }
    public void Btn2()
    {
         LoadPlayerScene(1);
    }
    public void Btn3()
    {
        LoadPlayerScene(2);
    }
    public void Btn4()
    {
        LoadPlayerScene(3);
    }

}
