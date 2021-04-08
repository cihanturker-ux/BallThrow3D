using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cake : MonoBehaviour
{
    private Rigidbody cakeRb;

    void Awake()
    {
        cakeRb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //cakeRb.AddForce(0,0,-100,ForceMode.Impulse);
    }
    
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyController>().agent.enabled = false;
            other.gameObject.GetComponent<EnemyController>().animator.SetBool("Walk", false);
            other.gameObject.GetComponent<EnemyController>().GetHit();
            this.gameObject.SetActive(false);
            other.gameObject.GetComponentInChildren<DeathMaterial>().enabled = true;
        }
    }
}
