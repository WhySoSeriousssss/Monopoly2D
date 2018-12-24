using UnityEngine;
using UnityEngine.UI;

public class NTurnButtonPanel : MonoBehaviour {

    #region Singleton
    public static NTurnButtonPanel instance;
    private void Awake()
    {
        if (instance != null)
            Debug.Log("Multiple NTurnButtonPanel. Something went wrong");
        instance = this;
    }
    #endregion


    [SerializeField]
    private Button rollButton;
    [SerializeField]
    private Button finishButton;


    // debug panel
    [SerializeField]
    private Transform manualDicePanel;
    [SerializeField]
    private InputField input;
    [SerializeField]
    private Button okButton;
    [SerializeField]
    private Toggle additionalRollToggle;




    private void Update()
    {
        if (NPlayer.thisPlayer.IsMoving && finishButton.interactable == true)
            finishButton.interactable = false;
        else if (!NPlayer.thisPlayer.IsMoving && finishButton.interactable == false)
            finishButton.interactable = true;
    }

    public void Initialize()
    {
        NGameplay.instance.OnMyTurnStartedCallback += OnMyTurnStarted;
        if (NGameplay._isDebug)
        {
            rollButton.gameObject.SetActive(false);
        }
        else
        {
            manualDicePanel.gameObject.SetActive(false);
        }
    }

    public void OnRollButtonClicked()
    {
        if (NGameplay.currentPlayerOrder == NPlayer.thisPlayer.Order)
        {
            NPlayerController.instance.RollDice();

            rollButton.gameObject.SetActive(false);
            finishButton.gameObject.SetActive(true);
        }
    }

    public void OnFinishButtonClicked()
    {
        NPlayer.thisPlayer.HasFinished = true;
        finishButton.gameObject.SetActive(false);
    }

    public void OnMyTurnStarted()
    {
        if (NGameplay._isDebug)
        {
            manualDicePanel.gameObject.SetActive(true);
        }
        else
        {
            rollButton.gameObject.SetActive(true);
        }
        if (NPlayer.thisPlayer.IsInJail)
        {
            manualDicePanel.gameObject.SetActive(false);
            rollButton.gameObject.SetActive(false);
            NDialogManager.instance.CallJailDialog();
            finishButton.gameObject.SetActive(true);
        }
    }


    // debug panel
    public void OnManualDiceButtonClicked()
    {
        if (NGameplay.currentPlayerOrder == NPlayer.thisPlayer.Order)
        {
            int value = int.Parse(input.text);
            bool additinalRoll = additionalRollToggle.isOn;
            NPlayerController.instance.RollDiceManual(value, additinalRoll);

            manualDicePanel.gameObject.SetActive(false);
            finishButton.gameObject.SetActive(true);
        }
    }
}
