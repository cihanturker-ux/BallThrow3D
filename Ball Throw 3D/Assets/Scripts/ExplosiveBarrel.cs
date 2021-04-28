using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    public static ExplosiveBarrel instance;
    public GameObject Barrel, BarrelMesh, ExplosionParticle, ExplosionArea;

    [SerializeField]
    private float range;
    
    void Awake()
    {
        instance = this;
        Barrel.SetActive(true);
        BarrelMesh.SetActive(true);
        ExplosionParticle.SetActive(false);
        ExplosionArea.SetActive(false);
    }
    
    public void ExplodeBarrels(bool isExplode)
    {
        if (isExplode)
        {
            StartCoroutine(ExplodeRoutine());
        }
    }

    IEnumerator ExplodeRoutine()
    {
        ExplosionParticle.SetActive(true);
        ExplosionArea.SetActive(true);
        BarrelMesh.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        ExplosionParticle.SetActive(false);
        ExplosionArea.SetActive(false);
        Barrel.SetActive(false);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Cake"))
        {
            ExplodeBarrels(true);
        }
    }
    
    
}
