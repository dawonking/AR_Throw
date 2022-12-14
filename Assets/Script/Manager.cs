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
     �����˻� -> ��ġ �� ������Ʈ ��ġ���� -> ���ۿ��� ���-> Ȯ�ν� ��ġ ����
     ��ҽ� �ٽ� ��ġ����
     ���� �� ó������
     
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

    //���۽� �ʱ�ȭ �� ����
    private void Start()
    {
        resetbtn();
        state = State.scan;
    }

    //���� ��ư
    public void resetbtn()
    {
        state = State.scan;
        canvaslist[0].SetActive(true);
        canvaslist[1].SetActive(false);
        artrackble_off(true);
        ballObj.SetActive(false);
        ARTouch.instance.objPrefab.SetActive(false);
    }

    //���ӽ��� ��ư
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

    //����ġ �ʱ�ȭ
    public void returnball()
    {
        ballObj.transform.position = ballresetpos.position;
        ballObj.transform.rotation = ballresetpos.rotation;
        shootcheck = false;
        ballObj.SetActive(true);        
    }

    //ARPlaneManager���� ���� �˻��� ����ڰ� �����ѵ� ������ ������ �ʸ��� �������� ��Ȱ��ȭ �����ش�.
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
        LogText.text = "�� ���� : " + c;
    }

}
