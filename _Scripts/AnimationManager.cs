using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour {

    public GameObject kangAnim;
    public GameObject dingoAnim;
    public GameObject crocAnim;
    public GameObject emuAnim;


    void Start()
    { 
        kangAnim.SetActive(false);
        dingoAnim.SetActive(false);
        crocAnim.SetActive(false);
        emuAnim.SetActive(false);
    }

    public void AnimOff()
    {
        kangAnim.SetActive(false);
        dingoAnim.SetActive(false);
        crocAnim.SetActive(false);
        emuAnim.SetActive(false);
    }

}
