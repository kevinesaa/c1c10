using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlMainMenuController : MonoBehaviour
{
    public Animator exitAnimation;
    //public GameObject loadingScreen;


    public void InitialAnimation()
    {
        exitAnimation.SetTrigger("initialGame");
    }

    /*public void LoadingExit()
    {
        loadingScreen.SetActive(gameObject);
    }
    */
}
