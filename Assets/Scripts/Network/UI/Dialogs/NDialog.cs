using UnityEngine;

public class NDialog : MonoBehaviour {

    [PunRPC]
    public void RPC_DestroyDialog()
    {
        Destroy(gameObject);
    }
}
