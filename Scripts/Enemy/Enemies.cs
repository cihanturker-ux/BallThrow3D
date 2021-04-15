using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    public static List<EnemyController> enemiesList = new List<EnemyController>();

    void Start()
    {
        EnemyController[] enemies = FindObjectsOfType<EnemyController>();
        
        foreach (EnemyController enemy in enemies)
        {
            enemiesList.Add(enemy);
        }
    }
}
