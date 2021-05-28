using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class NetworkController : MonoBehaviourPunCallbacks
{
    [Header("LOGIN")]
    public GameObject loginPn;
    public InputField playerNameInput;
    string tempPlayerName;

    [Space]
    [Header("LOBBY")]
    public GameObject lobbyPn;
    public InputField roomNameInput;
    public GameObject background;
    string tempRoomName;

    [Space]
    [Header("PLAYER")]
    public GameObject playerPUN;
    public GameObject mainCamera;

    [Space]
    [Header("Vitória")]
    public GameObject painelVitoria;
    public Text textVencedor;
    public Text textPerdedor;

    [Space]
    [Header("Menssagem na Tela")]
    public Text msgText;
    public string msgEntrada = " Entrou na Sala!";
    public string msgSaida = " Saiu da Sala!";


    void Start()
    {
        Debug.Log("Arma " + PlayerPrefs.GetInt("Arma"));
        //PlayerPrefs.DeleteAll();
        //PlayerPrefs.Save();
        Debug.Log("Arma " + PlayerPrefs.GetInt("Arma"));
        loginPn.gameObject.SetActive(true);
        lobbyPn.gameObject.SetActive(false);
        msgText.gameObject.SetActive(false);

        tempPlayerName = "Player" + Random.Range(1, 10);
        playerNameInput.text = tempPlayerName;

        tempRoomName = "Arena" + Random.Range(1, 10);
    }

    //######## Minhas Funções ##################
    public void Login()
    {
        PhotonNetwork.ConnectUsingSettings();

        if (playerNameInput.text != "")
        {
            PhotonNetwork.NickName = playerNameInput.text;
        }
        else
        {
            PhotonNetwork.NickName = tempPlayerName;
        }

        loginPn.gameObject.SetActive(false);
        lobbyPn.gameObject.SetActive(true);
        roomNameInput.text = tempRoomName;
    }

    public void QuickSearch()
    {
        PhotonNetwork.JoinLobby();
    }

    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions() { MaxPlayers = 2 };
        PhotonNetwork.JoinOrCreateRoom(roomNameInput.text, roomOptions, TypedLobby.Default);
    }

    public void ExibeMsg(string msg)
    {
        msgText.gameObject.SetActive(true);
        msgText.text = msg;
        StartCoroutine(WaitForHide(4f));
    }

    IEnumerator WaitForHide(float tempo)
    {
        yield return new WaitForSeconds(tempo);
        msgText.gameObject.SetActive(false);
    }

    //############# PUN Callbacks ##################
    public override void OnConnected()
    {
        Debug.LogWarning("############# LOGIN #############");
        Debug.LogWarning("OnConnected");
    }

    public override void OnConnectedToMaster()
    {
        Debug.LogWarning("OnConnectedToMaster");
        Debug.LogWarning("Server: " + PhotonNetwork.CloudRegion);
        Debug.LogWarning("Ping: " + PhotonNetwork.GetPing());
    }

    public override void OnJoinedLobby()
    {
        Debug.LogWarning("############# LOOBY #############");
        Debug.LogWarning("OnJoinedLobby");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(tempRoomName);
    }

    public override void OnJoinedRoom()
    {
        Debug.LogWarning("OnJoinedRoom");
        Debug.LogWarning("Nome da Sala: " + PhotonNetwork.CurrentRoom.Name);
        Debug.LogWarning("Nome da Player: " + PhotonNetwork.NickName);
        Debug.LogWarning("Players Conectados: " + PhotonNetwork.CurrentRoom.PlayerCount);

        /*if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Debug.LogWarning("SPAWN PLAYER 1");
            Time.timeScale = 0;
        }
        else
        {
            Debug.LogWarning("SPAWN PLAYER 2");
            Time.timeScale = 1;

        }*/

        loginPn.gameObject.SetActive(false);
        lobbyPn.gameObject.SetActive(false);
        background.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(false);

        Vector3 pos = new Vector3(Random.Range(-15, 15), playerPUN.transform.position.y + 2, Random.Range(-15, 15));

        PhotonNetwork.Instantiate(playerPUN.name, pos, playerPUN.transform.rotation, 0);
        //Debug.LogError("PLAYERPUN.NAME: " + playerPUN.name);
    }

    public override void OnPlayerEnteredRoom(Player p1)
    {
        ExibeMsg(p1.NickName + msgEntrada);
    }

    public override void OnPlayerLeftRoom(Player p1)
    {
        ExibeMsg(p1.NickName + msgSaida);
    }

    public void OnFinish(string perdedor, string vencedor)
    {
        //pausa o jogo
        Time.timeScale = 0;
        //atualiza nomes e chama tela de vitória
        textPerdedor.text = perdedor;
        textVencedor.text = vencedor;
        painelVitoria.SetActive(true);
    }

}
