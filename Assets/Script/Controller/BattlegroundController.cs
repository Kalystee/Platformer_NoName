using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class BattlegroundController : MonoBehaviour {

    public static BattlegroundController singleton { get; private set; }

    private Level level { get { return Level.singleton; } }
    private BattlegroundDisplayer bgDisplayer { get { return BattlegroundDisplayer.singleton; } }
    private BattlegroundUI bgUI { get { return BattlegroundUI.singleton; } }
    
    public AbilityBase abilitySelected;

    void Awake()
    {
        if (singleton != null)
        {
            Debug.LogError("Une instance de BattlegroundController existe déja !");
            Destroy(this);
            return;
        }

        singleton = this;
        level.RegisterOnDeployement(delegate (BattleCharacter bc) { if(bc == level.playingBattleCharacter)
            {
                NextPerso();
            }
        });
    }
    
    void Update ()
    {
        //DEBUG : Uniquement pour les phases de test
        DebugButtons();
        
        if (level.battleCharacterList.Count > 0 && level.playingBattleCharacter != null)
        {
            if (level.playingBattleCharacter.teamTag == TeamTag.Ally)
            {
                PlayerTurn();
            }
            else
            {
                AIBattleCharacter ai = level.playingBattleCharacter as AIBattleCharacter;
                AIBehaviour behaviour = ai.behaviour;
                if (behaviour.IsTurnEnded(ai))
                {
                    NextPerso();
                }
                else
                {
                    behaviour.PlayTurn(ai);
                }
            }
        }
    }

    private void PlayerTurn()
    {
        //On obtient la position de la souris dans le terrain
        Int2 mousePos = WorldUtils.GetWorldMousePosition();
        
        if(Input.GetKeyDown(KeyCode.P))
        {
            LootBag lb = Level.singleton.GetLootBagAt(mousePos);
            if(lb != null)
            {
                Find.WindowStack.Add(new Window_Inventory(lb.inventory));
            }
        }

        if (level.playingBattleCharacter != null && level.playingBattleCharacter.teamTag == TeamTag.Ally)
        {
            //Si une ability est sélectionnée
            if (abilitySelected != null)
            {
                //On nettoie toutes les soulignements
                bgDisplayer.ResetColors();
                TileBase mouseTile = level.arena.GetTile(mousePos);

                //On regarde toute les cases autour du BattleCharacter
                for (int i = this.level.playingBattleCharacter.pos.x - abilitySelected.Range; i <= this.level.playingBattleCharacter.pos.x + abilitySelected.Range; i++)
                {
                    for (int j = this.level.playingBattleCharacter.pos.y - abilitySelected.Range; j <= this.level.playingBattleCharacter.pos.y + abilitySelected.Range; j++)
                    {
                        Int2 selectedPos = new Int2(i, j);
                        TileBase selectedTile = level.GetTileAt(selectedPos);

                        //Si la case est valide et à porté on la colore donc
                        if (selectedTile != null && selectedTile.IsWalkable && Int2.Distance(selectedPos, level.playingBattleCharacter.pos) <= abilitySelected.Range)
                        {
                            if (WorldUtils.HasLineOfSightTo(level.playingBattleCharacter.pos, selectedPos))
                                bgDisplayer.ColorTile(selectedPos, new Color32(25, 12, 251, 128));
                            else
                                bgDisplayer.ColorTile(selectedPos, new Color32(128, 0, 0, 128));
                        }
                    }
                }

                //On regarde si la selection est à porté
                if (mouseTile != null && mouseTile.IsWalkable && Int2.Distance(level.playingBattleCharacter.pos, mousePos) <= abilitySelected.Range && level.arena.IsInsideArena(mousePos) && WorldUtils.HasLineOfSightTo(level.playingBattleCharacter.pos, mousePos))
                {
                    //On regarde toute les cases de la zone d'effet
                    for (int x = 0; x < abilitySelected.Area.Width; x++)
                    {
                        for (int y = 0; y < abilitySelected.Area.Height; y++)
                        {
                            if (abilitySelected.Area.effectZone[x, y])
                            {
                                Int2 selectedPos = new Int2(x, y) + mousePos - abilitySelected.Area.Center;
                                TileBase selectedTile = level.GetTileAt(selectedPos);

                                //Si la case est valide et à porté on la colore donc
                                if (selectedTile != null && selectedTile.IsWalkable)
                                {
                                    bgDisplayer.ColorTile(selectedPos, new Color32(0, 255, 255, 128));
                                }
                            }
                        }
                    }
                    
                    if (Input.GetMouseButtonDown(0))
                    {
                        //On enleve alors les PA du joueurs
                        level.playingBattleCharacter.UseAbility(abilitySelected, mousePos);
                        //On retire l'abilité séléctionnée
                        abilitySelected = null;
                        //On retire les marqueurs de couleurs
                        bgDisplayer.ResetColors();
                    }
                    
                }
                if (Input.GetMouseButtonDown(1))
                {
                    //On retire les marqueurs de couleurs
                    bgDisplayer.ResetColors();

                    //Si on clique droit on annule la compétence séléctionné
                    abilitySelected = null;
                }
            }
            else
            {
                //On retire les marqueurs de couleurs
                bgDisplayer.ResetColors();

                //On vérifie que l'emplacement est libre pour du déplacement et que un personnage joue
                if (level.arena.IsInsideArena(mousePos) && level.playingBattleCharacter != null)
                {
                    //On cherche le chemin de la position du personnage à la souris
                    Path path = level.pathFinder.FindPath(level.playingBattleCharacter.pos, mousePos);
                    //Si le chemin n'est pas null et que le personnage a les capacité a se déplacer
                    if (path != null && path.NodePath.Count > 0)
                    {
                        //On souligne le chemin pour la prévisualisation
                        int moveCost = 0;
                        List<Node> nodePath = level.pathFinder.FindPath(level.playingBattleCharacter.pos, mousePos).NodePath;
                        for (int i = 0; i < nodePath.Count; i++)
                        {
                            Node n = nodePath[i];
                            moveCost += n.movementPenalty;

                            //On affiche une couleur verte si c'est à porter de mouvement, rouge sinon
                            if (moveCost <= level.playingBattleCharacter.useablePA)
                            {
                                bgDisplayer.ColorTile(n.NodePosition, new Color32(25, 150, 15, 150));
                            }
                            else
                            {
                                bgDisplayer.ColorTile(n.NodePosition, new Color32(150, 25, 15, 150));
                            }
                        }

                        //Si le joueur clique...
                        if (Input.GetMouseButtonDown(0))
                        {
                            //...on récupère le plus long point qu'il peut atteindre...
                            Node node = path.GetNodeWithCostAt(level.playingBattleCharacter.useablePA);

                            if (node != null)
                            {
                                //...et on le déplace !
                                level.playingBattleCharacter.ModifyPA(-path.GetCostTo(node));
                                level.playingBattleCharacter.MoveToPosition(node.NodePosition);
                            }
                        }
                    }
                }
            }
        }
    }
   
    /// <summary>
    /// Méthode permettant les tests
    /// </summary>
    private void DebugButtons()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            int init = Random.Range(0, 10000);
            level.DeployPlayerCharacter(WorldUtils.GetWorldMousePosition(), new Character(Random.Range(0, 100000).ToString(), 25, 5, 5, 5, init, 5));
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            int init = Random.Range(0, 10000);
            level.DeployAICharacter(WorldUtils.GetWorldMousePosition(), new Character(Random.Range(0, 100000).ToString(), 25, 5, 5, 5, init, 5), TeamTag.Enemy, AIBehaviourDefOf.test);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            int init = Random.Range(0, 10000);
            level.DeployAICharacter(WorldUtils.GetWorldMousePosition(), new Character(Random.Range(0, 100000).ToString(), 25, 5, 5, 5, init, 5), TeamTag.Neutral, AIBehaviourDefOf.test);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            level.LootOnGround(WorldUtils.GetWorldMousePosition(), new ItemStack(ItemDefOf.firstAidKit, Random.Range(1,6)));
        }
    }
  
    /// <summary>
    /// Methode permettant de finir son tour et de passer au personnage suivant
    /// </summary>
    public void NextPerso()
    {
        if(level.playingBattleCharacter != null)
            level.playingBattleCharacter.UpdateBuffs();
        level.GetPlayingCharacter();
    }

    public void SelectAbility(AbilityBase ability)
    {
        //On assigne la compétence
        this.abilitySelected = ability;
    }
}
