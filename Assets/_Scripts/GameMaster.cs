using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameMaster : MonoBehaviour {
    ///HANDLES TEXT UPDATE ON PICKUP ETC

    public bool isPlaying;
    public int points;
    public Text pointsText;
    public Text keyInputText;
    public Text timerText;

    void Start()
    {
        this.isPlaying = true;
    }

	// Update is called once per frame
	void Update () {
        pointsText.text = "Points: " + points;
	}
}
