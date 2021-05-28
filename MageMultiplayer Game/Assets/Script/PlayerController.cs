using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine.UI;

public class PlayerController : MonoBehaviourPun
{
    float frente;
    float girar;
    public PhotonView photonview;
    public Camera myCamera;

    [Header("VIDA")]
    public Image playerHealthFill;
    public Text playerName;
    public float playerHealthMax = 100f;
    public float playerHealthCurrent = 0f;

    [Header("BALA")]
    public GameObject bullet;
    public GameObject bulletPhotonView;
    public GameObject spawnBullet;
    public float tempoAtaque;
    private bool ataque;

    #region Metodos da Unity
    void Start()
    {
        photonview = GetComponent<PhotonView>();

        if(!photonView.IsMine)
        {
            myCamera.gameObject.SetActive(false);
        }

        Debug.LogWarning("Name: " + PhotonNetwork.NickName + " PhotonView: " + photonview.IsMine);
        frente = 20;
        girar = 90;
        playerName.text = PhotonNetwork.NickName;
        HealthManager(playerHealthMax);
    }
    void Update()
    {        
        if (photonview.IsMine)
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(0, 0, (frente * Time.deltaTime));
            }

            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(0, 0, (-frente * Time.deltaTime));
            }

            if (Input.GetKey(KeyCode.A))
            {
                transform.Rotate(0, (-girar * Time.deltaTime), 0);
            }

            if (Input.GetKey(KeyCode.D))
            {
                transform.Rotate(0, (girar * Time.deltaTime), 0);
            }

            Shooting();
        }
        
    }
    #endregion

    #region Meus Metodos

    void HealthManager(float value)
    {
        Debug.LogWarning("HealthManager");
        Debug.LogWarning("value= "+ value);
            playerHealthCurrent += value;
            playerHealthFill.fillAmount = playerHealthCurrent / 100;
        
    }

    public void TakeDamage( float value)
    {
        Debug.LogWarning("TakeDamage");
        Debug.LogWarning("value= " + value);
        photonView.RPC("TakeDamageNetwork", RpcTarget.AllBuffered, value);
    }

    [PunRPC]
    void TakeDamageNetwork(float value)
    {
        Debug.LogWarning("TakeDamageNetwork");
        HealthManager(value);
    }

    void Shooting()
    {
        if (Input.GetMouseButtonDown(0) && ataque)
        {
                photonView.RPC("Shoot", RpcTarget.All);
                ataque = false;
        }
        else
        {
            if (tempoAtaque <= 0)
            {
                ataque = true;
                tempoAtaque = 1.5f;
            }
        }
        tempoAtaque -= Time.deltaTime;
        if (Input.GetMouseButtonDown(1) && ataque)
        {
            PhotonNetwork.Instantiate(bulletPhotonView.name, spawnBullet.transform.position, spawnBullet.transform.rotation);
        }
        else
        {
            if (tempoAtaque <= 0)
            {
                ataque = true;
                tempoAtaque = 1.5f;
            }
        }
    }

    [PunRPC]
    void Shoot()
    {
        Instantiate(bullet, spawnBullet.transform.position, spawnBullet.transform.rotation);
    }

    #endregion
}
