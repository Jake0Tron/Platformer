using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DoorController : MonoBehaviour {

    public int levelToLoad;
    private GameMaster gm;

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            gm.keyInputText.text = "Hit [E] to enter";
            if (Input.GetKeyDown("e"))
            {
                gm.isPlaying = false;
                Application.LoadLevel(levelToLoad);
            }
        }
    }

    public void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (Input.GetKeyDown("e"))
            {
                Application.LoadLevel(levelToLoad);
            }
        }
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            gm.keyInputText.text = "";
        }
    }

	// Use this for initialization
	void Start () {
        this.gm = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
