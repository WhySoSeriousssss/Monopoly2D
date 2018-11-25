using UnityEngine;
using UnityEngine.UI;

public class NPlayerInfo : MonoBehaviour {

    [SerializeField]
    private Text moneyText;
    [SerializeField]
    private Text nameText;
    [SerializeField]
    private Image playerAvatar;
    [SerializeField]
    private Image background;

    Color normalColor;
    Color highlightedColor;
    Color bankruptcyColor;

    bool isHighlighted = false;

    NPlayer _player;


    public void Initialize(NPlayer player)
    {
        _player = player;
        moneyText.text = "$" + player.CurrentMoney;
        nameText.text = player.photonView.owner.NickName;
        playerAvatar.sprite = player.SR.sprite;
        playerAvatar.color = player.SR.color;

        normalColor = background.color;
        highlightedColor = new Color(1.0f, 0.8f, 0.1f);
        bankruptcyColor = new Color(1.0f, 0f, 0f);

        player.OnMoneyChangedCallBack += UpdateMoney;
        //player.OnPlayerBankruptyCallBack += PlayerBankrupt;
    }

    public void UpdateMoney()
    {
        moneyText.text = "$" + _player.CurrentMoney;
    }

    public void ToggleHighlighted(bool highlight)
    {
        if (isHighlighted != highlight)
        {
            isHighlighted = highlight;
            background.color = isHighlighted ? highlightedColor : normalColor;
        }
    }

    public void PlayerBankrupt()
    {
        background.color = bankruptcyColor;
    }
}
