using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifetime : MonoBehaviour
{
    [SerializeField] private float _lifetime;

    private float _time;

    public float lifetime { get => _lifetime; set => _lifetime = value; }

    void Update()
    {
        _time += Time.deltaTime;
        if(_time > _lifetime)
        {
            Destroy(gameObject);
        }
    }
}
