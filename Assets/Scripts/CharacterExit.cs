using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterExit : MonoBehaviour
{
    public Animator characterAnimation;

    public void InitialExit()
    {
        characterAnimation.SetTrigger("initialGame");

    }
}
