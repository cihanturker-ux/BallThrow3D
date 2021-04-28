using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    
    [SerializeField] private RectTransform crosshair;

    [HideInInspector] public bool isAlive = true;
    [HideInInspector] public bool hasGameStarted = false;
    [HideInInspector] public bool hasGameFinished = false;
    [HideInInspector] public bool hasRotationStarted = false;

    [Header("Player Controls")]
    public float shootingPeriod = 0.6f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private GameObject ballPrefab, ball2Prefab;
    [SerializeField] private float shootingSpeed = 140f;

    private float verticalRotation = 0f;
    private float horizontalRotation = 0f;
    private bool hasCoroutineStarted = false;
    private Coroutine coroutine;

    private float timer;

    private Vector3 targetPosition;
    private Vector2 center;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        center = new Vector2(Screen.width / 2, Screen.height / 2);
        crosshair.position = center;

        timer = shootingPeriod;
#if UNITY_EDITOR
        rotationSpeed = 400f;
#endif
    }

    private void Update()
    {
        if (isAlive && !hasGameFinished)
        {
#if UNITY_EDITOR
            ReadInput();
#elif UNITY_ANDROID
            ReadInput2();
#endif
        }
    }

    private void FixedUpdate()
    {
        if (isAlive && hasRotationStarted && !hasGameFinished)
        {
#if UNITY_EDITOR
            float deltaX = Input.GetAxis("Mouse X") * rotationSpeed * Time.fixedDeltaTime;
            float deltaY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.fixedDeltaTime;
            RotateThePlayer(deltaX, deltaY);
#elif UNITY_ANDROID
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                float deltaX = touch.deltaPosition.x * rotationSpeed * Time.fixedDeltaTime;
                float deltaY = touch.deltaPosition.y * rotationSpeed * Time.fixedDeltaTime;
                RotateThePlayer(deltaX, deltaY);
            }
#endif
        }
    }

    private void ReadInput2()
    {
        timer += Time.deltaTime;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    crosshair.position = center;
                    crosshair.GetComponent<Image>().enabled = true;

                    if (timer >= shootingPeriod)
                    {
                        timer = 0f;
                        hasRotationStarted = true;

                        if (!hasCoroutineStarted)
                        {
                            hasCoroutineStarted = true;
                            coroutine = StartCoroutine(ShootContinuously());
                            hasCoroutineStarted = false;
                        }
                    }
                    break;

                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    Vector2 tempVector = crosshair.position;
                    tempVector += touch.deltaPosition * 0.25f;
                    crosshair.position = tempVector;
                    break;

                case TouchPhase.Ended:
                    crosshair.GetComponent<Image>().enabled = false;
                    hasRotationStarted = false;
                    StopCoroutine(coroutine);
                    break;
            }
        }

    }

    private void ReadInput()
    {
        timer += Time.deltaTime;

        if (Input.GetMouseButtonDown(0))
        {
            crosshair.position = center;
            crosshair.GetComponent<Image>().enabled = true;

            if (timer >= shootingPeriod)
            {
                timer = 0f;
                hasRotationStarted = true;

                if (!hasCoroutineStarted)
                {
                    hasCoroutineStarted = true;
                    coroutine = StartCoroutine(ShootContinuously());
                    hasCoroutineStarted = false;
                }
            }
        }
        else if (Input.GetMouseButton(0))
        {
            Vector2 tempVector = crosshair.position;
            float x = Input.GetAxis("Mouse X") * 3f;
            float y = Input.GetAxis("Mouse Y") * 3f;
            tempVector += new Vector2(x, y);
            crosshair.position = tempVector;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            crosshair.GetComponent<Image>().enabled = false;
            hasRotationStarted = false;
            StopCoroutine(coroutine);
        }
    }

    private void RotateThePlayer(float deltaX, float deltaY)
    {
        verticalRotation -= deltaY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

        horizontalRotation += deltaX;
        horizontalRotation = Mathf.Clamp(horizontalRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0f);
    }

    IEnumerator ShootContinuously()
    {
        while (true)
        {
            targetPosition = transform.position + transform.forward * 10f;

            if (!SpecialPower.instance.isPowerEnabled)
            {
                GameObject ball = Instantiate(ballPrefab, shootingPoint.position, Quaternion.LookRotation(targetPosition - transform.position));
                ball.GetComponent<Rigidbody>().AddForce((targetPosition - ball.transform.position) * shootingSpeed);
                Destroy(ball, 2f);
                yield return new WaitForSecondsRealtime(shootingPeriod);
            }
            else if (SpecialPower.instance.isPowerEnabled)
            {
                GameObject ball2 = Instantiate(ball2Prefab, shootingPoint.position, Quaternion.LookRotation(targetPosition - transform.position));
                ball2.GetComponent<Rigidbody>().AddForce((targetPosition - ball2.transform.position) * shootingSpeed * 2);
                Destroy(ball2, 2f);
                yield return new WaitForSecondsRealtime(shootingPeriod / 2f);
            }
        }
    }
}
