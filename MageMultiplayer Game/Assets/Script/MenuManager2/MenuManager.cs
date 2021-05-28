using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class MenuManager : MonoBehaviourPunCallbacks
{
    public GameObject canvasSearch;
    public GameObject canvasWaiting;
    public GameObject canvasCountdown;
    public GameObject canvasLoading;
    public Text LobbyName;
    public GameObject lobbyPreFab;
    public GameObject Player1Img;
    public GameObject Player2Img;

    public GameObject player;
    public Transform[] spawnPosition;

    int spawnPositionUsed;

    void Start()
    {
        PanelControler(canvasSearch.name);

        PhotonNetwork.SendRate = 25;
        PhotonNetwork.SerializationRate = 15;
    }

    void PanelControler(string activePanel)
    {
        canvasSearch.SetActive(activePanel.Equals(canvasSearch.name));
        canvasWaiting.SetActive(activePanel.Equals(canvasWaiting.name));
        canvasCountdown.SetActive(activePanel.Equals(canvasCountdown.name));
        canvasLoading.SetActive(activePanel.Equals(canvasLoading.name));
    }
    public void LeftRoom()
    {
        Debug.Log("DESCONECTADO");
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.Disconnect();
    }

    public void ButtonShearch()
    {
        PanelControler(canvasLoading.name);
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        RoomOptions roomOptions = new RoomOptions();

        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 2;

        string roomName = "Sala: " + Random.Range(1, 10);               
        LobbyName.text = roomName;
        LobbyName.gameObject.SetActive(true);

        PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        PanelControler(canvasWaiting.name);
        Debug.LogWarning("FOI PORRA");

        //SE TIVER 1 PLAYER, ENTÃO O OUTRO VAI SPAWNAR EM OUTRO LUGAR//
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Debug.LogWarning("SPAWN PLAYER 1");
            Player1Img.gameObject.SetActive(true);
            spawnPositionUsed = 0;
        }
        else
        {
            Debug.LogWarning("SPAWN PLAYER 2");
            Player1Img.gameObject.SetActive(true);
            Player2Img.gameObject.SetActive(true);
            spawnPositionUsed = 1;
        }
    }

    public void OnPlayerEnteredRoom(Player newPlayer)
    {

        Hashtable props = new Hashtable
        {
            { CountdownTimer.CountdownStartTime, (float)PhotonNetwork.Time }
        };
        PhotonNetwork.CurrentRoom.SetCustomProperties(props);
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        Debug.Log("OnRoomPropertiesUpdate");
        PanelControler(canvasCountdown.name);
    }

    void CountdownAction()
    {
        PanelControler("");        
        Debug.LogWarning("COUNTDOWNACTION");
        lobbyPreFab.gameObject.SetActive(false);
        PhotonNetwork.Instantiate(player.name, spawnPosition[spawnPositionUsed].position, spawnPosition[spawnPositionUsed].rotation);
    }

    public override void OnEnable()
    {
        base.OnEnable();
        Debug.LogWarning("ENABLED");
        CountdownTimer.OnCountdownTimerHasExpired += CountdownAction;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        Debug.LogWarning("DISABLED");
        CountdownTimer.OnCountdownTimerHasExpired -= CountdownAction;
    }



}