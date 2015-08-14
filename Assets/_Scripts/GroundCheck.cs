using UnityEngine;
using System.Collections;

public class GroundCheck : MonoBehaviour {
    /// <summary>
    /// HANDLES PLAYER Animator variable for Grounded
    /// </summary>
    
    private PlayerController player;
	
    // Use this for initialization
	void Start () {
        this.player = gameObject.GetComponentInParent<PlayerController>();
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.isTrigger)
            player.grounded = true;
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (!col.isTrigger)
            player.grounded = true;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (!col.isTrigger)
            player.grounded = false;
    }
}
