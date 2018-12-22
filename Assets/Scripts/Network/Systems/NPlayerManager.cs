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

    public NPlayer FindGamePlayer(PhotonPlayer player)
    {
        return _gamePlayers.Find(x => player == x.photonView.owner);
    }

    /*
    public void StartTurn()
    {
        photonView.RPC("RPC_ChangeFinishStatus", PhotonTargets.MasterClient, false, PhotonNetwork.player);
    }
    
    [PunRPC]
    public void RPC_ChangeFinishStatus(bool hasFinished, PhotonPlayer caller)
    {
        if (!PhotonNetwork.isMasterClient)
            return;
        FindGamePlayer(caller).HasFinished = hasFinished;
        //photonView.RPC("RPC_ReceiveFinishStatus", PhotonTargets.All, hasFinished, caller);
    }
    [PunRPC]
    public void RPC_ReceiveFinishStatus(bool hasFinished, PhotonPlayer caller)
    {
        NPlayer player = FindGamePlayer(caller);
        player.HasFinished = hasFinished;
    }
    */

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

        if (dice1 == dice2)
            NGameplay.instance.additionalDice = true;
        photonView.RPC("RPC_ReceiveDice", PhotonTargets.All, dice1, dice2, caller);
    }

    [PunRPC]
    public void RPC_ReceiveDice(int dice1, int dice2, PhotonPlayer caller)
    {
        NPlayer player = FindGamePlayer(caller);
        StartCoroutine(player.Move(dice1 + dice2));
    }

    // debug use
    public void RollDiceManual(int dice, bool additionalRoll)
    {
        photonView.RPC("RPC_RollDiceManual", PhotonTargets.MasterClient, dice, additionalRoll, PhotonNetwork.player);
    }

    [PunRPC]
    public void RPC_RollDiceManual(int dice, bool additionalRoll, PhotonPlayer caller)
    {
        if (!PhotonNetwork.isMasterClient)
            return;
        if (additionalRoll)
            NGameplay.instance.additionalDice = true;
        photonView.RPC("RPC_ReceiveDiceManual", PhotonTargets.All, dice, caller);
    }

    [PunRPC]
    public void RPC_ReceiveDiceManual(int dice, PhotonPlayer caller)
    {
        NPlayer player = FindGamePlayer(caller);
        StartCoroutine(player.Move(dice));
    }


    public void PassGo(NPlayer player)
    {
        player.ChangeMoney(NGameplay.passGoBonus);
    }



    public void TradeProperty(PhotonPlayer trader, PhotonPlayer tradee, int[] propA, int[] propB, int moneyA, int moneyB)
    {
        photonView.RPC("RPC_TradeProperty", PhotonTargets.MasterClient, trader, tradee, propA, propB, moneyA, moneyB);
    }

    [PunRPC]
    public void RPC_TradeProperty(PhotonPlayer trader, PhotonPlayer tradee, int[] propA, int[] propB, int moneyA, int moneyB)
    {
        if (!PhotonNetwork.isMasterClient)
            return;
        NPlayer playerA = FindGamePlayer(trader);
        NPlayer playerB = FindGamePlayer(tradee);

        playerA.ChangeMoney(-moneyA);
        playerA.ChangeMoney(moneyB);
        playerB.ChangeMoney(moneyA);
        playerB.ChangeMoney(-moneyB);

        foreach(int propID in propB)
        {
            NProperty property = NBoardManager.instance.Properties[propID];
            playerB.LoseProperty(property);
            playerA.ObtainProperty(property);
            property.SoldTo(playerA);
            photonView.RPC("RPC_SetPropertyOwnerMarker", PhotonTargets.All, propID, trader);
        }

        foreach (int propID in propA)
        {
            NProperty property = NBoardManager.instance.Properties[propID];
            playerA.LoseProperty(property);
            playerB.ObtainProperty(property);
            property.SoldTo(playerB);
            photonView.RPC("RPC_SetPropertyOwnerMarker", PhotonTargets.All, propID, tradee);
        }

    }


    public void PurchaseProperty(NProperty property)
    {
        photonView.RPC("RPC_PurchaseProperty", PhotonTargets.MasterClient, property.PropertyID, property.PurchasePrice, PhotonNetwork.player);
    }
    
    [PunRPC]
    public void RPC_PurchaseProperty(int propertyID, int purchasePrice, PhotonPlayer caller)
    {
        if (!PhotonNetwork.isMasterClient)
            return;
        NPlayer player = FindGamePlayer(caller);
        NProperty property = NBoardManager.instance.Properties[propertyID];

        if (player.CurrentMoney >= purchasePrice)
        {
            player.ChangeMoney(-purchasePrice);
            player.ObtainProperty(property);
            property.SoldTo(player);
            photonView.RPC("RPC_SetPropertyOwnerMarker", PhotonTargets.All, propertyID, caller);
        }
    }

    [PunRPC]
    private void RPC_SetPropertyOwnerMarker(int propertyID, PhotonPlayer player)
    {
        NProperty prop = NBoardManager.instance.FindProperty(propertyID);
        prop.SetNewOwnerMarker(FindGamePlayer(player));
    }


    public void AuctionProperty(NProperty property)
    {
        NDialogManager.instance.CallAuctionDialog(property);
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
        NPlayer player = FindGamePlayer(caller);
        //Debug.Log(caller.NickName + " wants to upgrade " + landToUpgrade.PropertyName);

        if (player.CurrentMoney < landToUpgrade.UpgradePrice || landToUpgrade.IsMortgaged)
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
        NPlayer player = FindGamePlayer(caller);

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
        NPlayer player = FindGamePlayer(caller);

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
        NPlayer player = FindGamePlayer(caller);

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
        NProperty prop = NBoardManager.instance.FindProperty(propertyID);
        prop.ToggleMortgagedBackground(isMortgaged);
    }



    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }
}
