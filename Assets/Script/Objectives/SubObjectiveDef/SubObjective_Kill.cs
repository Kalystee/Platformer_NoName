using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;

[DataContract]
public class SubObjective_KillAll : SubObjective, ISubObj_Enemies
{
    [DataMember]
    public new const string type = "killAll";

    [DataMember]
    public List<Character> enemy { get; protected set; }
    
    public SubObjective_KillAll(string name, string desc, bool isPrimary) : base(name, desc, false, isPrimary)
    {
        enemy = new List<Character>();
    }

    public SubObjective_KillAll(string name, string desc, bool isPrimary, params Character[] kills) : base(name,desc,true,isPrimary)
    {
        enemy.AddRange(kills);
    }
    
    /// <summary>
    /// Méthode permettant de savoir si tous les ennemies sont morts
    /// </summary>
    /// <returns>True si ils sont tous morts, false sinon</returns>
    public override bool IsSubObjectiveDone()
    {
        foreach (Character bc in enemy)
        {
            //TODO : Ajouter dans Level une fonction pour savoir si un bc est mort ou non
        }
        return true;
    }
}
