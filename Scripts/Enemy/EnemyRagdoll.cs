using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRagdoll : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private Animator animator = null;
    
    private Rigidbody[] ragdollBodies;
    private Collider[] ragdollColliders;

    public static EnemyRagdoll instance;

    private void Awake()
    {
        ragdollBodies = GetComponentsInChildren<Rigidbody>();
        ragdollColliders = GetComponentsInChildren<Collider>();
    }

    void Start()
    {
        
        ToggleRagdoll(false);
    }

    public void GetHit()
    {
        ToggleRagdoll(true);
        Vector3 explosionPos = new Vector3(-1,0.5f,-1);
        foreach (Rigidbody rb in ragdollBodies)
        {
            rb.AddExplosionForce(1f, explosionPos, 
                1f, 3.0f, ForceMode.Impulse);
        }
    }
    
    private void ToggleRagdoll(bool state)
    {
        animator.enabled = !state;

        foreach (Rigidbody rb in ragdollBodies)
        {
            rb.isKinematic = !state;
        }
        
        foreach (Collider collider in ragdollColliders)
        {
            //collider.enabled = state;
        }
    }
}
