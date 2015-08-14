using UnityEngine;
using System.Collections;

public class PlayerMeleeAttack : MonoBehaviour {

    //toggles a trigger that will determine if something collides, and deals damage

    private bool attacking = false;

    private float attackTimer = 0;
    private float attackCD = 0.3f;

    public Collider2D attackTrigger;

    private Animator anim;

	void Awake () {
        anim = gameObject.GetComponent<Animator>();
        attackTrigger.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("f") && !attacking)
        {
            attacking = true;
            attackTimer = attackCD;
            attackTrigger.enabled = true;
        }
        if (attacking)
        {
            if (attackTimer > 0)
            {
                attackTimer -= Time.deltaTime;
            }
            else
            {
                attacking = false;
                attackTrigger.enabled = false;
            }
        }
            anim.SetBool("Attacking", attacking);
	}
}
