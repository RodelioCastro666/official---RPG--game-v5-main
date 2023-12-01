using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NewGrassTile : Tile
{
    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {

        return base.StartUp(position, tilemap, go);
    }

#if UNITY_EDITOR
    [MenuItem("Assets/Create/Tiles/NewGrassTile")]

    public static void CreateGrassTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save NewGrassTile", "New NewGrassTile", "asset", "Save NewGrassTile", "Assets");
        if (path == "")
        {
            return;
        }
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<NewGrassTile>(), path);

    }
#endif
}
