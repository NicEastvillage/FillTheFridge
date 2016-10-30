using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Block : MonoBehaviour {

    public List<BlockPiece> pieces = new List<BlockPiece>();

	public void AddPiece(int x, int y, int z)
    {
        BlockPiece p = LevelController.instance.GetBlockPiece(x, y, z);
        if (p == null)
        {
            p = (Instantiate(LevelController.instance.blockPiecePrefab, transform) as GameObject).GetComponent<BlockPiece>();
            p.Init(x, y, z);
        } else
        {
            Debug.Log("Adding block piece failed. Position is occupied!");
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
