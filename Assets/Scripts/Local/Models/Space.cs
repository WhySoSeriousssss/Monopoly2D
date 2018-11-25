using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Space : MonoBehaviour {

	public virtual void StepOn(Player player)
    {
        Debug.Log("step on space");
    }
}
