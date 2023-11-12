using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LibSM64;

public class PlatformSpawner : MonoBehaviour
{
    public GameObject prefab;
    public GameObject previewPrefab;
    public bool isLeft = false;

    private Vector3 startPoint;
    private Vector3 endPoint;
    public float spawnSpeed = 5;

    private GameObject previewRectangle;

    Vector3 center;
    Vector3 size;
 
    void Update()
    {
        //When button pressed, set the start point and instantiate the preview rectangle
        if(OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch)){
            startPoint = transform.position;
            previewRectangle = Instantiate(previewPrefab, startPoint, Quaternion.identity);
        }

        
        //While button held, update the preview
        if(OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch)){
            endPoint = transform.position;
            UpdateCenterAndSize();
            UpdatePreviewRectangle();
        }
        //When button released, spawn the rectangle
        if(OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch)){
            Spawn();
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
        prefab.transform.localScale = size;
        GameObject rectangle = Instantiate(prefab, center, Quaternion.identity);
        rectangle.GetComponent<SM64DynamicTerrain>().SetPosition(center);
        rectangle.GetComponent<SM64DynamicTerrain>().SetRotation(Quaternion.identity);
            
    }
}
