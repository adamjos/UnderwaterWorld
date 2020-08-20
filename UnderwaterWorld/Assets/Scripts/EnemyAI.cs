using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


public class EnemyAI : MonoBehaviour
{

    public Transform target;

    public float speed = 2f;
    public float nextWaypointDistance = 3f;
    public float stoppingDistance = 10f;
    public float followDistance = 30f;


    Rigidbody rb;
    CharacterCombat combat;
    CharacterStats targetStats;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        combat = GetComponent<CharacterCombat>();
        targetStats = target.GetComponent<CharacterStats>();
    }



    private void Update()
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        if ((distanceToTarget < followDistance))
        {
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            Quaternion lookRot = Quaternion.LookRotation(directionToTarget, Vector3.up);
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 10f));
            rb.AddForce(directionToTarget * speed * Time.deltaTime);

            if (distanceToTarget < stoppingDistance)
            {
                rb.velocity = Vector3.zero;
                combat.Attack(targetStats);
            }
                
        }
    }
            
}




