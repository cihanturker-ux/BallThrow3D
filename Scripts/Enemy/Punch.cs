using UnityEngine;

public class Punch : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController.instance.isAlive = false;
            TapToStart.instance.LevelFailed();

            foreach (Transform enemy in Enemies.instance.enemies[0])
            {
                enemy.GetComponent<EnemyController>().agent.ResetPath();
            }
        }
    }
}
