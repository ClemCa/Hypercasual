using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transporter : MonoBehaviour
{
    [SerializeField] private Vector2 _origin;
    [SerializeField] private Vector2 _destination;
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _exitPropulsion = 10f;

    private float _progress;
    private Transform _player;

    private static Transporter priority;

    private Vector3 Origin
    {
        get
        {
            var extents = GetComponent<Collider>().bounds.extents;
            extents.x *= _origin.x;
            extents.y *= _origin.y;
            extents.z = 0;
            return GetComponent<Collider>().bounds.center + extents;
        }
    }
    private Vector3 Destination
    {
        get
        {
            var extents = GetComponent<Collider>().bounds.extents;
            extents.x *= _destination.x;
            extents.y *= _destination.y;
            extents.z = 0;
            return GetComponent<Collider>().bounds.center + extents;
        }
    }

    void Update()
    {
        if (priority == this)
        {
            _player.GetComponent<Rigidbody>().MovePosition(Vector3.Lerp(_player.transform.position, Vector3.Lerp(Origin, Destination, _progress), Time.deltaTime * 5));
            _progress += Time.deltaTime * _speed;
            if(_progress >= 1)
            {
                _player.GetComponent<Rigidbody>().velocity = (Destination - Origin).normalized * _exitPropulsion;
                _player.GetComponent<PlayerController>().enabled = true;
                priority = null;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (priority != null)
                return;
            priority = this;
            _player = other.transform;
            other.GetComponent<PlayerController>().enabled = false;
            var current = InverseLerp(Origin, Destination, other.transform.position);
            int count = 3;
            if(current.x > 1 || current.x < 0)
            {
                current.x = 0;
                count--;
            }
            if (current.y > 1 || current.y < 0)
            {
                current.y = 0;
                count--;
            }
            if (current.z > 1 || current.z < 0)
            {
                current.z = 0;
                count--;
            }
            if (count == 0)
                _progress = 0;
            else
                _progress = (current.x + current.y + current.z) / count;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player") && priority == this)
        {
            other.GetComponent<PlayerController>().enabled = true;
            priority = null;
        }
    }

    private Vector3 InverseLerp(Vector3 a, Vector3 b, Vector3 value)
    {
        value.x = Mathf.InverseLerp(a.x, b.x, value.x);
        value.y = Mathf.InverseLerp(a.y, b.y, value.y);
        value.z = Mathf.InverseLerp(a.z, b.z, value.z);
        return value;
    }
}
