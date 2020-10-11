using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
[RequireComponent(typeof(ARReferencePointManager))]
[RequireComponent(typeof(ARPlaneManager))]
public class ReferencePointManager : MonoBehaviour
{

    private ARRaycastManager arRaycastManager;

    private ARReferencePointManager arReferencePointManager;

    private ARPlaneManager arPlaneManager;

    private List<ARReferencePoint> referencePoints = new List<ARReferencePoint>();

    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();




    [SerializeField]
    private Button toggleButton;

    [SerializeField]
    private GameObject usaPlane;


    private bool planeSearch = true;

    private void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();

        arReferencePointManager = GetComponent<ARReferencePointManager>();

        arPlaneManager = GetComponent<ARPlaneManager>();

        toggleButton.onClick.AddListener(TogglePlaneDetection);

    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if(referencePoints.Count >= 2)
        {
            usaPlane.SetActive(true);

            float aX = referencePoints[0].transform.position.x;
            float aZ = referencePoints[0].transform.position.z;

            float bX = referencePoints[1].transform.position.x;
            float bZ = referencePoints[1].transform.position.z;

            usaPlane.transform.localScale = new Vector3((bX - aX)/10, 1, (bZ - aZ)/10);
            usaPlane.transform.position = new Vector3((aX+bX) / 2, (referencePoints[0].gameObject.transform.position.y), (aZ+bZ) / 2);
            adjustAspectRatio(1.305f, usaPlane);
            //referencePoints[0].gameObject.GetComponent<Material>().color = new Color32(0, 255, 0, 255);
            //referencePoints[1].gameObject.transform.rotation.SetEulerAngles(referencePoints[1].gameObject.transform.rotation;

        }

        if (Input.touchCount == 0)
        {
            return;
        }

        Touch touch = Input.GetTouch(0);

        if(touch.phase != TouchPhase.Began)
        {
            return;
        }

        if (arRaycastManager.Raycast(touch.position, hits,TrackableType.PlaneWithinPolygon) && !planeSearch)
        {
            Pose hitPose = hits[0].pose;
            ARReferencePoint referencePoint = arReferencePointManager.AddReferencePoint(hitPose);

            if(referencePoint == null)
            {
                Debug.Log("There was an error creating a reference point here");
            }
            else if(!planeSearch)
            {
                referencePoints.Add(referencePoint);
            }
        }
        
    }

    private void TogglePlaneDetection()
    {
        
        arPlaneManager.enabled = !arPlaneManager.enabled;
        planeSearch = arPlaneManager.enabled;


        foreach (ARPlane plane in arPlaneManager.trackables)
        {
            plane.gameObject.SetActive(arPlaneManager.enabled);
            
        }

        toggleButton.GetComponentInChildren<Text>().text = arPlaneManager.enabled ? "Disable Plane Detection" : "Enable Plane Detection";
    }

    private void adjustAspectRatio(float lOverWRatio,GameObject plane)
    {
        float planeX = plane.transform.localScale.x;
        float planeZ = plane.transform.localScale.z;

        if(planeX/planeZ < lOverWRatio)
        {
            //Z must go down
            planeX = planeZ * lOverWRatio;
        }
        else if(planeX/planeZ > lOverWRatio)
        {
            //X must go down
            planeZ = planeX * (1 / lOverWRatio);
        }
        plane.transform.localScale = new Vector3(planeX, 1, planeZ);


    }
}
