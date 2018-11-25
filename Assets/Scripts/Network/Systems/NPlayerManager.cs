using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPlayerManager : Photon.PunBehaviour {

    #region Singleton
    public static NPlayerManager instance;
    private void Awake()
    {
        if (instance != null)
            Debug.Log("Multiple NPlayerManager. Something went wrong");
        instance = this;
    }
    #endregion

    List<NPlayer> _gamePlayers = new List<NPlayer>();


    private void Start()
    {
        _gamePlayers = new List<NPlayer>(FindObjectsOfType<NPlayer>());
    }

    public void StartTurn()
    {
        photonView.RPC("RPC_ChangeFinishStatus", PhotonTargets.MasterClient, false, PhotonNetwork.player);
    }
    public void FinishTurn()
    {
        photonView.RPC("RPC_ChangeFinishStatus", PhotonTargets.MasterClient, true, PhotonNetwork.player);
    }
    [PunRPC]
    public void RPC_ChangeFinishStatus(bool hasFinished, PhotonPlayer caller)
    {
        if (!PhotonNetwork.isMasterClient)
            return;
        photonView.RPC("RPC_ReceiveFinishStatus", PhotonTargets.All, hasFinished, caller);
    }
    [PunRPC]
    public void RPC_ReceiveFinishStatus(bool hasFinished, PhotonPlayer caller)
    {
        NPlayer player = _gamePlayers.Find(x => caller == x.photonView.owner);
        player.HasFinished = hasFinished;
    }


    public void RollDice()
    {
        photonView.RPC("RPC_RollDice", PhotonTargets.MasterClient, PhotonNetwork.player);
    }
    [PunRPC]
    public void RPC_RollDice(PhotonPlayer caller)
    {
        if (!PhotonNetwork.isMasterClient)
            return;
        int dice1 = Random.Range(1, 7);
        int dice2 = Random.Range(1, 7);
        int step = dice1 + dice2;

        photonView.RPC("RPC_ReceiveDice", PhotonTargets.All, step, caller);
    }
    [PunRPC]
    public void RPC_ReceiveDice(int step, PhotonPlayer caller)
    {
        NPlayer player = _gamePlayers.Find(x => caller == x.photonView.owner);
        StartCoroutine(player.Move(step));
    }



    public void TryToBuyProperty(NProperty property)
    {
        photonView.RPC("RPC_TryToBuyProperty", PhotonTargets.MasterClient, property.PropertyID, PhotonNetwork.player);
    }

    [PunRPC]
    public void RPC_TryToBuyProperty(int propertyID, PhotonPlayer caller)
    {
        if (!PhotonNetwork.isMasterClient)
            return;
        NPlayer player = _gamePlayers.Find(x => caller == x.photonView.owner);
        NProperty property = NBoardManager.instance.Properties[propertyID];

        property.Owner = player;
        NBoardManager.instance.PropertySoldToPlayer(propertyID, player.Order);

        if (player.CurrentMoney > property.PurchasePrice)
        {
            player.ChangeMoney(-property.PurchasePrice);
            NPlayer.thisPlayer.ObtainProperty(property);
        }
    }

    public void AuctionProperty(NProperty property)
    {

    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }
}
