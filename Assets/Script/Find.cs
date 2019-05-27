using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Find
{
    public static WindowStack WindowStack
    {
        get { return Current.windowStack; }
    }

    public static Game Game
    {
        get { return Current.game; }
    }

    public static Party Party
    {
        get { return Current.party; }
    }

    public static Objective Objective
    {
        get { return Find.Game?.Objective; }
    }

    public static Core Core
    {
        get { return Current.core; }
    }

    public static Level Level
    {
        get { return Find.Game?.Level; }
    }

    public static Arena Arena
    {
        get { return Find.Level?.arena; }
    }

    public static BattleCharacter PlayingBC
    {
        get { return Find.Level?.playingBattleCharacter; }
    }
}
