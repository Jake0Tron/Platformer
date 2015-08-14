using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthHUD : MonoBehaviour {
    // for storage
    public Sprite[] hearts;
    // for display
    public Image heartsUI;

    private PlayerController player;

	void Start () {
        this.player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
	}
	
	void Update () {
	    // update which sprites are shown according to player health
        heartsUI.sprite = hearts[player.remainingHealth];
	}
}
