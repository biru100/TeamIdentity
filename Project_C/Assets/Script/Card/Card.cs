using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class Card
{
    public string CardName { get; protected set; }
    public string CardActionName { get; protected set; }
    /// <summary>
    /// Tokenize '_'
    /// </summary>
    public string CardLoreFormat { get; protected set; }
    public List<float> CardStatus { get; protected set; }

    public Sprite FrontSprite { get; protected set; }
    public Sprite BackSprite { get; protected set; }

    public Card(int i)
    {
        if (i == 0)
        {
            CardName = "Power Attack";
            CardLoreFormat = "범위내 적에게 _ 만큼의 데미지를 준다.";
            CardStatus = new List<float>() { 80f };

            CardActionName = "PlayerPowerAttackAction";

            FrontSprite = ResourceManager.GetResource<Sprite>("Sprites/card_power_atk");
            BackSprite = ResourceManager.GetResource<Sprite>("Sprites/card_sample_back");
        }
        else if (i == 1)
        {
            CardName = "Power Up";
            CardLoreFormat = "카드 주문력을 _ 올려준다.";
            CardStatus = new List<float>() { 50f };

            CardActionName = "PlayerCardPowerUpAction";

            FrontSprite = ResourceManager.GetResource<Sprite>("Sprites/card_atk_up");
            BackSprite = ResourceManager.GetResource<Sprite>("Sprites/card_sample_back");
        }
        else if (i == 2)
        {
            CardName = "Family Kill";
            CardLoreFormat = "같은 방에 있는 동일한 유닛에게 _ 만큼의 데미지를 준다.";
            CardStatus = new List<float>() { 50f };

            CardActionName = "PlayerFamilyKillAction";

            FrontSprite = ResourceManager.GetResource<Sprite>("Sprites/card_family_kill");
            BackSprite = ResourceManager.GetResource<Sprite>("Sprites/card_sample_back");
        }
    }

    public Card(string name, string loreFormat, List<float> status,
        Sprite front, Sprite back)
    {
        CardName = name;
        CardLoreFormat = loreFormat;
        CardStatus = status;

        FrontSprite = front;
        BackSprite = back;
    }

    public string GetLore()
    {
        string[] tokens = CardLoreFormat.Split('_');
        StringBuilder sb = new StringBuilder();
        for(int i = 0; i < tokens.Length; ++i)
        {
            sb.Append(tokens[i]);
            if (tokens.Length != 1 && CardStatus.Count > i)
            {
                sb.Append( ((PlayerStatus.CurrentStatus.CardPowerSupport != PlayerStatus.CurrentStatus.BaseCardPowerSupport) ||
                    (PlayerStatus.CurrentStatus.CardPowerScale != PlayerStatus.CurrentStatus.BaseCardPowerScale) ? "*" : "")
                    + (CardStatus[i] + PlayerStatus.CurrentStatus.CardPowerSupport) * PlayerStatus.CurrentStatus.CardPowerScale);
            }
        }
        return sb.ToString();
    }

}
