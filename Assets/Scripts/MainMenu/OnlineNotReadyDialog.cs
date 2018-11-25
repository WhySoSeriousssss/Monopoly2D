using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlineNotReadyDialog : MonoBehaviour {

	public void OkButtonOnClicked()
    {
        Destroy(gameObject);
    }
}
