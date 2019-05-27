public class AIBattleCharacter : BattleCharacter
{
    public readonly AIBehaviour behaviour;

    public AIBattleCharacter(Character character, TeamTag tag, int x, int y, AIBehaviour behaviour) : base(character, tag, x, y)
    {
        this.behaviour = behaviour;
    }
}
