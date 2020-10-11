using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
[RequireComponent(typeof(ARReferencePointManager))]
[RequireComponent(typeof(ARPlaneManager))]
public class ReferencePointManager1 : MonoBehaviour
{

    private ARRaycastManager arRaycastManager;

    private ARReferencePointManager arReferencePointManager;

    private ARPlaneManager arPlaneManager;

    private List<ARReferencePoint> referencePoints = new List<ARReferencePoint>();

    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    [SerializeField]
    private Text debugLog;

    [SerializeField]
    private Text referencePointCount;

    [SerializeField]
    private Button toggleButton;

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
        if (Input.touchCount == 0)
        {
            return;
        }

        Touch touch = Input.GetTouch(0);

        if(touch.phase != TouchPhase.Began)
        {
            return;
        }

        if (arRaycastManager.Raycast(touch.position, hits,TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;
            ARReferencePoint referencePoint = arReferencePointManager.AddReferencePoint(hitPose);

            if(referencePoint == null)
            {
                Debug.Log("There was an error creating a reference point here");
                debugLog.text += "There was an error creating a reference point here";
            }
            else
            {
                referencePoints.Add(referencePoint);
                referencePointCount.text = $"Reference Point Count: {referencePoints.Count}";
            }
        }
    }

    private void TogglePlaneDetection()
    {

        arPlaneManager.enabled = !arPlaneManager.enabled;

        foreach(ARPlane plane in arPlaneManager.trackables)
        {
            plane.gameObject.SetActive(arPlaneManager.enabled);
        }

        toggleButton.GetComponentInChildren<Text>().text = arPlaneManager.enabled ? "Disable Plane Detection" : "Enable Plane Detection";
    }
}
