using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

    private Vector2 velocity;
    
    public float smoothTimeY;
    public float smoothTimeX;

    public GameObject player;

    public bool cameraIsBound;

    public Vector3 minCamPos;
    public Vector3 maxCamPos;

	// Use this for initialization
	void Start () {
        this.player = GameObject.FindGameObjectWithTag("Player");
	}

    void FixedUpdate()
    {
        float posX = Mathf.SmoothDamp(transform.position.x, player.transform.position.x, ref velocity.x, smoothTimeX);
        float posY = Mathf.SmoothDamp(transform.position.y, player.transform.position.y, ref velocity.y, smoothTimeY);

        transform.position = new Vector3(posX, posY, transform.position.z);

        if (cameraIsBound)
        {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x , minCamPos.x, maxCamPos.x) , 
                                             Mathf.Clamp(transform.position.y , minCamPos.y, maxCamPos.y) ,
                                             Mathf.Clamp(transform.position.z , minCamPos.z, maxCamPos.z));
        }
    }
}
