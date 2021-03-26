using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class Intro2 : MonoBehaviour 
{
	public Button playGame;
	public PlayableDirector director;
	public GameObject canvas;
	int status;
	public GameObject bg;

	void Start()
	{
		status = 0;
	}

	void Update()
	{
		if (status == 1 && Input.GetKeyDown (KeyCode.Space)) 
		{
			SceneManager.LoadScene("Gameplay");
		}
	}

	public void OnMouseUpAsButton()
	{
		director.Play ();
		canvas.SetActive (false);
		bg.SetActive (false);
		status = 1;
	}
}
