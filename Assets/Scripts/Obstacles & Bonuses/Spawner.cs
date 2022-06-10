using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _spawn;
    [SerializeField] private float _delay = 1;
    [SerializeField] private float _lifetime = 1;

    private float _time;

    void Update()
    {
        _time += Time.deltaTime;
        if(_time >= _delay)
        {
            _time = 0;
            var r = Instantiate(_spawn, transform.position, Quaternion.identity, transform);
            r.transform.localScale = transform.localScale;
            if(_lifetime != -1)
                r.AddComponent<Lifetime>().lifetime = _lifetime;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            Destroy(gameObject);
    }
}
