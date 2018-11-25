using UnityEngine;

public class InJailDialog : MonoBehaviour {

    public void BailButtonOnClick()
    {
        LocPlayerController.instance.BailToRelease();
        Destroy(gameObject);
    }

    public void RollButtonOnClick()
    {
        LocPlayerController.instance.RollDiceToRelease();
        Destroy(gameObject);
    }

    public void UseCardButtonOnClick()
    {
        LocPlayerController.instance.UseCardToRelease();
        Destroy(gameObject);
    }
}
