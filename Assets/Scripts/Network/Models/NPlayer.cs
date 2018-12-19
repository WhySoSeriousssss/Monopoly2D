using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPlayer : Photon.MonoBehaviour {

    public static NPlayer thisPlayer;

    static List<Color> allColors = new List<Color>()
    { Color.red, Color.yellow, Color.blue, Color.green, Color.grey, Color.cyan};


    private int _order = -1;
    public int Order { get { return _order; } set { _order = value; } }

    private bool _hasFinished = false;
    public bool HasFinished { get { return _hasFinished; } set { _hasFinished = value; } }

    private bool _canStart = false;
    public bool CanStart { get { return _canStart; } set { _canStart = value; } }

    private bool _isMoving = false;
    public bool IsMoving { get { return _isMoving; } set { _isMoving = value; } }

    private int _currentMoney = 0;
    public int CurrentMoney { get { return _currentMoney; } }

    private int _currentPosition;
    public int CurrentPosition { get { return _currentPosition; } }

    List<NProperty> _properties = new List<NProperty>();
    public List<NProperty> Properties { get { return _properties; } }

    int _numRailRoads = 0;
    public int NumRaildRoads
    {
        get
        {
            _numRailRoads = 0;
            foreach (NProperty prop in _properties)
            {
                if (prop is NRailroad)
                    _numRailRoads++;
            }
            return _numRailRoads;
        }
    }

    private SpriteRenderer _sr;
    public SpriteRenderer SR { get { return _sr; } }

    // all the spaces on the gameboard
    static List<NSpace> allSpaces;
    static int numSpaces;

    // all the properties on the gameboard
    static List<NProperty> allProperties;

    // used to update the money text on the player card
    public delegate void OnMoneyChanged();
    public OnMoneyChanged OnMoneyChangedCallBack;


    public static void Initialize()
    {
        if (allSpaces == null)
        {
            allSpaces = NBoardManager.instance.Spaces;
            numSpaces = allSpaces.Count;
        }
        if (allProperties == null)
        {
            allProperties = NBoardManager.instance.Properties;
        }
    }


    private void Start()
    {
        if (photonView.isMine)
        {
            thisPlayer = this;
        }
        _sr = GetComponentInChildren<SpriteRenderer>();
    }


    public IEnumerator Move(int steps)
    {
        _isMoving = true;
        int oldPos = _currentPosition;
        for (int i = 0; i < steps; i++)
        {
            Vector3 translate = allSpaces[(_currentPosition + 1) % numSpaces].transform.position - allSpaces[_currentPosition].transform.position;
            transform.Translate(translate);
            _currentPosition = (_currentPosition + 1) % numSpaces;
            yield return new WaitForSeconds(0.2f);
        }
        // Pass GO
        if (_currentPosition == oldPos + steps - numSpaces)
            NPlayerManager.instance.PassGo(this);

        NSpace space = allSpaces[_currentPosition];
        space.StepOn(this);

        _isMoving = false;
    }


    public void SetOrder(int newOrder)
    {
        if (!PhotonNetwork.isMasterClient)
            return;
        photonView.RPC("RPC_SetOrder", PhotonTargets.All, newOrder);
    }
    [PunRPC]
    private void RPC_SetOrder(int newOrder)
    {
        _order = newOrder;
    }


    public void ChangeMoney(int increment)
    {
        if (!PhotonNetwork.isMasterClient)
            return;
        photonView.RPC("RPC_ChangeMoney", PhotonTargets.All, increment);
    }
    [PunRPC]
    private void RPC_ChangeMoney(int increment)
    {
        _currentMoney += increment;
        OnMoneyChangedCallBack.Invoke();
    }


    public void ObtainProperty(NProperty property)
    {
        if (!PhotonNetwork.isMasterClient)
            return;
        photonView.RPC("RPC_ObtainProperty", PhotonTargets.All, property.PropertyID);
    }
    [PunRPC]
    public void RPC_ObtainProperty(int propertyID)
    {
        NProperty property = allProperties[propertyID];
        _properties.Add(property);
    }

    public void LoseProperty(NProperty property)
    {
        if (!PhotonNetwork.isMasterClient)
            return;
        photonView.RPC("RPC_LoseProperty", PhotonTargets.All, property.PropertyID);
    }
    [PunRPC]
    public void RPC_LoseProperty(int propertyID)
    {
        NProperty property = allProperties[propertyID];
        _properties.Remove(property);
    }


    public void SetMoney(int newMoney)
    {
        if (!PhotonNetwork.isMasterClient)
            return;
        photonView.RPC("RPC_SetMoney", PhotonTargets.All, newMoney);
    }
    [PunRPC]
    private void RPC_SetMoney(int newMoney)
    {
        _currentMoney = newMoney;
    }


    public void SetColor(int newColor)
    {
        if (!PhotonNetwork.isMasterClient)
            return;
        photonView.RPC("RPC_SetColor", PhotonTargets.All, newColor);
    }
    [PunRPC]
    private void RPC_SetColor(int newColor)
    {
        _sr.color = allColors[newColor];
    }


    public void SetTokenPosition(Vector3 newPos)
    {
        photonView.RPC("RPC_SetTokenPosition", PhotonTargets.All, newPos);
    }
    [PunRPC]
    private void RPC_SetTokenPosition(Vector3 newPos)
    {
        _sr.gameObject.transform.position = newPos;
        _sr.gameObject.transform.localScale = new Vector3(0.4f, 0.4f, 1);
    }



    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
