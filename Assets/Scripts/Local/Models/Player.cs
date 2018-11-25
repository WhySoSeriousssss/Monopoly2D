using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    string playerName;
    public string PlayerName { get { return playerName; } }

    int currentMoney;
    public int CurrentMoney { get { return currentMoney; } set { currentMoney = value; } }

    int currentLocation;
    public int CurrentLocation { get { return currentLocation; } set { currentLocation = value; } }


    List<Property> properties = new List<Property>();
    public List<Property> Properties { get { return properties; } }

    int numRailRoads = 0;
    public int NumRaildRoads
    {
        get
        {
            numRailRoads = 0;
            foreach (Property prop in properties)
            {
                if (prop is Railroad)
                    numRailRoads++;
            }
            return numRailRoads;
        }
    }

    bool hasFinished = false;
    public bool HasFinished { get { return hasFinished; } }

    bool isMoving = false;
    public bool IsMoving { get { return isMoving; } }

    #region Jail related
    bool isInJail = false;
    public bool IsInJail { get { return isInJail; } set { isInJail = value; } }

    int turnsInJail = 0;
    public int TurnsInJail { get { return turnsInJail; } set { turnsInJail = value; } }
    #endregion


    SpriteRenderer sr;
    public SpriteRenderer SR { get { return sr; } }

    // all the spaces on the gameboard
    List<Space> spaces;
    int numSpaces;

    // used to update the money text on the player card
    public delegate void OnMoneyChanged();
    public OnMoneyChanged OnMoneyChangedCallBack;

    public delegate void OnPlayerBankrupt();
    public OnPlayerBankrupt OnPlayerBankruptyCallBack;


    public void Initialize(string name, int initMoney, Color color)
    {
        playerName = name;
        currentMoney = initMoney;
        currentLocation = 0;
        sr = GetComponentInChildren<SpriteRenderer>();
        sr.color = color;

        spaces = BoardManager.instance.spaces;
        numSpaces = BoardManager.numSpaces;
    }

    public void StartTurn()
    {
        hasFinished = false;
    }

    public void FinishTurn()
    {
        hasFinished = true;
    }

    public void GetMoney(int amount)
    {
        currentMoney += amount;
        OnMoneyChangedCallBack.Invoke();
    }

    public void LoseMoney(int amount)
    {
        currentMoney -= amount;
        OnMoneyChangedCallBack.Invoke();
    }

    public void Bankrupt()
    {
        OnPlayerBankruptyCallBack.Invoke();

    }

    public void OwnProperty(Property property)
    {
        properties.Add(property);
    }

    public IEnumerator Move(int steps)
    {
        isMoving = true;
        int oldPos = currentLocation;
        for (int i = 0; i < steps; i++)
        {
            Vector3 translate = spaces[(currentLocation + 1) % numSpaces].transform.position - spaces[currentLocation].transform.position;
            transform.Translate(translate);
            currentLocation = (currentLocation + 1) % numSpaces;
            yield return new WaitForSeconds(0.2f);
        }
        // Pass by GO
        if (currentLocation == oldPos + steps - numSpaces)
            LocPlayerController.instance.PassByGO();

        Space space = spaces[currentLocation];
        space.StepOn(this);

        isMoving = false;
    }

    public void Exchange(int moneyOffered, List<Property> propertiesOffered, 
        int moneyReceived, List<Property> propertiesReceived)
    {
        LoseMoney(moneyOffered);
        GetMoney(moneyReceived);
        foreach(Property prop in propertiesOffered)
        {
            properties.Remove(prop);
        }
        foreach(Property prop in propertiesReceived)
        {
            prop.SoldTo(this);
            properties.Add(prop);
        }
    }

    public void Construct(Land land)
    {
        if (properties.Contains(land))
        {
            land.Upgrade();
        }
    }

    public void Sell(Land land)
    {
        if (properties.Contains(land))
        {
            land.Degrade();
        }
    }

    public void Mortgage(Property property)
    {
        if (properties.Contains(property) && !property.IsMortgaged)
        {
            property.IsMortgaged = true;
            property.ToggleMortgagedBackground();
            GetMoney(property.PurchasePrice / 2);
            Debug.Log(playerName + " mortgaged " + property.PropertyName);
        }
    }

    public void Redeem(Property property)
    {
        if (properties.Contains(property) && property.IsMortgaged)
        {
            property.IsMortgaged = false;
            property.ToggleMortgagedBackground();
            LoseMoney((int)(property.PurchasePrice / 2 * 1.1f));
            Debug.Log(playerName + " redeemed " + property.PropertyName);
        }
    }

    
    public void GetIntoJail(Jail jail)
    {
        isInJail = true;
        
        Vector3 translate = jail.transform.position - transform.position;
        transform.Translate(translate);
        transform.position = new Vector3(transform.position.x, transform.position.y, -1);
        CurrentLocation = jail.LocationIndex;
    }

    public void GetOutOfJail()
    {
        isInJail = false;
        turnsInJail = 0;
    }
}
