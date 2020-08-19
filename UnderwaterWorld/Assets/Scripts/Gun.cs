using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    public float damage = 10f; // Gun damage
    public float range = 100f; // Gun range
    public float impactForce = 30f; // Force applied to target
    public float fireRate = 0.5f; // Shoots per seconds
    private float nextTimeToFire = 0f; // Shooting cooldown 

    public float hookTime = 1f; // Travel time with hook
    public float finishDist = 1f; // Stopping distance from target

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;


    public CharacterController playerController;
    public Transform player;
    public LineRenderer lineRenderer;
    public float lineWidth = 0.05f;

    [SerializeField] private Transform ProjectilePrefab;

    private void Start()
    {
        playerController = player.GetComponent<CharacterController>();
        lineRenderer = player.GetComponent<LineRenderer>();
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.sortingOrder = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            //ProjectileShoot();

            Shoot();
        } 

        if (Input.GetButtonDown("Fire2"))
        {
            HookShoot();
        } 
        else
        {
            lineRenderer.enabled = false;
        }

    }

    void Shoot ()
    {
        muzzleFlash.Play();

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 2f);

        }
    }

    void HookShoot ()
    {
        RaycastHit hookHit;

        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hookHit, range))
        {
            Debug.Log("Hook hit " + hookHit.transform.name);

            Target hookTarget = hookHit.transform.GetComponent<Target>();
            if (hookTarget != null)
            {
                StartCoroutine(SmoothLerp(hookTime, hookHit));
            }
        }
    }

    private IEnumerator SmoothLerp (float time, RaycastHit hookHit)
    {
        Vector3 startPos = player.position;
        Vector3 endPos = hookHit.point - (hookHit.point - player.position).normalized * finishDist;

        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            player.position = Vector3.Lerp(startPos, endPos, (elapsedTime / time));
            lineRenderer.enabled = true;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, muzzleFlash.transform.position);
            lineRenderer.SetPosition(1, hookHit.point);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

    }
        
    void ProjectileShoot ()
    {
        Vector3 shootDir = fpsCam.transform.forward;
        Quaternion shootRot = fpsCam.transform.rotation * Quaternion.AngleAxis(90f, Vector3.right);

        Transform harpoonTransform = Instantiate(ProjectilePrefab, muzzleFlash.transform.position, shootRot);

        harpoonTransform.GetComponent<ProjectilePhysics>().Setup(shootDir);

    }


}
