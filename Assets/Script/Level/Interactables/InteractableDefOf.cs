using System.Collections.Generic;
using UnityEngine;

public static class InteractableDefOf
{
    //Liste de tout les Interactable
    public static Dictionary<string, InteractableBase> interactableDefList = new Dictionary<string, InteractableBase>();

    public static InteractableBase GetInteractableBaseNamed(string name)
    {
        if (interactableDefList.ContainsKey(name))
        {
            return interactableDefList[name];
        }
        else
        {
            Debug.LogWarning($"L'Interactable '{name}' n'existe pas !");
            return null;
        }
    }

    //Déclaration de tout les Interactable
    public static InteractableBase clickMe = new InteractableBase("clickMe", "click", (BattleCharacter bc, Int2 p) => Debug.Log("Click !"));
}
