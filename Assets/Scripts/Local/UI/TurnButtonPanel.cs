using UnityEngine;
using UnityEngine.UI;

public class TurnButtonPanel : MonoBehaviour {

    #region Singleton
    public static TurnButtonPanel instance;
    private void Awake()
    {
        if (instance != null)
            Debug.Log("Multiple BoardManager. Something went wrong");
        instance = this;
    }
    #endregion

    public Button rollButton;
    public Button finishButton;
    public Button bankruptcyButton;
    public DicePointSelectPanel dpsp;

    Player currentPlayer;


    private void Start()
    {
        LocGameManager.instance.OnCurrentPlayerChangedCallBack += SetCurrentPlayer;
    }

    void SetCurrentPlayer(Player player)
    {
        currentPlayer = player;
        ToggleButtons(false);
    }

    private void Update()
    {
        if (finishButton.IsActive())
        {
            if (finishButton.interactable && currentPlayer.IsMoving == true)
            {
                finishButton.interactable = false;
            }
            else if (!finishButton.interactable && !currentPlayer.IsMoving)
            {
                finishButton.interactable = true;
            }
        }
    }

    public void OnRollButtonClicked()
    {
        if (!LocGameManager.instance.debugOn)
        {
            LocPlayerController.instance.ClickRollButton();
        }
        else
        {
            dpsp.gameObject.SetActive(true);
        }
        ToggleButtons(true);
    }


    public void OnFinishButtonClicked()
    {
        currentPlayer.FinishTurn();
    }

    public void OnBankruptButtonClicked()
    {
        currentPlayer.Bankrupt();
    }

    
    // toggle between Roll and Finish buttons
    // false:  Roll. true: Finish
    public void ToggleButtons(bool hasRolled)
    {
        rollButton.gameObject.SetActive(!hasRolled);
        finishButton.gameObject.SetActive(hasRolled);
    }

    
}
