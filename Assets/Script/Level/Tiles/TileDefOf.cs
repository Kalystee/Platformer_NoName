using System.Collections.Generic;
using UnityEngine;

public static class TileDefOf
{
    //Liste de tout les TileBase
    public static Dictionary<string, TileBase> tileDefList = new Dictionary<string, TileBase>();

    public static TileBase GetTileBaseNamed(string name)
    {
        if(tileDefList.ContainsKey(name))
        {
            return tileDefList[name];
        }
        else
        {
            Debug.LogWarning($"Le Tile '{name}' n'existe pas !");
            return null;
        }
    }

    //Déclaration de tout les Tiles
    public static TileBase normalTile = new TileBase("normal", "normal");
    public static TileBase obstacleTile = new TileBase("obstacle", "obstacle", false, true);
    public static TileBase unwalkableTile = new TileBase("unwalkable", "unwalkable", false, false);
    public static TileSlow slowTile = new TileSlow("slow", "slow", 1, true);
}
