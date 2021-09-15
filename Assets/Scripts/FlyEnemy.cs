using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEnemy : MonoBehaviour {

    private const string PLAYER_CLOSE_FACE_ANIMATION = "player_close";
    private static readonly Vector3 ROTATION_Y_180 = new Vector3(0, 180, 0);

    public Animator faceAnimator;
    public float moveSpeed;

    private bool isPlayerClose;
    private bool isGoingToRigth;
    private GameObject player;
    private Vector3 initialPosition;
    private Vector3 targetPosition;
    

    private void Start()
    {
        initialPosition = transform.position;
    }


    private void Update()
    {
        if (isPlayerClose)
        {
            targetPosition = player.transform.position;
        }
        else
        {
            if(transform.position != initialPosition)
            {
                targetPosition = initialPosition;
            }
        }

        if (transform.position != targetPosition)
        {
            float dir = targetPosition.x - transform.position.x;
            if (dir > 0) 
            {
                if(!isGoingToRigth)
                {
                    isGoingToRigth = true;
                    transform.eulerAngles += ROTATION_Y_180;
                }
            }
            else
            {
                if (isGoingToRigth)
                {
                    isGoingToRigth = false;
                    transform.eulerAngles += ROTATION_Y_180;
                }
            }
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       if(collision.CompareTag("Player"))
       {
            isPlayerClose = true;
            player = collision.gameObject;
            faceAnimator.SetBool(PLAYER_CLOSE_FACE_ANIMATION, true);
       }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerClose = false;
            faceAnimator.SetBool(PLAYER_CLOSE_FACE_ANIMATION, false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet")) {

            collision.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
