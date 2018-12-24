using UnityEngine;

public class NJailDialog : MonoBehaviour {

    public void OnBailButtonClicked()
    {
        NPlayerController.instance.BailToGetOut(PhotonNetwork.player);
        Destroy(gameObject);
    }

    public void OnRollButtonClicked()
    {
        NPlayerController.instance.RollDiceToGetOut(PhotonNetwork.player);
        Destroy(gameObject);
    }

    public void OnUseCardButtonClicked()
    {
        NPlayerController.instance.UseCardToRelease();
        Destroy(gameObject);
    }
}
