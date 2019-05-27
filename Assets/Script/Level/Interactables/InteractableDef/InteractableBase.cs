using System;
using UnityEngine;

public class InteractableBase
{
    public string Name { get; protected set; }

    //Propriété : Interagissable
    public bool IsInteractable { get; protected set; }
    //Propriété : Sprite
    private string interactableSpriteName;
    public Sprite Sprite { get { return SpriteManager.GetSpriteOfInteractableNamed(interactableSpriteName); } }

    //Action à appliquer lors de l'interaction
    protected Action<BattleCharacter, Int2> onInteraction;

    public InteractableBase(string name, string sprite, Action<BattleCharacter, Int2> onUsed)
    {
        //On ajoute à la liste de tout les Interactable pour les sauvegarde/chargements plus tard.
        InteractableDefOf.interactableDefList.Add(name, this);

        //On enregistre tout les attributs d'un Interactable
        Name = name;
        interactableSpriteName = sprite;
        onInteraction = onUsed;
        IsInteractable = true;
    }

    /// <summary>
    /// Intéragit avec l'objet
    /// </summary>
    /// <param name="user">Personnage utilisateur</param>
    /// <param name="position">Position de l'objet</param>
    public virtual void Interact(BattleCharacter bc, Int2 position)
    {
        if (IsInteractable)
        {
            IsInteractable = false;
            onInteraction?.Invoke(bc, position);
        }
    }
}
