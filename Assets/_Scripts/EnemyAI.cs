using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private GameObject oldBody;
    [SerializeField] private GameObject newBody;

    public bool NONtransformingPlayer = false;

    [Header("Detection Settings")]
    [SerializeField] private float visionRange = 20f;
    [SerializeField] private float visionAngle = 60f; // Cone angle in degrees
    [SerializeField] private LayerMask visionLayerMask;

    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 3.5f;
    [SerializeField] private float chaseSpeed = 7f;
    [SerializeField] private float stoppingDistance = 1.5f;
    [SerializeField] private float angularSpeed = 120f;
    [SerializeField] private float acceleration = 8f;

    [Header("Roaming Settings")]
    [SerializeField] private float roamSpeed = 2.5f;
    [SerializeField] private float roamRadius = 15f;
    [SerializeField] private float roamWaitTime = 3f;

    [Header("Body Swap Distance")]
    [SerializeField] private float bodySwapDistance = 5f;

    private Transform playerTransform;
    private float distanceToPlayer;
    private bool canSeePlayer = false;
    private Vector3 roamDestination;
    private float roamTimer = 0f;

    private void Start()
    {
        // Initialize NavMeshAgent if not assigned
        if (navMeshAgent == null)
            navMeshAgent = GetComponent<NavMeshAgent>();

        // Find player with Player tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            playerTransform = playerObj.transform;
        else
            Debug.LogWarning("Player with tag 'Player' not found!");

        // Initialize bodies
        UpdateBodyState();

        // Initialize roaming
        PickNewRoamDestination();
        roamTimer = roamWaitTime;
    }

    private void Update()
    {
        if (playerTransform == null)
            return;

        // Calculate distance to player
        distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        // Check if player is visible
        canSeePlayer = IsPlayerInSight();

        // Update body state based on distance
        UpdateBodyState();

        // Control AI behavior
        if (canSeePlayer)
        {
            ChasePlayer();
        }
        else
        {
            Idle();
        }
    }

    private bool IsPlayerInSight()
    {
        if (playerTransform == null)
            return false;

        // Check distance
        if (distanceToPlayer > visionRange)
            return false;

        // Check angle
        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        if (angleToPlayer > visionAngle / 2f)
            return false;

        // Raycast to check for obstacles
        Ray ray = new Ray(transform.position + Vector3.up * 0.5f, directionToPlayer);
        if (Physics.Raycast(ray, distanceToPlayer, visionLayerMask))
            return false;

        return true;
    }

    private void ChasePlayer()
    {
        // Set NavMeshAgent destination to player
        navMeshAgent.SetDestination(playerTransform.position);

        // Apply chase speed settings
        navMeshAgent.speed = chaseSpeed;
        navMeshAgent.stoppingDistance = stoppingDistance;
        navMeshAgent.angularSpeed = angularSpeed;
        navMeshAgent.acceleration = acceleration;
    }

    private void Idle()
    {
        // Roam around
        Roam();
    }

    private void Roam()
    {
        // Update roam timer
        roamTimer -= Time.deltaTime;

        // Pick new destination if reached current one or timer expired
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance || roamTimer <= 0f)
        {
            PickNewRoamDestination();
            roamTimer = roamWaitTime;
        }

        // Apply roam speed settings
        navMeshAgent.speed = roamSpeed;
        navMeshAgent.stoppingDistance = 0.5f;
        navMeshAgent.angularSpeed = angularSpeed;
        navMeshAgent.acceleration = acceleration;
    }

    private void PickNewRoamDestination()
    {
        // Generate random point on navmesh within roam radius
        Vector3 randomDirection = Random.insideUnitSphere * roamRadius;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, roamRadius, NavMesh.AllAreas))
        {
            roamDestination = hit.position;
            navMeshAgent.SetDestination(roamDestination);
        }
    }

    private void UpdateBodyState()
    {

        if (!NONtransformingPlayer){
        // Swap bodies based on distance
        bool shouldUseNewBody = distanceToPlayer <= bodySwapDistance;

        if (oldBody != null)
            oldBody.SetActive(!shouldUseNewBody);

        if (newBody != null)
            newBody.SetActive(shouldUseNewBody);

    }
    }

    /// <summary>
    /// Returns whether the enemy can currently see the player
    /// </summary>
    public bool CanSeePlayer() => canSeePlayer;

    /// <summary>
    /// Returns the current distance to the player
    /// </summary>
    public float GetDistanceToPlayer() => distanceToPlayer;

    private void OnDrawGizmos()
    {
        // Draw vision range sphere
        Gizmos.color = canSeePlayer ? Color.red : Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRange);

        // Draw vision cone
        Vector3 coneDirection = transform.forward;
        float halfAngle = visionAngle / 2f;

        // Draw cone edges
        Gizmos.color = canSeePlayer ? Color.red : new Color(1f, 1f, 0f, 0.5f);

        // Left edge of cone
        Vector3 leftDirection = Quaternion.AngleAxis(-halfAngle, transform.up) * coneDirection;
        Gizmos.DrawLine(transform.position, transform.position + leftDirection * visionRange);

        // Right edge of cone
        Vector3 rightDirection = Quaternion.AngleAxis(halfAngle, transform.up) * coneDirection;
        Gizmos.DrawLine(transform.position, transform.position + rightDirection * visionRange);

        // Draw arc at end of cone (multiple points)
        int arcSegments = 8;
        for (int i = 0; i < arcSegments; i++)
        {
            float angle1 = -halfAngle + (halfAngle * 2f / arcSegments) * i;
            float angle2 = -halfAngle + (halfAngle * 2f / arcSegments) * (i + 1);

            Vector3 point1 = Quaternion.AngleAxis(angle1, transform.up) * coneDirection * visionRange;
            Vector3 point2 = Quaternion.AngleAxis(angle2, transform.up) * coneDirection * visionRange;

            Gizmos.DrawLine(transform.position + point1, transform.position + point2);
        }

        // Draw raycast to player if in sight
        if (canSeePlayer && Application.isPlaying)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position + Vector3.up * 0.5f, playerTransform.position);
        }

        // Draw body swap distance circle
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, bodySwapDistance);
    }
}
