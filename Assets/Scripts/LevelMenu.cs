using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenu : MonoBehaviour
{
    [SerializeField] private Transform _buttonParent;
    [SerializeField] private Vector2 _forceResolution;

    void Awake()
    {
        // force resolutoin
        
    }

    void Start()
    {
        int lastUnlock;
        if (PlayerPrefs.HasKey("LastLevel"))
        {
            lastUnlock = PlayerPrefs.GetInt("LastLevel");
        }
        else
        {
            lastUnlock = 0;
        }
        for(int i = 0; i < _buttonParent.childCount; i++)
        {
            _buttonParent.GetChild(i).GetComponent<Button>().interactable = i <= lastUnlock;
        }
    }

    public void OpenLevel(int i)
    {
        SceneManager.LoadScene("Level" + i);
    }
}
