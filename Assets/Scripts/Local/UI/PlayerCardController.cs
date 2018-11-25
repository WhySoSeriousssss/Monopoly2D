using UnityEngine;
using UnityEngine.UI;

public class PlayerCardController : MonoBehaviour {

    public Text moneyText;
    public Text nameText;
    public Image playerAvatar;
    public Image background;

    Color normalColor;
    Color highlightedColor;
    Color bankruptcyColor;

    bool isHighlighted = false;

    Player player;


    public void Initialize(Player player)
    {
        this.player = player;
        moneyText.text = "$" + player.CurrentMoney;
        nameText.text = player.PlayerName;
        playerAvatar.sprite = player.SR.sprite;
        playerAvatar.color = player.SR.color;

        normalColor = background.color;
        highlightedColor = new Color(1.0f, 0.8f, 0.1f);
        bankruptcyColor = new Color(1.0f, 0f, 0f);

        player.OnMoneyChangedCallBack += UpdateMoney;
        player.OnPlayerBankruptyCallBack += PlayerBankrupt;
    }

    public void UpdateMoney()
    {
        moneyText.text = "$" + player.CurrentMoney;
    }

    public void ToggleHighlighted()
    {
        isHighlighted = !isHighlighted;
        background.color = isHighlighted ? highlightedColor : normalColor;
    }

    public void PlayerBankrupt()
    {
        background.color = bankruptcyColor;
    }
}
