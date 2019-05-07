using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TreeTile : Tile        //Implements from unity tile script
{

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)          //Use startup function to change sort order of each instantiated object
    {
        go.GetComponent<SpriteRenderer>().sortingOrder = -position.y * 2;               //use startup function to change sort order of each instantiated object
                                                                                        //TODO FIX NULL EXECPTION ERROR from this code

        return base.StartUp(position, tilemap, go);
    }


#if UNITY_EDITOR
    [MenuItem("Assets/Create/Tiles/TreeTile")]
    public static void CreateTreeTile()             //Previously had this set to CreateWaterTile, upon changing it still has null execption error from above ^^
    {
        string path = EditorUtility.SaveFilePanelInProject("Save TreeTile", "New TreeTile", "asset", "Save treeTile", "Assets");
        if (path == "")
        {
            return;
        }
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<TreeTile>(), path);
    }

#endif

}
