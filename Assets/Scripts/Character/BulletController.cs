using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    [Range(0, float.MaxValue)]
    public float speed = 5;
    [Range(0, float.MaxValue)]
    public float lifeTime = 5;

    private bool moving;
    private float timeOver;
    private PooledObject poolObject;
    private Animator boomAnimation;

    void OnEnable()
    {
        Invoke("LifeOver", lifeTime);
    }

    void Start ()
    {
        poolObject = GetComponent<PooledObject>();
        boomAnimation = GameObject.Find("boomAnimation").GetComponent<Animator>();
    }
    
    void Update ()
    {
        if(moving)        
        {
            transform.position += Time.deltaTime * speed * transform.right;
            timeOver -= Time.deltaTime;
        }
	}

    public void Setup(Vector3 position, Quaternion rotation )
    {
        this.transform.position = position;
        this.transform.rotation = rotation;
        timeOver = lifeTime;
        this.moving = true;
    }

    private void LifeOver()
    {
        if(timeOver <= 0)
        {
            this.moving = false;
            this.gameObject.SetActive(false);
            poolObject.ReturnObjectToPool();
        }
        else
        {
            Invoke("LifeOver", timeOver);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnBulletCrash(collision.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnBulletCrash(collision.gameObject);
    }

    private void OnBulletCrash(GameObject other)
    {
        if (other.CompareTag("Enemy"))
        {
            BoomAnimation(other);
            other.SetActive(false);
        }

        if (other.CompareTag("boss"))
        {
            BossController bossController = other.GetComponent<BossController>();
            bossController.ModifyHelth(-1);
            BoomAnimation(other);
            if (!bossController.IsAlive)
            {
                bossController.ShowFinish();
                other.SetActive(false);
            }
        }
    }

    private void BoomAnimation(GameObject other)
    {
        timeOver = 0;
        boomAnimation.gameObject.transform.position = other.transform.position;
        boomAnimation.SetTrigger("boom");
        LifeOver();
    }
}
