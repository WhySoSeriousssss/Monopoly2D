using UnityEngine;
using UnityEngine.UI;

public class DicePointSelectPanel : MonoBehaviour {

    public InputField input;
    public Button okButton;
    public Toggle additionalRollToggle;

    public void OKButtonOnClick()
    {
        int value = int.Parse(input.text);
        bool additinalRoll = additionalRollToggle.isOn;
        LocPlayerController.instance.ReceiveDicePoint(value, additinalRoll);
        gameObject.SetActive(false);
    }


}
