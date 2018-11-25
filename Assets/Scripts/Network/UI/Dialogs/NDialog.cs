using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NDialog : MonoBehaviour {

    [PunRPC]
    public void RPC_DestroyDialog()
    {
        Destroy(gameObject);
    }
}
