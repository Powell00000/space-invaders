namespace Game.Gameplay
{
    public interface IUnit
    {
        EFaction Faction { get; }

        void ReceiveDamage(float amout);
        void ShootIfCan();

        void ForceDeath();
    }
}