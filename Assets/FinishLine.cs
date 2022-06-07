using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class FinishLine : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _fireworks;

    void Awake()
    {
        foreach (var firework in _fireworks)
        {
            var main = firework.main;
            main.playOnAwake = false;
            firework.Stop();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Controller>().enabled = false;
            foreach(var firework in _fireworks)
            {
                firework.Play();
            }
            DelayedLeaving();
        }
    }

    private async void DelayedLeaving()
    {
        await System.Threading.Tasks.Task.Delay(5000);
        int lastUnlock;
        if (PlayerPrefs.HasKey("LastLevel"))
        {
            lastUnlock = PlayerPrefs.GetInt("LastLevel");
        }
        else
        {
            lastUnlock = 0;
        }
        int num = int.Parse((new string(SceneManager.GetActiveScene().name.Where(c => char.IsDigit(c)).ToArray())));
        if (num > lastUnlock) // latest level name is always i+1
            PlayerPrefs.SetInt("LastLevel", num); // update progression 
        SceneManager.LoadSceneAsync("LevelSelection");
    }
}
