using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MobileControls : MonoBehaviour {

    public PlayerController player;
    public Button moveLeft, moveRight, jump, meleeButton, shootButton;
    public bool movingRight;
    public bool pushingMovementButton;

    private PlayerMeleeAttack melee;
    public PlayerShoot shoot;

	// Use this for initialization
	void Start () {
        this.player = GetComponentInParent<PlayerController>();
        this.shoot = this.player.GetComponentInChildren<PlayerShoot>();
        this.melee = GetComponentInParent<PlayerMeleeAttack>();
        this.pushingMovementButton = false;
        this.movingRight = true;
	}

    public void Jump()
    {
        this.player.Jump();
    }

    public void Left()
    {
        this.pushingMovementButton = true;
        this.movingRight = false;
        this.player.MoveLeft();
    }

    public void Right()
    {
        this.pushingMovementButton = true;
        this.movingRight = true;
        this.player.MoveRight();
    }

    public void Melee()
    {
        this.melee.Attack();
    }

    public void Shoot()
    {
        this.shoot.Attack();
    }

    public void OnDirectionMove(string dir)
    {
        this.pushingMovementButton = true;
        if (dir.ToLower() == "right")
        {
            this.movingRight = true;
        }
        else if (dir.ToLower() == "left")
        {
            this.movingRight = false;
        }
    }

    public void OnDirectionMoveStop()
    {
        this.pushingMovementButton = false;
    }

	// Update is called once per frame
	void Update () {
        if (this.pushingMovementButton)
        {
            if (this.movingRight)
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
