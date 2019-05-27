using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Level
{
    public static Level singleton;

    #region Attributes
    private Action<BattleCharacter> onDeployement;
    private Action<BattleCharacter> onGettingPlayingCharacter;
    private Action<LootBag> onPopingLootbag;

    //Dictionnaire des personnage par leur coordonnée
    public List<BattleCharacter> battleCharacterList { get; private set; }
    public List<LootBag> lootbags { get; private set; }

    //Référence à l'arène utilisé
    public Arena arena { get; private set; }
    public BattleCharacter playingBattleCharacter { get; private set; }
    public PathFinder pathFinder { get; private set; }
    #endregion

    #region Constructors
    private Level(Arena playground)
    {
        singleton = this;

        this.arena = playground;
        this.battleCharacterList = new List<BattleCharacter>();
        this.lootbags = new List<LootBag>();

        pathFinder = new PathFinder(this);
    }

    private Level(string JSON)
    {
        singleton = this;

        this.arena = JsonConvert.DeserializeObject<Arena>(JSON);
        this.battleCharacterList = new List<BattleCharacter>();
        this.lootbags = new List<LootBag>();

        pathFinder = new PathFinder(this);
    }
    #endregion

    #region Instantiante
    /// <summary>
    /// Génère un level a partir d'un JSON
    /// </summary>
    /// <param name="JSON">Texte JSON</param>
    public static Level InstantiateLevel(string JSON)
    {
        singleton = new Level(JSON);
        Level.InitiateParty();
        return singleton;
    }

    /// <summary>
    /// Génère un level a partir d'une arène
    /// </summary>
    /// <param name="playground">Arène</param>
    public static Level InstantiateLevel(Arena playground)
    {
        singleton = new Level(playground);
        return singleton;
    }

    /// <summary>
    /// Méthode permettant d'initialiser le groupe (les 4 persos sont ajouter a Party)
    /// </summary>
    public static  void InitiateParty()
    {
        Find.Party.Money += 100;
        Character jack = new Character("Jack", 30, 10, 15, 15, 17, 5, "Tomb");
        Find.Party.CommualInventory.AddItems(new ItemStack(ItemDefOf.firstAidKit));
        
        jack.gears.Weapon = ItemDefOf.pistol;
        jack.gears.Helmet = ItemDefOf.helmet;
        jack.gears.Chestplate = ItemDefOf.chest;
        jack.gears.Boots = ItemDefOf.boot;
        jack.gears.Gloves = ItemDefOf.gloves;

        jack.gears.AddConsumable(ItemDefOf.conso1);
        jack.gears.AddConsumable(ItemDefOf.conso2);
        jack.gears.AddConsumable(ItemDefOf.conso3);
        
        jack.gears.AddGadjet(ItemDefOf.gadjet1);
        jack.gears.AddGadjet(ItemDefOf.gadjet2);
        jack.gears.AddGadjet(ItemDefOf.gadjet3);

        Find.Party.AddCharacterToParty(jack);
        /*
       Find.Party.AddCharacterToParty(new Character("Anna", 30, 10, 15, 15, 17, 5, "Bag"));
        Find.Party.AddCharacterToParty(new Character("Nathan", 30, 10, 15, 15, 17, 5, "Tomb"));
        Find.Party.AddCharacterToParty(new Character("Lily", 30, 10, 15, 15, 17, 5, "Bag"));*/
    }

   
    #endregion

    #region Callbacks
    public void RegisterOnDeployement(Action<BattleCharacter> action)
    {
        this.onDeployement += action;
    }

    public void RegisterOnGettingPlayingCharacter(Action<BattleCharacter> action)
    {
        this.onGettingPlayingCharacter += action;
    }

    public void RegisterOnPopingLootbag(Action<LootBag> action)
    {
        this.onPopingLootbag += action;
    }
    #endregion

    #region BattleCharacter
    /// <summary>
    /// Retourne la liste des personnage appartenant 
    /// </summary>
    /// <param name="team"></param>
    /// <returns></returns>
    public BattleCharacter[] GetBattleCharactersOfTeam(TeamTag team)
    {
        return battleCharacterList.Where(bc => bc.teamTag == team).ToArray();
    }
    #endregion

    #region PathFinding
    /// <summary>
    /// Modifie la grille en conséquence du terrain sur la position
    /// </summary>
    /// <param name="x">Position en largeur</param>
    /// <param name="y">Position en hauteur</param>
    public void ModifyPath(int x, int y)
    {
        pathFinder.ModifyGrid(new Int2(x, y), GetTileNode(x, y));
    }

    /// <summary>
    /// Modifie la grille en conséquence du terrain sur la position du BattleCharacter
    /// </summary>
    /// <param name="pos">Position</param>
    public void ModifyPath(Int2 pos)
    {
        pathFinder.ModifyGrid(pos, GetTileNode(pos));
    }

    /// <summary>
    /// Modifie la grille en conséquence du terrain sur la position du BattleCharacter
    /// </summary>
    /// <param name="bc">BattleCharacter séléctionné</param>
    public void ModifyPath(BattleCharacter bc)
    {
        pathFinder.ModifyGrid(bc.pos, GetTileNode(bc.pos));
    }

    /// <summary>
    /// Permet de générer une node selon le niveau
    /// </summary>
    /// <param name="x">Position en largeur</param>
    /// <param name="y">Position en hauteur</param>
    /// <returns>Nouvelle Node correspondante au niveau</returns>
    public Node GetTileNode(int x, int y)
    {
        return new Node(IsTileWalkable(x, y), new Int2(x, y), arena.GetTileMovementCost(x, y));
    }

    /// <summary>
    /// Permet de générer une node selon le niveau
    /// </summary>
    /// <param name="pos">Position</param>
    /// <returns>Nouvelle Node correspondante au niveau</returns>
    public Node GetTileNode(Int2 pos)
    {
        return GetTileNode(pos.x, pos.y);
    }
    #endregion

    #region SpawnThings
    /// <summary>
    /// Faire apparaitre un lootbag au sol, ou remplis un déja existant
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="items">[</param>
    public void LootOnGround(Int2 pos, params ItemStack[] items)
    {
        if (arena.IsInsideArena(pos))
        {
            LootBag lootbag = lootbags.Find(lb => lb.pos == pos);
            if (lootbag != null)
            {
                lootbag.inventory.AddItems(items);
            }
            else
            {
                LootBag bag = new LootBag(pos, items);
                lootbags.Add(bag);
                onPopingLootbag?.Invoke(bag);
            }
        }
    }

    /// <summary>
    /// Permet de déployer un personnage sur un Tile
    /// </summary>
    /// <param name="x">Position en largeur</param>
    /// <param name="y">Position en hauteur</param>
    /// <param name="character">Personnage à déployer</param>
    public bool DeployAICharacter(int x, int y, Character character, TeamTag tag, AIBehaviour behav, bool rearrange = true)
    {
        if (IsTileWalkable(x, y))
        {
            AIBattleCharacter ai = new AIBattleCharacter(character, tag, x, y, behav);
            if (!battleCharacterList.Contains(ai))
            {
                battleCharacterList.Add(ai);
                if (rearrange)
                {
                    battleCharacterList = battleCharacterList.OrderByDescending(b => b.Initiative).ToList();
                    playingBattleCharacter = battleCharacterList.First();
                }
                ai.RegisterOnDeathAction(delegate ()
                {
                    battleCharacterList.Remove(ai);
                    ModifyPath(ai);
                });
                ai.RegisterOnMoveAction(delegate (Int2 pos)
                {
                    ModifyPath(pos);
                });
                ModifyPath(ai);
                onDeployement?.Invoke(ai);
                return true;
            }
            else
            {
                Debug.LogError("Tentative de déployer un personnage déja existant !");
            }
        }
        return false;
    }

    /// <summary>
    /// Permet de déployer un personnage sur un Tile
    /// </summary>
    /// <param name="pos">Position</param>
    /// <param name="character">Personnage à déployer</param>
    public bool DeployAICharacter(Int2 pos, Character character, TeamTag tag, AIBehaviour behav, bool rearrange = true)
    {
        return DeployAICharacter(pos.x, pos.y, character, tag, behav, rearrange);
    }

    /// <summary>
    /// Permet de déployer un personnage sur un Tile
    /// </summary>
    /// <param name="x">Position en largeur</param>
    /// <param name="y">Position en hauteur</param>
    /// <param name="character">Personnage à déployer</param>
    public bool DeployPlayerCharacter(int x, int y, Character character, bool rearrange = true)
    {
        if (IsTileWalkable(x, y))
        {
            BattleCharacter bc = new BattleCharacter(character, TeamTag.Ally, x, y);
            if (!battleCharacterList.Contains(bc))
            {
                battleCharacterList.Add(bc);
                if (rearrange)
                {
                    battleCharacterList = battleCharacterList.OrderByDescending(b => b.Initiative).ToList();
                    playingBattleCharacter = battleCharacterList.First();
                }
                bc.RegisterOnDeathAction(delegate ()
                {
                    battleCharacterList.Remove(bc);
                    ModifyPath(bc);
                });
                bc.RegisterOnMoveAction(delegate (Int2 pos)
                {
                    ModifyPath(pos);
                });
                ModifyPath(bc);
                onDeployement?.Invoke(bc);
                return true;
            }
            else
            {
                Debug.LogError("Tentative de déployer un personnage déja existant !");
            }
        }
        return false;
    }

    /// <summary>
    /// Permet de déployer un personnage sur un Tile
    /// </summary>
    /// <param name="pos">Position</param>
    /// <param name="character">Personnage à déployer</param>
    public bool DeployPlayerCharacter(Int2 pos, Character character, bool rearrange = true)
    {
        return DeployPlayerCharacter(pos.x, pos.y, character, rearrange);
    }
    #endregion

    #region Gameplay
    /// <summary>
    /// Retourne le prochain joueur à jouer
    /// </summary>
    /// <returns>Prochain joueur à jouer</returns>
    public BattleCharacter GetPlayingCharacter()
    {
        if (battleCharacterList.Count > 0)
        {
            battleCharacterList.RemoveAt(0);
            battleCharacterList.Add(playingBattleCharacter);
            playingBattleCharacter = battleCharacterList.First();
            playingBattleCharacter.ResetPA();
            onGettingPlayingCharacter?.Invoke(playingBattleCharacter);
            return playingBattleCharacter;
        }
        else
        {
            Debug.LogWarning("Aucun BattleCharacter n'est sur le terrain ! Impossible d'obtenir un BattleCharacter ! Retourne null");
            return null;
        }
    }

    public LootBag GetLootBagAt(Int2 pos)
    {
        if(lootbags.Any(i => i.pos == pos))
        {
            return lootbags.First(i => i.pos == pos);
        }
        return null;
    }
    #endregion

    #region Utils
    /// <summary>
    /// Permet de savoir si un Character est en vie
    /// </summary>
    /// <param name="c">Character cible</param>
    /// <returns>True si le Character est toujours sur le terrain</returns>
    public bool IsACharacterAlive(Character c)
    {
        return battleCharacterList.Any(bc => bc.character == c);
    }

    /// <summary>
    /// Permet de savoir si une case est libre pour se déplacer
    /// </summary>
    /// <param name="x">Position en largeur</param>
    /// <param name="y">Position en hauter</param>
    /// <returns>LA case est libre pour les déplacements</returns>
    public bool IsTileWalkable(int x, int y)
    {
        if(battleCharacterList.Any(bc => bc.pos == new Int2(x,y)))
        {
            return false;
        }
        else
        {
            return arena.IsTileWalkable(x, y);
        }
    }

    /// <summary>
    /// Permet de savoir si une case est libre pour se déplacer
    /// </summary>
    /// <param name="pos">Position</param>
    /// <returns>LA case est libre pour les déplacements</returns>
    public bool IsTileWalkable(Int2 pos)
    {
        return IsTileWalkable(pos.x, pos.y);
    }


    /// <summary>
    /// Retourne le BattleCharacter en position x,y
    /// </summary>
    /// <param name="x">Position en largeur</param>
    /// <param name="y">Position en hauteur</param>
    /// <returns>Personnage placé en x,y ou null si aucun</returns>
    public BattleCharacter GetBattleCharacterAt(int x, int y)
    {
        if (battleCharacterList.Count(bc => bc.pos == new Int2(x, y)) > 0)
            return battleCharacterList.First(bc => bc.pos == new Int2(x, y));
        else
            return null;
    }

    /// <summary>
    /// Retourne le BattleCharacter sur la position
    /// </summary>
    /// <param name="pos">Position</param>
    /// <returns>Personnage placé en x,y ou null si aucun</returns>
    public BattleCharacter GetBattleCharacterAt(Int2 pos)
    {
        return GetBattleCharacterAt(pos.x, pos.y);
    }

    /// <summary>
    /// Retourne l'Interactable en position x,y
    /// </summary>
    /// <param name="x">Position en largeur</param>
    /// <param name="y">Position en hauteur</param>
    /// <returns>Interactable placé en x,y ou null si aucun</returns>
    public InteractableBase GetInteractableAt(int x, int y)
    {
        if (arena.interactableList.Count(i => i.Key == new Int2(x, y)) > 0)
            return arena.interactableList.First(i => i.Key == new Int2(x, y)).Value;
        else
            return null;
    }

    /// <summary>
    /// Retourne l'Interactable sur la position
    /// </summary>
    /// <param name="pos">Position</param>
    /// <returns>Interactable placé en x,y ou null si aucun</returns>
    public InteractableBase GetInteractableAt(Int2 pos)
    {
        return GetInteractableAt(pos.x, pos.y);
    }

    /// <summary>
    /// Retourne le Tile en position x,y
    /// </summary>
    /// <param name="x">Position en largeur</param>
    /// <param name="y">Position en hauteur</param>
    /// <returns>Tile en x,y ou null si hors du terrain</returns>
    public TileBase GetTileAt(int x, int y)
    {
        return arena.GetTile(x, y);
    }

    /// <summary>
    /// Retourne le Tile sur la position
    /// </summary>
    /// <param name="pos">Position</param>
    /// <returns>Tile en x,y ou null si hors du terrain</returns>
    public TileBase GetTileAt(Int2 pos)
    {
        return GetTileAt(pos.x, pos.y);
    }

    /// <summary>
    /// Affecte les entité touché par une abilité lancé en x,y
    /// </summary>
    /// <param name="x">Position en largeur</param>
    /// <param name="y">Position en hauteur</param>
    /// <param name="ability">Abilité utilisé</param>
    public void AffectInZone(int x, int y, AbilityBase ability)
    {
        List<Int2> affectedPositions = arena.GetAffectedPositions(x, y, ability.Area);
        
        BattleCharacter[] affectedBattleCharcter = battleCharacterList.Where(bc => affectedPositions.Contains(bc.pos)).ToArray();
        //InteractableBase[] affectedInteractable = arena.interactableList.Where(p => affectedPositions.Contains(p.Key)).ToDictionary(p => p.Key, p => p.Value).Values.ToArray();

        foreach(BattleCharacter bc in affectedBattleCharcter)
        {
            if(ability is AbilityDamage)
            {
                bc.ModifyHealth((ability as AbilityDamage).GetDamageApplied());
            }
            else if(ability is AbilityBuff)
            {
                bc.AddBuff((ability as AbilityBuff).Buff.Copy());
            }
            else
            {
                Debug.Log("L'ability n'est pas de type AbilityDamage, il faut donc gérer ce cas");
            }
        }
    }

    /// <summary>
    /// Affecte les entité touché par une abilité lancé en x,y
    /// </summary>
    /// <param name="pos">Position</param>
    /// <param name="ability">Abilité utilisé</param>
    public void AffectInZone(Int2 pos, AbilityBase ability)
    {
        AffectInZone(pos.x, pos.y, ability);
    }
    #endregion
}
