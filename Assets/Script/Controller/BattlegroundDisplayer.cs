using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattlegroundDisplayer : MonoBehaviour
{
    public static BattlegroundDisplayer singleton { get; private set; }

    private Level level { get { return Level.singleton; } }
    private BattlegroundController bgController { get { return BattlegroundController.singleton; } }
    private BattlegroundUI bgUI { get { return BattlegroundUI.singleton; } }

    Dictionary<Int2, SpriteRenderer> arenaSpriteDictionnary;
    Dictionary<Int2, SpriteRenderer> interactableDictionnary;
    Dictionary<Int2, SpriteRenderer> lootbagDictionnary;
    Dictionary<BattleCharacter, SpriteRenderer> characterDictionnary;
    SpriteRenderer[,] highlightTiles;
    List<SpriteRenderer> highlightedTiles;
    
    public Sprite characterTestSprite;
    public Sprite lootSprite;

    GameObject lootbagParent;
    GameObject highlightParent;
    GameObject tileParent;
    GameObject interactableParent;
    GameObject characterParent;

    public void Awake()
    {
        if(singleton != null)
        {
            Debug.LogError("Une instance de BattlegroundDisplayer existe déja !");
            Destroy(this);
            return;
        }

        singleton = this;

        highlightedTiles = new List<SpriteRenderer>();

        //On initialise les dictionnaires
        arenaSpriteDictionnary = new Dictionary<Int2, SpriteRenderer>();
        interactableDictionnary = new Dictionary<Int2, SpriteRenderer>();
        lootbagDictionnary = new Dictionary<Int2, SpriteRenderer>();
        characterDictionnary = new Dictionary<BattleCharacter, SpriteRenderer>();
        
        //On enregistre les Callbacks
        level.RegisterOnDeployement(OnDeployement);
        level.RegisterOnGettingPlayingCharacter(OnGettingPlayingCharacter);
        level.RegisterOnGettingPlayingCharacter(bc => ResetColors());
        level.RegisterOnPopingLootbag(OnPopingLootbag);

        //On intialise le tableau des marqueurs
        highlightTiles = new SpriteRenderer[level.arena.Width, level.arena.Height];

        //On crée le parent des lootbags
        lootbagParent = new GameObject("Lootbags");
        lootbagParent.transform.parent = transform;

        //On crée tout les markeurs
        highlightParent = new GameObject("Highlight");
        highlightParent.transform.parent = transform;
        for (int x = 0; x < level.arena.Width; x++)
        {
            for (int y = 0; y < level.arena.Height; y++)
            {
                SpriteRenderer sr = new GameObject($"Highlighter Tile [{x},{y}]", typeof(SpriteRenderer)).GetComponent<SpriteRenderer>();
                sr.transform.parent = highlightParent.transform;
                sr.transform.rotation = Quaternion.Euler(90, 0, 0);
                sr.transform.position = new Vector3(x, 0, y);
                sr.sprite = SpriteManager.GetSpriteOfTileNamed("blank");
                sr.sortingLayerName = "arena_ground";
                sr.sortingOrder = 5;
                sr.gameObject.layer = LayerMask.NameToLayer("Tile");
                sr.color = new Color32(0, 0, 0, 0); 
                highlightTiles[x, y] = sr;
            }
        }

        //On crée toutes les Tiles
        tileParent = new GameObject("Tiles");
        tileParent.transform.parent = transform;
        for (int x = 0; x < level.arena.Width; x++)
        {
            for (int y = 0; y < level.arena.Height; y++)
            {
                SpriteRenderer sr = new GameObject($"Arena Tile [{x},{y}]", typeof(SpriteRenderer)).GetComponent<SpriteRenderer>();
                TileBase tile = level.arena.GetTile(x, y);
                if (tile != null)
                    sr.sprite = tile.Sprite;
                sr.transform.rotation = Quaternion.Euler(90, 0, 0);
                sr.transform.position = new Vector3(x, 0, y);
                sr.transform.parent = tileParent.transform;
                sr.sortingLayerName = "arena_ground";
                sr.gameObject.layer = LayerMask.NameToLayer("Tile");
                arenaSpriteDictionnary.Add(new Int2(x, y), sr);
            }
        }

        //On crée toutes les Interactables
        interactableParent = new GameObject("Interactables");
        interactableParent.transform.parent = transform;
        foreach (Int2 pos in level.arena.interactableList.Keys)
        {
            SpriteRenderer sr = new GameObject($"Interactable [{pos}]", typeof(SpriteRenderer), typeof(AlwaysFaceCamera)).GetComponent<SpriteRenderer>();
            InteractableBase ib = level.arena.interactableList[pos];
            if (ib != null)
                sr.sprite = ib.Sprite;
            sr.transform.position = new Vector3(pos.x, 0, pos.y);
            sr.transform.rotation = Quaternion.Euler(30, 45, 0);
            sr.transform.localScale *= 0.75f;
            sr.transform.parent = interactableParent.transform;
            sr.gameObject.layer = LayerMask.NameToLayer("Interactable");
            sr.sortingLayerName = "arena_interactable";
            interactableDictionnary.Add(pos, sr);
        }
        
        characterParent = new GameObject("Characters");
        characterParent.transform.parent = transform;
    }

    /// <summary>
    /// Nettoie les cases coloré
    /// </summary>
    public void ResetColors()
    {
        foreach(SpriteRenderer sr in highlightTiles)
        {
            sr.color = new Color32(0, 0, 0, 0);
            highlightedTiles.Remove(sr);
        }
    }

    /// <summary>
    /// Colore une case d'une certaine couleur
    /// </summary>
    /// <param name="x">Position en largeur</param>
    /// <param name="y">Position en hauteur</param>
    /// <param name="color">Couleur du highlight</param>
    public void ColorTile(int x, int y, Color color)
    {
        if (level.arena.IsInsideArena(x, y))
        {
            if (highlightedTiles.Contains(highlightTiles[x, y]))
            {
                highlightTiles[x, y].color = color;
                highlightedTiles.Add(highlightTiles[x, y]);
            }
            else
            {
                highlightTiles[x, y].color = color;
            }
        }
    }
    
    /// <summary>
    /// Colore une case d'une certaine couleur
    /// </summary>
    /// <param name="pos">Position</param>
    /// <param name="color">Couleur du highlight</param>
    public void ColorTile(Int2 pos, Color color)
    {
        ColorTile(pos.x, pos.y, color);
    }

    public void OnPopingLootbag(LootBag lb)
    {
        if (!lootbagDictionnary.ContainsKey(lb.pos))
        {
            //On crée le lootbag
            SpriteRenderer sr = new GameObject($"Lootbag [{lb.pos}]", typeof(SpriteRenderer), typeof(AlwaysFaceCamera)).GetComponent<SpriteRenderer>();
            sr.transform.position = new Vector3(lb.pos.x, 0, lb.pos.y);
            sr.transform.localScale *= 0.75f;
            sr.transform.rotation = Quaternion.Euler(30, 45, 0);
            sr.sprite = lootSprite;
            sr.transform.parent = lootbagParent.transform;
            sr.sortingLayerName = "arena_lootbag";
            sr.gameObject.layer = LayerMask.NameToLayer("Lootbag");
            lootbagDictionnary.Add(lb.pos, sr);
        }
        else
        {
            Debug.LogError("La position à déja été utilisée par quelque chose");
        }
    }

    public void OnDeployement(BattleCharacter bc)
    {
        if (!characterDictionnary.ContainsKey(bc))
        {
            //On crée le personnage
            SpriteRenderer sr = new GameObject($"Character [{bc.pos}]", typeof(SpriteRenderer), typeof(AlwaysFaceCamera), typeof(cakeslice.Outline)).GetComponent<SpriteRenderer>();
            sr.GetComponent<cakeslice.Outline>().enabled = false;
            sr.transform.position = new Vector3(bc.pos.x, 0, bc.pos.y);
            sr.transform.localScale *= 0.75f;
            sr.transform.rotation = Quaternion.Euler(30, 45, 0);
            sr.color = bc.teamTag.color;
            sr.sprite = characterTestSprite;
            sr.transform.parent = characterParent.transform;
            sr.sortingLayerName = "arena_character";
            sr.gameObject.layer = LayerMask.NameToLayer("Character");
            characterDictionnary.Add(bc, sr);
            bc.RegisterOnMoveAction(delegate(Int2 pos)
            {
                sr.transform.position = new Vector3(pos.x, 0, pos.y);
            });
            bc.RegisterOnDeathAction(delegate ()
            {
                Destroy(sr.gameObject);
                characterDictionnary.Remove(bc);
            });

            foreach (BattleCharacter battleCharacter in characterDictionnary.Keys)
            {
                characterDictionnary[battleCharacter].GetComponent<cakeslice.Outline>().enabled = false;
                characterDictionnary[battleCharacter].transform.localScale = Vector3.one * 0.75f;
            }

            BattleCharacter first = level.battleCharacterList.First();
            OnGettingPlayingCharacter(first);
        }
        else
        {
            Debug.LogError("La position à déja été utilisée par quelque chose");
        }
    }

    public void OnGettingPlayingCharacter(BattleCharacter bc)
    {
        BattleCharacter battleCharacter = level.battleCharacterList.Last();
        
        characterDictionnary[battleCharacter].GetComponent<cakeslice.Outline>().enabled = false;
        characterDictionnary[battleCharacter].transform.localScale = Vector3.one * 0.75f;

        characterDictionnary[bc].GetComponent<cakeslice.Outline>().enabled = true;
        characterDictionnary[bc].transform.localScale = Vector3.one;
    }
}
