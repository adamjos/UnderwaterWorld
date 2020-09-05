using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingGun : MonoBehaviour
{

    public float range = 100f; // Grappling gun range
    public float grappleTime = 1f; // Travel time with grappling gun
    public float stoppingDist = 3f; // Stopping distance from target
    public float lineWidth = 0.05f; // Line width of grapple line

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public CharacterController playerController;
    public Transform player;
    public LineRenderer lineRenderer;

    private bool isGrappling = false;

    // Start is called before the first frame update
    void Start()
    {
        playerController = player.GetComponent<CharacterController>();
        lineRenderer = player.GetComponent<LineRenderer>();
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.sortingOrder = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !isGrappling)
        {
            GrappleShoot();
        }
        else
        {
            lineRenderer.enabled = false;
        }

    }

    void GrappleShoot()
    {
        
        RaycastHit grappleHit;

        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out grappleHit, range))
        {
            Debug.Log("Grapple hit " + grappleHit.transform.name);

            Target hookTarget = grappleHit.transform.GetComponent<Target>();
            if (hookTarget != null)
            {
                muzzleFlash.Play();
                isGrappling = true;
                StartCoroutine(SmoothLerp(grappleTime, grappleHit));
            }
        }
    }

    private IEnumerator SmoothLerp(float time, RaycastHit hookHit)
    {
        Vector3 startPos = player.position;
        Vector3 endPos = hookHit.point - (hookHit.point - player.position).normalized * stoppingDist;

        float elapsedTime = 0f;

        while (Input.GetButton("Fire1"))
        {
            player.position = Vector3.Lerp(startPos, endPos, (elapsedTime / time));
            lineRenderer.enabled = true;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, muzzleFlash.transform.position);
            lineRenderer.SetPosition(1, hookHit.point);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isGrappling = false;

    }

}
