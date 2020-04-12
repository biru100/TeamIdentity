using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Data;
using System.IO;
using Mono.Data.Sqlite;

public class SqliteManager : BehaviorSingleton<SqliteManager>
{
    public static readonly string DBPath = Application.dataPath + "/GameDB.db";
    public static readonly string DBConnectionPath = "URI=file:" + Application.dataPath + "/GameDB.db";

    IDbConnection Connection { get; set; }
    bool IsConnect { get => Connection != null ? Connection.State == ConnectionState.Open : false; }

    public void Initialize()
    {
        InitializeDBFile();
        DBConnect();

        if(IsConnect)
        {

        }
    }

    void InitializeDBFile()
    {
        File.Copy(Application.streamingAssetsPath + "/GameDB.db", DBPath);
    }

    void DBConnect()
    {
        try
        {
            Connection = new SqliteConnection(DBConnectionPath);
            Connection.Open();

            if (Connection.State == ConnectionState.Open)
            {
                Debug.Log("Connection");
            }
            else
            {
                Connection = null;
                Debug.Log("Not Connection");
            }
        }
        catch(Exception e)
        {
            Connection = null;
            Debug.Log(e);
        }
    }

    IEnumerator Parse()
    {
        yield return null;
    }

    void Test()
    {
        if(IsConnect)
        {
            IDbCommand command = Connection.CreateCommand();
            command.CommandText = "Select * From Item";
            IDataReader dataReader = command.ExecuteReader();

            while(dataReader.Read())
            {
                Debug.Log(dataReader.GetString(0) + " , " + dataReader.GetInt32(1));
            }

            dataReader.Dispose();
            command.Dispose();
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if(Connection != null)
        {
            Connection.Close();
            Connection = null;
        }
    }
}
