public abstract class AIBehaviour {

    /// <summary>
    /// Fonction appelé lors d'un tour (en boucle)
    /// </summary>
    /// <param name="character">AIBattleCharacter utilisé pour l'IA</param>
    public abstract void PlayTurn(AIBattleCharacter character);

    /// <summary>
    /// Le tour est-il fini pour l'IA ?
    /// </summary>
    /// <param name="character">AIBattleCharacter utilisé pour l'IA</param>
    /// <returns>True si tour considéré comme terminé</returns>
    public abstract bool IsTurnEnded(AIBattleCharacter character);
}
