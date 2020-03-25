using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[RequireComponent(typeof(RectTransform))]
public class TilePreview : MonoBehaviour
{
    //prefab
    void Start()
    {
        GameObject elementPrefab = ResourceManager.GetResource<GameObject>("UI/TilePreviewElement");
        float height = ((RectTransform)elementPrefab.transform).sizeDelta.y;
        float intervalHeight = 50f;

        DirectoryInfo di = new DirectoryInfo(Application.dataPath + "/Resources/Tiles/");
        FileInfo[] info = di.GetFiles("*.prefab");

        RectTransform rtransform = (RectTransform)transform;
        rtransform.sizeDelta = new Vector2(rtransform.sizeDelta.x, height * info.Length + Mathf.Max(info.Length - 1, 0) * intervalHeight);

        for(int i = 0; i < info.Length; ++i)
        {
            GameObject element = 
                Instantiate(elementPrefab, transform);

            element.transform.localPosition = new Vector3(0f, -i * (height + intervalHeight), 0f);

            element.GetComponent<TilePreviewElement>().tile = ResourceManager.GetResource<GameObject>("Tiles/" + info[i].Name.Split('.')[0]);
        }
    }
}
