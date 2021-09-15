using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public event Action OnCloneRewindFinish;

    public GameObject handReference;
    public float maxTravelTime;
    public float shootVelocity;
    public float rewindVelocity;
    public bool IsShooting { get; private set; }
    public bool IsRewinding { get { return travelTime >= maxTravelTime; } }

    private float travelTime;
    private HandController clone;
    private GameObject original;

    private void Awake()
    {
        IsShooting = false;
    }

    public void Shoot()
    {
        if (clone == null)
        {
            clone = Instantiate(this, handReference.transform.position,Quaternion.identity) as HandController;
            clone.handReference = handReference;
            clone.OnCloneRewindFinish = OnCloneRewindFinish;
            clone.original = this.gameObject;
        }
        gameObject.transform.position = handReference.transform.position;
        clone.transform.position = transform.position;
        clone.transform.rotation = transform.rotation;
        clone.transform.localScale = transform.lossyScale;
        clone.gameObject.SetActive(true);
        clone.travelTime = 0;
        clone.IsShooting = true;
        gameObject.SetActive(false);
    }

    
    private void Update()
    {
        if (IsShooting)
        {
            travelTime += Time.deltaTime;
            Vector3 handVelocity = shootVelocity * Time.deltaTime * Vector2.left;
            if (IsRewinding)
            {
                handVelocity.x *= rewindVelocity;
                IsShooting = transform.position.x <= handReference.transform.position.x;
                if (!IsShooting)
                {
                    RewindFinish();
                }
            }
            
            transform.position += handVelocity;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        IHelthModifier helthModifier = collision.gameObject.GetComponent(typeof(IHelthModifier)) as IHelthModifier;
        if (helthModifier != null)
            helthModifier.ModifyHelth(-1);
    }

    void RewindFinish()
    {
        transform.position = handReference.transform.position;
        if (OnCloneRewindFinish != null)
            OnCloneRewindFinish();

        gameObject.SetActive(false);
        if (original != null)
            original.SetActive(true);
    }
}
