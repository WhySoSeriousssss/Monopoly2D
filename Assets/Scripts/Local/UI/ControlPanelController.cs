using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ControlPanelController : MonoBehaviour {

    #region Singleton
    public static ControlPanelController instance;
    private void Awake()
    {
        if (instance != null)
            Debug.Log("Multiple ControlPanelController. Something went wrong");
        instance = this;
    }
    #endregion

    // 0: exchange 1: construct 2: Mortgage 3: Sell 4: Redeem
    public Button[] controlButtons;

    Player currentPlayer;

    bool buttonActivated = false;
    Button currentActivatedButton;

    bool groupInteractable = false;

    private void Start()
    {
        LocGameManager.instance.OnCurrentPlayerChangedCallBack += SetCurrentPlayer;
    }
    
    private void Update()
    {
        if (currentPlayer == null) return;
        if (groupInteractable && currentPlayer.IsMoving)
        {
            SetGroupInteractable(false);
        }
        else if (!groupInteractable && !currentPlayer.IsMoving)
        {
            SetGroupInteractable(true);
        }
    }
    

    public void TradeButtonOnClick()
    {
        ToggleButton(0);
        if (buttonActivated)
        {
            DialogHandler.instance.CallTradingDialog(currentPlayer);
        }
        else
        {
            TradingDialog td = FindObjectOfType<TradingDialog>();
            if (td != null)
                Destroy(td.gameObject);
        }
    }

    public void ConstructButtonOnClick()
    {
        ToggleButton(1);
        if (buttonActivated)
        {
            StartCoroutine(WaitForPlayerConstructProperty());
        }
    }

    public void MortgageButtonOnClick()
    {
        ToggleButton(2);
        if (buttonActivated)
        {
            StartCoroutine(WaitForPlayerMortgageProperty());
        }
    }

    public void SellButtonOnClick()
    {
        ToggleButton(3);
        if (buttonActivated)
        {
            StartCoroutine(WaitForPlayerSellProperty());
        }
    }

    public void RedeemButtonOnClick()
    {
        ToggleButton(4);
        if (buttonActivated)
        {
            StartCoroutine(WaitForPlayerRedeemProperty());
        }
    }

    IEnumerator WaitForPlayerConstructProperty()
    {
        while (buttonActivated)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null && hit.collider.CompareTag("Land"))
                {
                    Land land = hit.collider.GetComponent<Land>();
                    if (land.Owner == currentPlayer)
                    {
                        currentPlayer.Construct(land);
                    }
                }
            }
            yield return null;
        }
    }

    IEnumerator WaitForPlayerMortgageProperty()
    {
        while(buttonActivated)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null && (hit.collider.CompareTag("Land") || hit.collider.CompareTag("Railroad")))
                {
                    Property property = hit.collider.GetComponent<Property>();
                    if (property.Owner == currentPlayer)
                    {                        
                        currentPlayer.Mortgage(property);
                    }
                }
            }
            yield return null;
        }
    }

    IEnumerator WaitForPlayerSellProperty()
    {
        while (buttonActivated)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null && hit.collider.CompareTag("Land"))
                {
                    Land land = hit.collider.GetComponent<Land>();
                    if (land.Owner == currentPlayer)
                    {
                        currentPlayer.Sell(land);
                    }
                }
            }
            yield return null;
        }
    }

    IEnumerator WaitForPlayerRedeemProperty()
    {
        while (buttonActivated)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null && (hit.collider.CompareTag("Land") || hit.collider.CompareTag("Railroad")))
                {
                    Property property = hit.collider.GetComponent<Property>();
                    if (property.Owner == currentPlayer)
                    {
                        currentPlayer.Redeem(property);
                    }
                }
            }
            yield return null;
        }
    }

    void ToggleButton(int buttonIndex)
    {
        if (!buttonActivated)
        {
            buttonActivated = true;
            controlButtons[buttonIndex].GetComponent<ControlButton>().ToggleButtonText();
            currentActivatedButton = controlButtons[buttonIndex];
            foreach (Button b in controlButtons)
            {
                if (b != controlButtons[buttonIndex])
                    b.interactable = false;
            }
        }
        else
        {
            DeactiveCurrentButton();
            foreach (Button b in controlButtons)
            {
                b.interactable = true;
            }
        }
    }

    void DeactiveCurrentButton()
    {

        buttonActivated = false;
        currentActivatedButton.GetComponent<ControlButton>().ToggleButtonText();
        currentActivatedButton = null;
    }

    void SetGroupInteractable(bool interactable)
    {
        foreach (Button button in controlButtons)
            button.interactable = interactable;

        groupInteractable = interactable;
    }

    void SetCurrentPlayer(Player player)
    {
        if (buttonActivated)
            DeactiveCurrentButton();
        groupInteractable = true;
        SetGroupInteractable(groupInteractable);
        currentPlayer = player;
    }
}
