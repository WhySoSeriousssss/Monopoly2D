using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

    public GameObject localGameSettingPanelPrefab;
    public GameObject onlineNotReadyPrefab;

	public void LocalGameButtonOnClicked()
    {
        Instantiate(localGameSettingPanelPrefab);
    }

    public void OnlineGameButtonOnClicked()
    {
        Instantiate(onlineNotReadyPrefab);
    }
}
