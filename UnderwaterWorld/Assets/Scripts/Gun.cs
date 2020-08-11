using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    public float damage = 10f;
    public float range = 100f;
    public float impactForce = 30f;
    public float fireRate = 0.5f;

    public float hookSpeed = 35f;
    public float hookTime = 1f;
    public float finishDist = 1f;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    public CharacterController playerController;
    public Transform player;
    
    private float nextTimeToFire = 0f;

    [SerializeField] private Transform pfHarpoonPhysics;

    private void Start()
    {
        playerController = player.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            HarpoonShoot();

            //Shoot();
        } 

        if (Input.GetButtonDown("Fire2"))
        {
            HookShoot();
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
            elapsedTime += Time.deltaTime;
            yield return null;
        }

    }
        
    void HarpoonShoot ()
    {
        Vector3 shootDir = fpsCam.transform.forward;
        Quaternion shootRot = fpsCam.transform.rotation * Quaternion.AngleAxis(90f, Vector3.right);

        Transform harpoonTransform = Instantiate(pfHarpoonPhysics, muzzleFlash.transform.position, shootRot);

        harpoonTransform.GetComponent<HarpoonPhysics>().Setup(shootDir);

        //GameObject harpoon = Instantiate(harpoonPrefab) as GameObject;
        //harpoon.transform.position = muzzleFlash.transform.position;
        //harpoon.transform.rotation = muzzleFlash.transform.rotation;
        //harpoon.transform.rotation = Quaternion.Euler(90f, muzzleFlash.transform.rotation.y, muzzleFlash.transform.rotation.z);
        //Rigidbody rb = harpoon.GetComponent<Rigidbody>();
        //rb.velocity = fpsCam.transform.forward * harpoonSpeed;
    }


}
