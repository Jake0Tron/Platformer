using UnityEngine;
using System.Collections;

public class TurretAI : MonoBehaviour {

    public int remainingHealth;
    public int maxHealth;

    public float distance;
    public float wakeRange;
    public float shootInterval;
    public float bulletSpeed = 100;
    public float bulletTimer;

    public bool awake = false;
    public bool lookingRight = true;

    public GameObject bullet;
    public Transform target;
    public Animator anim;
    public Transform shootPointLeft, shootPointRight;
    public BoxCollider2D damageBox;
    private GameMaster gm;
    public Animation damageAnimation;

	void Awake () {
        this.anim = GetComponent<Animator>();
        this.damageAnimation = gameObject.GetComponent<Animation>();
        this.gm = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
	}

    void Start()
    {
        this.remainingHealth = maxHealth;
    }
	
	// Update is called once per frame
	void Update () {
        this.anim.SetBool("Awake", awake);
        this.anim.SetBool("LookingRight", lookingRight);
        // see if player is in range
        RangeCheck();
        //see which side player is on
        SideCheck();

        CheckDeath();
        
	}

    void CheckDeath()
    {
        if (remainingHealth <= 0)
        {
            gm.points += 5;
            Destroy(gameObject);
        }
    }

    void SideCheck()
    {
        if (target.transform.position.x > transform.position.x)
        {
            lookingRight = true;
        }
        else if (target.transform.position.x < transform.position.x)
        {
            lookingRight = false;
        }
    }

    /// <summary>
    /// check if player is in range
    /// </summary>
    void RangeCheck()
    {
        // get distance between target and turret
        this.distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance < wakeRange)
            awake = true;
        else
            awake = false;
    }
    // shoots bullets
    public void Attack(bool attackingRight)
    {
        // time out shots
        bulletTimer += Time.deltaTime;
        
        if (bulletTimer >= shootInterval)
        {
            // shoot a bullet at player:
            // get direction to shoot
            Vector2 direction = target.transform.position - transform.position;
            // set to a unit vector
            direction.Normalize();
            GameObject bulletClone;
            if (attackingRight)
            {
                // shoot right
                bulletClone = Instantiate(bullet, shootPointRight.transform.position, shootPointRight.transform.rotation) as GameObject;
                bulletClone.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
            }
            else
            {
                // shoot left
                bulletClone = Instantiate(bullet, shootPointLeft.transform.position, shootPointLeft.transform.rotation) as GameObject;
                bulletClone.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
            }
            bulletTimer = 0;
        }
    }

    public void Damage(int dmgAmt)
    {
        if (dmgAmt >= this.remainingHealth)
        {
            this.remainingHealth = 0;
        }
        else
        {
            this.remainingHealth -= dmgAmt;
        }
        gameObject.GetComponent<Animation>().Play("Turret_TakeDamage");
    }

    
}
