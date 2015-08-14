using UnityEngine;
using System.Collections;

public class BaseEnemyController : MonoBehaviour {

    public int maxHealth = 5;
    public int remainingHealth;

    public Collider2D col;
    private GameMaster gm;

    public void Damage(int dmgAmt)
    {
        if (dmgAmt >= this.remainingHealth)
        {
            this.remainingHealth = 0;
        }
        else
        {
            this.remainingHealth -= dmgAmt;
            //this.anim.Play("Player_Damage");
        }
    }

    void Die()
    {
        // give 5 points on enemy death
        gm.points+=5;
        Destroy(gameObject);
    }

	// Use this for initialization
	void Start () {
        this.remainingHealth = maxHealth;
        this.col = gameObject.GetComponentInChildren<PolygonCollider2D>();
        this.gm = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
	}
	
	// Update is called once per frame
	void Update () {
        if (this.remainingHealth <= 0)
        {
            Die();
        }
	}

    public void OnTriggerEnter2D(Collider2D c)
    {
        if ( c.isTrigger)
        {
            //Debug.Log(c.name);
            
            if (c.CompareTag("AttackTrigger")){
                Debug.Log("Attack Trigger"); 
            }
        }
    }
}
