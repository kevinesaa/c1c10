using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour {

    public float speed;
    public float distance;
    public Transform groundDetection;
    public Animator animator;

    private bool movingRight;
   

    private static readonly Vector3 RIGTH_ROTATION = new Vector3(0, 0, 0);
    private static readonly Vector3 LEFT_ROTATION = new Vector3(0, -180, 0);
    private const string ATTACK_ANIMATION = "attacking";
    private const string DOWN = "down";
    private const string RESPAWN = "responw";




    // Update is called once per frame
    void Update () 
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distance);
        if (groundInfo.collider == false)
        {
            if (movingRight == true)
            {
                transform.eulerAngles = LEFT_ROTATION;
                movingRight = false;
            }
            else
            {
                transform.eulerAngles = RIGTH_ROTATION;
                movingRight = true;
            }
        }
    }

    private void OnEnable()
    {
        animator.SetTrigger(RESPAWN);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Bullet"))
        {
            animator.SetTrigger(DOWN);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Bullet"))
        {
            animator.SetTrigger(DOWN);
        }
    }


}
