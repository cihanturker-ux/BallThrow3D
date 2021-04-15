using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(true); // activate explosion VFX

        GameObject otherObject = other.gameObject;

        if (otherObject.CompareTag("Enemy"))
        {
            gameObject.SetActive(false);
            otherObject.GetComponent<EnemyController>().agent.enabled = false;
            otherObject.GetComponent<EnemyController>().animator.SetBool("Walk", false);
            otherObject.GetComponent<EnemyController>().GetHit();
            otherObject.GetComponentInChildren<DeathMaterial>().enabled = true;

            Enemies.enemiesList.Remove(otherObject.GetComponent<EnemyController>());

            if (Enemies.enemiesList.Count == 0)
            {
                TapToStart.instance.LevelFinished();
            }
        }

        if (otherObject.CompareTag("Ground"))
        {
            Destroy(gameObject, 0.3f);
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
    }
}
