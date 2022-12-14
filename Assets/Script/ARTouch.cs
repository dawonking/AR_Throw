using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARTouch : MonoBehaviour
{

    private static ARRaycastManager raycastManager;
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    //좌우로 움직이는 오브젝트
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
        //레이캐스트 체크 및 종류 확인
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

    //터치여부 확인
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


        //지면터치관련
        if (ARTouch.Raycast(Input.GetTouch(0).position, out Pose hitPose) && Manager.manager.state == State.scan)
        {

            //터치한 영역에 오브젝트 활성화
            if (!objPrefab.activeInHierarchy)
            {
                objPrefab.SetActive(true);
            }
            objPrefab.transform.position = hitPose.position;
            objPrefab.transform.rotation = hitPose.rotation;
        }

        //인게임 카메라 레이케스트
        if (Input.touchCount == 0) return;
        Touch touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began && Manager.manager.state == State.play)
        {
            ray = arCamera.ScreenPointToRay(touch.position);

            //볼 터치시 포물선 방향으로 진행
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
