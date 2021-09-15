using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetect : MonoBehaviour {

    private const string ATTACK_ANIMATION = "attacking";

    public Animator animator;

    private static reset mReset;

    private void Start()
    {
        if (mReset == null)
            mReset = GameObject.FindObjectOfType<reset>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            animator.SetBool(ATTACK_ANIMATION, false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            animator.SetBool(ATTACK_ANIMATION, true);
            StartCoroutine(delay());
        }
    }

    IEnumerator delay()
    {
        yield return new WaitForSeconds(.5f);
        mReset.myReset();
        StopCoroutine(delay());
    }
}
