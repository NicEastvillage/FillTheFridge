using UnityEngine;
using System.Collections;

public class CameraBehaviour : MonoBehaviour {

    public float distanceToCenter = 10;
    public float currentAngleR = 0;
    public float currentAngleH = 0;
    public float speed = 0.1f;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0))
        {
            currentAngleR = (currentAngleR + Input.GetAxis("Mouse X") * speed) % 360;
            currentAngleH = Mathf.Clamp(currentAngleH + Input.GetAxis("Mouse Y") * speed, -80, 80);
        }

        UpdatePosition();
	}

    void UpdatePosition()
    {
        transform.position = new Vector3(distanceToCenter, 0, 0);
        transform.LookAt(new Vector3());
        transform.RotateAround(new Vector3(), Vector3.up, currentAngleR);
        transform.RotateAround(new Vector3(), transform.right, -currentAngleH);
    }
}
