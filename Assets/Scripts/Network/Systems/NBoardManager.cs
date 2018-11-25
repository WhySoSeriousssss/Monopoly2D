using System.Collections.Generic;
using UnityEngine;
using System.Xml.Linq;
using System;

public class NBoardManager : Photon.MonoBehaviour {

    #region Singleton
    public static NBoardManager instance;
    private void Awake()
    {
        if (instance != null)
            Debug.Log("Multiple BoardManager. Something went wrong");
        instance = this;
    }
    #endregion

    
    private GameObject _board;
    
    [SerializeField]
    private GameObject goPrefab;
    [SerializeField]
    private GameObject jailPrefab;
    [SerializeField]
    private GameObject parkingPrefab;
    [SerializeField]
    private GameObject parkingFeePrefab;
    [SerializeField]
    private GameObject goToJailPrefab;
    [SerializeField]
    private GameObject chancePrefab;
    [SerializeField]
    private GameObject chestPrefab;
    [SerializeField]
    private GameObject spinWheelPrefab;
    [SerializeField]
    private GameObject landPrefab;
    [SerializeField]
    private GameObject railroadPrefab;
    
    public static Vector3 startPos = new Vector3(4, -4, 0);
    public static float spaceWidth = 3;
    public static float spaceHeight = 5;
    public static float scale = 0.25f;
    public static int numSpaces = 0;

    Vector3 ownerMarkerOffset = new Vector3(0, 0.72f, 0);

    Color landBgColor;
    Color railroadBgColor;
    Color mortgageBgColor;

    private List<NSpace> _spaces = new List<NSpace>();
    public List<NSpace> Spaces { get { return _spaces; } }

    private List<NProperty> _properties = new List<NProperty>();
    public List<NProperty> Properties { get { return _properties; } }

    Dictionary<string, GameObject> _spaceTypes = new Dictionary<string, GameObject>();

    public Dictionary<Color, List<NLand>> groups = new Dictionary<Color, List<NLand>>();

    private List<List<int>> _propertiesByPlayer = new List<List<int>>();
    public List<List<int>> PropertiesByPlayer { get { return _propertiesByPlayer; } }

    private List<NPlayer> _propertyOwnership = new List<NPlayer>();
    public List<NPlayer> PropertyOwnership { get { return _propertyOwnership; } }


    public void Initialize()
    {
        // set the property background color
        landBgColor = new Color(0.938f, 0.875f, 0.449f);
        mortgageBgColor = new Color(0.949f, 0.496f, 0.426f);
        railroadBgColor = Color.white;


        _spaceTypes.Add("go", goPrefab);
        _spaceTypes.Add("jail", jailPrefab);
        _spaceTypes.Add("parking", parkingPrefab);
        _spaceTypes.Add("gotojail", goToJailPrefab);
        _spaceTypes.Add("chance", chancePrefab);
        _spaceTypes.Add("chest", chestPrefab);
        _spaceTypes.Add("spinner", spinWheelPrefab);
        _spaceTypes.Add("parkingfee", parkingFeePrefab);
        _spaceTypes.Add("land", landPrefab);
        _spaceTypes.Add("railroad", railroadPrefab);
        
        _board = new GameObject("Board");
        LoadMap("map1.xml");
    }

    public void LoadMap(string fileName)
    {
        XDocument xdoc;
        xdoc = XDocument.Load("Assets/StreamingAssets/Maps/" + fileName);
        XElement root = xdoc.Root;
        var items = root.Elements("space");

        Vector3 pos = startPos;
        Vector3 increment = new Vector3(), squareIncrement = new Vector3();
        Quaternion rot = Quaternion.identity;

        int edge = 0;
        foreach (XElement item in items)
        {
            string type = item.Attribute("type").Value;
            if (type == "go" || type == "jail" || type == "parking" || type == "gotojail")
            { // TO-DO: algorithm needs improving :(
                // making some parameters
                int a = edge % 2, b = (edge + 1) % 2;
                int c = 0, d = 0;
                if (edge == 0 || edge == 3) c = -1;
                else c = 1;
                if (edge == 1) d = -1;
                else d = 1;

                // position offset
                if (edge > 0)
                    pos += new Vector3(a * d * (spaceHeight - spaceWidth) / 2 * scale, b * d * (spaceHeight - spaceWidth) / 2 * scale, 0);

                // Create the square space
                _spaces.Add(CreateSpace(_spaceTypes[type], pos, Quaternion.identity, scale));

                // calculating increments for the next edge
                increment = new Vector3(b * c * spaceWidth * scale, a * c * spaceWidth * scale, 0);
                squareIncrement = new Vector3(b * c * (spaceHeight + spaceWidth) / 2 * scale, a * c * (spaceHeight + spaceWidth) / 2 * scale, 0);

                pos += squareIncrement;
                rot = Quaternion.Euler(0, 0, (4 - edge) * 90);
                edge++;
            }
            else
            {
                if (type == "land")
                {
                    string pName = item.Element("name").Value;
                    Color gpColor;
                    ColorUtility.TryParseHtmlString(item.Element("group").Value, out gpColor);
                    int pPrice = int.Parse(item.Element("purchasePrice").Value);
                    int uPrice = int.Parse(item.Element("upgradePrice").Value);
                    int[] tolls = new int[6];
                    XElement tollItems = item.Element("toll");
                    for (int i = 0; i <= 5; i++)
                        tolls[i] = int.Parse(tollItems.Element("lvl" + i).Value);
                    NLand newLand = CreateLand(pName, pPrice, uPrice, tolls, gpColor, pos, rot, scale);
                    _spaces.Add(newLand);
                    _properties.Add(newLand);
                    if (!groups.ContainsKey(gpColor))
                    {
                        List<NLand> landList = new List<NLand>();
                        landList.Add(newLand);
                        groups.Add(gpColor, landList);
                    }
                    else
                    {
                        groups[gpColor].Add(newLand);
                    }
                }
                else if (type == "railroad")
                {
                    string name = item.Element("name").Value;
                    int pPrice = int.Parse(item.Element("purchasePrice").Value);
                    int initToll = int.Parse(item.Element("initialToll").Value);
                    NRailroad newRailroad = CreateRailroad(name, pPrice, initToll, pos, rot, scale);
                    _spaces.Add(newRailroad);
                    _properties.Add(newRailroad);
                }
                else if (type == "spinner" || type == "chest" || type == "chance" || type == "parkingfee")
                {
                    _spaces.Add(CreateSpace(_spaceTypes[type], pos, rot, scale));
                }
                pos += increment;
            }
            numSpaces++;
        }
    }

    NSpace CreateSpace(GameObject obj, Vector3 pos, Quaternion rotation, float scale)
    {
        // create the object
        GameObject placeObj = Instantiate(obj, pos, rotation);
        placeObj.transform.SetParent(_board.transform);
        placeObj.transform.localScale = new Vector3(scale, scale, 1);

        NSpace space = placeObj.GetComponent<NSpace>();
        return space;
    }

    NLand CreateLand(string name, int purchasePrice, int updagradePrice, int[] tollFees, Color groupColor,
        Vector3 pos, Quaternion rotation, float scale)
    {

        NLand land = CreateSpace(_spaceTypes["land"], pos, rotation, scale) as NLand;

        // Set the text
        TextMesh[] textMesh = land.GetComponentsInChildren<TextMesh>();
        textMesh[0].text = name;
        textMesh[1].text = purchasePrice.ToString();

        // Set the color
        SpriteRenderer[] sr = land.GetComponentsInChildren<SpriteRenderer>();
        sr[0].color = landBgColor;
        sr[1].color = groupColor;

        // set owner marker position
        Vector3 omp = pos + Quaternion.AngleAxis(land.gameObject.transform.rotation.eulerAngles.z, Vector3.forward) * ownerMarkerOffset;
        land.OwnerMarkerPos = omp;

        land.Initialize(name, _properties.Count, purchasePrice, updagradePrice, tollFees, groupColor, landBgColor, mortgageBgColor);
        return land;
    }

    NRailroad CreateRailroad(string name, int purchasePrice, int initToll,
        Vector3 pos, Quaternion rotation, float scale)
    {
        NRailroad railroad = CreateSpace(_spaceTypes["railroad"], pos, rotation, scale) as NRailroad;

        // Set the text
        TextMesh textMesh = railroad.GetComponentInChildren<TextMesh>();
        textMesh.text = name;

        // Set the color


        // set owner marker position
        Vector3 omp = pos + Quaternion.AngleAxis(railroad.gameObject.transform.rotation.eulerAngles.z, Vector3.forward) * ownerMarkerOffset;
        railroad.OwnerMarkerPos = omp;

        railroad.Initialize(name, _properties.Count, purchasePrice, initToll, railroadBgColor, mortgageBgColor);
        return railroad;
    }

    
    public void UpdateGroupUpgradeble(Color group)
    {
        List<NLand> lands = groups[group];
        if (lands.Count == 0) return;
        NPlayer owner0 = lands[0].Owner;

        foreach (NLand land in lands)
        {
            if (land.Owner == null || land.Owner != owner0)
                return;
        }
        int minLevel = lands[0].CurrentLevel;
        foreach (NLand land in lands)
        {
            if (land.CurrentLevel < minLevel)
                minLevel = land.CurrentLevel;
        }
        foreach (NLand land in lands)
        {
            if (land.CurrentLevel == minLevel)
                land.Upgradeble = true;
            else
                land.Upgradeble = false;
        }
    }

    public void UpdateGroupDegradeble(Color group)
    {
        List<NLand> lands = groups[group];
        if (lands.Count == 0) return;
        NPlayer owner0 = lands[0].Owner;

        foreach (NLand land in lands)
        {
            if (land.Owner == null || land.Owner != owner0)
                return;
        }
        int maxLevel = lands[0].CurrentLevel;
        foreach (NLand land in lands)
        {
            if (land.CurrentLevel > maxLevel)
                maxLevel = land.CurrentLevel;
        }
        foreach (NLand land in lands)
        {
            if (land.CurrentLevel == maxLevel)
                land.Degradable = true;
            else
                land.Degradable = false;
        }
    }
    
    public void PropertySoldToPlayer(int propertyID, int playerID)
    {
        if (!PhotonNetwork.isMasterClient)
            return;
        //_propertyOwnership[propertyID] = NGameplay.instance.Players[playerID];
        photonView.RPC("RPC_SetPropertyOwnerMarker", PhotonTargets.All, propertyID, playerID);
    }

    [PunRPC]
    private void RPC_SetPropertyOwnerMarker(int propertyID, int playerID)
    {
        NProperty prop = Array.Find(FindObjectsOfType<NProperty>(), x => x.PropertyID == propertyID);

        // remove old marker
        if (prop.OwnerMarkerSR != null)
        {
            Destroy(prop.OwnerMarkerSR);
            prop.OwnerMarkerSR = null;
        }

        // add new markers
        prop.OwnerMarkerSR = Instantiate(NGameplay.instance.Players[playerID].SR, prop.OwnerMarkerPos, Quaternion.identity);
        prop.OwnerMarkerSR.transform.localScale = new Vector3(0.2f, 0.2f);
    }

    public void UpgradeLand(int propertyID)
    {

    }

    [PunRPC]
    public void RPC_UpgradeLand()
    {

    }


    public void DegradeLand()
    {

    }

    [PunRPC]
    public void RPC_DegradeLand()
    {

    }
}
