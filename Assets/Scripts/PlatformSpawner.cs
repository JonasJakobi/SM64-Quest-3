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
    // Start is called before the first frame update

    Vector3 center;
    Vector3 size;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch)){
            startPoint = transform.position;
            previewRectangle = Instantiate(previewPrefab, startPoint, Quaternion.identity);

        }

        
        //While button held
        if(OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch)){
            endPoint = transform.position;
            UpdateCenterAndSize();
            UpdatePreviewRectangle();
        }
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
            
        //Delete preview rectangle
        Destroy(previewRectangle);
        // Spawn the rectangle prefab at the calculated center with the calculated size
        //Scale prefab before instantiating so that mario64 code can use the scale factor
        prefab.transform.localScale = size;
        GameObject rectangle = Instantiate(prefab, center, Quaternion.identity);
        rectangle.GetComponent<SM64DynamicTerrain>().SetPosition(center);
        rectangle.GetComponent<SM64DynamicTerrain>().SetRotation(Quaternion.identity);
            
    }
}
