using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private Rigidbody[] ragdollBodies;
    private Collider[] ragdollColliders;
    private EnemyState enemyStateChecker;

    public static EnemyController instance;
    public Animator animator;
    public NavMeshAgent agent;
    public Transform target;
    //public float lookRadius = 5f;

    void Awake()
    {
        enemyStateChecker = GetComponent<EnemyState>();
        instance = this;
        animator = GetComponentInChildren<Animator>();
        ragdollBodies = GetComponentsInChildren<Rigidbody>();
        ragdollColliders = GetComponentsInChildren<Collider>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        ToggleRagdoll(false);
        animator.SetBool("Idle", true);
    }

    void Update()
    {
        //float distance = Vector3.Distance(target.position, transform.position);
        GetControl();
    }

    void GetControl()
    {
        if (enemyStateChecker.state != EnemyState.State.DEAD)
        {
            if (enemyStateChecker.state == EnemyState.State.IDLE)
            {
                Time.timeScale = 1;
                animator.ResetTrigger("Punch");
                animator.SetBool("Idle", true);
                animator.SetBool("Walk", false);
            }
            else if (enemyStateChecker.state == EnemyState.State.WALK)
            {
                animator.SetBool("Idle", false);
                animator.SetBool("Walk", true);
                agent.SetDestination(target.position);
            }
            else if (enemyStateChecker.state == EnemyState.State.ATTACK)
            {
                agent.speed = 0f;
                StartCoroutine(SetPunch());
            }
        }
    }

    IEnumerator SetPunch()
    {
        Time.timeScale = 0.4f;
        animator.SetBool("Idle", false);
        animator.SetBool("Walk", false);
        animator.SetTrigger("Punch");
        yield return new WaitForSeconds(1);
        animator.ResetTrigger("Punch");
        animator.SetBool("Idle", true);
    }
    
    public void GetHit()
    {
        enemyStateChecker.Dead(true);
        Enemies.enemiesList.Remove(transform); // Listenin aktif olarak güncellenmesi, yeri değiştirilebilir.
        ToggleRagdoll(true);
        Vector3 explosionPos = new Vector3(-1,0.5f,-1);
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
    
    //void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, lookRadius);
    //}
}
