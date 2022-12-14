using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveLoop : MonoBehaviour
{
    [SerializeField]
    Vector3 pos;

    [SerializeField]
    float delta = 2.0f;

    [SerializeField]
    float speed = 3.0f;


    void Start()
    {
        pos = transform.localPosition;
    }

    //sin형태로 일정 주기로 움직임
    void Update()
    {
        Vector3 v = pos;
        v.x += delta * Mathf.Sin(Time.time * speed);
        transform.localPosition = v;
    }
}
