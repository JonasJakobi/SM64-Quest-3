using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LibSM64;

public class UpdateGlobalMesh : MonoBehaviour
{
    [SerializeField]
    OVRSceneVolumeMeshFilter oVRSceneVolumeMeshFilter;

    private bool globalMeshDone = false;
    [SerializeField]
    BoxCollider marioCollider;

    private void Start() {
        //Find gameobject with name Mario (this game object will spawn at runtime so Find() is used)
        GameObject mario = GameObject.Find("Mario");
        marioCollider = mario.GetComponent<BoxCollider>();
    }

    void Update()
    {
        if(oVRSceneVolumeMeshFilter.IsCompleted != globalMeshDone){
            globalMeshDone = oVRSceneVolumeMeshFilter.IsCompleted;
            Debug.Log("Global Mesh Done");
            StartCoroutine(UpdateTerrain());
        }


    }
    /// <summary>
    /// SM64 terrain needs to be re-enabled in order to update the mesh collider
    /// </summary>
    /// <returns></returns>
    IEnumerator UpdateTerrain(){
        SM64DynamicTerrain terrain = GetComponent<SM64DynamicTerrain>();
        terrain.enabled = false;
        yield return new WaitForSeconds(0.1f);
        //prevent mario from colliding with the mesh collider while terrain gets re-enabled, as that crashes the game.
        while(marioColliding()){
            Debug.Log("Mario is colliding");
            yield return new WaitForSeconds(0.1f);
        }
        Debug.Log("Enabling Global Mesh!");
        //check if mario is touching our mesh collider
        terrain.enabled = true;
    }


    private bool marioColliding(){
        return marioCollider.bounds.Intersects(GetComponent<MeshCollider>().bounds);
    }
}
