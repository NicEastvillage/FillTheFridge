using UnityEngine;
using System.Collections;

public class MouseController : MonoBehaviour {

    [Header("Scene and settings")]
    public Camera camera;
    public GameObject moveIndicatorPrefab;
    public float blockDragVelocityLimit = 5;

    [Header("Variables")]
    public Transform moveIndicator;
    public Block dragBlock;
    public Vector3 dragNormal;
    public Vector3 dragHitPoint;
    public Vector3 dragblockRelativeToHitPoint;

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
            BlockPiece p = hitInfo.collider.GetComponent<BlockPiece>();
            if (p != null)
            {
                moveIndicator.transform.position = hitInfo.collider.gameObject.transform.position + hitInfo.normal * 0.51f;
                moveIndicator.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);

                moveIndicator.gameObject.SetActive(true);

                if (Input.GetMouseButtonDown(0))
                {
                    dragBlock = p.block;
                    dragNormal = hitInfo.normal;
                    dragHitPoint = hitInfo.point;
                    dragblockRelativeToHitPoint = dragBlock.transform.position - dragHitPoint;
                    dragBlock.rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                }
            }
        } else
        {
            moveIndicator.gameObject.SetActive(false);
        }

        if (dragBlock != null)
        {
            Plane nPlane = new Plane(dragNormal, dragHitPoint);
            float dist = -1;
            if (nPlane.Raycast(ray, out dist)) { };

            if (dist != -1)
            {
                Vector3 newPos = ray.GetPoint(dist) + dragblockRelativeToHitPoint;
                Vector3 mov = newPos - dragBlock.transform.position;
                if (mov.magnitude > blockDragVelocityLimit) mov *= blockDragVelocityLimit / mov.magnitude;

                dragBlock.rigidbody.velocity = mov;

                // TODO: Test this in the future. Maybes it is better:
                // http://answers.unity3d.com/questions/425403/detect-collisions-with-object-following-mouse-move.html
                // It uses a spring joint to pull the object to the mouse position
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (dragBlock != null)
            {
                dragBlock.SnapBasedOnPosition();
                dragBlock.rigidbody.constraints = RigidbodyConstraints.FreezeAll;
                dragBlock.rigidbody.velocity = Vector3.zero;
                dragBlock = null;
            }
        }
	}
}
