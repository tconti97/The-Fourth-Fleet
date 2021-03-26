using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class PlayVideos : MonoBehaviour {


    public GameObject exitButton;
    public GameObject playButton;
    public GameObject nameText;

    public GameObject intro1;
    public GameObject intro2;

    public IEnumerator coroutine1;
    public IEnumerator coroutine2;

	// Use this for initialization
	void Start ()
    {
        exitButton.SetActive(false);
        playButton.SetActive(false);
        nameText.SetActive(false);

        //coroutine1 = ClipTiming1();
        //coroutine2 = ClipTiming2();
        StartCoroutine(coroutine1);
    }

    /* IEnumerator ClipTiming1()
     {
         yeild return new WaitForSeconds(27.0f);
         StopCoroutine(coroutine1);
     }

     https://docs.unity3d.com/ScriptReference/MonoBehaviour.StopCoroutine.html
     */
}
