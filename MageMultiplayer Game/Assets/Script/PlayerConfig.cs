using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConfig : MonoBehaviour
{
    public Sprite playerIcon;
    public PlayerData playerData;

    void Start()
    {
        playerIcon = playerData.playerIcon;
    }
}
