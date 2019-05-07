using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.U2D;

//Unity looks at each pixel and return color, based on color returns will place a tile

/// <summary>
/// THIS CLASS NO LONGER IS USED!! SWITCHED OVER TO USING UNITY'S BUILT IN TILE PALETTE
/// </summary>

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private Transform map;

    [SerializeField]
    private Texture2D[] mapData;        //Uses array so map can generate in layers: Grass/water & trees/characters

    [SerializeField]
    private MapElement[] mapElements;   //Array with tiles

    [SerializeField]
    private Sprite defaultTile;         //Used to measure space between tiles

    //Dictionary - collection that cosists of a key: used to access dictionary, value:output given from specific key

    private Dictionary<Point, GameObject> waterTiles = new Dictionary<Point, GameObject>();

    [SerializeField]
    private SpriteAtlas waterAtlas;


    /// <summary>
    /// The position of bottom left corner of screen; uses to generate map
    /// </summary>
    private Vector3 WorldStartPos
    {
        get
        {
            return Camera.main.ScreenToWorldPoint(new Vector3(0, 0));                                                                  
        }
    }


    
    // Start is called before the first frame update
    void Start()
    {
        //GameObject go = waterTiles[new Point(10, 2)];     //Hardcoded way to return exact gameObject in position

        GenerateMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    
    private void GenerateMap()
    {
        int height = mapData[0].height;
        int width = mapData[0].width;

        for (int i = 0; i < mapData.Length; i++)             //Runs through all layers to generate
        {
            for(int x = 0; x < mapData[i].width; x++)       //Runs through current layers width
            {
                for(int y = 0; y < mapData[i].height; y++)  //Runs through current layers height
                {
                    Color c = mapData[i].GetPixel(x, y);    //Gets color of current pixel

                    MapElement newElement = Array.Find(mapElements, e => e.MyColor == c);       //Looks through bitmap colors stores each pixel as newElement

                    if(newElement != null)     //If newElement matches same color as bitmap
                    {               
                        float xPos = WorldStartPos.x + (defaultTile.bounds.size.x * x);     //Correct placement of tiles horizontally by using the width of tile * the position it should be.
                        float yPos = WorldStartPos.y + (defaultTile.bounds.size.y * y);     //Correct placement of tiles vertically by using the height of tile * the position it should be.

                        //Creates tile
                        GameObject go = Instantiate(newElement.MyElementPrefab);    //Spawns corresponding prefab in gameworld

                        //Set tile position
                        go.transform.position = new Vector2(xPos, yPos);

                        if (newElement.MyTileTag == "Water")
                        {
                            waterTiles.Add(new Point(x,y), go);     //Uses point as a key (must be unique), Game object
                        }

                        //Checks if placing a tree
                        if(newElement.MyTileTag == "Tree")
                        {
                            go.GetComponent<SpriteRenderer>().sortingOrder = height*2 - y*2;    //Generates trees on layers, using their height * 2 - y*2; this reverses the values so the ones lower are in front 
                        }                                                                       //Must multiply by 2 so that player can set sorting order layer so they can walk behind trees
                                                                                                //For example one tree is 8 and the one above is 10; player sort order will set 9
                        //Makes tile a child of map
                        go.transform.parent = map;

                    }
                }
            }
        }

        CheckWater();

    }
   
    /// <summary>
    /// Used to figure out which water sprite to use based on how much land/water surrounds each square
    /// </summary>
    public void CheckWater()
    {
        foreach (KeyValuePair<Point, GameObject> tile in waterTiles)
        {
            string composition = TileCheck(tile.Key);

            if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'E' && composition[6] == 'W')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("0");
            }
            if (composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'E' && composition[6] == 'W')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("1");
            }
            if (composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'E' && composition[6] == 'E')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("2");
            }
            if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'W')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("3");
            }
            if (composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'E')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("4");
            }
            if (composition[1] == 'E' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'W')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("5");
            }
            if (composition[1] == 'W' && composition[4] == 'W' && composition[3] == 'E' && composition[6] == 'W')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("6");
            }
            if (composition[1] == 'W' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'E')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("7");
            }
            if (composition[1] == 'W' && composition[3] == 'E' && composition[4] == 'E' && composition[6] == 'E')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("8");
            }
            if (composition[1] == 'E' && composition[3] == 'E' && composition[4] == 'E' && composition[6] == 'W')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("9");
            }
            if (composition[1] == 'W' && composition[3] == 'E' && composition[4] == 'E' && composition[6] == 'W')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("10");
            }
            if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'E')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("11");
            }
            if (composition[1] == 'E' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'E')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("12");
            }
            if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'E' && composition[6] == 'E')
            {
                tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("13");
            }
            if (composition[3] == 'W' && composition[5] == 'E' && composition[6] == 'W')
            {
                GameObject go = Instantiate(tile.Value, tile.Value.transform.position, Quaternion.identity, map);
                go.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("14");
                go.GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
            if (composition[1] == 'W' && composition[2] == 'E' && composition[4] == 'W')
            {
                GameObject go = Instantiate(tile.Value, tile.Value.transform.position, Quaternion.identity, map);
                go.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("15");
                go.GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
            if (composition[4] == 'W' && composition[6] == 'W' && composition[7] == 'E')
            {
                GameObject go = Instantiate(tile.Value, tile.Value.transform.position, Quaternion.identity, map);
                go.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("16");
                go.GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
            if (composition[0] == 'E' && composition[1] == 'W' && composition[3] == 'W')
            {
                GameObject go = Instantiate(tile.Value, tile.Value.transform.position, Quaternion.identity, map);
                go.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("17");
                go.GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
            if (composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'W')       //Random chance for water ripple
            {
                int randomTile = UnityEngine.Random.Range(0, 100);
                if (randomTile < 15)
                {
                    tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("19");
                }
            }
            if (composition[1] == 'W' && composition[2] == 'W' && composition[3] == 'W' && composition[4] == 'W' && composition[5] == 'W' && composition[6] == 'W') //Random chance for lily pad
            {
                int randomTile = UnityEngine.Random.Range(0, 100);
                if (randomTile < 10)
                {
                    tile.Value.GetComponent<SpriteRenderer>().sprite = waterAtlas.GetSprite("20");
                }

            }

        }
    }

    public string TileCheck(Point currentPoint)
    {
        string composition = string.Empty;

        for (int x = -1; x <= 1; x++)       //For loops run through each position in a square around point, comparing neighbors
        {
            for(int y = -1; y <= 1; y++)
            {
                if(x != 0 || y != 0)        //Doesn't check itself; start pos will be water
                {
                    if (waterTiles.ContainsKey(new Point(currentPoint.MyX+x, currentPoint.MyY+y)))      //Checks if water tiles contains key to neighbors key, means neighbor has water
                    {
                        composition += "W";
                    }
                    else
                    {
                        composition += "E";
                    }



                }

            }
        }

                                //Debug.Log(composition);     //Debug which produces string needed to figure water type
        return composition;

    }

  
}

[Serializable]
public class MapElement             //Different map elements, grass, water, tree
{
    [SerializeField]
    private string tileTag;

    [SerializeField]                //Reads color, matches it with prefab and places in world
    private Color color;

    [SerializeField]
    private GameObject elementPrefab;

    public GameObject MyElementPrefab   //Property
    {
        get
        {
            return elementPrefab;
        }
    }

    public Color MyColor
    {
        get
        {
            return color;
        }
    }

    public string MyTileTag
    {
        get
        {
            return tileTag;
        }
    }

}

public struct Point                 //Scruct = lightweight class, not a reference. Must set values in struct
{
    public int MyX { get; set; }
    public int MyY { get; set; }

    public Point(int x, int y)      //Constructor which can set x,y coord values
    {
        this.MyX = x;
        this.MyY = y;
    }

}