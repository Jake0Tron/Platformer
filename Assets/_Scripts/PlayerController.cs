using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    // private vars
    private GameMaster gm;
    private Animator anim;

    // stats

    // public vars
    /// <summary>
    /// Max sped player can travel in air or on ground
    /// </summary>
    public float maxSpeed = 3.0f;
    /// <summary>
    /// acceleration speed of player from stop
    /// </summary>
    public float speed = 50.0f;
    /// <summary>
    /// force exerted on player to jump upwards
    /// </summary>
    public float jumpPower = 150.0f;
    /// <summary>
    /// factor by which player decelerates on ground
    /// </summary>
    public float frictionFactor = 0.85f;
    /// <summary>
    /// speed applied to player in negative direction when on wall (to project upwards after)
    /// </summary>
    public float wallJumpSpeed = 1.5f;
	/// <summary>
	/// Count of number of dashes player can use
	/// </summary>
	public int dashCount = 0;

    public bool wallSliding;
    public bool grounded;
    public bool canDblJump;

    public int remainingHealth;
    public int maxHealth = 5;

    // TODO: Pool projectiles, limit to a certain number (5? think boshy)

    public Rigidbody2D rb;
    public MobileControls mobi;

    public Transform wallCheckPoint;
    public bool wallCheck;
    public LayerMask wallLayerMask;
    public bool facingRight = true;

    #region mobile input methods
	/// <summary>
	/// called on Left mobile Button press
	/// </summary>
    public void MoveLeft()
    {
        transform.localScale = new Vector3(-1, 1, 1);
        facingRight = false;
        rb.AddForce((Vector2.right * -speed));
    }
	/// <summary>
	/// Called on RIght Mobile Button Press
	/// </summary>
    public void MoveRight()
    {
        transform.localScale = new Vector3(1, 1, 1);
        facingRight = true;
        rb.AddForce((Vector2.right * speed));
    }
    #endregion mobile input

    #region Player action Behaviour 
    /// <summary>
    /// applies an upward force onto the player's rigidbody
    /// </summary>
    public void Jump()
    {
        // Double Jump capability
        if (grounded)
        {
            // first jump
            this.rb.AddForce(Vector2.up * jumpPower);
            this.canDblJump = true;
        }	
        else if (!grounded && canDblJump)
        {
            // double jump
            this.canDblJump = false;
            this.rb.velocity = new Vector2(rb.velocity.x, 0);
            this.rb.AddForce(Vector2.up * this.jumpPower * 0.85f);
        }
        if (wallSliding)
        {
            // TWEAK JUMPS OFF WALL
            if (facingRight)
            {
                rb.AddForce(new Vector2(-1, wallJumpSpeed) * jumpPower);
            }
            else
            {
                rb.AddForce(new Vector2(1, wallJumpSpeed) * jumpPower);
            }
        }
    }

    /// <summary>
    /// Applies dmgAmt of damage to player
    /// </summary>
    /// <param name="dmgAmt">amount of damage to be applied</param>
    public void Damage(int dmgAmt)
    {
        if (dmgAmt >= this.remainingHealth)
        {
            this.remainingHealth = 0;
        }
        else
        {
            this.remainingHealth -= dmgAmt;
            this.anim.Play("Player_Damage");
        }
    }

    /// <summary>
    /// exerts a force on the player for a duration of time sending them in the reverse x direction by a factor of KnockPower
    /// </summary>
    /// <param name="knockDur">length of time player is knocked up</param>
    /// <param name="knockPwr">power of the force applied to player's rigidbody</param>
    /// <param name="knockDir">direction to apply the knockback (generally backwards)</param>
    /// <returns></returns>
    public IEnumerator KnockBack(float knockDur, float knockPwr, Vector3 knockDir)
    {
        float timer = 0.0f;
        while (knockDur > timer)
        {
            timer += Time.deltaTime;

            // reset velocity
            rb.velocity = new Vector2(rb.velocity.x, 0);

            // potential for Damage boosting here! 

            rb.AddForce( new Vector3(knockDir.x * 100, -knockDir.y * knockPwr, transform.position.z));
            //rb.AddForce(new Vector3(knockDir.x * -100, -knockDir.y * knockPwr, transform.position.z));
        }
        yield return 0;
    }
    
	/// <summary>
    /// called on death to reset level. 
    /// HANDLE CHECKPOINTS AND SUCH HERE IF NECESSARY
    /// </summary>
    void Die()
    {
        if (this.remainingHealth <= 0)
        {
            // do something on death here!
            // Death UI? Stats? High Score?
            Application.LoadLevel(Application.loadedLevel);
        }
    }

    /// <summary>
    /// behaviour when player collides with a climbable wall
    /// </summary>
    void HandleWallSlide()
    {
        // if colliding with a climbable wall
        rb.velocity = new Vector2(rb.velocity.x, -0.7f);
        wallSliding = true;
        //canDblJump = true;

        if (Input.GetButtonDown("Jump"))
        {
            // TWEAK JUMPS OFF WALL
            if (facingRight)
            {
                rb.AddForce(new Vector2(-1, wallJumpSpeed) * jumpPower);
            }
            else
            {
                rb.AddForce(new Vector2(1, wallJumpSpeed) * jumpPower);
            }
        }
    }

    /// <summary>
    /// adds the specified amount of health (limited to max health)
    /// </summary>
    /// <param name="amount"></param>
    void GiveHealth(int amount)
    {
        if (this.remainingHealth + amount > this.maxHealth)
        {
            this.remainingHealth = this.maxHealth;
        }
        else
        {
            this.remainingHealth += amount;
        }
    }
    #endregion player action behaviour

    #region trigger behaviour
    /// <summary>
    /// Collision handling
    /// HANDLE PICKUPS ETC HERE
    /// </summary>
    /// <param name="col">collider that you're hitting</param>
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Coin"))
        {
            Destroy(col.gameObject);
            gm.points++;

        }
        else if (col.CompareTag("ManMug"))
        {
            Destroy(col.gameObject);
            gm.points+=3;
            GiveHealth(2);
        }
        else if (col.CompareTag("BeerCan"))
        {
            Destroy(col.gameObject);
            gm.points += 2;
            GiveHealth(1);
        }
        else if (col.CompareTag("Coffee"))
        {
            Destroy(col.gameObject);
            gm.points += 2;
            GiveHealth(1);
        }
        else if (col.CompareTag("Latte"))
        {
            Destroy(col.gameObject);
            gm.points += 3;
            GiveHealth(2);
        }
    }

    /// <summary>
    /// When Collision finishes (USED FOR TRIGGERS THAT WRITE TO GAMEMASTER INPUT 
    /// </summary>
    /// <param name="col">collider/trigger that player is exiting</param>
    void OnTriggerExit2D(Collider2D col)
    {
        gm.keyInputText.text = "";
    }
    #endregion trigger behaviour

    #region monobehaviour
    void Start()
    {
        this.mobi = GetComponent<MobileControls>();
        this.remainingHealth = this.maxHealth;
        this.rb = gameObject.GetComponent<Rigidbody2D>();
        this.anim = gameObject.GetComponent<Animator>();
        this.gm = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
    }

    void Update()
    {
        if (this.remainingHealth > this.maxHealth)
            this.remainingHealth = this.maxHealth;

        if (this.remainingHealth <= 0)
        {
            this.remainingHealth = 0;
            Invoke("Die", 1.0f);
        }

        // Animate
        this.anim.SetBool("Grounded", grounded);
        this.anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));

        // adjust direction
        if (Input.GetAxis("Horizontal") < -0.1f)
        {
            MoveLeft();
        }
        if (Input.GetAxis("Horizontal") > 0.1f)
        {
            MoveRight();
        }

        // Jumping - space
        if (Input.GetButtonDown("Jump"))
        {
            Jump();            
        }
    }


    /// <summary>
    /// applies a simulated friction to the player (no PhysicsMaterial needed)
    /// </summary>
    void SimulateFriction()
    {
        // simulate Friction on x axis
        Vector3 easeVel = rb.velocity;
        easeVel.x *= this.frictionFactor;
        easeVel.y = rb.velocity.y;
        easeVel.z = 0.0f;
        // fake friction
        if (grounded)
        {
            rb.velocity = easeVel;
        }
    }

    /// <summary>
    /// Handle Physics and input here
    /// </summary>
    void FixedUpdate()
    {
        SimulateFriction();

        // Horizontal movement
        float h = Input.GetAxis("Horizontal");
        if (grounded)
        {
            rb.AddForce((Vector2.right * speed) * h);
        }
        else
        {
            // reduce speed in the air?
            rb.AddForce((Vector2.right * speed * 1.0f) * h);
        }
        // limit max speed on x axis
        if (rb.velocity.x > this.maxSpeed)
        {
            rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
        }
        if (rb.velocity.x < -this.maxSpeed)
        {
            rb.velocity = new Vector2(-maxSpeed, rb.velocity.y);
        }

        if (!grounded)
        {
            wallCheck = Physics2D.OverlapCircle(wallCheckPoint.position, 0.1f, wallLayerMask);
            if (wallCheck && !grounded)
            {
                if ((facingRight && Input.GetAxis("Horizontal") > 0.1f) || (!facingRight && Input.GetAxis("Horizontal") < -0.1f))
                {
                    // holding left into wall or right into wall
                    HandleWallSlide();
                }
                else
                {
                    wallSliding = false;
                }
			}
			else
			{
				wallSliding = false;
			}
        }
    }
    #endregion Monobehaviour
}
