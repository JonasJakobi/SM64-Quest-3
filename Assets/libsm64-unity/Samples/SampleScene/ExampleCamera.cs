using System;
using UnityEngine;
using LibSM64;

public class ExampleCamera : MonoBehaviour
{
    [SerializeField] GameObject target = null;
    [SerializeField] float radius = 15;
    [SerializeField] float elevation = 5;

    void LateUpdate()
    {
        var m = target.transform.position;
        var n = transform.position;
        m.y = 0;
        n.y = 0;
        n = (n - m).normalized * radius;
        n = Quaternion.AngleAxis( Input.GetAxis("HorizontalCam"), Vector3.up ) * n;
        n += m;
        n.y = target.transform.position.y + elevation;

        transform.position = n;
        transform.LookAt( target.transform.position );
    }
}
