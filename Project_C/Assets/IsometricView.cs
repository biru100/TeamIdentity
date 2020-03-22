using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class IsometricView : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        Quaternion q1 = Quaternion.Inverse(Quaternion.LookRotation(new Vector3(-1, 1, 1).normalized, Vector3.up));
        Quaternion q2 = Quaternion.LookRotation(new Vector3(-1, 1, 1).normalized, Vector3.up);
        File.WriteAllText(Application.dataPath + "/111.txt", q1.x + "\n" + q1.y + "\n" + q1.z + "\n" + q1.w + "\n");
        File.WriteAllText(Application.dataPath + "/222.txt", q2.x + "\n" + q2.y + "\n" + q2.z + "\n" + q2.w + "\n");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
