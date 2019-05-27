public abstract class Buff  {

    public abstract string Name { get; }
    public abstract string Description { get; }
    public abstract bool IsBuff { get; }

    protected int time;
    public int GetTime()
    {
        return time;
    }

    public void ModifyTime(int value)
    {
        time += value;
    }

    public Buff(int nbTour)
    {
        time = nbTour;
    }

    public abstract Buff Copy();

    public override bool Equals(object obj)
    {
        bool res = false;

        if(this == obj)
        {
            res = true;
        }
        else if(obj is Buff)
        {
            Buff b = obj as Buff;
            res = this.Name.Equals(b.Name);
        }
        return res;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return base.ToString();
    }
}
