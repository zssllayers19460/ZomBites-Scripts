using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    public bool hasStopped = false;
    public bool isDead = false;
    public float stoppingDistance = 3f;
    public GameObject[] loot;
    public NavMeshAgent agent = null;
    [SerializeField] private float detectionRadius = 20f; // The radius within which the zombie can detect players
    private float timeOfLastAttack = 0;
    private bool isWandering = false; // Flag to indicate if the zombie is currently wandering
    private Vector3 wanderDestination; // The destination point for wandering
    private Transform target;
    private Animator anim = null;
    private ZombieStats zombieStats = null;
    private Ragdoll ragdoll;

    private void Start()
    {
        GetReferences();
    }

    private void Update()
    {

        FindTarget(); // New function to find the closest player in range
        MoveToTarget();
        RotateToTarget();
    }

    public void OnShot(Transform shooter)
    {
        // Calculate the direction from the zombie to the shooter
        Vector3 directionToShooter = shooter.position - transform.position;

        // Set the target to the shooter and move towards the shooter
        target = shooter;
        agent.SetDestination(target.position);
        anim.SetFloat("Speed", 1f, 0.2f, Time.deltaTime);
        RotateToTarget();

        // Check if the direction to the shooter is within a certain angle
        float angleToShooter = Vector3.Angle(directionToShooter, transform.forward);
        if (angleToShooter < 90f)
        {
            // Set the destination to the shooter position
            agent.SetDestination(target.position);
        }
        else
        {
            // Set the destination to a point closer to the shooter along the direction vector
            Vector3 destination = transform.position + directionToShooter.normalized * stoppingDistance;
            agent.SetDestination(destination);
        }

        // Stop wandering behavior
        StopCoroutine(Wander());
        isWandering = false;
    }

    private void FindTarget()
    {
        // Find all GameObjects with the "Player" tag within the detection radius
        Collider[] players = Physics.OverlapSphere(transform.position, detectionRadius);
        List<Transform> playerTransforms = new List<Transform>();

        // Filter the player objects from the colliders based on the tag
        foreach (Collider player in players)
        {
            if (player.CompareTag("Player"))
            {
                playerTransforms.Add(player.transform);
            }
        }

        // If there are players in range, set the closest one as the target
        if (playerTransforms.Count > 0)
        {
            float closestDistance = Mathf.Infinity;
            foreach (Transform playerTransform in playerTransforms)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
                if (distanceToPlayer < closestDistance)
                {
                    closestDistance = distanceToPlayer;
                    target = playerTransform;
                }
            }
        }
        else
        {
            // If no players are in range, set target to null to keep wandering
            target = null;
        }
    }

    private void MoveToTarget()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);
            anim.SetFloat("Speed", 1f, 0.2f, Time.deltaTime);
            RotateToTarget();

            float distanceToTarget = Vector3.Distance(target.position, transform.position);

            if (distanceToTarget <= agent.stoppingDistance)
            {
                anim.SetFloat("Speed", 0f, 0.05f, Time.deltaTime);

                if (!hasStopped)
                {
                    hasStopped = true;
                    timeOfLastAttack = Time.time;
                }

                if (Time.time >= timeOfLastAttack + zombieStats.attackSpeed)
                {
                    timeOfLastAttack = Time.time;
                    CharacterStats targetStats = target.GetComponent<CharacterStats>();
                    AttackTarget(targetStats);
                }
            }
            else
            {
                if (hasStopped)
                    hasStopped = false;
            }
        }
        else
        {
            // If no target, start wandering
            if (!isWandering)
            {
                StartCoroutine(Wander());
            }
        }
    }

    private IEnumerator Wander()
    {
        isWandering = true;

        // Generate a random destination within a certain range
        Vector3 randomDestination = transform.position + Random.insideUnitSphere * 10f;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDestination, out hit, 10f, NavMesh.AllAreas);

        wanderDestination = hit.position;
        agent.SetDestination(wanderDestination);
        anim.SetFloat("Speed", 1f, 0.2f, Time.deltaTime);
        RotateToTarget();

        while (isWandering)
        {
            // Check if the zombie has reached the destination
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                // Wait for a random amount of time before generating a new destination
                float wanderWaitTime = Random.Range(1f, 5f);
                yield return new WaitForSeconds(wanderWaitTime);

                // Generate a new random destination
                randomDestination = transform.position + Random.insideUnitSphere * 10f;
                NavMesh.SamplePosition(randomDestination, out hit, 10f, NavMesh.AllAreas);

                wanderDestination = hit.position;
                agent.SetDestination(wanderDestination);
                anim.SetFloat("Speed", 1f, 0.2f, Time.deltaTime);
                RotateToTarget();
            }

            yield return null;
        }
    }

    private void RotateToTarget()
    {
        if (target != null)
        {
            Vector3 direction = target.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = rotation;
        }
    }

    private void AttackTarget(CharacterStats statsToDamage)
    {
        if(isDead == false)
        {
            anim.SetTrigger("attack");
            zombieStats.DealDamage(statsToDamage);
        }
    }

    public void Die()
    {
        // Disables the ZombieController script 
        enabled = false;
        isDead = true;
        agent.enabled = false;
        anim.enabled = false;
        zombieStats.enabled = false;
        hasStopped = true;
        zombieStats.canAttack = false;
        ragdoll.EnableRagdoll();

        // Loot Drops
        int randomDrop = Random.Range(0, 10);
        int randomPickup = Random.Range(0, loot.Length - 1);

        if (randomDrop <= 1)
        {
            Instantiate(loot[randomPickup], transform.position + new Vector3(0, 0.15f, 0), Quaternion.identity);
        }
        // Disable collider so you don't walk into it when the zombie is dead
        ragdoll.DisableRagdollColliders();
    }

    private void GetReferences()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        zombieStats = GetComponent<ZombieStats>();
        ragdoll = GetComponent<Ragdoll>();
    }
}