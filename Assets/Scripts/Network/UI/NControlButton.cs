using UnityEngine;
using UnityEngine.UI;

public class NControlButton : MonoBehaviour {

    Text text;
    string buttonName;

    // Use this for initialization
    void Start()
    {
        text = GetComponentInChildren<Text>();
        buttonName = text.text;
    }

    public void ToggleButtonText()
    {
        if (text.text == buttonName)
        {
            text.text = "Finish";
        }
        else
        {
            text.text = buttonName;
        }
    }
}
