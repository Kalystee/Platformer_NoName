using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game
{
    private Level level;
    public Level Level
    {
        get
        {
            return this.level;
        }
        set
        {
            level = value;
        }
    }

    private Objective objective;
    public Objective Objective
    {
        get
        {
            return this.objective;
        }
        set
        {
            objective = value;
            Find.WindowStack.Add(new Window_Objectives(value));
        }
    }
}
