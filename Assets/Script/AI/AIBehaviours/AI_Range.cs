using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AI_Range : AIBehaviour
{
    public override bool IsTurnEnded(AIBattleCharacter aiBCharacter)
    {
        return aiBCharacter.useablePA == 0;
    }

    public override void PlayTurn(AIBattleCharacter aiBCharacter)
    {
        List<BattleCharacter> players = Level.singleton.battleCharacterList.Where(bc => bc.teamTag == TeamTag.Ally).ToList();
        //On trie la liste afin de savoir quel personnage Ally est le plus proche de nous, ce sera la victime prioritaire
        players.OrderBy(bc => Level.singleton.pathFinder.FindPath(aiBCharacter.pos, bc.pos).Cost);

        //ON REGARDE SANS UN PREMIER TEMPS SI ON A LA RANGE 

        //Pour chaque Ability du personnage
        foreach (AbilityBase abi in aiBCharacter.character.useableAbilities)
        {
            //Si une ability fait des damage et si on a assez de PA pour la faire et Si la cible est a portée
            if(abi is AbilityDamage)
            {
                if ((abi as AbilityDamage).IsDamage && abi.Cost <= aiBCharacter.useablePA && abi.Range >= Int2.Distance(aiBCharacter.pos, players[0].pos))
                {
                    //On utilise l'ability
                    aiBCharacter.UseAbility(abi, players[0].pos);
                }
            }
        }

        //ON SE RAPPROCHE SI NECESSAIRE SINON ON FUIT


    }
}
