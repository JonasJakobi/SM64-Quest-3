using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LibSM64;
/// <summary>
/// Invisible copy of the platform that can be grabbed by the player and moved around.
/// </summary>
public class PlatformGrabbable : MonoBehaviour
{
    SM64DynamicTerrain platform;
    // Start is called before the first frame update
    void Start()
    {
        //Detach from parent object but keep position and rotation
        platform = transform.parent.GetComponent<SM64DynamicTerrain>();
        transform.parent = null;


    }

    // Update is called once per frame
    void Update()
    {
        if(platform == null || platform.gameObject == null){
            Destroy(gameObject);
        }
        
        if (IsMoved())
        {
            //move the platform to our position
            platform.SetPosition(transform.position);
            platform.SetRotation(transform.rotation);
        }
    }

    private bool IsMoved()
    {
        return transform.position != platform.position || transform.rotation != platform.rotation;
    }

}
