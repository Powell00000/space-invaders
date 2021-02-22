namespace Game.Gameplay
{
   public class GameplayStateMachine : SimpleStateMachine<EGameplayState>
   {
      public GameplayStateMachine() : base(EGameplayState.None)
      {
      }
   }
}