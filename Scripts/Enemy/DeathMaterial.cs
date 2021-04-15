using UnityEngine;

public class DeathMaterial : MonoBehaviour
{
    [SerializeField] private Material Dissolve;
    [SerializeField] private GameObject deathExplosion;

    void Start()
    {
        GetComponent<SpawnEffect>().enabled = true;
        GetComponent<Renderer>().material = Dissolve;
        Instantiate(deathExplosion, transform.position, Quaternion.identity);
    }
}
