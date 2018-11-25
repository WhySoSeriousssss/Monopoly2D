using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Linq;

public class BoardManager : MonoBehaviour {

    #region Singleton
    public static BoardManager instance;
    private void Awake()
    {
        if (instance != null)
            Debug.Log("Multiple BoardManager. Something went wrong");
        instance = this;
    }
    #endregion

    Dictionary<string, GameObject> spaceTypes = new Dictionary<string, GameObject>();
    
    public GameObject goPrefab;
    public GameObject jailPrefab;
    public GameObject parkingPrefab;
    public GameObject gotojailPrefab;
    public GameObject chancePrefab;
    public GameObject chestPrefab;
    public GameObject spinnerPrefab;
    public GameObject parkingFeePrefab;
    public GameObject landPrefab;
    public GameObject railroadPrefab;

    public static Vector3 startPos = new Vector3(4, -4, 0);
    public static float spaceWidth = 3;
    public static float spaceHeight = 5;
    public static float scale = 0.25f;
    public static int numSpaces = 0;

    Vector3 ownerMarkerOffset = new Vector3(0, 0.72f, 0);

    Color landBgColor;
    Color railroadBgColor;
    Color mortgageBgColor;

    public List<Space> spaces = new List<Space>();
    public Dictionary<Color, List<Land>> groups = new Dictionary<Color, List<Land>>();


    public void Initialize()
    {
        // set the property background color
        ColorUtility.TryParseHtmlString("#F0E073", out landBgColor);
        railroadBgColor = Color.white;
        ColorUtility.TryParseHtmlString("#F37F6D", out mortgageBgColor);

        spaceTypes.Add("go", goPrefab);
        spaceTypes.Add("jail", jailPrefab);
        spaceTypes.Add("parking", parkingPrefab);
        spaceTypes.Add("gotojail", gotojailPrefab);
        spaceTypes.Add("chance", chancePrefab);
        spaceTypes.Add("chest", chestPrefab);
        spaceTypes.Add("spinner", spinnerPrefab);
        spaceTypes.Add("parkingfee", parkingFeePrefab);
        spaceTypes.Add("railroad", railroadPrefab);
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
            { // algorithm needs improving :(
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
                spaces.Add(CreateSpace(spaceTypes[type], pos, Quaternion.identity, scale));
                
                // calculating increments for the next edge
                increment = new Vector3(b * c * spaceWidth * scale, a * c * spaceWidth * scale, 0);
                squareIncrement = new Vector3(b * c * (spaceHeight + spaceWidth) / 2 * scale, a * c * (spaceHeight + spaceWidth) / 2 * scale, 0);

                pos += squareIncrement;
                rot = Quaternion.Euler(0, 0, (4 - edge) * 90);
                edge++;
            }
            else { 
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
                    Land newLand = CreateLand(pName, pPrice, uPrice, tolls, gpColor, pos, rot, scale);
                    spaces.Add(newLand);
                    if (!groups.ContainsKey(gpColor))
                    {
                        List<Land> landList = new List<Land>();
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
                    Railroad newRailroad = CreateRailroad(name, pPrice, initToll, pos, rot, scale);
                    spaces.Add(newRailroad);
                }
                else if (type == "spinner" || type == "chest" || type == "chance" || type == "parkingfee")
                {
                    spaces.Add(CreateSpace(spaceTypes[type], pos, rot, scale));
                }
                pos += increment;
            }
            numSpaces++;
        }
    }

    Space CreateSpace(GameObject obj, Vector3 pos, Quaternion rotation, float scale)
    {
        // create the object
        GameObject placeObj = Instantiate(obj, pos, rotation);

        // Set the scale
        Transform transform = placeObj.GetComponent<Transform>();
        transform.localScale = new Vector3(scale, scale, 1);

        Space space = placeObj.GetComponent<Space>();
        return space;
    }

    Land CreateLand(string name, int purchasePrice, int updagradePrice, int[] tollFees, Color groupColor, 
        Vector3 pos, Quaternion rotation, float scale)
    {

        Land land = CreateSpace(landPrefab, pos, rotation, scale) as Land;

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

        land.Initialize(name, purchasePrice, updagradePrice, tollFees, groupColor, landBgColor, mortgageBgColor);
        return land;
    }

    Railroad CreateRailroad(string name, int purchasePrice, int initToll, 
        Vector3 pos, Quaternion rotation, float scale)
    {
        Railroad railroad = CreateSpace(railroadPrefab, pos, rotation, scale) as Railroad;

        // Set the text
        TextMesh textMesh = railroad.GetComponentInChildren<TextMesh>();
        textMesh.text = name;

        // Set the color


        // set owner marker position
        Vector3 omp = pos + Quaternion.AngleAxis(railroad.gameObject.transform.rotation.eulerAngles.z, Vector3.forward) * ownerMarkerOffset;
        railroad.OwnerMarkerPos = omp;

        railroad.Initialize(name, purchasePrice, initToll, railroadBgColor, mortgageBgColor);
        return railroad;
    }

    public void UpdateGroupUpgradeble(Color group)
    {
        List<Land> lands = groups[group];
        if (lands.Count == 0) return;
        Player owner0 = lands[0].Owner;

        foreach (Land land in lands)
        {
            if (land.Owner == null || land.Owner != owner0)
                return;
        }
        int minLevel = lands[0].CurrentLevel;
        foreach (Land land in lands)
        {
            if (land.CurrentLevel < minLevel)
                minLevel = land.CurrentLevel;
        }
        foreach(Land land in lands)
        {
            if (land.CurrentLevel == minLevel)
                land.Upgradeble = true;
            else
                land.Upgradeble = false;
        }
    }

    public void UpdateGroupDegradeble(Color group)
    {
        List<Land> lands = groups[group];
        if (lands.Count == 0) return;
        Player owner0 = lands[0].Owner;

        foreach (Land land in lands)
        {
            if (land.Owner == null || land.Owner != owner0)
                return;
        }
        int maxLevel = lands[0].CurrentLevel;
        foreach (Land land in lands)
        {
            if (land.CurrentLevel > maxLevel)
                maxLevel = land.CurrentLevel;
        }
        foreach (Land land in lands)
        {
            if (land.CurrentLevel == maxLevel)
                land.Degradable = true;
            else
                land.Degradable = false;
        }
    }
}
