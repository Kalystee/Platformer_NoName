using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[DataContract]
public class Arena
{
    //Dictionnaire des Interactable selon leur position
    [DataMember]
    public Dictionary<Int2, InteractableBase> interactableList { get; protected set; }
    //Tableau 2D de toutes les Tiles
    [DataMember]
    protected TileBase[,] arenaTiles;

    [DataMember]
    public int Width { get { return arenaTiles.GetLength(0); } }
    [DataMember]
    public int Height { get { return arenaTiles.GetLength(1); } }
    
    protected Arena() { }

    public Arena(int width, int height)
    {
        arenaTiles = new TileBase[width, height];
        interactableList = new Dictionary<Int2, InteractableBase>();
    }

    public Arena(TileBase[,] tiles, Dictionary<Int2, InteractableBase> interactable)
    {
        arenaTiles = tiles;
        interactableList = interactable;
    }

    /// <summary>
    /// Récupère le Tile en x,y
    /// </summary>
    /// <param name="x">Position en largeur</param>
    /// <param name="y">Position en hauteur</param>
    /// <returns>Retourne le Tile en x,y sinon null</returns>
    public TileBase GetTile(int x, int y)
    {
        if(IsInsideArena(x,y))
        {
            return arenaTiles[x, y];
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Récupère le Tile en x,y
    /// </summary>
    /// <param name="pos">Position</param>
    /// <returns>Retourne le Tile en x,y sinon null</returns>
    public TileBase GetTile(Int2 pos)
    {
        return GetTile(pos.x, pos.y);
    }

    /// <summary>
    /// Retourne le coup en PA pour passer sur le Tile en x,y
    /// </summary>
    /// <param name="x">Position en largeur</param>
    /// <param name="y">Position en hauteur</param>
    /// <returns>Coup en PA</returns>
    public int GetTileMovementCost(int x, int y)
    {
        int cost = 1;
        TileBase tile = GetTile(x, y);
        if(tile is TileSlow)
        {
            cost += ((TileSlow)tile).penaltyPA;
        }
        return cost;
    }

    /// <summary>
    /// Retourne le coup en PA pour passer sur le Tile en x,y
    /// </summary>
    /// <param name="pos">Position</param>
    /// <returns>Coup en PA</returns>
    public int GetTileMovementCost(Int2 pos)
    {
        return GetTileMovementCost(pos.x, pos.y);
    }

    /// <summary>
    /// Remplace une tile de l'arène en x,y
    /// </summary>
    /// <param name="x">Position en largeur</param>
    /// <param name="y">Position en hauteur</param>
    /// <param name="tile">Tile de remplacement</param>
    /// <returns>Retourne si la Tile a bien été placé</returns>
    public bool SetTile(int x, int y, TileBase tile)
    {
        //On regarde si la case que l'on essaie d'assigner est dans la zone de l'arène
        if(IsInsideArena(x, y))
        {
            //On change le sol
            arenaTiles[x, y] = tile;
            return true;
        }
        else
        {
            //On déclenche une erreur sinon !
            Debug.LogError($"Les coordonnés ({x}, {y}) sont hors de l'arène !");
            return false;
        }
    }
    
    /// <summary>
     /// Remplace une tile de l'arène en x,y
     /// </summary>
     /// <param name="pos">Position</param>
     /// <param name="tile">Tile de remplacement</param>
     /// <returns>Retourne si la Tile a bien été placé</returns>
    public bool SetTile(Int2 pos, TileBase tile)
    {
        return SetTile(pos.x, pos.y, tile);
    }

    /// <summary>
    /// Place un interactable en x,y
    /// </summary>
    /// <param name="x">Position en largeur</param>
    /// <param name="y">Position en hauteur</param>
    /// <param name="interactable">Interactable à placer</param>
    /// <returns>Retourne si l'Interactable a bien été placé</returns>
    public bool SetInteractable(int x, int y, InteractableBase interactable)
    {
        //On regarde si la case que l'on essaie d'assigner est disponible
        if (IsTileOpen(x,y))
        {
            //On ajoute un Interactable
            interactableList.Add(new Int2(x, y), interactable);
            return true;
        }
        else
        {
            //On déclenche une erreur sinon !
            Debug.LogError($"Les coordonnés ({x}, {y}) sont indisponible pour un Interactable !");
            return false;
        }
    }
    
    /// <summary>
     /// Place un interactable en x,y
     /// </summary>
     /// <param name="pos">Position</param>
     /// <param name="interactable">Interactable à placer</param>
     /// <returns>Retourne si l'Interactable a bien été placé</returns>
    public bool SetInteractable(Int2 pos, InteractableBase interactable)
    {
        return SetInteractable(pos.x, pos.y, interactable);
    }

    /// <summary>
    /// Permet de savoir si une coordonnée x,y est dans l'arène
    /// </summary>
    /// <param name="x">Position en largeur</param>
    /// <param name="y">Position en hauteur</param>
    /// <returns>Retourne si la coordonnée est dans l'arène</returns>
    public bool IsInsideArena(int x, int y)
    {
        //On regarde si x et y sont compris dans le tableau.
        return x >= 0 && y >= 0 && x < Width && y < Height;
    }

    /// <summary>
    /// Permet de savoir si une coordonnée x,y est dans l'arène
    /// </summary>
    /// <param name="pos">Position</param>
    /// <returns>Retourne si la coordonnée est dans l'arène</returns>
    public bool IsInsideArena(Int2 pos)
    {
        return IsInsideArena(pos.x, pos.y);
    }

    /// <summary>
    /// Permet de savoir si cet emplacement est disponible pour un Interactable
    /// </summary>
    /// <param name="x">Position en largeur</param>
    /// <param name="y">Position en hauteur</param>
    /// <returns>Retourne si la coordonnée est déja occupé</returns>
    public bool IsTileOpen(int x, int y)
    {
        //On regarde si la case est dans l'arène
        if (IsInsideArena(x, y))
        {
            //On regarde si aucun Interactable n'y se trouve déja
            return !interactableList.ContainsKey(new Int2(x, y));
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Permet de savoir si cet emplacement est disponible pour un Interactable
    /// </summary>
    /// <param name="pos">Position</param>
    /// <returns>Retourne si la coordonnée est déja occupé</returns>
    public bool IsTileOpen(Int2 pos)
    {
        return IsTileOpen(pos.x, pos.y);
    }

    /// <summary>
    /// Verifie si la case est praticable
    /// </summary>
    /// <param name="x">Position en largeur</param>
    /// <param name="y">Position en hauteur</param>
    /// <returns>Retourne si le terrain est praticable</returns>
    public bool IsTileWalkable(int x, int y)
    {
        if(interactableList.ContainsKey(new Int2(x,y)))
        {
            return false;
        }
        else
        {
            TileBase tile = GetTile(x, y);
            if (tile != null)
                return GetTile(x, y).IsWalkable;
            else
                return false;
        }
    }

    /// <summary>
    /// Verifie si la case est praticable
    /// </summary>
    /// <param name="pos">Position</param>
    /// <returns>Retourne si le terrain est praticable</returns>
    public bool IsTileWalkable(Int2 pos)
    {
        return IsTileWalkable(pos.x, pos.y);
    }

    /// <summary>
    /// Retourne la liste des toutes les positions affecté par une zone depuis un point
    /// </summary>
    /// <param name="x">Position en largeur</param>
    /// <param name="y">Position en hauteur</param>
    /// <param name="effectZone">Type de zone d'effet</param>
    /// <returns>Liste des possition affectés</returns>
    public List<Int2> GetAffectedPositions(int x, int y, EffectZone effectZone)
    {
        List<Int2> listOfPositions = new List<Int2>();
        for (int i = 0; i < effectZone.Width; i++)
        {
            for (int j = 0; j < effectZone.Height; j++)
            {
                int posX = i + x - effectZone.Center.x;
                int posY = j + y - effectZone.Center.y;
                if (IsInsideArena(posX, posY) && effectZone.effectZone[i,j])
                {
                    listOfPositions.Add(new Int2(posX, posY));
                }
            }
        }
        return listOfPositions;
    }
    
    /// <summary>
     /// Retourne la liste des toutes les positions affecté par une zone depuis un point
     /// </summary>
     /// <param name="pos">Position</param>
     /// <param name="effectZone">Type de zone d'effet</param>
     /// <returns>Liste des possition affectés</returns>
    public List<Int2> GetAffectedPositions(Int2 pos, EffectZone effectZone)
    {
        return GetAffectedPositions(pos.x, pos.y, effectZone);
    }
}
