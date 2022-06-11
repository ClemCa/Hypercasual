using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOnHit : MonoBehaviour
{
    private bool brick = false;

    public bool Brick { get => brick; }

    void OnTriggerEnter(Collider other)
    {
        if (brick)
            return;
        if (other.CompareTag("Player"))
        {
            if(TryGetComponent<ResponsiveScaling>(out var res))
            {
                res.enabled = false;
            }
            if (TryGetComponent<ScreenRelativeScaling>(out var scr))
            {
                scr.enabled = false;
            }
            var rb = gameObject.AddComponent<Rigidbody>();
            rb.AddTorque(Vector3.right * other.GetComponent<Rigidbody>().velocity.y * 10, ForceMode.Impulse);
            rb.AddForce(Vector3.forward * 1 + Vector3.down * 1, ForceMode.Impulse);
            rb.useGravity = false;
            brick = true;
        }
    }
}
