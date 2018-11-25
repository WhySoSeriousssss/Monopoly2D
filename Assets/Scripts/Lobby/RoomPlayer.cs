using ExitGames.Client.Photon;

public class RoomPlayer : Photon.PunBehaviour {

    // Player Status
    private bool _isReady = false;
    public bool IsReady { get { return _isReady; } }

    private int _colorIndex = 0;
    //public int ColorIndex { get { return _colorIndex; } }


    //// The PhotonPlayer it represents
    private PhotonPlayer _photonPlayer;
    public PhotonPlayer PhotonPlayer { get { return _photonPlayer; } }

    // The RoomPlayerSlot it resides
    private RoomPlayerSlot _roomPlayerSlot;
    public RoomPlayerSlot RoomPlayerSlot { get { return _roomPlayerSlot; } }

    // The game player it contains
    private NPlayer _gamePlayer;
    public NPlayer GamePlayer { get { return _gamePlayer; } }

    private void Awake()
    {
        DontDestroyOnLoad(this);
        _photonPlayer = photonView.owner;
        _gamePlayer = GetComponent<NPlayer>();
        //SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    private void Start()
    {
        Hashtable customProps = _photonPlayer.CustomProperties;

        if (customProps["Color"] != null
            && customProps["IsReady"] != null)
        {
            _colorIndex = (int)customProps["Color"];
            _isReady = (bool)customProps["IsReady"];
        }
        else
        {
            Hashtable newCustomProps = new Hashtable();
            newCustomProps.Add("Color", _colorIndex);
            newCustomProps.Add("IsReady", _isReady);
            _photonPlayer.SetCustomProperties(newCustomProps);
        }

        RoomManager.instance.CreatePlayerSlot(this, photonView.isMine);

        DisplayPlayerInfo();
    }

    public void AssignRoomPlayerSlot(RoomPlayerSlot roomPlayerSlot)
    {
        _roomPlayerSlot = roomPlayerSlot;
    }

    public void ToggleReadyStatus()
    {
        _isReady = !_isReady;
        Hashtable customProperties = new Hashtable();
        customProperties.Add("IsReady", _isReady);
        _photonPlayer.SetCustomProperties(customProperties);
    }

    public void ChangeColor(int newColorIndex)
    {
        _colorIndex = newColorIndex;
        Hashtable customProperties = new Hashtable();
        customProperties.Add("Color", newColorIndex);
        _photonPlayer.SetCustomProperties(customProperties);
    }

    public void DisplayPlayerInfo()
    {
        if (_roomPlayerSlot != null)
        {
            _roomPlayerSlot.SetReadyStatus(_isReady);
            _roomPlayerSlot.SetColor(_colorIndex);
        }
    }


    public override void OnPhotonPlayerPropertiesChanged(object[] playerAndUpdatedProps)
    {
        PhotonPlayer player = playerAndUpdatedProps[0] as PhotonPlayer;
        //Hashtable props = playerAndUpdatedProps[1] as Hashtable;
        
        if (player == _photonPlayer)
        {
            Hashtable props = player.CustomProperties;
            _isReady = (bool)props["IsReady"];
            _colorIndex = (int)props["Color"];

            DisplayPlayerInfo();
        }
    }

    /*
    public void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log(photonView.isMine);
        //if (photonView.isMine)
        //{
        //    NPlayer.AssignLocalPlayer(_gamePlayer);
        //}
    }
    */

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
