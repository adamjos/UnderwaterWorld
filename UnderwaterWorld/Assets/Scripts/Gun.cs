using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    public int damage = 10; // Gun damage
    public float range = 100f; // Gun range
    public float impactForce = 30f; // Force applied to target
    public float fireRate = 0.5f; // Shoots per seconds

    public int maxAmmo = 10;
    private int currentAmmo;
    public float reloadTime = 1f;
    private bool isReloading = false;

    public Animator animator;
    public GameObject scopeOverlay;
    public GameObject weaponCamera;

    public float scopedFOV = 15f;
    private float normalFOV;

    public float upRecoil = 0f;
    public float sideRecoil = 0f;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public AudioSource shootingSound;
    public AudioSource reloadSound;

    private float nextTimeToFire = 0f; // Shooting cooldown

    private MouseLook mouseLook;

    private void Start()
    {
        currentAmmo = maxAmmo;
        shootingSound = GetComponents<AudioSource>()[0];
        reloadSound = GetComponents<AudioSource>()[1];
        mouseLook = fpsCam.GetComponent<MouseLook>();
        normalFOV = fpsCam.fieldOfView;
    }

    private void OnEnable()
    {
        isReloading = false;
        animator.SetBool("Reloading", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isReloading)
        {
            return;
        }

        if (currentAmmo <= 0 || Input.GetKeyDown(KeyCode.R))
        {
            if (scopeOverlay != null)
            {
                OnUnscoped();
            }
            animator.SetBool("Aiming", false);
            animator.SetBool("Shooting", false);
            StartCoroutine(Reload());
            return;
        }


        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;

            Shoot();
        } else //if (Input.GetButtonUp("Fire1"))
        {
            animator.SetBool("Shooting", false);
        }
        
        if (Input.GetButtonDown("Fire2"))
        {
            animator.SetBool("Aiming", true);

            if (scopeOverlay != null)
            {
                StartCoroutine(OnScoped());
            }

        } else if (Input.GetButtonUp("Fire2"))
        {
            animator.SetBool("Aiming", false);

            if (scopeOverlay != null)
            {   
                OnUnscoped();
            }

        }

    }

    void OnUnscoped ()
    {
        scopeOverlay.SetActive(false);
        weaponCamera.SetActive(true);

        fpsCam.fieldOfView = normalFOV;
    }

    IEnumerator OnScoped ()
    {
        yield return new WaitForSeconds(0.15f);
        scopeOverlay.SetActive(true);
        weaponCamera.SetActive(false);

        normalFOV = fpsCam.fieldOfView;
        fpsCam.fieldOfView = scopedFOV;
    }

    IEnumerator Reload ()
    {
        isReloading = true;
        reloadSound.Play();
        Debug.Log("Reloading...");

        animator.SetBool("Reloading", true);

        yield return new WaitForSeconds(reloadTime -0.25f);
        animator.SetBool("Reloading", false);
        yield return new WaitForSeconds(0.25f);

        currentAmmo = maxAmmo;
        isReloading = false;
        if (Input.GetButton("Fire2"))
        {
            animator.SetBool("Aiming", true);
            if (scopeOverlay != null)
            {
                StartCoroutine(OnScoped());
            }
        }
    }

    void Shoot ()
    {
        muzzleFlash.Play();
        shootingSound.Play();
        animator.SetBool("Shooting", true);
        mouseLook.AddRecoil(upRecoil / 5f, sideRecoil / 5f);
        currentAmmo--;

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

    public int GetCurrentAmmo ()
    {
        return currentAmmo;
    }

}
