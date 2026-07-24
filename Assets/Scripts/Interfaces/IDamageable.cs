// Anything that can be hurt: player, enemies, breakables
public interface IDamageable
{
    void TakeDamage(int amount);
    bool IsDead { get; }
}
