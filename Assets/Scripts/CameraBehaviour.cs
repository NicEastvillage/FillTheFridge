using UnityEngine;
using System.Collections;

public class CameraBehaviour : MonoBehaviour {

    public float zoom = 10;
    public float currentAngleR = 0;
    public float currentAngleH = 0;
    public float speed = 0.1f;
    public float zoomSpeed = 0.2f;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(1))
        {
            currentAngleR = (currentAngleR + Input.GetAxis("Mouse X") * speed) % 360;
            currentAngleH = Mathf.Clamp(currentAngleH + Input.GetAxis("Mouse Y") * speed, -80, 80);
        }

        zoom = Mathf.Clamp(zoom - Input.mouseScrollDelta.y * zoomSpeed, 1.1f, 3.2f);

        UpdatePosition();
	}

    void UpdatePosition()
    {
        transform.position = new Vector3(zoom + Mathf.Pow(zoom, 3), 0, 0);
        transform.LookAt(new Vector3());
        transform.RotateAround(new Vector3(), Vector3.up, currentAngleR);
        transform.RotateAround(new Vector3(), transform.right, -currentAngleH);
    }
}
