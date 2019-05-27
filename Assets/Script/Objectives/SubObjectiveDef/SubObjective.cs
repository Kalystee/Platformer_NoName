using System.Runtime.Serialization;

[DataContract]
public abstract class SubObjective
{
    [DataMember]
    public const string type = "abstractSubObj";

    [DataMember]
    public string Title { get; protected set; }

    [DataMember]
    public string Description { get; protected set; }
    
    public bool IsCriticalFailure { get; protected set; }

    [DataMember]
    public bool IsPrimary { get; protected set; }

    public abstract bool IsSubObjectiveDone();

    public SubObjective(string titre, string desc ,bool criticalFail ,bool primary)
    {
        this.Title = titre;
        this.Description = desc;
        this.IsCriticalFailure = criticalFail;
        this.IsPrimary = primary;
    }

   
}
