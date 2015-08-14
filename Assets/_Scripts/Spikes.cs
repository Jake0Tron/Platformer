using UnityEngine;
using System.Collections;

public class Spikes : MonoBehaviour {

    private PlayerController player;
    public int damageAmt = 2;
    public float knockDur = 0.02f;
    public float knockPwr = 350f;

    public float dmgTime, dmgCool = 1.5f;

    Vector3 knockDir;

	void Start () {
        this.player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && this.dmgTime >= dmgCool)
        {
            this.player.Damage(damageAmt);
            this.dmgTime = 0;
            StartCoroutine(this.player.KnockBack(knockDur, knockPwr, this.player.transform.position));
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player") && this.dmgTime >= dmgCool)
        {
            this.player.Damage(damageAmt);
            this.dmgTime = 0;
            StartCoroutine(this.player.KnockBack(knockDur, knockPwr, this.player.transform.position));
        }
    }

	void Update () {
        this.dmgTime += Time.deltaTime;
	}
}
