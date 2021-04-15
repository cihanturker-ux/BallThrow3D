using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public static PlayerAnimator instance;

    public Animator animator;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (PlayerController.instance.isAlive && !PlayerController.instance.hasGameFinished)
        {
            if (Input.GetMouseButtonDown(0))
            {
                animator.SetBool("isShooting", true);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                animator.SetBool("isShooting", false);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
