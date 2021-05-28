using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class SelectPlayer : MonoBehaviour
{
    public int playerSelected = 0;
    public GameObject playerList;
    public Image playerIconCanvas;

    public GameObject myplayerCanvas;
    PhotonView photonView;

    private void Start()
    {
        photonView = this.GetComponent<PhotonView>();

        if (!photonView.IsMine)
        {
            myplayerCanvas.gameObject.SetActive(false);
        }
        playerSelected = PlayerPrefs.GetInt("MAGO");
        SwitchPlayer();
    }

    public void SwitchPlayer()
    {
        Debug.LogWarning("Entrou no SwitchPlayer");
        Debug.Log("PlayerSelected é: "+playerSelected);
        //RodandoOSelect();
        photonView.RPC("SwitchPlayerRPC", RpcTarget.AllBuffered);
    }

    /*public void RodandoOSelect()
    {
        for (int i = 0; i < playerSelected; i++)
        {
            Debug.LogError("RODANDO");
            ButtonRightRPC();
        }
    }*/
    public void ButtonRight()
    {
        photonView.RPC("ButtonRightRPC", RpcTarget.AllBuffered);
    }

    public void ButtonLeft()
    {
        photonView.RPC("ButtonLeftRPC", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void SwitchPlayerRPC()
    {
        int i = 0;

        foreach (Transform item in playerList.transform)
        {
            if (i == playerSelected)
            {
                item.gameObject.SetActive(true);

                if (item.gameObject.GetComponent<PlayerConfig>())
                {
                    playerIconCanvas.sprite = item.gameObject.GetComponent<PlayerConfig>().playerIcon;
                }
            }
            else
            {
                item.gameObject.SetActive(false);
            }
            i++;
        }
    }

    [PunRPC]
    public void ButtonRightRPC()
    {
        playerSelected++;

        if (playerSelected > (playerList.transform.childCount - 1))
        {
            playerSelected = 0;
        }
        SwitchPlayer();
    }

    [PunRPC]
    public void ButtonLeftRPC()
    {
        playerSelected--;

        if (playerSelected < 0)
        {
            playerSelected = playerList.transform.childCount - 1;
        }
        SwitchPlayer();
    }
}
