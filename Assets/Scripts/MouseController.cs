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
    public SpringJoint dragSpringJoint;

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

        if (Physics.Raycast(ray, out hitInfo, 100, LayerMask.GetMask("Block")) && dragBlock == null)
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

                    dragBlock.gameObject.layer = 9; // moving block
                    foreach (BlockPiece piece in dragBlock.pieces)
                    {
                        piece.gameObject.layer = 9; // moving block
                    }
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
                MoveBlockTo(dragBlock, newPos);
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (dragBlock != null)
            {
                dragBlock.SnapBasedOnPosition();
                dragBlock.rigidbody.constraints = RigidbodyConstraints.FreezeAll;
                dragBlock.rigidbody.velocity = Vector3.zero;

                dragBlock.gameObject.layer = 8; // block
                foreach (BlockPiece piece in dragBlock.pieces)
                {
                    piece.gameObject.layer = 8; // block
                }

                dragBlock = null;
            }
        }
	}

    void MoveBlockTo(Block b, Vector3 position)
    {
        // Only move a third of a block at the time; how many times is that (ceiled)?
        Vector3 diff = position - b.transform.position;
        int steps = Mathf.CeilToInt(Mathf.Max(Mathf.Abs(diff.x), Mathf.Abs(diff.y), Mathf.Abs(diff.z)) / 3f);
        for (int i = 0; i < steps; i++)
        {
            MoveBlockX(b, diff.x / steps);
            MoveBlockY(b, diff.y / steps);
            MoveBlockZ(b, diff.z / steps);
        }
    }

    void MoveBlockX(Block b, float vx)
    {
        // try to move block vx along x axis
        if (IsBlockColliding(b)) return; // if block is already colliding this function is meaningless

        b.transform.position += new Vector3(vx, 0, 0);

        // if it is now colliding, move back and move only half the amount instead
        if (IsBlockColliding(b)) 
        {
            b.transform.position -= new Vector3(vx, 0, 0);
            MoveBlockX(b, vx / 2);
            // is repeated until no collision
        }
    }

    void MoveBlockY(Block b, float vy)
    {
        // try to move block vy along y axis
        if (IsBlockColliding(b)) return; // if block is already colliding this function is meaningless

        b.transform.position += new Vector3(0, vy, 0);

        // if it is now colliding, move back and move only half the amount instead
        if (IsBlockColliding(b))
        {
            b.transform.position -= new Vector3(0, vy, 0);
            MoveBlockY(b, vy / 2);
            // is repeated until no collision
        }
    }

    void MoveBlockZ(Block b, float vz)
    {
        // try to move block vz along z axis
        if (IsBlockColliding(b)) return; // if block is already colliding this function is meaningless

        b.transform.position += new Vector3(0, 0, vz);

        // if it is now colliding, move back and move only half the amount instead
        if (IsBlockColliding(b))
        {
            b.transform.position -= new Vector3(0, 0, vz);
            MoveBlockZ(b, vz / 2);
            // is repeated until no collision
        }
    }

    bool IsBlockColliding(Block b)
    {
        foreach (BlockPiece p in b.pieces)
        {
            Collider[] cols = Physics.OverlapBox(p.transform.position, Vector3.one * .495f, Quaternion.identity, LayerMask.GetMask("Block", "Border"));

            if (cols.Length > 0)
            {
                return true;
            }
        }
        return false;
    }
}
