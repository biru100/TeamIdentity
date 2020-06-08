using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

[Serializable]
public class UserCardData
{
    public UserCardData(int index, int count)
    {
        cardIndex = index;
        cardCount = count;
    }

    public int cardIndex;
    public int cardCount;
}

[Serializable]
public class DeckData
{
    public string DeckName = "새로운 덱";
    public bool IsPrepareToUse;
    public int CardCount;
    public List<UserCardData> DeckCards = new List<UserCardData>();
}

[Serializable]
public class UserData
{
    protected UserData()
    {

    }

    protected void SerDefaultCard()
    {
        List<CardTable> cardList = DataManager.GetDatas<CardTable>();
        OwnedCardList = new List<UserCardData>();
        foreach (var ct in cardList)
        {
            OwnedCardList.Add(new UserCardData(ct._Index, 5));
        }
    }

    protected static UserData _instance = null;

    public static UserData Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = LoadFile();
                SaveFile();
            }
            return _instance;
        }
    }

    public static UserData LoadFile()
    {
        string path = Application.persistentDataPath + "/MagiaCarta/SaveDat.userData";
        if (File.Exists(path))
        {
            string data = File.ReadAllText(Application.persistentDataPath + "/MagiaCarta/SaveDat.userData");
            return JsonUtility.FromJson<UserData>(data);
        }

        UserData us = new UserData();
        us.SerDefaultCard();
        return us;
    }

    public static void SaveFile()
    {
        string data = JsonUtility.ToJson(_instance);

        DirectoryInfo di = new DirectoryInfo(Application.persistentDataPath + "/MagiaCarta");
        if (!di.Exists)
            di.Create();

        File.WriteAllText(Application.persistentDataPath + "/MagiaCarta/SaveDat.userData", data);
    }

    public List<UserCardData> OwnedCardList = new List<UserCardData>();
    public List<DeckData> OwnedDeckList = new List<DeckData>();
}
