using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System;

public class LevelController : MonoBehaviour {

    [Header("Scene Objects")]
    public GameObject boxAreaPlaneXposi;
    public GameObject boxAreaPlaneXnega;
    public GameObject boxAreaPlaneYposi;
    public GameObject boxAreaPlaneYnega;
    public GameObject boxAreaPlaneZposi;
    public GameObject boxAreaPlaneZnega;

    [Header("Settings")]
    public int sizeX = 5;
    public int sizeY = 5;
    public int sizeZ = 5;
    public GameObject snapPointPrefab;
    public GameObject blockPiecePrefab;
    public TextAsset levelxml;
    public Material defaultBlockMaterial;
    public string snapToggleKey = "q";

    [Header("Variables")]
    public List<Block> allBlocks;
    public List<GameObject> snapPointObjects;
    public bool showSnapPoints = false;
    
    public static LevelController instance;

    void Awake()
    {
        instance = this;
    }

	// Use this for initialization
	void Start () {
        ReadLevelXml(levelxml);
        CreateSnapPointVisuals();
        ShowSnapPoints(showSnapPoints);
        UpdateAreaBoxSize();
    }
	
	// Update is called once per frame
	void Update () {
        //snap points on/ff
        if (Input.GetKeyDown(snapToggleKey))
        {
            ShowSnapPoints(!showSnapPoints);
        }
	}

    public void ShowSnapPoints(bool t)
    {
        showSnapPoints = t;
        foreach (GameObject snap in snapPointObjects)
        {
            snap.SetActive(showSnapPoints);
        }
    }

    void UpdateAreaBoxSize()
    {
        boxAreaPlaneXposi.transform.position = new Vector3(sizeX / 2f, 0, 0);
        boxAreaPlaneXposi.transform.localScale = new Vector3(0, sizeY, sizeZ);
        boxAreaPlaneXnega.transform.position = new Vector3(- sizeX / 2f, 0, 0);
        boxAreaPlaneXnega.transform.localScale = new Vector3(0, sizeY, sizeZ);
        boxAreaPlaneYposi.transform.position = new Vector3(0, sizeY / 2f, 0);
        boxAreaPlaneYposi.transform.localScale = new Vector3(sizeX, 0, sizeZ);
        boxAreaPlaneYnega.transform.position = new Vector3(0, - sizeY / 2f, 0);
        boxAreaPlaneYnega.transform.localScale = new Vector3(sizeX, 0, sizeZ);
        boxAreaPlaneZposi.transform.position = new Vector3(0, 0, sizeZ / 2f);
        boxAreaPlaneZposi.transform.localScale = new Vector3(sizeX, sizeY, 0);
        boxAreaPlaneZnega.transform.position = new Vector3(0, 0, - sizeZ / 2f);
        boxAreaPlaneZnega.transform.localScale = new Vector3(sizeX, sizeY, 0);
    }

    void CreateSnapPointVisuals()
    {
        List<GameObject> temp = new List<GameObject>(snapPointObjects);
        foreach (GameObject snap in temp)
        {
            snapPointObjects.Remove(snap);
            Destroy(snap.gameObject);
        }

        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                for (int z = 0; z < sizeZ; z++)
                {
                    snapPointObjects.Add(Instantiate(snapPointPrefab, GetWorldPosition(x, y, z), Quaternion.identity, transform) as GameObject);
                }
            }
        }
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
        XmlNode l = levelNodelist[0];

        //set size
        try
        {
            sizeX = Int32.Parse(l.Attributes["x"].Value);
            sizeY = Int32.Parse(l.Attributes["y"].Value);
            sizeZ = Int32.Parse(l.Attributes["z"].Value);
        }
        catch (FormatException e)
        {
            Debug.LogError(e.Message);
        }

        //find all blocks
        XmlNodeList blockNodelist = l.SelectNodes("block");

        foreach (XmlNode block in blockNodelist)
        {
            //create block
            Block b = (new GameObject("block")).AddComponent<Block>();
            allBlocks.Add(b);

            //create material
            Material mat = new Material(defaultBlockMaterial);
            mat.color = BlockColor.FromString(block.Attributes["color"].Value);

            b.SetMaterial(mat);

            //find children (pieces)
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
