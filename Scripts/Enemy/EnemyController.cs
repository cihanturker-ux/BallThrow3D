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
    private Transform target;

    void Awake()
    {
        enemyStateChecker = GetComponent<EnemyState>();
        instance = this;
        animator = GetComponentInChildren<Animator>();
        ragdollBodies = GetComponentsInChildren<Rigidbody>();
        ragdollColliders = GetComponentsInChildren<Collider>();
        agent = GetComponent<NavMeshAgent>();
        target = FindObjectOfType<PlayerController>().gameObject.transform;
    }

    void Start()
    {
        ToggleRagdoll(false);
        animator.SetBool("Idle", true);
    }

    void Update()
    {
        GetControl();
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject otherObject = other.gameObject;

        if (otherObject.CompareTag("ExplosionArea"))
        {
            GetComponent<EnemyController>().agent.enabled = false;
            GetComponent<EnemyController>().animator.SetBool("Walk", false);
            GetComponent<EnemyController>().GetHit();
            GetComponentInChildren<DeathMaterial>().enabled = true;
            
            Enemies.enemiesList.Remove(GetComponent<EnemyController>());
            if (Enemies.enemiesList.Count == 0)
            {
                TapToStart.instance.LevelFinished();
            }
        }
    }

    void GetControl()
    {
        if (enemyStateChecker.state != EnemyState.State.DEAD && PlayerController.instance.hasGameStarted)
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
        ToggleRagdoll(true);
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
