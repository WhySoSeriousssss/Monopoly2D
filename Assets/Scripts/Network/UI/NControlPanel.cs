using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NControlPanel : MonoBehaviour
{

    #region Singleton
    public static NControlPanel instance;
    private void Awake()
    {
        if (instance != null)
            Debug.Log("Multiple NControlPanel. Something went wrong");
        instance = this;
    }
    #endregion

    // 0: exchange 1: construct 2: Mortgage 3: Sell 4: Redeem
    [SerializeField]
    private Button[] _controlButtons;

    bool buttonActivated = false;
    Button currentActivatedButton;

    bool groupInteractable = true;


    private void Update()
    {
        if (NPlayer.thisPlayer.Order != NGameplay.currentPlayerOrder)
        {
            if (groupInteractable)
                SetGroupInteractable(false);
        }
        else
        {
            if (groupInteractable && NPlayer.thisPlayer.IsMoving)
            {
                SetGroupInteractable(false);
            }
            else if (!groupInteractable && !NPlayer.thisPlayer.IsMoving)
            {
                SetGroupInteractable(true);
            }
        }
    }


    public void OnTradeButtonClicked()
    {
        //ToggleButton(0);
        //if (buttonActivated)
        //{
        if (FindObjectOfType<NSelectTradeeDialog>() == null)
            NDialogManager.instance.CallSelectTradeeDialog(PhotonNetwork.player);
        //}
        //else
        //{
        //    NSelectTradeeDialog std = FindObjectOfType<NSelectTradeeDialog>();
        //    if (std != null)
        //        Destroy(std.gameObject);
        //    NTradingDialog td = FindObjectOfType<NTradingDialog>();
        //    if (td != null)
        //        Destroy(td.gameObject);
        //}
    }

    public void OnConstructButtonClicked()
    {
        ToggleButton(1);
        if (buttonActivated)
        {
            StartCoroutine(WaitForPlayerSelectProperty(1));
        }
    }

    public void OnMortgageButtonClicked()
    {
        ToggleButton(2);
        if (buttonActivated)
        {
            StartCoroutine(WaitForPlayerSelectProperty(2));
        }
    }

    public void OnSellButtonClicked()
    {
        ToggleButton(3);
        if (buttonActivated)
        {
            StartCoroutine(WaitForPlayerSelectProperty(3));
        }
    }

    public void OnRedeemButtonClicked()
    {
        ToggleButton(4);
        if (buttonActivated)
        {
            StartCoroutine(WaitForPlayerSelectProperty(4));
        }
    }

    public IEnumerator WaitForPlayerSelectProperty(int buttonIndex)
    {
        while (buttonActivated)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null && hit.collider.CompareTag("Land"))
                {
                    NLand land = hit.collider.GetComponent<NLand>();
                    if (NPlayer.thisPlayer.Properties.Contains(land))
                    {
                        switch (buttonIndex)
                        {
                            case 1:
                                NPlayerManager.instance.UpgradeLand(land.PropertyID, PhotonNetwork.player);
                                break;
                            case 2:
                                NPlayerManager.instance.MortgageProperty(land.PropertyID, PhotonNetwork.player);
                                break;
                            case 3:
                                NPlayerManager.instance.DegradeLand(land.PropertyID, PhotonNetwork.player);
                                break;
                            case 4:
                                NPlayerManager.instance.RedeemProperty(land.PropertyID, PhotonNetwork.player);
                                break;

                        }
                    }
                }
            }
            yield return null;
        }
    }

    public IEnumerator WaitForPlayerConstructProperty()
    {
        while (buttonActivated)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null && hit.collider.CompareTag("Land"))
                {
                    NLand land = hit.collider.GetComponent<NLand>();
                    if (NPlayer.thisPlayer.Properties.Contains(land))
                    {
                        //NPlayer.thisPlayer.Construct(land);
                    }
                }
            }
            yield return null;
        }
    }

    public IEnumerator WaitForPlayerMortgageProperty()
    {
        while (buttonActivated)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null && (hit.collider.CompareTag("Land") || hit.collider.CompareTag("Railroad")))
                {
                    Property property = hit.collider.GetComponent<Property>();
                    if (property.Owner == NPlayer.thisPlayer)
                    {
                        //NPlayer.thisPlayer.Mortgage(property);
                    }
                }
            }
            yield return null;
        }
    }

    public IEnumerator WaitForPlayerSellProperty()
    {
        while (buttonActivated)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null && hit.collider.CompareTag("Land"))
                {
                    Land land = hit.collider.GetComponent<Land>();
                    if (land.Owner == NPlayer.thisPlayer)
                    {
                        //NPlayer.thisPlayer.Sell(land);
                    }
                }
            }
            yield return null;
        }
    }

    public IEnumerator WaitForPlayerRedeemProperty()
    {
        while (buttonActivated)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null && (hit.collider.CompareTag("Land") || hit.collider.CompareTag("Railroad")))
                {
                    Property property = hit.collider.GetComponent<Property>();
                    if (property.Owner == NPlayer.thisPlayer)
                    {
                        //NPlayer.thisPlayer.Redeem(property);
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
            _controlButtons[buttonIndex].GetComponent<NControlButton>().ToggleButtonText();
            currentActivatedButton = _controlButtons[buttonIndex];
            foreach (Button b in _controlButtons)
            {
                if (b != _controlButtons[buttonIndex])
                    b.interactable = false;
            }
        }
        else
        {
            DeactiveCurrentButton();
            foreach (Button b in _controlButtons)
            {
                b.interactable = true;
            }
        }
    }

    void DeactiveCurrentButton()
    {

        buttonActivated = false;
        currentActivatedButton.GetComponent<NControlButton>().ToggleButtonText();
        currentActivatedButton = null;
    }

    void SetGroupInteractable(bool interactable)
    {
        foreach (Button button in _controlButtons)
            button.interactable = interactable;

        groupInteractable = interactable;
    }

}
