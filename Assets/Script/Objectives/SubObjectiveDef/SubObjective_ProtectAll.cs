using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

[DataContract]
public class SubObjective_ProtectAll : SubObjective, ISubObj_Neutral
{
    [DataMember]
    public new const string type = "protectAll";

    [DataMember]
    public List<Character> neutral { get; protected set; }

    public SubObjective_ProtectAll(string titre, string desc, bool isPrimary) : base(titre, desc, true, isPrimary)
    {
        neutral = new List<Character>();
    }

    public SubObjective_ProtectAll(string titre,string desc, bool isPrimary, params Character[] protects) : base(titre, desc, true, isPrimary)
    {
        this.neutral.AddRange(protects);
    } 

    /// <summary>
    /// Méthode permetant de savoir si l'objectif est atteint et si les cibles sont toujours en vie
    /// </summary>
    /// <returns>true si réussi, false sinon</returns>
    public override bool IsSubObjectiveDone()
    {
        return true;
        //return neutral.Any( TODO : Ajouter dans Level une fonction pour savoir si un bc est mort ou non );
    }
}
