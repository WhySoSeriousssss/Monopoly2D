using UnityEngine;
using UnityEngine.UI;

public class NPropertyMiniCard : MonoBehaviour {

    public Image groupColor;
    public Text label;
    public Toggle toggle;

    NProperty _property;
    public NProperty Property { get { return _property; } }

    private int _side;
    private int _index;


    public void Initialize(NProperty property, int side, int index, bool interactable)
    {
        _property = property;

        NLand land = property as NLand;
        if (land != null)
        {
            groupColor.color = land.Group;
        }
        label.text = property.PropertyName;

        _side = side;
        _index = index;
        toggle.interactable = interactable;
    }


    public void OnToggleChanged(bool newValue)
    {
        if (NTradingDialog.instance != null)
            NTradingDialog.instance.ToggleProperty(_side, _index, newValue);
    }


    public bool IsSelected()
    {
        return toggle.isOn;
    }

    public void SetToggle(bool isSelected)
    {
        toggle.isOn = isSelected;
    }
}
