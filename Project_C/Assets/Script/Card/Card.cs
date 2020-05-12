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
    public List<bool> IsVariableStatus { get; protected set; }

    public int Cost { get; protected set; }
    public int ParameterCount { get; protected set; }

    public Sprite FrontSprite { get; protected set; }
    public Sprite BackSprite { get; protected set; }

    public CardTargetType TargetType { get; protected set; }

    public CardTable Data { get; protected set; }

    //temp
    public Card(int i)
    {
        CardTable data = DataManager.GetDatas<CardTable>()[i];
        Data = data;
        Cost = data._Cost;
        CardName = data._krName;
        CardLoreFormat = data._Lore;
        CardStatus = data._Parameter.ToList();
        IsVariableStatus = data._IsVariable.ToList();
        ParameterCount = data._ParameterCount;
        CardActionName = data._FSM;
        TargetType = data._TargetType;

        FrontSprite = ResourceManager.GetResource<Sprite>(data._ImagePath);
        BackSprite = ResourceManager.GetResource<Sprite>("Sprites/card_sample_back");
    }

    public Card(CardTable data)
    {
        Data = data;
        Cost = data._Cost;
        CardName = data._krName;
        CardLoreFormat = data._Lore;
        CardStatus = data._Parameter.ToList();
        IsVariableStatus = data._IsVariable.ToList();
        ParameterCount = data._ParameterCount;
        CardActionName = data._FSM;
        TargetType = data._TargetType;

        FrontSprite = ResourceManager.GetResource<Sprite>(data._ImagePath);
        BackSprite = ResourceManager.GetResource<Sprite>("Sprites/card_sample_back");
    }

    public string GetLore()
    {
        string[] tokens = CardLoreFormat.Split('_');
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < tokens.Length; ++i)
        {
            sb.Append(tokens[i]);
            if (tokens.Length != 1 && ParameterCount > i)
            {
                sb.Append(CardStatus[i]);
            }
        }
        return sb.ToString();
    }

    public string GetLore(PlayerStatus player)
    {
        string[] tokens = CardLoreFormat.Split('_');
        StringBuilder sb = new StringBuilder();
        for(int i = 0; i < tokens.Length; ++i)
        {
            sb.Append(tokens[i]);
            if (tokens.Length != 1 && ParameterCount > i)
            {
                sb.Append(IsVariableStatus[i] ? (player.CardPowerSupport > player.BaseCardPowerSupport ? "*" : "") +
                    (CardStatus[i] + player.CardPowerSupport).ToString() : CardStatus[i].ToString());
            }
        }
        return sb.ToString();
    }

}
