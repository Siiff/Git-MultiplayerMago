﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine.UI;

public class PlayerController : MonoBehaviourPun
{
    [Header("PLAYER MOV")]
    float playerSpeed;
    Rigidbody rb;
    float frente;
    float girar;
    public PhotonView photonview;
    NetworkController _networkController;
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
        rb = GetComponent<Rigidbody>();
        _networkController = GameObject.Find("NetworkController").GetComponent<NetworkController>();

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
    private void FixedUpdate()
    {
        if (photonview.IsMine && Time.timeScale != 0)
        {
            Moving();
            Shooting();
            DebugHotkeys();
        }
    }
    void Update()
    {        
    }
    void Moving()
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
    }
    #endregion

    #region Meus Metodos

    void HealthManager(float value)
    {
        Debug.LogWarning("HealthManager");
        Debug.LogWarning("value= " + value);
            playerHealthCurrent += value;
            playerHealthFill.fillAmount = playerHealthCurrent / 100;

        IsDeadCheck();
    }

    public void TakeDamage( float value)
    {
        Debug.LogError("TOMEI DANO");
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
                tempoAtaque = 1f;
            }
        }
        if (Input.GetMouseButtonDown(1) && ataque)
        {
            PhotonNetwork.Instantiate(bulletPhotonView.name, spawnBullet.transform.position, spawnBullet.transform.rotation);
            PhotonNetwork.Instantiate(bulletPhotonView.name, spawnBullet.transform.position, spawnBullet.transform.rotation);
            ataque = false;
        }
        else
        {
            if (tempoAtaque <= 0)
            {
                ataque = true;
                tempoAtaque = 3.5f;
            }
        }
        tempoAtaque -= Time.deltaTime;
    }

    [PunRPC]
    void Shoot()
    {
        Instantiate(bullet, spawnBullet.transform.position, spawnBullet.transform.rotation);
    }

    [PunRPC]
    void DebugHotkeys()
    {
        //SELF DAMAGE
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.K))
        {
            Debug.LogWarning(">> SELF DAMAGE <<");
            TakeDamage(-10f);
        }
    }

    //[PunRPC]
    void IsDeadCheck()
    {
        if (playerHealthCurrent <= 0f)
        {
            string _perdedor = PhotonNetwork.NickName;
            string _vencedor = _perdedor;
                     
            if (PhotonNetwork.PlayerListOthers.Length != 0)
                _vencedor = PhotonNetwork.PlayerListOthers[0].NickName;

            if(!photonview.IsMine)
            {
                string temp = _perdedor;
                _perdedor = _vencedor;
                _vencedor = temp;
            }
            
            _networkController.OnFinish(_perdedor, _vencedor);
        }
    }

    #endregion
}