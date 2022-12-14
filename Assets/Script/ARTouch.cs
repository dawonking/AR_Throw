using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARTouch : MonoBehaviour
{

    private static ARRaycastManager raycastManager;
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    public GameObject objPrefab;
    private Vector2 touchPos;

    [SerializeField]
    private Camera arCamera;
    [SerializeField]
    private LayerMask placedObjectLayerMask;
    private Ray ray;
    private RaycastHit hit;

    public static ARTouch instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        objPrefab = Instantiate(objPrefab);
        objPrefab.SetActive(false);        
    }

    public static bool Raycast(Vector2 screenPosition, out Pose pose)
    {
        if (raycastManager.Raycast(screenPosition, hits, TrackableType.Planes))
        {
            pose = hits[0].pose;
            return true;
        }
        else
        {
            pose = Pose.identity;
            return false;
        }
    }

    public static bool TryGetInputPosition(out Vector2 position)
    {
        position = Vector2.zero;

        if (Input.touchCount == 0)
        {
            return false;
        }

        position = Input.GetTouch(0).position;

        if (Input.GetTouch(0).phase != TouchPhase.Began)
        {
            return false;
        }
       
        return true;
    }

    private void Update()
    {
        if (!ARTouch.TryGetInputPosition(out touchPos))
        {            
            return;
        }

        //touchpos = Input.GetTouch(0).position;
        if (ARTouch.Raycast(Input.GetTouch(0).position, out Pose hitPose) && Manager.manager.state == State.scan)
        {
            if (!objPrefab.activeInHierarchy)
            {
                objPrefab.SetActive(true);
            }
            objPrefab.transform.position = hitPose.position;
            objPrefab.transform.rotation = hitPose.rotation;
        }

        if (Input.touchCount == 0) return;
        Touch touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began && Manager.manager.state == State.play)
        {
            ray = arCamera.ScreenPointToRay(touch.position);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.tag == "ball")
                {                    
                    Manager.manager.ballObj.GetComponent<ballShoot>().addforce();
                }
            }
        }

    }

}
