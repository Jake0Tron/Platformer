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
		//Touch t = Input.GetTouch(0);
        Touch[] touches = Input.touches;

        for (int i = 0; i < Input.touchCount; i++)
        {

            if (touches[i].phase == TouchPhase.Began) { 
				//this.player.gm.debugText.text = touches[i].position.x.ToString() + " , " + touches[i].position.y.ToString() + "\n" + Screen.width.ToString() + " " + Screen.height.ToString();
				
				// THIS RETURNS THE PIXEL LOCATION OF THE TOUCH! 
				// USE SCREEN SIZE TO FACTOR PERCENTAGE
                
				if ((touches[i].position.x / Screen.width) < 0.35f && (touches[i].position.y / Screen.height) < 0.25f)
                {
					//this.player.gm.debugText.text ="Left "+ (touches[i].position.x / Screen.width) + " " + (touches[i].position.y / Screen.height);
                    this.directionTouch = touches[i].fingerId;
					if ((touches[i].position.x / Screen.width) < 0.175)
					{
						this.player.MoveLeft();
					}
					else if ((touches[i].position.x / Screen.width) > 0.175)
					{
						this.player.MoveRight();
					}
                }
                else if ((touches[i].position.x / Screen.width )> 0.65f && (touches[i].position.y / Screen.height) < 0.25f)
                {
					//this.player.gm.debugText.text ="Right "+ (touches[i].position.x / Screen.width) + " " + (touches[i].position.y / Screen.height);
                    this.actionTouch = touches[i].fingerId;
					if ((touches[i].position.x / Screen.width) < 0.825 && (touches[i].position.y / Screen.height) < 0.25)
					{
						this.shoot.Attack();
					}
					else if ((touches[i].position.x / Screen.width) > 0.825)
					{
						this.player.Jump();
					}
                }
            }
            // clear on release
            if (touches[i].phase == TouchPhase.Ended)
            {
				if (touches[i].fingerId == actionTouch)
				{
					actionTouch = -1;
				}else if (touches[i].fingerId == directionTouch){
					directionTouch = -1;
				}
            }
        }
	}
}
