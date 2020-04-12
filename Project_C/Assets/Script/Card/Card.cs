using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class Card
{
    public string CardName { get; protected set; }
    /// <summary>
    /// Tokenize '_'
    /// </summary>
    public string CardLoreFormat { get; protected set; }
    public List<float> CardStatus { get; protected set; }

    public Sprite FrontSprite { get; protected set; }
    public Sprite BackSprite { get; protected set; }

    public Card()
    {
        CardName = "dummy";
        CardLoreFormat = "";
        CardStatus = new List<float>();

        FrontSprite = ResourceManager.GetResource<Sprite>("Sprites/card_sample_front");
        BackSprite = ResourceManager.GetResource<Sprite>("Sprites/card_sample_back");
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
                sb.Append(CardStatus[i]);
            }
        }
        return sb.ToString();
    }

}
