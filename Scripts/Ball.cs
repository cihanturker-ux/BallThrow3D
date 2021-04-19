using UnityEngine;

public class Ball : MonoBehaviour
{
    public static bool feedbackBool = false;
    public static int index = 0;

    void OnCollisionEnter(Collision other)
    { 
        gameObject.transform.GetChild(0).gameObject.SetActive(true); // activate explosion VFX

        GameObject otherObject = other.gameObject;

        if (otherObject.CompareTag("Enemy"))
        {
            if (!otherObject.GetComponent<EnemyController>().isDead)
            {
                otherObject.GetComponent<EnemyController>().isDead = true;

                if (feedbackBool)
                {
                    int randomIndex = Random.Range(0, TapToStart.instance.feedbacks.Count);
                    GameObject text = Instantiate(TapToStart.instance.feedbacks[randomIndex], TapToStart.instance.canvas.transform);
                    Destroy(text, 1f);
                }
                feedbackBool = !feedbackBool;

                SpecialPower.instance.UpdateFever(); // Update fever UI element

                gameObject.SetActive(false);
                otherObject.GetComponent<EnemyController>().agent.enabled = false;
                otherObject.GetComponent<EnemyController>().animator.SetBool("Walk", false);
                otherObject.GetComponent<EnemyController>().GetHit();
                otherObject.GetComponentInChildren<DeathMaterial>().enabled = true;

                Enemies.instance.enemies[0].Remove(otherObject.transform);
                Enemies.instance.JumpOverTheRoof();
            }
        }

        if (otherObject.CompareTag("Ground"))
        {
            Destroy(gameObject, 0.3f);
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
    }
}
