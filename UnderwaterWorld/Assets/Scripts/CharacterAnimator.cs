using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    private Animator animator;
    private CharacterCombat combat;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        combat = GetComponent<CharacterCombat>();
        rb = GetComponent<Rigidbody>();

        combat.OnAttack += SetAttacking;
        combat.OnFinishedAttack += SetNotAttacking;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Magnitude(rb.velocity) > 0f)
        {
            animator.SetBool("IsMoving", true);
        } else
        {
            animator.SetBool("IsMoving", false);
        }
        
    }

    void SetAttacking ()
    {
        animator.SetBool("IsAttacking", true);
    }

    void SetNotAttacking ()
    {
        animator.SetBool("IsAttacking", false);
    }

}
