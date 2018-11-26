using UnityEngine;

public class NLand : NProperty {

    Color _group;
    public Color Group { get { return _group; } }

    int _upgradePrice;
    public int UpgradePrice { get { return _upgradePrice; } }

    int _currentLevel = 0;
    public int CurrentLevel { get { return _currentLevel; } }

    public static int maxLevel = 5;
    int[] _rents = new int[maxLevel];
    public int[] Rents { get { return _rents; } }

    bool _upgradable = false;
    public bool Upgradable { get { return _upgradable; } set { _upgradable = value; } }

    bool _degradable = false;
    public bool Degradable { get { return _degradable; } set { _degradable = value; } }

    TextMesh levelText;

    public void Initialize(string name, int id, int pPrice, int uPrice, int[] rents,
        Color groupColor, Color backgroundColor, Color mortgagedBackgroundColor)
    {
        _propertyName = name;
        _propertyID = id;
        _group = groupColor;
        _purchasePrice = pPrice;
        _upgradePrice = uPrice;
        _rents = rents;
        _currentRent = rents[_currentLevel];
        _owner = null;

        bgColor = backgroundColor;
        mortgagedBgColor = mortgagedBackgroundColor;
        backgroundSR = GetComponentInChildren<SpriteRenderer>();

        levelText = GetComponentsInChildren<TextMesh>()[2];

    }

    
    public override void SoldTo(NPlayer player)
    {
        base.SoldTo(player);
        NBoardManager.instance.UpdateGroupUpgradeble(_group);
    }
    

    public void Upgrade()
    {
        if (_upgradable && _currentLevel < maxLevel)
        {
            _currentRent = _rents[++_currentLevel];
            NBoardManager.instance.UpdateGroupUpgradeble(_group);
            NBoardManager.instance.UpdateGroupDegradeble(_group);
        }
    }

    public void Degrade()
    {
        if (_degradable && _currentLevel > 0)
        {
            _currentRent = _rents[--_currentLevel];
            NBoardManager.instance.UpdateGroupUpgradeble(_group);
            NBoardManager.instance.UpdateGroupDegradeble(_group);
        }
    }

    public void SetLevelText(int newLevel)
    {
        if (newLevel == 0)
            levelText.text = "";
        else
            levelText.text = "lv. " + newLevel;
    }
}
