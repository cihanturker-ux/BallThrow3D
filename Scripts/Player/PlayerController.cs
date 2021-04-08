using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform point;
    [SerializeField] private GameObject cakePrefab;
    [SerializeField] private float shootingPeriod = 0.5f;
    [SerializeField] private float shootingSpeed = 100f;

    public bool isAlive = true;

    private float verticalRotation = 0f;
    private float horizontalRotation = 0f;
    private bool hasRotationStarted = false;
    private bool hasCoroutineStarted = false;
    private Coroutine coroutine;

    float timer = 0.0f;

    void Awake()
    {
        instance = this;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (isAlive) { ReadInput(); }
    }

    private void FixedUpdate()
    {
        if (isAlive && hasRotationStarted) { RotateThePlayer(); }
    }

    private void ReadInput()
    {
        timer += Time.deltaTime;

        if (Input.GetMouseButtonDown(0))
        {
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
        else if (Input.GetMouseButtonUp(0))
        {
            hasRotationStarted = false;
            StopCoroutine(coroutine);
        }
    }

    private void RotateThePlayer()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.fixedDeltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.fixedDeltaTime;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

        horizontalRotation += mouseX;
        horizontalRotation = Mathf.Clamp(horizontalRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0f);
    }

    IEnumerator ShootContinuously()
    {
        while (true)
        {
            Vector3 touchedPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
            Vector3 targetPos = playerCamera.ScreenToWorldPoint(touchedPos);

            GameObject cake = Instantiate(cakePrefab, point.position, Quaternion.LookRotation(targetPos - transform.position));
            cake.GetComponent<Rigidbody>().AddForce((targetPos - cake.transform.position) * shootingSpeed);
            Destroy(cake, 1.5f);

            yield return new WaitForSecondsRealtime(shootingPeriod);
        }
    }
}
