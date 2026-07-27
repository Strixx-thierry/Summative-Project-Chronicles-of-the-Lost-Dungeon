// Slot 1 is always Slash; the class special (slot 2) is one of the rest, unlocked by a pickup
public enum AbilityType
{
    Slash,       // basic melee arc, all classes start with this
    SpinSlash,   // Knight special: 360 degree AoE
    Gun,         // Gunner special: pooled projectiles
    SuperPunch   // Brawler special: heavy single hit
}
