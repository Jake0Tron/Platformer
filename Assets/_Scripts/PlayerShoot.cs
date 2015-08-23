using UnityEngine;
using System.Collections;

public class PlayerShoot : MonoBehaviour {

    public float shotInterval;
    public float shotTimer;
    public float bulletSpeed;
	public int shotLimit = 5;
	public int shotCount;
    public PlayerController player;
    public Transform shotPoint;
    
    public GameObject bullet;

    public void Attack()
    {
        Vector2 direction;
		if (shotCount < shotLimit)
		{
			if (player.facingRight)
				direction = new Vector2(bulletSpeed, 0);
			else
				direction = new Vector2(-bulletSpeed, 0);

			if (shotTimer >= shotInterval)
			{
				GameObject bulletClone;
				bulletClone = Instantiate(bullet, shotPoint.transform.position, shotPoint.transform.rotation) as GameObject;
				bulletClone.GetComponent<Rigidbody2D>().velocity = direction;
				shotTimer = 0;
			}
			//shotCount++;
		}
    }

	// Use this for initialization
	void Start () {
		this.shotCount = 0;
        this.player = gameObject.GetComponentInParent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
        shotTimer += Time.deltaTime;

        if (Input.GetButtonDown("Fire2"))
        {
            Attack();
        }
	}
}
