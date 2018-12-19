using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NSelectTradeeDialog : MonoBehaviour {

    public GameObject tradeeButtonPrefab;
    public Transform playerList;

    public void Initialize(PhotonPlayer caller)
    {
        foreach (PhotonPlayer p in PhotonNetwork.playerList)
        {
            if (p != caller)
            {
                GameObject button = Instantiate(tradeeButtonPrefab);
                button.transform.SetParent(playerList, false);
                button.GetComponentInChildren<Text>().text = p.NickName;
                button.GetComponent<Button>().onClick.AddListener(delegate { OnTradeeButtonClicked(p); });
            }
        }
    }

    void OnTradeeButtonClicked(PhotonPlayer tradee)
    {
        NDialogManager.instance.CallTradingDialog(PhotonNetwork.player, tradee);
        Destroy(gameObject);
    }

    public void OnCancelButtonClicked()
    {
        Destroy(gameObject);
    }
}
