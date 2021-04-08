using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;

    void Update()
    {
        if (PlayerController.instance.isAlive)
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
