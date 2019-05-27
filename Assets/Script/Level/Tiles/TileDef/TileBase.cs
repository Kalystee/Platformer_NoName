using UnityEngine;

public class TileBase
{
    public string Name { get; private set; }

    //Propriété : Permet la marche
    public bool IsWalkable { get; protected set; }
    //Propriété : Permet bloquer la vision
    public bool IsBlockingSight { get; protected set; }
    //Propriété : Sprite
    private string tileSpriteName;
    public Sprite Sprite { get { return SpriteManager.GetSpriteOfTileNamed(tileSpriteName); } }

    public TileBase(string name, string spriteName, bool walkable = true, bool blockingSight = false)
    {
        //On ajoute à la liste de tout les Tiles pour les sauvegarde/chargements plus tard.
        TileDefOf.tileDefList.Add(name, this);

        //On enregistre tout les attributs d'un Tile
        Name = name;
        tileSpriteName = spriteName;
        IsWalkable = walkable;
        IsBlockingSight = blockingSight;
    }
}
