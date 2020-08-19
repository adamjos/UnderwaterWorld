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

    Path path;
    int currentWayPoint = 0;
    bool reachedEndOfPath = false;

    //Seeker seeker;
    Rigidbody rb;
    CharacterController controller;

    // Start is called before the first frame update
    void Start()
    {
        //seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();

        //InvokeRepeating("UpdatePath", 0f, 0.5f);
        
    }

    /*
    void UpdatePath ()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    void OnPathComplete (Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }
    */
    /*
    // Update is called once per frame
    void FixedUpdate()
    {
        if (path == null)
        {
            return;
        }

        if (currentWayPoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        } else
        {
            reachedEndOfPath = false;
        }

        Vector3 direction = (path.vectorPath[currentWayPoint] - rb.position).normalized;
        Vector3 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);
        
        float distance = Vector3.Distance(rb.position, path.vectorPath[currentWayPoint]);

        if (distance < nextWaypointDistance)
        {
            currentWayPoint++;
        }

    }
    */

    private void Update()
    {
        // calculate distance to player
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        if ((distanceToTarget < followDistance))
        {
            // set player as target
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            Quaternion lookRot = Quaternion.LookRotation(directionToTarget, Vector3.up);
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 10f));
            rb.AddForce(directionToTarget * speed * Time.deltaTime);

            if (distanceToTarget < stoppingDistance)
            {
                rb.velocity = Vector3.zero;
            }
                
            
                

            //start chasing target
            // call some enemy movement controller to move towards

                
            
                // Attack target
        }
    }
            
}




