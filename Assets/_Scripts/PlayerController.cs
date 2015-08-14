using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    public float maxSpeed = 3.0f;
    public float speed = 50.0f;
    public float jumpPower = 150.0f;
    public float frictionFactor = 0.85f;
    public float wallJumpSpeed = 1.5f;

    public bool wallSliding;
    public bool grounded;
    public bool canDblJump;

    // stats
    public int remainingHealth;
    public int maxHealth = 5;

    private GameMaster gm;
    public Rigidbody2D rb;
    private Animator anim;
    public MobileControls mobi;

    public Transform wallCheckPoint;
    public bool wallCheck;
    public LayerMask wallLayerMask;
    public bool facingRight = true;

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

    void Die()
    {
        if (this.remainingHealth <= 0)
        {
            // do something on death here!
            // Death UI? Stats? High Score?
            Application.LoadLevel(Application.loadedLevel);
        }
    }

    void Start()
    {
        this.mobi = GetComponent<MobileControls>();
        this.remainingHealth = this.maxHealth;
        this.rb = gameObject.GetComponent<Rigidbody2D>();
        this.anim = gameObject.GetComponent<Animator>();
        this.gm = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
    }
    public void Jump()
    {
        // Double Jump capability
        if (grounded)
        {
            this.rb.AddForce(Vector2.up * jumpPower);
            this.canDblJump = true;
        }
        else if (!grounded && canDblJump)
        {
            this.canDblJump = false;
            this.rb.velocity = new Vector2(rb.velocity.x, 0);
            this.rb.AddForce(Vector2.up * this.jumpPower);
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

    public void MoveLeft()
    {
        transform.localScale = new Vector3(-1, 1, 1);
        facingRight = false;
        rb.AddForce((Vector2.right * -speed));
    }

    public void MoveRight()
    {
        transform.localScale = new Vector3(1, 1, 1);
        facingRight = true;
        rb.AddForce((Vector2.right * speed));
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

    // physics and input
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
        }
    }

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

    /// <summary>
    /// PICKUP
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

    void OnTriggerExit2D(Collider2D col)
    {
        gm.keyInputText.text = "";
    }
}
