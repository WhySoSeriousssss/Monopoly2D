using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoPanel : MonoBehaviour {

    public InputField nameInput;
    public Dropdown colorDropDown;

	
    public string GetInputPlayerName()
    {
        return nameInput.text;
    }

    public string GetSelectedColorString()
    {
        return colorDropDown.captionText.text;
    }

    public void SetColorDropDown(List<string> colorStrings)
    {
        for (int i = 0; i < colorStrings.Count; i++)
        {
            colorDropDown.options.Add(new Dropdown.OptionData(colorStrings[i]));
        }
    }
}
