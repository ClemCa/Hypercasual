using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour
{
    [SerializeField] private RotateOnHit[] _lock;

    private bool brick;

    void Update()
    {
        if (brick)
            return;
        bool unlocked = true;
        for(int i = 0; i < _lock.Length; i++)
        {
            if (!_lock[i].Brick)
                unlocked = false;
        }
        if (unlocked)
        {
            brick = true;
            var rb = gameObject.AddComponent<Rigidbody>();
            rb.AddTorque(Vector3.right * 10, ForceMode.Impulse);
            rb.AddForce(Vector3.forward * 1 + Vector3.down * 1, ForceMode.Impulse);
            rb.useGravity = false;
        }
    }
}
