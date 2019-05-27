using UnityEngine;

public abstract class AbilityBase  {

    //Propriété : Nom
    public string Name { get; private set; }
    //Propriété : Portée
    public int Range { get; protected set; }
    //Propriété : Coup en PA
    public int Cost { get; protected set; }
    //Propriété : Zone d'effet autour de l'impact
    public EffectZone Area { get; protected set; }
    //Propriété : Sprite
    private string abilitySpriteName;

    public Sprite Sprite { get { return SpriteManager.GetSpriteOfAbilityNamed(abilitySpriteName); } }

    public AbilityBase(string name, string spriteName, int range, int cost, EffectZone effectZone)
    {
        this.Name = name;
        this.Range = range;
        this.Cost = cost;
        this.abilitySpriteName = spriteName;
        this.Area = effectZone;
    }

   
}
