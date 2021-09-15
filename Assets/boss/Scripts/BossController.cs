using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour, IHelthModifier
{
    private const string ANIM_HAND_ATTACK = "handAttack";
    private const string ANIM_HAND_UP = "handUp";

    public GameObject bossFinish;

    public int initialHelth;
    public float attackTimer;
    public HandController handController;

    public bool IsAttacking { get; set; }
    public bool IsPlayerClose { get; set; }
    public int Helth { get; set; }
    public bool IsAlive { get { return Helth > 0; } }

    private bool IsTimeToAttack { get { return timeToAttack >= attackTimer; } }
    private float timeToAttack;
    private Animator animator;

    private void Awake()
    {
        Helth = initialHelth;
        animator = GetComponent<Animator>();
        handController.OnCloneRewindFinish += OnRewindFinish;
    }

    private void OnEnable()
    {
        Helth = initialHelth;
        bossFinish.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsAlive && IsPlayerClose) 
        {
            if (IsTimeToAttack)
            {
                if (!IsAttacking)
                {
                    IsAttacking = true;
                    Attack();
                }
            }
            else
            {
                timeToAttack += Time.deltaTime;
            }
        }
        
    }

    public void ShowFinish() 
    {
        bossFinish.SetActive(true);
    }

    public void ModifyHelth(int modifier)
    {
        Helth += modifier;
    }


    public void Attack()
    {
        animator.SetTrigger(ANIM_HAND_ATTACK);
    }

    public void OnHandDown()
    {
        handController.Shoot();
    }

    private void OnRewindFinish()
    {
        animator.SetTrigger(ANIM_HAND_UP);
    }

    public void OnAttackFinish()
    {
        timeToAttack = 0;
        IsAttacking = false;
    }

    
}
