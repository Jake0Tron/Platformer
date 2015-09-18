using UnityEngine;
using System.Collections;

public class PlayerDash : MonoBehaviour
{

	public float dashForce;
	public float upwardForce;

	public PlayerController player;

	// Use this for initialization
	void Start()
	{
		this.player = GetComponentInParent<PlayerController>();
	}

	void Dash()
	{
		if (this.player.facingRight){
			this.player.rb.AddForce(new Vector2(dashForce * this.player.maxSpeed, this.player.rb.velocity.y + upwardForce));
		}
		else
		{
			this.player.rb.AddForce(new Vector2(dashForce * -this.player.maxSpeed, this.player.rb.velocity.y + upwardForce));
		}

		this.player.dashCount--;
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.C))
		{
			if (this.player.dashCount > 0)
			{
				Dash();
			}
		}
	}
}
