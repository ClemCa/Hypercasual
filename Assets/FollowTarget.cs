using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float _lagSpeed = 2;


    void Start()
    {
        var pos = transform.position;
        pos.y = target.position.y;
        transform.position = pos;
    }

    void Update()
    {
        var pos = transform.position;
        pos.y = Mathf.Max(pos.y, target.position.y);
        transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * _lagSpeed);
    }

}