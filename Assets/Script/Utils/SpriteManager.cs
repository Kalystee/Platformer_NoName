using System.Collections.Generic;
using UnityEngine;

public static class SpriteManager
{
    //Dictionnaire de sprites
    private static Dictionary<string, Sprite> tileSpriteDictionnary;
    private static Dictionary<string, Sprite> interactableSpriteDictionnary;
    private static Dictionary<string, Sprite> itemsSpriteDictionnary;
    private static Dictionary<string, Sprite> abilitiesSpriteDictionnary;

    /// <summary>
    /// Permet de charger tout les sprites dans le dictionnaire prévu
    /// </summary>
    public static void LoadAllSprites()
    {
        //On créer des Dictionnaires neuf
        tileSpriteDictionnary = new Dictionary<string, Sprite>();
        interactableSpriteDictionnary = new Dictionary<string, Sprite>();
        itemsSpriteDictionnary = new Dictionary<string, Sprite>();
        abilitiesSpriteDictionnary = new Dictionary<string, Sprite>();

        //On récupère tout les sprites et on les installe dans les dictionnaires correspondants
        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/Tiles/");
        foreach(Sprite s in sprites)
        {
            tileSpriteDictionnary.Add(s.name, s);
        }
        
        sprites = Resources.LoadAll<Sprite>("Sprites/Interactables/");
        foreach (Sprite s in sprites)
        {
            interactableSpriteDictionnary.Add(s.name, s);
        }
        
        sprites = Resources.LoadAll<Sprite>("Sprites/Items/");
        foreach (Sprite s in sprites)
        {
            itemsSpriteDictionnary.Add(s.name, s);
        }
        
        sprites = Resources.LoadAll<Sprite>("Sprites/Abilities/");
        foreach (Sprite s in sprites)
        {
            abilitiesSpriteDictionnary.Add(s.name, s);
        }

        Debug.Log("Tous les sprites ont été chargés !");
    }

    /// <summary>
    /// Récupère le sprite du Tile nommé comme tel
    /// </summary>
    /// <param name="spriteName">Nom du sprite cherché</param>
    /// <returns>Le sprite du Tile cherché ou null si inconnu</returns>
    public static Sprite GetSpriteOfTileNamed(string spriteName)
    {
        if(spriteName == null)
        {
            return null;
        }

        //On regarde si le dictionnaire a été initialisé
        if(tileSpriteDictionnary == null)
        {
            Debug.LogWarning("Le dictionnaire des Sprites n'a jamais été initialisé ! Il va dont être initialisé maintenant mais cela peut créer des problèmes !");
            LoadAllSprites();
        }

        //On regarde si le dictionnaire contient le sprite nommé comme tel
        if(tileSpriteDictionnary.ContainsKey(spriteName))
        {
            //On retourne alors le sprite nommé ainsi
            return tileSpriteDictionnary[spriteName];
        }
        else
        {
            Debug.LogWarning($"Le sprite '{spriteName}' n'est pas existant !");
            //Sinon on retourne null
            return null;
        }
    }

    /// <summary>
    /// Récupère le sprite de l'Interactable nommé comme tel
    /// </summary>
    /// <param name="spriteName">Nom du sprite cherché</param>
    /// <returns>Le sprite de l'Interactable cherché ou null si inconnu</returns>
    public static Sprite GetSpriteOfInteractableNamed(string spriteName)
    {
        if (spriteName == null)
        {
            return null;
        }

        //On regarde si le dictionnaire a été initialisé
        if (interactableSpriteDictionnary == null)
        {
            Debug.LogWarning("Le dictionnaire des Sprites n'a jamais été initialisé ! Il va dont être initialisé maintenant mais cela peut créer des problèmes !");
            LoadAllSprites();
        }

        //On regarde si le dictionnaire contient le sprite nommé comme tel
        if (interactableSpriteDictionnary.ContainsKey(spriteName))
        {
            //On retourne alors le sprite nommé ainsi
            return interactableSpriteDictionnary[spriteName];
        }
        else
        {
            Debug.LogWarning($"Le sprite '{spriteName}' n'est pas existant !");
            //Sinon on retourne null
            return null;
        }
    }


    /// <summary>
    /// Récupère le sprite de l'Item nommé comme tel
    /// </summary>
    /// <param name="spriteName">Nom du sprite cherché</param>
    /// <returns>Le sprite de l'Item cherché ou null si inconnu</returns>
    public static Sprite GetSpriteOfItemNamed(string spriteName)
    {
        if (spriteName == null)
        {
            return null;
        }

        //On regarde si le dictionnaire a été initialisé
        if (itemsSpriteDictionnary == null)
        {
            Debug.LogWarning("Le dictionnaire des Sprites n'a jamais été initialisé ! Il va dont être initialisé maintenant mais cela peut créer des problèmes !");
            LoadAllSprites();
        }

        //On regarde si le dictionnaire contient le sprite nommé comme tel
        if (itemsSpriteDictionnary.ContainsKey(spriteName))
        {
            //On retourne alors le sprite nommé ainsi
            return itemsSpriteDictionnary[spriteName];
        }
        else
        {
            Debug.LogWarning($"Le sprite '{spriteName}' n'est pas existant !");
            //Sinon on retourne null
            return null;
        }
    }


    /// <summary>
    /// Récupère le sprite de l'Ability nommé comme tel
    /// </summary>
    /// <param name="spriteName">Nom du sprite cherché</param>
    /// <returns>Le sprite de l'Ability cherché ou null si inconnu</returns>
    public static Sprite GetSpriteOfAbilityNamed(string spriteName)
    {
        if (spriteName == null)
        {
            return null;
        }

        //On regarde si le dictionnaire a été initialisé
        if (abilitiesSpriteDictionnary == null)
        {
            Debug.LogWarning("Le dictionnaire des Sprites n'a jamais été initialisé ! Il va dont être initialisé maintenant mais cela peut créer des problèmes !");
            LoadAllSprites();
        }

        //On regarde si le dictionnaire contient le sprite nommé comme tel
        if (abilitiesSpriteDictionnary.ContainsKey(spriteName))
        {
            //On retourne alors le sprite nommé ainsi
            return abilitiesSpriteDictionnary[spriteName];
        }
        else
        {
            Debug.LogWarning($"Le sprite '{spriteName}' n'est pas existant !");
            //Sinon on retourne null
            return null;
        }
    }
}
