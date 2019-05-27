using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

[DataContract]
public class Objective
{
    [DataMember]
    public string Title { get; protected set; }
    [DataMember]
    public string Description { get; protected set; }
    [DataMember]
    private List<SubObjective> listOfSubObjective;
    
    public IList<SubObjective> ListOfSub
    {
        get
        {
            return listOfSubObjective.AsReadOnly();
        }
    }

    public Objective(string title, string description)
    {
        Title = title;
        Description = description;
        listOfSubObjective = new List<SubObjective>();
    }

    public void AddSubObjective(SubObjective sub)
    {
        if(sub != null)
        {
            listOfSubObjective.Add(sub);
        }
    }

    protected List<Character> Allies { get; set; } //TODO : Récupérer le groupe du joueur

    /// <summary>
    /// Liste de tout les ennemies de tout les sous objectifs
    /// </summary>
    protected List<Character> Enemies
    {
        get
        {
            List<Character> enemies = new List<Character>();
            foreach(SubObjective so in listOfSubObjective)
            {
                if(so is ISubObj_Enemies)
                {
                    ISubObj_Enemies en = so as ISubObj_Enemies;
                    enemies.AddRange(en.enemy);
                }
            }
            return enemies;
        }
    }

    /// <summary>
    /// Liste de tout les neutral de tout les sous objectifs
    /// </summary>
    protected List<Character> Neutrals
    {
        get
        {
            List<Character> neutrals = new List<Character>();
            foreach (SubObjective so in listOfSubObjective)
            {
                if (so is ISubObj_Neutral)
                {
                    ISubObj_Neutral ne = so as ISubObj_Neutral;
                    neutrals.AddRange(ne.neutral);
                }
            }
            return neutrals;
        }
    }

    /// <summary>
    /// Méthode permettant de savoir si un objectif a échoué
    /// </summary>
    /// <returns>true si il a échoué, false sinon</returns>
    public bool HasFailed()
    {
       foreach(SubObjective sub in this.listOfSubObjective)
        {
            if(sub.IsPrimary && sub.IsCriticalFailure && !sub.IsSubObjectiveDone())
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Méthode permettant de savoir si un objectif a été atteint
    /// </summary>
    /// <returns>true si il est ateint, false sinon</returns>
    public bool IsComplete()
    {
       return listOfSubObjective.All(so => so.IsPrimary && so.IsSubObjectiveDone());
    }
    
}
