using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Вращение камеры  вокруг точки. 
/// </summary>
public class CameraControl : MonoBehaviour
{

    private Camera m_Camera;
    private Transform camTransform;
    public Transform target;
    // rotate
    public float distance = 10f;
    public float xspeed = 25f;
    public float yspeed = 12f;
    public float xsign = 1f;
    private float x;
    private float y;
    Vector3 prevPos = new Vector3();

    public void Awake()
    {

        m_Camera = Camera.main;
        camTransform = m_Camera.gameObject.transform;

        // get distance
        distance = Vector3.Distance(m_Camera.transform.position, target.transform.position);
        Input.simulateMouseWithTouches = true;

    }

    void LateUpdate()
    {

        // Rotation
        Vector3 forward = camTransform.TransformDirection(Vector3.up); // camera's transform
        Vector3 forward2 = target.TransformDirection(Vector3.up); // target's transform

        if (Vector3.Dot(forward, forward2) < 0) xsign = -1;
        else xsign = 1;

        if (Input.GetMouseButton(0))
        {

            if (prevPos != Vector3.zero && Input.mousePosition != prevPos)
            {

                x += xsign * (Input.mousePosition.x - prevPos.x) * xspeed * 0.02f;
                y -= (Input.mousePosition.y - prevPos.y) * yspeed * 0.02f;
                DoRotation(x, y);

            }

            prevPos = Input.mousePosition;

        }
        else
        {

            prevPos = Vector3.zero;

        }

    }

    void DoRotation(float x, float y)
    {

        Quaternion rotation = Quaternion.Euler(y, x, 0);
        Vector3 position = (rotation * new Vector3(0.0f, 0.0f, -distance)) + target.position;
        camTransform.rotation = rotation;
        camTransform.position = position;

    }

}
