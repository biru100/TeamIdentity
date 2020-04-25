using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Linq;

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

    public CardTargetType TargetType { get; protected set; }

    public CardTable Data { get; protected set; }

    //temp
    public Card(int i)
    {
        CardTable data = DataManager.GetDatas<CardTable>()[i];
        Data = data;
        CardName = data._krName;
        CardLoreFormat = data._Lore;
        CardStatus = data._Parameter.ToList();
        CardActionName = data._FSM;
        TargetType = data._TargetType;

        FrontSprite = ResourceManager.GetResource<Sprite>(data._ImagePath);
        BackSprite = ResourceManager.GetResource<Sprite>("Sprites/card_sample_back");
    }

    public Card(CardTable data)
    {
        Data = data;
        CardName = data._krName;
        CardLoreFormat = data._Lore;
        CardStatus = data._Parameter.ToList();
        CardActionName = data._FSM;
        TargetType = data._TargetType;

        FrontSprite = ResourceManager.GetResource<Sprite>(data._ImagePath);
        BackSprite = ResourceManager.GetResource<Sprite>("Sprites/card_sample_back");
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
