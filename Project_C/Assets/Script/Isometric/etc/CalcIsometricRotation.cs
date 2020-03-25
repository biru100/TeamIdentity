using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CalcIsometricRotation : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        Quaternion q = Quaternion.AngleAxis(-Mathf.Rad2Deg * Mathf.Asin(33f / 68f), new Vector3(1f, 0f, 1f).normalized);
        Quaternion q1 = Quaternion.Euler(0f, 45f, 0f);

        transform.rotation = q1 * q;

        Quaternion isoToWorld = transform.rotation;
        Quaternion worldToIso = Quaternion.Inverse(isoToWorld);

        File.WriteAllText(Application.dataPath + "/IsometricToWorld.txt", isoToWorld.x + "\t" +
            isoToWorld.y + "\t" +
            isoToWorld.z + "\t" +
            isoToWorld.w);

        File.WriteAllText(Application.dataPath + "/WorldToIsometric.txt", worldToIso.x + "\t" +
            worldToIso.y + "\t" +
            worldToIso.z + "\t" +
            worldToIso.w);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
