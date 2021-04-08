using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : MonoBehaviour
{
    public enum State
    {
        IDLE,
        WALK,
        ATTACK,
        DEAD
    }
    
    private Transform playerTarget;
    private State enemyState = State.WALK;
    public State state { get { return enemyState; } }
    
    private float distanceTotarget;

    void Awake()
    {
        playerTarget = GameObject.FindGameObjectWithTag("Player").transform;
        enemyState = State.WALK;
        Dead(false);
    }

    void Update()
    {
        SetState();
    }

    public void Dead(bool isDead)
    {
        if (isDead)
        {
            enemyState = State.DEAD;
        }
    }
    void SetState()
    {
        distanceTotarget = Vector3.Distance(transform.position, playerTarget.position);
        
        if (enemyState != State.DEAD)
        {
            if (!PlayerController.instance.isAlive)
            {
                enemyState = State.IDLE;
                print("state = Walk");
            }
            else if (distanceTotarget >= 1.8f)
            {
                enemyState = State.WALK;
                print("state = Walk");
            }
            else if (distanceTotarget < 1.8f)
            {
                enemyState = State.ATTACK;
                print("state = Attack");
            }
        }
        else if (enemyState == State.DEAD)
        {
            Destroy(this.gameObject, 2f);
            print("state = Dead");
        }

    }
}
