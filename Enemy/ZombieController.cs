using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    public bool hasStopped = false;
    public bool isDead = false;
    public float stoppingDistance = 3f;
    private float timeOfLastAttack = 0;
    public NavMeshAgent agent = null;
    private Animator anim = null;
    private ZombieStats zombieStats = null;
    [SerializeField] private Transform target;
    [SerializeField] private Collider[] ragdollColliders;
    [SerializeField] private Rigidbody[] ragdollRigidbodies;

    private void Start()
    {
        GetReferences();

        // Disable ragdoll at start
        foreach (Collider col in ragdollColliders)
        {
            if(!col.CompareTag("Enemy"))
            {
                col.enabled = false;
            }
        }
        foreach (Rigidbody rb in ragdollRigidbodies)
        {
            rb.isKinematic = true;
        }
    }

    private void Update()
    {
        if(!isDead)
        {
            target = PlayerController.instance;
            MoveToTarget();
            RotateToTarget();
        }
    }

    private void MoveToTarget()
    {
        agent.SetDestination(target.position);
        anim.SetFloat("Speed", 1f, 0.2f, Time.deltaTime);
        RotateToTarget();
        
        float distanceToTarget = Vector3.Distance(target.position, transform.position);
        
        if(distanceToTarget <= agent.stoppingDistance)
        {
            anim.SetFloat("Speed", 0f, 0.05f, Time.deltaTime);

            if(!hasStopped)
            {
                hasStopped = true;
                timeOfLastAttack = Time.time;
            }
            
            if(Time.time >= timeOfLastAttack + zombieStats.attackSpeed)
            {
                timeOfLastAttack = Time.time;
                CharacterStats targetStats = target.GetComponent<CharacterStats>();
                AttackTarget(targetStats);
            }
        }
        else
        {
            if(hasStopped)
                hasStopped = false;
        }
    }

    private void RotateToTarget()
    {
        //transform.LookAt(target);
        Vector3 direction = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = rotation;
    }

    private void AttackTarget(CharacterStats statsToDamage)
    {
        anim.SetTrigger("attack");
        zombieStats.DealDamage(statsToDamage);
    }

    public void Die()
    {
        isDead = true;
        agent.enabled = false;
        anim.enabled = false;   
        zombieStats.enabled = false;
        foreach (Collider col in ragdollColliders)
        {
            col.enabled = true;
        }
        foreach (Rigidbody rb in ragdollRigidbodies)
        {
            rb.isKinematic = false;
        }

        // Disable collider so you dont walk into it when zombies is dead
        foreach (Collider col in ragdollColliders)
        {
            if(col.CompareTag("Enemy"))
            {
                col.enabled = false;
            }
            else if(col.CompareTag("Player"))
            {
                col.enabled = false;
            }
        }
    }

    private void GetReferences()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        zombieStats = GetComponent<ZombieStats>();
        target = PlayerController.instance;
        ragdollColliders = GetComponentsInChildren<Collider>();
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
    }
}