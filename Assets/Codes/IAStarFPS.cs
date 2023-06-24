using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IAStarFPS : MonoBehaviour
{
    public GameObject target;
    public NavMeshAgent agent;
    public Animator anim;
    public SkinnedMeshRenderer render;
    public float DistanceToAttack = 3f;
    [SerializeField] private bool openWorldVariant;
    private float timeToDespawn = 10f;
    public float openWorldMoveSpeed = 20f;
    private Rigidbody rb;
    public enum States
    {
        pursuit,
        atacking,
        stoped,
        dead,
        damage,
        patrol,
    }

    public States state;

    // Start is called before the first frame update
    void Start()
    {
        if (!target)
        {
            target= GameObject.FindGameObjectWithTag("Player");
        }

        StartCoroutine("StoppedState");
    }

    internal void Damage()
    {
        StateMachine(States.damage);
    }

    // Update is called once per frame
    void Update()
    {
        if (!openWorldVariant)
        {
            anim.SetFloat("Velocidade", agent.velocity.magnitude);
        }

        if (openWorldVariant)
        {
            if (Vector3.Distance(transform.position, target.transform.position) <= 20f)
            {
                transform.LookAt(target.transform.position);
            }

            rb = gameObject.GetComponent<Rigidbody>();

            if (Vector3.Distance(transform.position, target.transform.position) <= 3)
            {
                rb.isKinematic = true;
            }   
            else
            {
                rb.isKinematic = false;
            }

            if (timeToDespawn >= 0)
            {
                timeToDespawn -= Time.deltaTime;
            }

            if (timeToDespawn <= 0 && Vector3.Distance(transform.position, target.transform.position) >= 55f && !render.isVisible)
            {
                Destroy(gameObject);
            }
        }
    }

    void StateMachine(States _state)
    {
        state= _state;
        switch (state)
        {
            case States.pursuit:
                StartCoroutine("PursuitState");
                break;
            case States.atacking:
                StartCoroutine("AttackState");
                break;
            case States.stoped:
                StartCoroutine("StoppedState");
                break;
            case States.dead:
                StartCoroutine("Dead");
                break;
            case States.damage:
                StartCoroutine("Damage");
                break;
            case States.patrol:
                StartCoroutine("Patrol");
                break;
        }
    }
    Vector3 RandomPosition(float range)
    {
        Vector3 pos;
        pos = transform.position + new Vector3(UnityEngine.Random.Range(-range, range)
            , 0
            , UnityEngine.Random.Range(-range, range));
          return pos;
    }
    private IEnumerator Patrol()
    {
        if (!openWorldVariant)
        {
            agent.isStopped = false;
            agent.destination = RandomPosition(20);
        }
        else
        {
            transform.position = transform.position;
            anim.SetFloat("Velocidade", 0.4f);
        }
      
        anim.SetBool("Attack", false);
        anim.SetBool("Damage", false);
        yield return new WaitForSeconds(1);
        if (Vector3.Distance(transform.position, target.transform.position) < DistanceToAttack * 4)
        {
            StateMachine(States.pursuit);
        }
        else
        if (UnityEngine.Random.value > 0.5)
        {
            StateMachine(States.stoped);
        }
        else
        {
            StateMachine(States.patrol);
        }
    }

    
    
    IEnumerator DamageState()
    {
        if (!openWorldVariant)
        {
            agent.isStopped = true;
        }
        else
        {
            transform.position = transform.position;
            anim.SetFloat("Velocidade", 0.4f);
        }

        anim.SetBool("Damage", true);
        for (int i = 0; i < 4; i++)
        {
            render.material.EnableKeyword("_EMISSION");
            yield return new WaitForSeconds(0.05f);
            render.material.DisableKeyword("_EMISSION");
            yield return new WaitForSeconds(0.05f);
        }
        StateMachine(States.pursuit);
    }

    public void Dead()
    {
        StopAllCoroutines();
        StateMachine(States.dead);
       
    }


    IEnumerator PursuitState()
    {
        if (!openWorldVariant)
        {
            agent.isStopped = false;
            agent.destination = target.transform.position;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, openWorldMoveSpeed * Time.deltaTime);
            anim.SetFloat("Velocidade", 3.6f);
        }

        anim.SetBool("Attack", false);
        anim.SetBool("Damage", false);
        yield return new WaitForSeconds(0.1f);
        if (Vector3.Distance(transform.position, target.transform.position) < DistanceToAttack)
        {
            StateMachine(States.atacking);
        }
        else
        if (Vector3.Distance(transform.position, target.transform.position) > DistanceToAttack * 5)
        {
            StateMachine(state = States.stoped);
        }
        else
        { 
            StateMachine(States.pursuit);
        }
    }

    IEnumerator AttackState()
    {
        if (!openWorldVariant)
        {
            agent.isStopped = true;
        }
        else
        {
            transform.position = transform.position;
            anim.SetFloat("Velocidade", 0.4f);
        }

        anim.SetBool("Attack", true);
        anim.SetBool("Damage", false);
        yield return new WaitForSeconds(0.1f);
        if (Vector3.Distance(transform.position, target.transform.position) > 4)
        {
            StateMachine(States.pursuit);
        }
        else
        {
           
            StateMachine(States.atacking);
        }
    }

    IEnumerator StoppedState()
    {
        if (!openWorldVariant)
        {
            agent.isStopped = true;
        }
        else
        {
            transform.position = transform.position;
            anim.SetFloat("Velocidade", 0.4f);
        }

        anim.SetBool("Attack", false);
        anim.SetBool("Damage", false);
        yield return new WaitForSeconds(1f);
        if (Vector3.Distance(transform.position, target.transform.position) < DistanceToAttack * 3)
        {
            StateMachine(States.pursuit);
        }else
        if (UnityEngine.Random.value > 0.5)
        {
            StateMachine(States.patrol);

        }
        else
        {
            StateMachine(States.stoped);
        }
    }

    IEnumerator DeadState()
    {
        if (!openWorldVariant)
        {
            agent.isStopped = true;
        }
        else
        {
            transform.position = transform.position;
            anim.SetFloat("Velocidade", 0.4f);
        }

        anim.SetBool("Attack", false);
        anim.SetBool("Dead", true);
        anim.SetBool("Damage", false);
        yield return new WaitForSeconds(0.05f);
    }

   
}
