using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//public enum EditorIOInterface
//{
//    E_IDLE,
//    E_SAVE,
//    E_LOAD
//}

[System.Serializable]
public class RoomWayToggleSet
{
    public MapWay way;
    public Toggle toggle;
}

public class EditorIOSystem : MonoBehaviour
{
    //public EditorIOInterface currentInterface = EditorIOInterface.E_IDLE;
    public GameObject saveInterface;
    public InputField saveMapName;
    public InputField saveMapTheme;
    public InputField saveMapDifficulty;
    public InputField saveMapWeight;
    public List<RoomWayToggleSet> DoorToggle;

    private void Start()
    {
        EnterIdleInterface();
    }

    public void EnterSaveInterface()
    {
        saveInterface.SetActive(true);
    }

    public void EnterLoadInterface()
    {
        SimpleFileBrowser.FileBrowser.ShowLoadDialog(LoadMap, EnterIdleInterface, false,
            Application.dataPath + "/Resources/Map/");
    }

    public void EnterIdleInterface()
    {
        saveInterface.SetActive(false);
    }

    public void SaveMap()
    {
        IsometricTileMap tileMap = FindObjectOfType<IsometricTileMap>();

        int way = 0;

        foreach(var set in DoorToggle)
        {
            way |= set.toggle.isOn ? (int)set.way : 0;
        }

        TileMapData data = new TileMapData()
        {
            mapName = saveMapName.text,
            mapTheme = saveMapTheme.text,
            mapWeight = int.Parse(saveMapWeight.text),
            mapMin = tileMap.min,
            mapMax = tileMap.max,
            mapDifficulty = int.Parse(saveMapDifficulty.text),
            mapData = tileMap.ToJson(),
            mapWay = way
        };

        File.WriteAllText(Application.dataPath + "/Resources/Map/" + data.mapName + ".txt", JsonUtility.ToJson(data));
    }

    public void LoadMap(string path)
    {
        TileMapData data = JsonUtility.FromJson<TileMapData>(File.ReadAllText(path));
        saveMapName.text = data.mapName;
        saveMapTheme.text = data.mapTheme;
        saveMapWeight.text = data.mapWeight.ToString();
        saveMapDifficulty.text = data.mapDifficulty.ToString();

        foreach (var set in DoorToggle)
        {
            set.toggle.isOn = (data.mapWay & (int)set.way) != 0;
        }

        IsometricTileMap tileMap = FindObjectOfType<IsometricTileMap>();
        tileMap.FromJson(data.mapData);
    }
}
