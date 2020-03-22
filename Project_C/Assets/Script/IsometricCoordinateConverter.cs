using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricCoordinationConverter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Matrix4x4 m = new Matrix4x4(new Vector4(Mathf.Sqrt(3) / Mathf.Sqrt(6), 0f, -Mathf.Sqrt(3) / Mathf.Sqrt(6), 0f),
            new Vector4(1f / Mathf.Sqrt(6), 2f / Mathf.Sqrt(6), 1f / Mathf.Sqrt(6), 0f),
            new Vector4(Mathf.Sqrt(2) / Mathf.Sqrt(6), -Mathf.Sqrt(2) / Mathf.Sqrt(6), Mathf.Sqrt(2) / Mathf.Sqrt(6), 0f),
            new Vector4(0f, 0f, 0f, 1f));

        Quaternion q = new Quaternion();
        q.w = Mathf.Sqrt(Mathf.Max(0, 1 + m[0, 0] + m[1, 1] + m[2, 2])) / 2;
        q.x = Mathf.Sqrt(Mathf.Max(0, 1 + m[0, 0] - m[1, 1] - m[2, 2])) / 2;
        q.y = Mathf.Sqrt(Mathf.Max(0, 1 - m[0, 0] + m[1, 1] - m[2, 2])) / 2;
        q.z = Mathf.Sqrt(Mathf.Max(0, 1 - m[0, 0] - m[1, 1] + m[2, 2])) / 2;
        q.x *= Mathf.Sign(q.x * (m[2, 1] - m[1, 2]));
        q.y *= Mathf.Sign(q.y * (m[0, 2] - m[2, 0]));
        q.z *= Mathf.Sign(q.z * (m[1, 0] - m[0, 1]));

        transform.rotation = q;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
