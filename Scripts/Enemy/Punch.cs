using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerController.instance.isAlive = false;
            
            print("GameOver");
        }
    }
}
