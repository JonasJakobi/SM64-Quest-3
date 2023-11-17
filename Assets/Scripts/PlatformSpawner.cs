using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LibSM64;
using System.Xml;
using TMPro;

public class PlatformSpawner : MonoBehaviour
{
    
    public TextMeshPro buildUI;
    public Transform controllerPoint;
    public SpawnableTerrainMenuItem[] spawnableTerrain;
    private MeshRenderer SpawningPointRenderer;
    public Material deleteModeMaterial;
    
    public GameObject previewPrefab;
    public bool isLeft = false;

    private Vector3 startPoint;
    private Vector3 endPoint;
    private int prefabIndex = 0;
    public float spawnSpeed = 5;

    private GameObject previewRectangle;
    [SerializeField]  
    private bool deleteMode = false;
    Vector3 center;
    Vector3 size;
    private void Start() {
        SpawningPointRenderer = controllerPoint.GetComponent<MeshRenderer>();
        UpdateUIAndMaterial();
    }
 
    void Update()
    {
       
        //if thumbstick is pressed down, turn on "delete mode"
        if(OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.RTouch)){
            deleteMode = !deleteMode;
            UpdateUIAndMaterial();
        }
        //if delete mode is on, try to delete
        if(deleteMode ){
            if(OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch)){
                TryDelete();
            }
            return;
        }
        /// -----------------IF IN DELETE MODE, FOLLOWING CODE WILL NOT RUN -------------------
        //if stick is pressed left or right, change prefab
        if(OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickLeft, OVRInput.Controller.RTouch)){
            ChangePrefab(false);
        }
        if(OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickRight, OVRInput.Controller.RTouch)){
            ChangePrefab(true);
        }
        //When button pressed, set the start point and instantiate the preview rectangle
        if(OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch)){
            
            startPoint = controllerPoint.position;
            previewRectangle = Instantiate(previewPrefab, startPoint, Quaternion.identity);
            previewRectangle.GetComponent<MeshRenderer>().material = spawnableTerrain[prefabIndex].previewMaterial;
        }

        
        //While button held, update the preview
        if(OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch)){
            
            endPoint = controllerPoint.position;
            UpdateCenterAndSize();
            UpdatePreviewRectangle();
        }
        //When button released, spawn the rectangle
        if(OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch)){
            Spawn();
        }
        
    }
    private void TryDelete(){
        //Check at the controllerPoint position for a Deletable
        Collider[] colliders = Physics.OverlapSphere(controllerPoint.position, 0.1f);
        Debug.Log("Found " + colliders.Length + " colliders to delete");
        foreach(Collider c in colliders){
            //if gameobject or parent has Deleteable tag, delete it
            if(c.transform.parent != null && c.transform.parent.gameObject.CompareTag("Deletable")){
                Destroy(c.transform.parent.gameObject);
                return;
            }
            if(c.gameObject.CompareTag("Deletable")){
                Destroy(c.gameObject);
                return;
            }
        }
        
    }
    private void ChangePrefab(bool inc){
        prefabIndex += inc ? 1 : -1;
        if(prefabIndex < 0){
            prefabIndex = spawnableTerrain.Length - 1;
        }
        else if (prefabIndex >= spawnableTerrain.Length){
            prefabIndex = 0;
        }
        UpdateUIAndMaterial();
        

    }
    private void UpdateUIAndMaterial(){
        if(deleteMode){
            SpawningPointRenderer.material = deleteModeMaterial;
            buildUI.text = "Delete";
            return;
        }
        else{
            SpawningPointRenderer.material = spawnableTerrain[prefabIndex].material;
            buildUI.text = spawnableTerrain[prefabIndex].name;
        }
        
    }


    private void UpdateCenterAndSize(){
        center = (startPoint + endPoint) / 2f;
        // Calculate the size of the rectangle
        size = new Vector3(Mathf.Abs(endPoint.x - startPoint.x), Mathf.Abs(endPoint.y - startPoint.y), Mathf.Abs(endPoint.z - startPoint.z));
    }

    private void UpdatePreviewRectangle(){
        previewRectangle.transform.position = center;
        previewRectangle.transform.localScale = size;
    }

    private void Spawn(){
        Destroy(previewRectangle);
        //Scale prefab before instantiating so that mario64 code can use the scale factor
        var prefab = spawnableTerrain[prefabIndex].prefab;
        GameObject rectangle = Instantiate(prefab, center, Quaternion.identity);
        rectangle.transform.localScale = size;
        var dt = rectangle.GetComponentInChildren<SM64DynamicTerrain>();
        dt.SetPosition(center);
        dt.SetRotation(Quaternion.identity);
        StartCoroutine(DisableAndReenableTerrain(dt));
            
    }
    private IEnumerator DisableAndReenableTerrain(SM64DynamicTerrain terrain){
        terrain.enabled = false;
        yield return new WaitForSeconds(0.05f);
        terrain.enabled = true;
    }
}
