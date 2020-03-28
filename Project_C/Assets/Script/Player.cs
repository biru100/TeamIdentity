using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    Rigidbody _body;
    Animator _anim;

    int _renderAngle;

    Transform _renderTransform;

    // Start is called before the first frame update
    void Start()
    {
        _body = GetComponent<Rigidbody>();
        _anim = GetComponentInChildren<Animator>();

        _renderAngle = (int)(Mathf.Round(transform.rotation.eulerAngles.y / 45f) * 45f);        
        _anim.Play("idle_" + _renderAngle);

        _renderTransform = GetComponentInChildren<RenderTransform>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 velocity = Vector3.zero;

        if(Input.GetKey(KeyCode.UpArrow))
        {
            velocity += new Vector3(-1f, 0f, 1f);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            velocity += new Vector3(1f, 0f, -1f);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            velocity += new Vector3(1f, 0f, 1f);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            velocity += new Vector3(-1f, 0f, -1f);
        }

        if(velocity.magnitude > 0.01f)
        {
            velocity.Normalize();
            transform.rotation = Quaternion.LookRotation(velocity, Vector3.up);
            transform.position += velocity * 1.1f * Time.deltaTime;
        }

        int currentAngle = (int)(Mathf.Round(transform.rotation.eulerAngles.y / 45f) * 45);

        if(currentAngle != _renderAngle)
            _anim.Play("idle_" + (int)currentAngle);

        _renderAngle = currentAngle;

        Vector3 zIgnorePosition = _renderTransform.position;
        zIgnorePosition.z = Camera.main.transform.position.z;
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, zIgnorePosition, 3f * Time.deltaTime);
    }
}
