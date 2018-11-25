using UnityEngine;

public class NetworkManager : Photon.PunBehaviour {

    private void Start()
    {
        if (PhotonNetwork.isMasterClient)
        {
            PhotonNetwork.Instantiate("OnlineGamePlay", Vector3.zero, Quaternion.identity, 0);
        }
    }
}
