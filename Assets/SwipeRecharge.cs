using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeRecharge : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().GiveSwipe();
            Destroy(gameObject);
        }
    }
}
