using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRubyShared;

public class PlayerController : MonoBehaviour, IHelthModifier
{
    private const string SHOW_EYE_ANIMATION = "SHOW_EYE";
    private const string SHOOT_ANIMATION = "shoot";
    private const string IS_WALKING_ANIMATION = "isWalking";
    private const string EXPLOD_ANIMATION = "explod";
    private const float NORMAL_TIME_SCALE_MOTION = 1f;

    public float movementSpeed;
    public float jumpForce;
    public float offsetRotation = 0;
    public float maxDegreeRotationPerSecond = 6;
    [Range(0, 360)]
    public float counterClockwiseMaxAngle = 360;
    [Range(-360, 0)]
    public float clockwiseMaxAngle = -360;
    public float firstSlowMotion = 0.75f;
    public float minSlowMotion;
    public float slowMotionFactor;
    public SimpleObjectPool bulletPool;
    public GameObject pivotGun;
    public GameObject pivotEye;
    public Animator eyeAnimator;
    public Animator pivotAnimation;
    public GroundChecker checker;

    private bool isSlowMotion;
    private float initialFixedDeltaTime;
    private float currentTimeScale;
    private Rigidbody2D mRigibody2D;
    private Animator playerAnimator;
    private Vector2 longPressPosition = Vector2.zero;
    private TapGestureRecognizer tapGesture;
    private LongPressGestureRecognizer longPressGesture;
    private SwipeGestureRecognizer swipeUp;
    private SwipeGestureRecognizer swipeRight;
    private SwipeGestureRecognizer swipeDown;
    private SwipeGestureRecognizer swipeLeft;


    private float axisInputHorizontal;

    // Use this for initialization
    private void Start()
    {
        initialFixedDeltaTime = Time.fixedDeltaTime;
        playerAnimator = GetComponent<Animator>();
        mRigibody2D = GetComponent<Rigidbody2D>();
        pivotGun.SetActive(false);
        pivotEye.SetActive(false);

        InstanceAllGesture();
        ConfigLongPressGesture();
        ConfigTapGesture();
        ConfigSwipeUp();
        ConfigSwipeRight();
        ConfigSwipeDown();
        ConfigSwipeLeft();

    }


    // Update is called once per frame

    void Update()
    {/*
        jumping = Input.GetButtonDown("Fire1");
        if (jumping && isGrounded)
        {
            Vector2 newVelocity = rb2d.velocity;
            newVelocity.y = jumpForce;
            rb2d.velocity = newVelocity;
        }*/
        SlowMotion();
        playerAnimator.SetBool(IS_WALKING_ANIMATION, checker.IsTouchingGround);
        //axisInputHorizontal = Input.GetAxis("Horizontal"); // controla con teclado
    }

    private void SlowMotion()
    {
        currentTimeScale = Time.timeScale;
        if (isSlowMotion)
        {
            currentTimeScale -= (slowMotionFactor * Time.unscaledDeltaTime);
        }
        else
        {
            currentTimeScale += (slowMotionFactor * Time.unscaledDeltaTime);
        }
        currentTimeScale = Mathf.Clamp(currentTimeScale, minSlowMotion, NORMAL_TIME_SCALE_MOTION);
        if (currentTimeScale != Time.timeScale)
        {
            Time.timeScale = currentTimeScale;
            Time.fixedDeltaTime = initialFixedDeltaTime * Time.timeScale;
        }
    }

    void FixedUpdate()
    {
        //isGrounded = Physics2D.OverlapCircle(groundCheck.position, radiusFoot, whatIsGround);   

        Vector2 newVelocity = mRigibody2D.velocity;  //  Correr continuo.
        newVelocity.x = movementSpeed;         //
        mRigibody2D.velocity = newVelocity;           //

        //rb2d.velocity = new Vector2(movementSpeed * axisInputHorizontal, rb2d.velocity.y); // Maneja con el teclado

    }


    void OnTap(GestureRecognizer gesture)
    {
        if (gesture.State == GestureRecognizerState.Ended)
        {
            Jump();
        }
    }

    private void Jump()
    {
        if (checker.IsTouchingGround)
        {
            Vector2 newVelocity = mRigibody2D.velocity;
            newVelocity.y = jumpForce;
            mRigibody2D.velocity = newVelocity;
        }
    }

    void OnLongPress(GestureRecognizer gesture)
    {
        if (gesture.State == GestureRecognizerState.Began)
        {
            FirstSlowMotion();
            longPressPosition.Set(gesture.FocusX, gesture.FocusY);
            RotatePivot(pivotGun.transform);
            RotatePivot(pivotEye.transform);
            pivotGun.SetActive(true);
            pivotEye.SetActive(true);
            eyeAnimator.SetBool(SHOW_EYE_ANIMATION, true);

        }

        if (gesture.State == GestureRecognizerState.Executing)
        {
            longPressPosition.Set(gesture.FocusX, gesture.FocusY);
            RotatePivot(pivotGun.transform);
            RotatePivot(pivotEye.transform);
        }

        if (gesture.State == GestureRecognizerState.Ended)
        {
            longPressPosition.Set(gesture.FocusX, gesture.FocusY);
            RotatePivot(pivotGun.transform);
            RotatePivot(pivotEye.transform);
            eyeAnimator.SetBool(SHOW_EYE_ANIMATION, false);
            isSlowMotion = false;
            pivotGun.SetActive(false);
            pivotEye.SetActive(false);
            playerAnimator.SetTrigger(SHOOT_ANIMATION);
           
        }
    }

    private void FirstSlowMotion()
    {
        isSlowMotion = true;
        Time.timeScale = firstSlowMotion;
        Time.fixedDeltaTime = initialFixedDeltaTime * Time.timeScale;
    }

    private void RotatePivot(Transform pivot)
    {
        Vector2 pivotAsScreenPoint =
                    Camera.main.WorldToScreenPoint(pivot.position);

        Vector2 diff = longPressPosition - pivotAsScreenPoint;
        float rotation = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        float maxCounterClothWise = counterClockwiseMaxAngle + offsetRotation;
        float maxClothWise = clockwiseMaxAngle + offsetRotation;
        rotation = rotation + offsetRotation;
        rotation = Mathf.Clamp(rotation, maxClothWise, maxCounterClothWise);
        rotation = Mathf.MoveTowardsAngle
                            (pivot.eulerAngles.z,
                             rotation,
                             maxDegreeRotationPerSecond);
        pivot.transform.eulerAngles = rotation * Vector3.forward;
    }

    public static float NomalizeAngle(float angle)
    {
        const float TURN = 360;
        if (angle > TURN)
            angle -= TURN;
        if (angle < -TURN)
            angle += TURN;
        return angle;
    }

    public void Shoot()
    {
        pivotAnimation.SetTrigger(EXPLOD_ANIMATION);
        GameObject bullet = bulletPool.GetObject();
        BulletController bulletController = bullet.GetComponent<BulletController>();
        bulletController.Setup(pivotGun.transform.position, pivotGun.transform.rotation);
    }

    //todo this make again better
    private Animator boomAnimation;
    private static reset mReset;
    public void ModifyHelth(int modifier)
    {
        if(mReset == null)
            mReset = GameObject.FindObjectOfType<reset>();

        if (boomAnimation ==  null)
            boomAnimation = GameObject.Find("boomAnimation").GetComponent<Animator>();
        boomAnimation.gameObject.transform.position = transform.position;
        boomAnimation.SetTrigger("boom");
        StartCoroutine(ResetAfterDelay());
    }

    IEnumerator ResetAfterDelay()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(.5f);
        mReset.myReset();
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        StopCoroutine(ResetAfterDelay());
    }

    private void OnSwipeUp(GestureRecognizer gesture)
    {
        if (gesture.State == GestureRecognizerState.Ended)
        {
            Debug.Log("Swipe Up");
        }
    }

    private void OnSwipeRight(GestureRecognizer gesture)
    {
        if (gesture.State == GestureRecognizerState.Ended)
        {
            Debug.Log("Swipe Right");
        }
    }

    private void OnSwipeDown(GestureRecognizer gesture)
    {
        if (gesture.State == GestureRecognizerState.Ended)
        {
            Debug.Log("Swipe Down");
        }
    }

    private void OnSwipeLetf(GestureRecognizer gesture)
    {
        if (gesture.State == GestureRecognizerState.Ended)
        {
            Debug.Log("Swipe Left");
        }
    }

    private void InstanceAllGesture()
    {
        tapGesture = new TapGestureRecognizer();
        longPressGesture = new LongPressGestureRecognizer();
        swipeUp = new SwipeGestureRecognizer();
        swipeRight = new SwipeGestureRecognizer();
        swipeDown = new SwipeGestureRecognizer();
        swipeLeft = new SwipeGestureRecognizer();
    }

    void ConfigTapGesture()
    {
        tapGesture.StateUpdated += OnTap;
        tapGesture.ThresholdSeconds = 0.25f;
        FingersScript.Instance.AddGesture(tapGesture);
    }

    void ConfigLongPressGesture()
    {
        longPressGesture.StateUpdated += OnLongPress;
        longPressGesture.MinimumDurationSeconds = 0.45f;
        FingersScript.Instance.AddGesture(longPressGesture);
    }

    void ConfigSwipeUp()
    {
        swipeUp.StateUpdated += OnSwipeUp;
        swipeUp.Direction = SwipeGestureRecognizerDirection.Up;
        swipeUp.MinimumDistanceUnits = 0.2f;
        FingersScript.Instance.AddGesture(swipeUp);

    }

    void ConfigSwipeRight()
    {
        swipeRight.StateUpdated += OnSwipeRight;
        swipeRight.Direction = SwipeGestureRecognizerDirection.Right;
        swipeRight.MinimumDistanceUnits = 0.2f;
        FingersScript.Instance.AddGesture(swipeRight);
    }

    void ConfigSwipeDown()
    {
        swipeDown.StateUpdated += OnSwipeDown;
        swipeDown.Direction = SwipeGestureRecognizerDirection.Down;
        swipeDown.MinimumDistanceUnits = 0.2f;
        FingersScript.Instance.AddGesture(swipeDown);
    }

    void ConfigSwipeLeft()
    {
        swipeLeft.StateUpdated += OnSwipeLetf;
        swipeLeft.Direction = SwipeGestureRecognizerDirection.Left;
        swipeLeft.MinimumDistanceUnits = 0.2f;
        FingersScript.Instance.AddGesture(swipeLeft);
    }
}

