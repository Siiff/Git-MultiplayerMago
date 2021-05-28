using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSelected : MonoBehaviour
{
    public int playernumber;
    //public SelectPlayer selectPlayer;

    void LoadPlayerScene(int value)
    {
        PlayerPrefs.SetInt("MAGO", value);
        playernumber = PlayerPrefs.GetInt("MAGO");
        Debug.LogError("Meu jogador é: " + playernumber);
        SceneManager.LoadScene("SampleScene");
    }
    public void Btn1()
    {
        LoadPlayerScene(0);
        Debug.LogWarning("NUMERO DO JOGADOR É: " + playernumber);
    }
    public void Btn2()
    {
        /*selectPlayer.ButtonRight();*/
        LoadPlayerScene(1);
        Debug.LogWarning("NUMERO DO JOGADOR É: " + playernumber);
    }
    public void Btn3()
    {
        /*selectPlayer.ButtonRight();
        selectPlayer.ButtonRight();*/
        LoadPlayerScene(2); 
        Debug.LogWarning("NUMERO DO JOGADOR É: " + playernumber);
    }
    public void Btn4()
    {
        /*selectPlayer.ButtonRight();
        selectPlayer.ButtonRight();
        selectPlayer.ButtonRight();*/
        LoadPlayerScene(3);
        Debug.LogWarning("NUMERO DO JOGADOR É: " + playernumber);
    }

}
