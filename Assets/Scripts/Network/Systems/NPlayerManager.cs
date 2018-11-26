using System;
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
        int dice1 = UnityEngine.Random.Range(1, 7);
        int dice2 = UnityEngine.Random.Range(1, 7);
        int step = dice1 + dice2;

        photonView.RPC("RPC_ReceiveDice", PhotonTargets.All, step, caller);
    }

    [PunRPC]
    public void RPC_ReceiveDice(int step, PhotonPlayer caller)
    {
        NPlayer player = _gamePlayers.Find(x => caller == x.photonView.owner);
        StartCoroutine(player.Move(step));
    }


    public void RollDiceManual(int dice)
    {
        photonView.RPC("RPC_ReceiveDice", PhotonTargets.All, dice, PhotonNetwork.player);
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

        if (player.CurrentMoney > property.PurchasePrice)
        {
            player.ChangeMoney(-property.PurchasePrice);
            player.ObtainProperty(property);
            property.SoldTo(player);
            photonView.RPC("RPC_SetPropertyOwnerMarker", PhotonTargets.All, propertyID, player.Order);
        }
    }

    [PunRPC]
    private void RPC_SetPropertyOwnerMarker(int propertyID, int playerID)
    {
        NProperty prop = Array.Find(FindObjectsOfType<NProperty>(), x => x.PropertyID == propertyID);
        prop.SetNewOwnerMarker(playerID);
    }


    public void AuctionProperty(NProperty property)
    {

    }



    public void UpgradeLand(int propertyID, PhotonPlayer caller)
    {
        photonView.RPC("RPC_UpgradeLand", PhotonTargets.MasterClient, propertyID, caller);
    }

    [PunRPC]
    public void RPC_UpgradeLand(int propertyID, PhotonPlayer caller)
    {
        if (!PhotonNetwork.isMasterClient)
            return;
        NLand landToUpgrade = NBoardManager.instance.Properties[propertyID] as NLand;
        NPlayer player = _gamePlayers.Find(x => x.photonView.owner == caller);
        //Debug.Log(caller.NickName + " wants to upgrade " + landToUpgrade.PropertyName);

        if (player.CurrentMoney < landToUpgrade.UpgradePrice)
            return;
        if (landToUpgrade.Upgradable && landToUpgrade.CurrentLevel < NLand.maxLevel)
        {
            player.ChangeMoney(-landToUpgrade.UpgradePrice);
            landToUpgrade.Upgrade();
            photonView.RPC("RPC_UpdateLandLevel", PhotonTargets.All, propertyID, landToUpgrade.CurrentLevel);
        }
    }


    public void DegradeLand(int propertyID, PhotonPlayer caller)
    {
        photonView.RPC("RPC_DegradeLand", PhotonTargets.MasterClient, propertyID, caller);
    }

    [PunRPC]
    public void RPC_DegradeLand(int propertyID, PhotonPlayer caller)
    {
        if (!PhotonNetwork.isMasterClient)
            return;
        NLand landToDegrade = NBoardManager.instance.Properties[propertyID] as NLand;
        NPlayer player = _gamePlayers.Find(x => x.photonView.owner == caller);

        if (landToDegrade.Degradable && landToDegrade.CurrentLevel > 0)
        {
            player.ChangeMoney(landToDegrade.UpgradePrice / 2);
            landToDegrade.Degrade();
            photonView.RPC("RPC_UpdateLandLevel", PhotonTargets.All, propertyID, landToDegrade.CurrentLevel);
        }
    }

    [PunRPC]
    public void RPC_UpdateLandLevel(int propertyID, int newLevel)
    {
        NLand land = NBoardManager.instance.Properties[propertyID] as NLand;
        land.SetLevelText(newLevel);
    }



    public void MortgageProperty(int propertyID, PhotonPlayer caller)
    {
        photonView.RPC("RPC_MortgageProperty", PhotonTargets.MasterClient, propertyID, caller);
    }

    [PunRPC]
    public void RPC_MortgageProperty(int propertyID, PhotonPlayer caller)
    {
        if (!PhotonNetwork.isMasterClient)
            return;

        NProperty property = NBoardManager.instance.Properties[propertyID];
        NPlayer player = _gamePlayers.Find(x => x.photonView.owner == caller);

        if (!property.IsMortgaged)
        {
            property.IsMortgaged = true;
            player.ChangeMoney(property.PurchasePrice / 2);
            photonView.RPC("RPC_TogglePropertyMortgagedBackground", PhotonTargets.All, propertyID, true); 
        }
    }

    public void RedeemProperty(int propertyID, PhotonPlayer caller)
    {
        photonView.RPC("RPC_RedeemProperty", PhotonTargets.MasterClient, propertyID, caller);
    }

    [PunRPC]
    public void RPC_RedeemProperty(int propertyID, PhotonPlayer caller)
    {
        if (!PhotonNetwork.isMasterClient)
            return;

        NProperty property = NBoardManager.instance.Properties[propertyID];
        NPlayer player = _gamePlayers.Find(x => x.photonView.owner == caller);

        if (property.IsMortgaged)
        {
            property.IsMortgaged = false;
            player.ChangeMoney(-(int)(property.PurchasePrice / 2 * 1.1f));
            photonView.RPC("RPC_TogglePropertyMortgagedBackground", PhotonTargets.All, propertyID, false);
        }
    }

    [PunRPC]
    public void RPC_TogglePropertyMortgagedBackground(int propertyID, bool isMortgaged)
    {
        NProperty prop = Array.Find(FindObjectsOfType<NProperty>(), x => x.PropertyID == propertyID);
        prop.ToggleMortgagedBackground(isMortgaged);
    }



    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }
}
