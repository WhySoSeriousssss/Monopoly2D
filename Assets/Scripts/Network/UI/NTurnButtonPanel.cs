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
    }

    public void OnRollButtonClicked()
    {
        if (NGameplay.currentPlayerOrder == NPlayer.thisPlayer.Order)
        {
            NPlayerManager.instance.RollDice();
        }

        rollButton.gameObject.SetActive(false);
        finishButton.gameObject.SetActive(true);
    }

    public void OnFinishButtonClicked()
    {
        NPlayerManager.instance.FinishTurn();
        finishButton.gameObject.SetActive(false);
    }

    public void OnMyTurnStarted()
    {
        rollButton.gameObject.SetActive(true);
    }
}
