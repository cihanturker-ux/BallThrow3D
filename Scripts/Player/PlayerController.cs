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
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private float shootingPeriod = 0.6f;
    [SerializeField] private float shootingSpeed = 140f;

    private float verticalRotation = 0f;
    private float horizontalRotation = 0f;
    private bool hasCoroutineStarted = false;
    private Coroutine coroutine;

    private float timer;

    private Vector3 targetPosition;
    private Vector2 crosshairPos = new Vector2(Screen.width / 2, Screen.height / 2);

    void Awake()
    {
        instance = this;
        timer = shootingPeriod;
    }

    private void Update()
    {
        if (isAlive && !hasGameFinished) { ReadInput2(); }
    }
    
    private void FixedUpdate()
    {
        if (isAlive && hasRotationStarted && !hasGameFinished) { RotateThePlayer2(); }
    }

    private void ReadInput2()
    {
        timer += Time.deltaTime;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began: // basıldı
                    crosshair.position = crosshairPos;
                    crosshair.GetComponent<Image>().enabled = true;

                    if (timer >= shootingPeriod)
                    {
                        timer = 0f;
                        hasRotationStarted = true;

                        if (!hasCoroutineStarted)
                        {
                            hasCoroutineStarted = true;
                            coroutine = StartCoroutine(ShootContinuously2());
                            hasCoroutineStarted = false;
                        }
                    }
                    break;

                case TouchPhase.Moved:      // devam
                case TouchPhase.Stationary: // devam
                    Vector2 tempVector = crosshair.position;
                    tempVector += touch.deltaPosition * 0.4f;
                    crosshair.position = tempVector;
                    break;

                case TouchPhase.Ended: // çekildi
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
            crosshair.position = Input.mousePosition;
            crosshair.GetComponent<Image>().enabled = true;

            if (timer >= shootingPeriod)
            {
                timer = 0f;
                hasRotationStarted = true;

                if (!hasCoroutineStarted)
                {
                    hasCoroutineStarted = true;
                    coroutine = StartCoroutine(ShootContinuously2());
                    hasCoroutineStarted = false;
                }
            }
        }
        else if (Input.GetMouseButton(0))
        {
            crosshair.position = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            crosshair.position = Input.mousePosition;
            crosshair.GetComponent<Image>().enabled = false;

            hasRotationStarted = false;

            StopCoroutine(coroutine);
        }
    }

    private void RotateThePlayer2()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            float deltaX = touch.deltaPosition.x * rotationSpeed * Time.fixedDeltaTime;
            float deltaY = touch.deltaPosition.y * rotationSpeed * Time.fixedDeltaTime;

            verticalRotation -= deltaY;
            verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

            horizontalRotation += deltaX;
            horizontalRotation = Mathf.Clamp(horizontalRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0f);
        }
    }

    private void RotateThePlayer()
    {
        float deltaX = Input.GetAxis("Mouse X") * rotationSpeed * Time.fixedDeltaTime;
        float deltaY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.fixedDeltaTime;

        verticalRotation -= deltaY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

        horizontalRotation += deltaX;
        horizontalRotation = Mathf.Clamp(horizontalRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0f);
    }

    IEnumerator ShootContinuously2()
    {
        while (true)
        {
            targetPosition = transform.position + transform.forward * 10f;

            GameObject ball = Instantiate(ballPrefab, shootingPoint.position, Quaternion.LookRotation(targetPosition - transform.position));
            ball.GetComponent<Rigidbody>().AddForce((targetPosition - ball.transform.position) * shootingSpeed);
            Destroy(ball, 2f);

            yield return new WaitForSecondsRealtime(shootingPeriod);
        }
    }

    IEnumerator ShootContinuously()
    {
        while (true)
        {
            Vector3 touchedPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
            Vector3 targetPos = playerCamera.ScreenToWorldPoint(touchedPos);

            GameObject ball = Instantiate(ballPrefab, shootingPoint.position, Quaternion.LookRotation(targetPos - transform.position));
            ball.GetComponent<Rigidbody>().AddForce((targetPos - ball.transform.position) * shootingSpeed);
            Destroy(ball, 2f);

            yield return new WaitForSecondsRealtime(shootingPeriod);
        }
    }
}
