using UnityEngine;
using System.Collections;

public class MouseController : MonoBehaviour {

    [Header("Scene and settings")]
    public Camera camera;
    public GameObject moveIndicatorPrefab;

    [Header("Variables")]
    public Transform moveIndicator;
    public GameObject dragObject;
    public Vector3 dragNormal;
    public Vector3 dragHitPoint;
    public Vector3 dragObjectRelativeToHitPoint;

    public static MouseController instance;

    void Awake()
    {
        instance = this;
    }

	// Use this for initialization
	void Start () {
	    if (moveIndicator == null)
        {
            moveIndicator = (Instantiate(moveIndicatorPrefab, transform) as GameObject).transform;
            moveIndicator.gameObject.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {

        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            moveIndicator.transform.position = hitInfo.collider.gameObject.transform.position + hitInfo.normal * 0.51f;
            moveIndicator.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);

            moveIndicator.gameObject.SetActive(true);

            if (Input.GetMouseButtonDown(0))
            {
                dragObject = hitInfo.collider.gameObject;
                dragNormal = hitInfo.normal;
                dragHitPoint = hitInfo.point;
                dragObjectRelativeToHitPoint = dragObject.transform.position - dragHitPoint;
            }

        } else
        {
            moveIndicator.gameObject.SetActive(false);
        }

        if (dragObject != null)
        {
            Plane nPlane = new Plane(dragNormal, dragHitPoint);
            float dist = -1;
            if (nPlane.Raycast(ray, out dist)) { };

            if (dist != -1)
            {
                /*
                Vector3 wantedPos = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));
                Vector3 newPos = draggedObject.transform.position;

                if (draggingNormal != Vector3.forward) newPos.z = wantedPos.z;
                if (draggingNormal != Vector3.right) newPos.x = wantedPos.x;
                if (draggingNormal != Vector3.up) newPos.y = wantedPos.y;
                */

                Vector3 newPos = ray.GetPoint(dist) + dragObjectRelativeToHitPoint;

                dragObject.transform.position = newPos;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            dragObject = null;
        }
	}
}
