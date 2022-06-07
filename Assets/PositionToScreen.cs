using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PositionToScreen : MonoBehaviour
{
    [SerializeField] private bool _right;
    [SerializeField] private float _yOffset;

    void Start()
    {
        Place();
    }

    void Update()
    {
        Place();
    }

    private void Place()
    {
        var screenPoint = new Vector3(_right ? Screen.width : 0, 0.5f, 0 - Camera.main.transform.position.z);
        var point = Camera.main.ScreenToWorldPoint(screenPoint);
        point.y += _yOffset;
        transform.position = point;
    }

}
