using UnityEngine;
using System.Collections;

public class BlockPiece : MonoBehaviour {

    public Vector3 coords;
    public Block block;

    public void Init(Block b, int x, int y, int z)
    {
        block = b;
        coords = new Vector3(x, y, z);
        transform.position = LevelController.instance.GetWorldPosition(x, y, z);
    }

    public void SetMaterial(Material mat)
    {
        if (mat == null) return;

        GetComponent<MeshRenderer>().material = mat;
    }

    public int x
    {
        get
        {
            return Mathf.FloorToInt(coords.x);
        }
    }

    public int y
    {
        get
        {
            return Mathf.FloorToInt(coords.y);
        }
    }

    public int z
    {
        get
        {
            return Mathf.FloorToInt(coords.z);
        }
    }
}
