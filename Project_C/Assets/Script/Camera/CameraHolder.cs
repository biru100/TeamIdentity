using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHolder : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.CurrentPlayer == null)
            return;

        RenderTransform target = Player.CurrentPlayer.RenderTrasform;

        float z = transform.position.z;
        Vector3 targetPos = target.transform.position;
        targetPos.z = z;

        transform.position = Vector3.Lerp(transform.position, targetPos, 4f * Time.deltaTime);
    }
}
