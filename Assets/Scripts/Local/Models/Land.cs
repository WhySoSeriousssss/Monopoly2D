using System.Collections;
using UnityEngine;

public class Land : Property {

    Color group;
    public Color Group { get { return group; } }
    
    int upgradePrice;
    public int UpgradePrice { get { return upgradePrice; } }

    int currentLevel = 0;
    public int CurrentLevel { get { return currentLevel; } }

    public static int maxLevel = 5;
    int[] rents = new int[maxLevel];
    public int[] Rents { get { return rents; } }

    bool upgradeble = false;
    public bool Upgradeble { get { return upgradeble; } set { upgradeble = value; } }

    bool degradable = false;
    public bool Degradable { get { return degradable; } set { degradable = value; } }

    TextMesh levelText;

    public void Initialize(string name, int pPrice, int uPrice, int[] rents,
        Color groupColor, Color backgroundColor, Color mortgagedBackgroundColor)
    {
        propertyName = name;
        group = groupColor;
        purchasePrice = pPrice;
        upgradePrice = uPrice;
        this.rents = rents;
        currentRent = rents[currentLevel];
        owner = null;

        bgColor = backgroundColor;
        mortgagedBgColor = mortgagedBackgroundColor;
        backgroundSR = GetComponentInChildren<SpriteRenderer>();

        levelText = GetComponentsInChildren<TextMesh>()[2];

    }

    public override void SoldTo(Player player)
    {
        base.SoldTo(player);
        BoardManager.instance.UpdateGroupUpgradeble(group);
    }

    public void Upgrade()
    {
        if (upgradeble && currentLevel < maxLevel)
        {
            owner.LoseMoney(upgradePrice);
            currentRent = rents[++currentLevel];
            levelText.text = "lv. " + currentLevel;
            BoardManager.instance.UpdateGroupUpgradeble(group);
            BoardManager.instance.UpdateGroupDegradeble(group);
        }
    }

    public void Degrade()
    {
        if (degradable && currentLevel > 0)
        {
            owner.GetMoney(upgradePrice / 2);
            currentRent = rents[--currentLevel];
            if (currentLevel > 0)
                levelText.text = "lv. " + currentLevel;
            else
                levelText.text = "";
            BoardManager.instance.UpdateGroupUpgradeble(group);
            BoardManager.instance.UpdateGroupDegradeble(group);
        }
    }
}
