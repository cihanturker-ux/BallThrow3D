using UnityEngine;

public class Punch : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController.instance.isAlive = false;
            TapToStart.instance.LevelFailed();

            foreach (EnemyController enemy in Enemies.enemiesList)
            {
                enemy.agent.ResetPath();
            }
        }
    }
}
