using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Timer : MonoBehaviour {

    float curTime = 0f, startTime = 0f, splitTime = 0f;

    GameMaster gm;

    void Awake()
    {
        this.gm = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
    }

	// Use this for initialization
	void Start () {
        this.startTime = Time.time;        
	}

    void CalculateTimer()
    {
        this.splitTime = (this.curTime - this.startTime);
    }

	// Update is called once per frame
	void Update () {
        this.curTime = Time.time;
        CalculateTimer();
        if (gm.isPlaying)
            this.gm.timerText.text = this.splitTime.ToString("0:00");
	}
}
