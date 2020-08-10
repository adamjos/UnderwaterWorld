using UnityEngine;

public class Gun : MonoBehaviour
{

    public float damage = 10f;
    public float range = 100f;
    public float impactForce = 30f;
    public float fireRate = 15f;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public CharacterController playerController;
    public Transform player;

    private float nextTimeToFire = 0f;

    private void Start()
    {
        playerController = player.GetComponent<CharacterController>();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        } 

        if (Input.GetButton("Fire2"))
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
                playerController.Move(hookHit.point - player.position);
            }
        }
    }

}
