using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Block : MonoBehaviour {

    public Material material;
    public List<BlockPiece> pieces = new List<BlockPiece>();

    public void SetMaterial(Material mat)
    {
        if (mat == null) return;

        material = mat;
        foreach (BlockPiece p in pieces)
        {
            p.SetMaterial(mat);
        }
    }

	public void AddPiece(int x, int y, int z)
    {
        if (LevelController.instance.IsWithinArea(x, y, z))
        {

            BlockPiece p = LevelController.instance.GetBlockPiece(x, y, z);
            if (p == null)
            {
                p = (Instantiate(LevelController.instance.blockPiecePrefab, transform) as GameObject).GetComponent<BlockPiece>();
                p.Init(x, y, z);
                p.SetMaterial(material);
            }
            else
            {
                Debug.Log("Adding block piece failed. Position is occupied!");
            }
        } else
        {
            Debug.LogError("Adding block piece failed. Position is out of bounds!");
        }
    }

    public BlockPiece GetBlockPiece(int x, int y, int z)
    {
        foreach (BlockPiece p in pieces)
        {
            if (x == p.x && y == p.y && z == p.z)
            {
                return p;
            }
        }
        return null;
    }
}
