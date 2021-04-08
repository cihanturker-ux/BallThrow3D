using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    public static List<Transform> enemiesList;

    void Start()
    {
        enemiesList = new List<Transform>();

        foreach (Transform child in transform)
        {
            enemiesList.Add(child);
        }
    }
}
