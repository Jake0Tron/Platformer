using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MobileControls : MonoBehaviour {

    public PlayerController player;
    public Button moveLeft, moveRight, jump, meleeButton, shootButton;
    public bool movingRight;
    public bool pushingMovementButton;
    int directionTouch, actionTouch;

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

        Touch[] touches = Input.touches;

        for (int i = 0; i < Input.touchCount; i++)
        {

            if (touches[i].phase == TouchPhase.Began) { 
                if (touches[i].position.x < 0.5f && touches[i].position.y < 0.25f)
                {
                    this.player.gm.debugText.text = "LEFT SIDE";
                    this.directionTouch = touches[i].fingerId;
                }
                else if (touches[i].position.x > 0.5f && touches[i].position.y < 0.25f)
                {
                    this.player.gm.debugText.text = "RIGHT SIDE";
                    this.actionTouch = touches[i].fingerId;
                }
            }
            if (touches[i].fingerId == directionTouch)
            {
                // handle individual directional differences here
                //if (touches[i].position.x){

                //}
            }
            else if (touches[i].fingerId == actionTouch)
            {
                // handle Action Differences here
            }

            // clear on release
            if (touches[i].phase == TouchPhase.Ended && (touches[i].fingerId == actionTouch))
            {
                this.player.gm.debugText.text = "";
                actionTouch = -1;
            }
            if (touches[i].phase == TouchPhase.Ended && (touches[i].fingerId == directionTouch))
            {
                this.player.gm.debugText.text = "";
                directionTouch = -1;
            }

        }






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
