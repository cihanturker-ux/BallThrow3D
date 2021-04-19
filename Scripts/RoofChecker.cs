using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofChecker : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> enemies = new List<GameObject>();

    void Start()
    {
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<EnemyController>().enabled = false;
        }
    }
        
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            foreach (GameObject enemy in enemies)
            {
                enemy.GetComponent<EnemyController>().enabled = true;
            }
        }
    }
}
