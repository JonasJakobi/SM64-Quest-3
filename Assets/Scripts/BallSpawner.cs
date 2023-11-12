using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Spawn physics balls to test collision with physical space.
/// </summary>
public class BallSpawner : MonoBehaviour
{
    public GameObject prefab;
    public bool isLeft = false;

    public float spawnSpeed = 5;

    // Update is called once per frame
    void Update()
    {

        if(OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.LTouch)){
            GameObject spawnedBall = Instantiate(prefab, transform.position, Quaternion.identity);
            spawnedBall.GetComponent<Rigidbody>().velocity = transform.forward * spawnSpeed;
        }
        
        
        
    }
}
