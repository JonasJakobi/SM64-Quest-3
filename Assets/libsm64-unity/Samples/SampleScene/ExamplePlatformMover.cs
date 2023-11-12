using UnityEngine;
using LibSM64;

public class ExamplePlatformMover : MonoBehaviour
{
    SM64DynamicTerrain platform;
    Quaternion rotStep;
    Vector3 ogPos;
    
    void Start()
    {
        ogPos = transform.position;
        platform = GetComponent<SM64DynamicTerrain>();
        rotStep = Quaternion.AngleAxis( 1f, Random.onUnitSphere );
    }

    void FixedUpdate()
    {
        platform.SetPosition( ogPos + Vector3.right * 50.0f * Mathf.Cos( 0.1f * Time.fixedTime + Mathf.PI ));
        platform.SetRotation( platform.rotation * rotStep );
    }
}
