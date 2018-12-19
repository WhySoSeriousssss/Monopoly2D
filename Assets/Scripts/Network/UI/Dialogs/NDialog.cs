using UnityEngine;

public class NDialog : Photon.MonoBehaviour {

    [PunRPC]
    public void RPC_DestroyDialog()
    {
        Destroy(gameObject);
    }
}
