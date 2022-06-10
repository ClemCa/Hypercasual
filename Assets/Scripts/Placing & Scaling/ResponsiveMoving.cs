using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ResponsiveMoving : MonoBehaviour
{
    [SerializeField] private float _speed = 1;
    [SerializeField] private float _position;
    [SerializeField] private Vector2 _offset;
    [SerializeField] private bool _direction;
    [SerializeField] private float _horizontalMovement;
    [SerializeField] private float _verticalMovement;


    void Update()
    {
        if (!Application.isPlaying)
        {
            var middleT = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, 0.5f, transform.position.z - Camera.main.transform.position.z)).x;
            var sideT = Camera.main.ScreenToWorldPoint(new Vector3(0, 0.5f, transform.position.z - Camera.main.transform.position.z)).x;
            var posT = transform.position;
            if (_horizontalMovement != 0)
                posT.x = _offset.x + (_position - 0.5f) * Mathf.Abs(middleT - sideT) * _horizontalMovement;
            if (_verticalMovement != 0)
                posT.y = _offset.y + (_position - 0.5f) * Mathf.Abs(middleT - sideT) * _verticalMovement;
            transform.position = posT;
            return;
        }
        if (_direction)
        {
            _position += Time.deltaTime * _speed;
            if(_position >= 1)
            {
                _direction = false;
            }
        }
        else
        {
            _position -= Time.deltaTime * _speed;
            if(_position <= 0)
            {
                _direction = true;
            }
        }
        var middle = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, 0.5f, transform.position.z - Camera.main.transform.position.z)).x;
        var side = Camera.main.ScreenToWorldPoint(new Vector3(0, 0.5f, transform.position.z - Camera.main.transform.position.z)).x;
        var pos = transform.position;
        if(_horizontalMovement != 0)
            pos.x = _offset.x + (_position - 0.5f) * Mathf.Abs(middle - side) * _horizontalMovement;
        if (_verticalMovement != 0)
            pos.y = _offset.y + (_position - 0.5f) * Mathf.Abs(middle - side) * _verticalMovement;
        transform.position = pos;
    }
}
