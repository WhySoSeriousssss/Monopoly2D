using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NDebugPanel : MonoBehaviour {

    [SerializeField]
    Text currentTurn;
    [SerializeField]
    Text info;

    void Update()
    {
        /*
        currentTurn.text = NGameplay.currentPlayerOrder.ToString();
        if (PhotonNetwork.inRoom)
        {
            info.text = "";
            RoomPlayer[] players = FindObjectsOfType<RoomPlayer>();
            foreach (RoomPlayer p in players)
            {
                info.text += p.PhotonPlayer.NickName + p.GamePlayer.HasFinished.ToString() + '\n';
            }
        }
        */
    }
}
