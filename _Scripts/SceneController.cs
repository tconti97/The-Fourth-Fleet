using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

    public Scene current;
    public int loadIndex;

    public PlayVideos pv;

    public void Start()
    {
        current = SceneManager.GetActiveScene();
        loadIndex = current.buildIndex;
    }


    public void OnMouseUpAsButton()
    {
        if (loadIndex == 0)
        SceneManager.LoadScene("Gameplay");

    if (loadIndex == 2 || loadIndex==3)
        SceneManager.LoadScene("Introduction");
    }
}
