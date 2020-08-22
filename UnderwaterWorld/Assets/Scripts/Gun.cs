using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    public int damage = 10; // Gun damage
    public float range = 100f; // Gun range
    public float impactForce = 30f; // Force applied to target
    public float fireRate = 0.5f; // Shoots per seconds
    public float upRecoil = 0f;
    public float sideRecoil = 0f;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public AudioSource shootingSound;

    private float nextTimeToFire = 0f; // Shooting cooldown

    private MouseLook mouseLook;

    private void Start()
    {
        shootingSound = GetComponent<AudioSource>();
        mouseLook = fpsCam.GetComponent<MouseLook>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;

            Shoot();
        } 

    }

    void Shoot ()
    {
        muzzleFlash.Play();
        shootingSound.Play();
        mouseLook.AddRecoil(upRecoil / 5f, sideRecoil / 5f);

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            CharacterStats targetStats = hit.transform.GetComponent<CharacterStats>();
            Target target = hit.transform.GetComponent<Target>();

            if (target != null)
            {
                target.TakeDamage(damage);
            }

            if (targetStats != null)
            {
                targetStats.TakeDamage(damage);
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 2f);

        }
    }

}
