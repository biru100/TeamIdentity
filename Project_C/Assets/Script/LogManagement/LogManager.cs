using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;

public interface ILogClass
{
    string ToLogString();
}

public interface ILogPrefix
{
    string ToLogPrefix();
}

public class BaseLogPrefix : ILogPrefix
{
    public static BaseLogPrefix GetInstance() { return new BaseLogPrefix(); }

    public DateTime Time { get; set; }

    public BaseLogPrefix()
    {
        Time = DateTime.Now;
    }

    public virtual string ToLogPrefix()
    {
        return Time.ToString();
    }
}

public class EventLogPrefix : BaseLogPrefix
{
    public static new EventLogPrefix GetInstance() { return new EventLogPrefix(); }

    public EventLogPrefix()
    {
        RoomIndex = RoomManager.Instance.CurrentRoom.RoomIndex;
        RoomName = RoomManager.Instance.CurrentRoom.MapData.mapName;
    }

    public Vector2Int RoomIndex { get; set; }
    public string RoomName { get; set; }

    public override string ToLogPrefix()
    {
        return base.ToLogPrefix() + "\troom index : " + RoomIndex.ToString() + "\troom name : " + RoomName;
    }
}


public class BaseLogData : ILogClass
{
    public enum LogType
    {
        E_Basic,
        E_UseCard,
        E_BurnCard,
        E_AddState,
        E_ChangeRoom,
        E_FactoringRoom
    }

    public LogType Type{ get; set; }
    public BaseLogPrefix LogPrefix { get; set; }

    public virtual string ToLogString()
    {
        return LogPrefix.ToLogPrefix();
    }
}

public class BasicLogData : BaseLogData
{
    public static BasicLogData GetInstance() { return new BasicLogData(); }

    public string Log { get; set; }

    public BasicLogData()
    {
        Type = LogType.E_Basic;
    }

    public void Init(string log)
    {
        LogPrefix = BaseLogPrefix.GetInstance();
        Log = log;
        LogManager.Instance.AddLog(this);
    }

    public override string ToLogString()
    {
        return base.ToLogString() + "\t" + Log + "\n";
    }
}

public class UseCardLogData : BaseLogData
{
    public static UseCardLogData GetInstance() { return new UseCardLogData(); }

    public int CardIndex { get; set; }
    public int UseCost { get; set; }
    public string CardName { get; set; }

    public UseCardLogData()
    {
        Type = LogType.E_BurnCard;
    }

    public void Init(CardInterface card)
    {
        LogPrefix = EventLogPrefix.GetInstance();
        CardIndex = card.CardData.Data._Index;
        CardName = card.CardData.Data._krName;
        UseCost = card.CardData.Data._Cost;

        LogManager.Instance.AddLog(this);
    }

    public override string ToLogString()
    {
        return base.ToLogString() + "\tUseCardLog\tcardIndex : " + CardIndex + "/tcardName : " + CardName + "\n";
    }
}

public class BurnCardLogData : BaseLogData
{
    public static BurnCardLogData GetInstance() { return new BurnCardLogData(); }

    public int CardIndex { get; set; }
    public string CardName { get; set; }

    public BurnCardLogData()
    {
        Type = LogType.E_BurnCard;
    }

    public void Init(Card card)
    {
        LogPrefix = EventLogPrefix.GetInstance();
        CardIndex = card.Data._Index;
        CardName = card.Data._krName;

        LogManager.Instance.AddLog(this);
    }

    public override string ToLogString()
    {
        return base.ToLogString() + "\tBurnCardLog\tcardIndex : " + CardIndex + "\tcardName : " + CardName + "\n";
    }
}

public class AddStateLogData : BaseLogData
{
    public static AddStateLogData GetInstance() { return new AddStateLogData(); }

    public CharacterStateType StateType { get; set; }
    public string OwnerName { get; set; }
    public float Duration { get; set; }
    public float Amount { get; set; }

    public AddStateLogData()
    {
        Type = LogType.E_AddState;
    }

    public void Init(CharacterStateType stateType, string ownerName, float duration, float amount)
    {
        LogPrefix = EventLogPrefix.GetInstance();
        StateType = stateType;
        OwnerName = ownerName;
        Duration = duration;
        Amount = amount;

        LogManager.Instance.AddLog(this);
    }

    public override string ToLogString()
    {
        return base.ToLogString() + "\tAddStateLog\towner name : " + OwnerName + "\tstate : " + StateType.ToString() + "\tduration : " + Duration + "\tamount : " + Amount + "\n";
    }
}

public class ChangeRoomLogData : BaseLogData
{
    public static ChangeRoomLogData GetInstance() { return new ChangeRoomLogData(); }

    public Vector2Int RoomIndex { get; set; }
    public string RoomName { get; set; }

    public ChangeRoomLogData()
    {
        Type = LogType.E_ChangeRoom;
    }

    public void Init(Room changedRoom)
    {
        LogPrefix = BaseLogPrefix.GetInstance();
        RoomIndex = changedRoom.RoomIndex;
        RoomName = changedRoom.MapData.mapName;

        LogManager.Instance.AddLog(this);
    }

    public override string ToLogString()
    {
        return base.ToLogString() + "\tChangeRoomLog\troom index : " + RoomIndex + "\troom name : " + RoomName + "\n";
    }
}


public class FactoringRoomLogData : BaseLogData
{
    public static FactoringRoomLogData GetInstance() { return new FactoringRoomLogData(); }

    public string Data { get; set; }

    public FactoringRoomLogData()
    {
        Type = LogType.E_FactoringRoom;
    }

    public void Init()
    {
        LogPrefix = BaseLogPrefix.GetInstance();
        Data = "";
        foreach (var rc in RoomManager.Instance.AllRoom)
        {
            Data += "room index : " + rc.RoomIndex + "\troom name : " + rc.RoomInstance.MapData.mapName + "\n";
        }

        LogManager.Instance.AddLog(this);
    }

    public override string ToLogString()
    {
        return base.ToLogString() + "\t" + Data;
    }
}


public class LogManager : BehaviorSingleton<LogManager>
{
    static List<BaseLogData> LogDatas { get; set; }

    protected override void Init()
    {
        base.Init();
        Application.logMessageReceived += _instance.AddErrorLog;
    }

    public void AddErrorLog(string logString, string stackTrace, LogType type)
    {
        if(type == LogType.Error || type == LogType.Exception)
            LogDatas.Add(new BasicLogData() { LogPrefix = BaseLogPrefix.GetInstance(), Log = logString + "\n" + stackTrace, Type = BaseLogData.LogType.E_Basic });
    }

    public void AddLog(BaseLogData logData)
    {
        if(LogDatas == null)
        {
            LogDatas = new List<BaseLogData>(1000);
        }

        LogDatas.Add(logData);
        PlayingDataManager.ReferencingLog(logData);

        if(LogDatas.Count == 1000)
        {
            SaveLog();
        }
    }

    void SaveLog()
    {
        StringBuilder sb = new StringBuilder();
        foreach(var log in LogDatas)
        {
            sb.Append(log.ToLogString());
        }

        DirectoryInfo di = new DirectoryInfo(Application.persistentDataPath + "/MagiaCarta");
        if (!di.Exists)
            di.Create();

        FileInfo fi = new FileInfo(Application.persistentDataPath + "/MagiaCarta/" + DateTime.Now.ToShortDateString() + "_LogData.txt");
        if (!fi.Exists)
        {
            File.WriteAllText(Application.persistentDataPath + "/MagiaCarta/" + DateTime.Now.ToShortDateString() + "_LogData.txt", sb.ToString());
        }
        else
        {
            string preData = File.ReadAllText(Application.persistentDataPath + "/MagiaCarta/" + DateTime.Now.ToShortDateString() + "_LogData.txt");
            File.WriteAllText(Application.persistentDataPath + "/MagiaCarta/" + DateTime.Now.ToShortDateString() + "_LogData.txt", preData + sb.ToString());
        }

        LogDatas.Clear();
    }

    protected override void OnDestroy()
    {
        LogDatas.Add(new BasicLogData() { LogPrefix = BaseLogPrefix.GetInstance(), Log = "-----------------game end" , Type = BaseLogData.LogType.E_Basic});
        SaveLog();
        if(_instance == this)
            Application.logMessageReceived -= _instance.AddErrorLog;
        base.OnDestroy();
    }
}
