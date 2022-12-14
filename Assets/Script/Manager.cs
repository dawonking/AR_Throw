using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR.ARFoundation;

public enum State
{
    scan,
    play
}

public class Manager : MonoBehaviour
{

    /*
     영역검색 -> 터치 후 오브젝트 위치설정 -> 시작여부 물어봄-> 확인시 위치 고정
     취소시 다시 위치고정
     리셋 시 처음으로
     
     */

    public TMP_Text LogText;

    public GameObject ballObj;
    public Transform ballresetpos;

    public ARPlaneManager m_arplane;
    public ARSession aRSession;

    public static Manager manager;

    public bool shootcheck = false;

    public State state;
    public Camera camera;

    [SerializeField]
    private GameObject objPrefab;

    [SerializeField]
    List<GameObject> canvaslist = new List<GameObject>();

    public int goalCount = 0;

    private void Awake()
    {
        if(manager == null)
        {
            manager = this;
            DontDestroyOnLoad(this.gameObject);
        }        
    }

    //시작시 초기화 및 설정
    private void Start()
    {
        resetbtn();
        state = State.scan;
    }

    //리셋 버튼
    public void resetbtn()
    {
        state = State.scan;
        canvaslist[0].SetActive(true);
        canvaslist[1].SetActive(false);
        artrackble_off(true);
        ballObj.SetActive(false);
        ARTouch.instance.objPrefab.SetActive(false);
    }

    //게임시작 버튼
    public void gamestartbtn()
    {
        goalCount = 0;
        canvaslist[0].SetActive(false);
        canvaslist[1].SetActive(true);
        state = State.play;
        artrackble_off(false);
        ballObj.SetActive(true);
        goal_text(goalCount);        
    }

    //공위치 초기화
    public void returnball()
    {
        ballObj.transform.position = ballresetpos.position;
        ballObj.transform.rotation = ballresetpos.rotation;
        shootcheck = false;
        ballObj.SetActive(true);        
    }

    //ARPlaneManager에서 영역 검색후 사용자가 선택한뒤 시작을 누르면 너머지 영역들을 비활성화 시켜준다.
    public void artrackble_off(bool enable)
    {
        foreach (var plane in m_arplane.trackables)
        {            
            plane.gameObject.SetActive(enable);
        }
        m_arplane.enabled = enable;     
        Debug.Log(m_arplane.trackables.count);
    }

    public void goal_text(int c)
    {
        LogText.text = "골 점수 : " + c;
    }

}
