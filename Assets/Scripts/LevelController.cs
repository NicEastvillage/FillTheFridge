using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System;

public class LevelController : MonoBehaviour {

    [Header("Settings")]
    public int sizeX = 5;
    public int sizeY = 5;
    public int sizeZ = 5;
    public GameObject blockPiecePrefab;
    public TextAsset levelxml;

    [Header("Variables")]
    public List<Block> allBlocks;
    
    public static LevelController instance;

    void Awake()
    {
        instance = this;
    }

	// Use this for initialization
	void Start () {
        ReadLevelXml(levelxml);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public bool IsWithinArea(int x, int y, int z)
    {
        return (x >= 0 && x < sizeX && y >= 0 && y < sizeY && z >= 0 && z < sizeZ);
    }

    public BlockPiece GetBlockPiece(int x, int y, int z)
    {
        foreach (Block b in allBlocks)
        {
            BlockPiece p = b.GetBlockPiece(x, y, z);
            if (p != null) return p;
        }
        return null;
    }

    public float GetWorldX(int x)
    {
        return - (sizeX - 1) / 2f + x;
    }

    public float GetWorldY(int y)
    {
        return - (sizeY - 1) / 2f + y;
    }

    public float GetWorldZ(int z)
    {
        return - (sizeZ - 1) / 2f + z;
    }

    public Vector3 GetWorldPosition(int x, int y, int z)
    {
        return new Vector3(GetWorldX(x), GetWorldY(y), GetWorldZ(z));
    }

    void OnDrawGizmos()
    {
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                for (int z = 0; z < sizeZ; z++)
                {
                    Gizmos.DrawCube(GetWorldPosition(x, y, z), Vector3.one * 0.1f);
                }
            }
        }
    }

    void ReadLevelXml(TextAsset asset)
    {
        // See: http://unitynoobs.blogspot.dk/2011/02/xml-loading-data-from-xml-file.html

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(asset.text);

        XmlNodeList levelNodelist = xmlDoc.GetElementsByTagName("level");

        // TODO: foreach level ... 
        // this time we only read 1 level
        XmlNodeList blockNodelist = levelNodelist[0].SelectNodes("block");

        foreach (XmlNode block in blockNodelist)
        {
            Block b = (new GameObject("block")).AddComponent<Block>();
            allBlocks.Add(b);

            XmlNodeList pieceNodelist = block.SelectNodes("piece");

            foreach (XmlNode piece in pieceNodelist)
            {
                int x = 0;
                int y = 0;
                int z = 0;

                try
                {
                    x = Int32.Parse(piece.Attributes["x"].Value);
                    y = Int32.Parse(piece.Attributes["y"].Value);
                    z = Int32.Parse(piece.Attributes["z"].Value);

                    b.AddPiece(x, y, z);
                }
                catch (FormatException e)
                {
                    Debug.LogError(e.Message);
                }
            }
        }
    }
}
