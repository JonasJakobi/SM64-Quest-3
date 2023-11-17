using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LibSM64;
public class MovingPlatform : MonoBehaviour
{


    

    [Header("References")]
    [SerializeField] 
    private Transform node1SpawnLocation;
    [SerializeField] 
    private Transform node2SpawnLocation;
    [SerializeField] 
    private SM64DynamicTerrain platform;
    [SerializeField]
    private GameObject nodePrefab;
    [Header("Variables to tweak")]
    [SerializeField] 
    private float speed = 1f;

    [Header("Visible for debugging")]
    [SerializeField] 
    private bool movingToOne;
    private Transform node1;

    private Transform node2;
    // Start is called before the first frame update
    void Start()
    {
      node1 = Instantiate(nodePrefab, node1SpawnLocation.position, Quaternion.identity).transform;
      node2 = Instantiate(nodePrefab, node2SpawnLocation.position, Quaternion.identity).transform;
    }

    // Update is called once per frame
    void Update()
    {
        MoveTowardsNode();
    }

    private void MoveTowardsNode(){
        
        var movementAmount = speed * Time.deltaTime;
        var node = movingToOne ? node1 : node2;
        //If we are already almost there, just snap to the node and swap direction
        if(Vector3.Distance(platform.transform.position, node.position) < movementAmount){

            platform.SetPosition(node.position);
            movingToOne = !movingToOne;
        }
        //otherwise move
        else{
            platform.SetPosition(Vector3.MoveTowards(platform.position, node.position, movementAmount));
        }
            
    }
   
}
