using UnityEngine;
using System.Collections;

public class BossDropper : MonoBehaviour {
    public BoxCollider2D trigger;
    public BoxCollider2D holder;

	// Use this for initialization
	void Start () {
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            this.holder.gameObject.SetActive(false);
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            this.holder.gameObject.SetActive(false);
        }
    }
}
