using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PropertyMiniCard : MonoBehaviour {

    public Image groupColor;
    public Text label;
    public Toggle toggle;

    Property property;
    public Property Property { get { return property; } }


    public void Initialize(Property property)
    {
        this.property = property;

        Land land = property as Land;
        if (land != null)
        {
            groupColor.color = land.Group;
        }
        label.text = property.PropertyName;
    }

    public bool IsSelected()
    {
        return toggle.isOn;
    }
}
