using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AI_Test : AIBehaviour
{
    public override bool IsTurnEnded(AIBattleCharacter aiBCharacter)
    {
        //retourne si le characterIA n'as plus de PA ou si il est bloquer et qu'il n'est pas a coté d'un de ses ennemy (Ally)
        return aiBCharacter.useablePA == 0 || (Level.singleton.pathFinder.IsStuck(aiBCharacter.pos) && !IsNextToEnemy(aiBCharacter));
    }

    /// <summary>
    /// Méthode permettant de savoir comment l'IA va jouer son tour
    /// </summary>
    /// <param name="aiBCharacter"></param>
    public override void PlayTurn(AIBattleCharacter aiBCharacter)
    {
        //On initialise la liste des joueurs qu'il cible a tous les personnage "Ally"


        //On trie la liste afin de savoir quel personnage Ally est le plus proche de nous, ce sera la victime prioritaire
        List<BattleCharacter> players = Level.singleton.battleCharacterList.Where(bc => bc.teamTag == TeamTag.Ally).ToList().OrderBy(bc => Level.singleton.pathFinder.FindPath(aiBCharacter.pos, bc.pos).Cost).ToList();

        //On regarde le chemin vers le joueur a la position 0 correspondant à la victime prioritaire
        Path path = Level.singleton.pathFinder.FindPath(aiBCharacter.pos, players[0].pos);

        //Si le chemin existe
        if (path != null)
        {
            //On regarde la case qui se situe le plus proche de la victime
            Node posfinal = path.GetNodeWithCostAt(aiBCharacter.useablePA);
            //Si le Node existe
            if (posfinal != null)
            {
                //On se déplace sur cette case
                aiBCharacter.ModifyPA(-path.GetCostTo(posfinal));
                aiBCharacter.MoveToPosition(posfinal.NodePosition);
            }
        }
        //Sinon on génère une erreur
        else
        {
            Debug.Log("Erreur Path null");
        }

        //Pour chaque Ability du personnage
        foreach (AbilityBase abi in aiBCharacter.character.useableAbilities)
        {
            //Si une ability fait des damage et si on a assez de PA pour la faire et Si la cible est a portée
            if (abi is AbilityDamage)
            {
                if ((abi as AbilityDamage).IsDamage && abi.Cost <= aiBCharacter.useablePA && abi.Range >= Int2.Distance(aiBCharacter.pos, players[0].pos))
                {
                    //On utilise l'ability
                    aiBCharacter.UseAbility(abi, players[0].pos);
                }
            }
        }
    }

        /// <summary>
        /// Méthode permettant de savoir si un personnageIA est a coté de l'ennemi
        /// </summary>
        /// <param name="aiBCharacter"></param>
        /// <returns>True si un ennemy est a coté, false sinon</returns>
        private bool IsNextToEnemy(AIBattleCharacter aiBCharacter)
    {
        return Int2.Distance(aiBCharacter.pos, Level.singleton.battleCharacterList.Where(bc => bc.teamTag == TeamTag.Ally).First().pos) == 1;
    }
}
