using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MobileControls : MonoBehaviour {

    public PlayerController player;
    public Button moveLeft, moveRight, jump, meleeButton, shootButton;
    public bool pushingMovementButton;

    private PlayerMeleeAttack melee;
    public PlayerShoot shoot;

	// Use this for initialization
	void Start () {
        this.player = GetComponentInParent<PlayerController>();
        this.shoot = this.player.GetComponentInChildren<PlayerShoot>();
        this.melee = GetComponentInParent<PlayerMeleeAttack>();
        this.pushingMovementButton = false;
	}

    public void Jump()
    {
        this.player.Jump();
    }

    public void Left()
    {
        this.player.MoveLeft();
    }

    public void Right()
    {
        this.player.MoveRight();
    }

    public void Melee()
    {
        this.melee.Attack();
    }

    public void Shoot()
    {
		Debug.Log("Shooting");
        this.shoot.Attack();
    }

    public void OnDirectionMove(string dir)
    {
        this.pushingMovementButton = true;
    }

    public void OnDirectionMoveStop()
    {
        this.pushingMovementButton = false;
    }

	// Update is called once per frame
	void Update () {
        if (this.pushingMovementButton)
        {
            if (this.player.facingRight)
            {
                Right();
            }
            else
            {
                Left();
            }
        }
	}
}
