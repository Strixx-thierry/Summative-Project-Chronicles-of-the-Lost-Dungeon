using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

// Holds the player's weapons as swappable strategies; 1/2 switch, left click attacks
public class AbilityController : MonoBehaviour
{
    [SerializeField] private Transform facing;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private int baseDamage = 40;

    private readonly List<IAbility> abilities = new List<IAbility>();
    private int current;
    private float cooldownTimer;
    private Animator animator;
    private ObjectPool<Projectile> projectilePool;

    public IReadOnlyList<IAbility> Abilities => abilities;
    public int CurrentIndex => current;
    public System.Action OnAbilitiesChanged;

    void Awake()
    {
        if (facing == null) facing = transform.Find("Yaw");
    }

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        if (projectilePrefab != null) projectilePool = new ObjectPool<Projectile>(projectilePrefab, 10);

        // Start with the ability that matches the chosen class
        var loader = GetComponent<PlayerModelLoader>();
        AbilityType startType = loader != null ? loader.Ability : AbilityType.Sword;
        abilities.Add(Create(startType));
        OnAbilitiesChanged?.Invoke();
    }

    void Update()
    {
        cooldownTimer -= Time.deltaTime;
        var kb = Keyboard.current;
        if (kb != null)
        {
            if (kb.digit1Key.wasPressedThisFrame) Switch(0);
            if (kb.digit2Key.wasPressedThisFrame) Switch(1);
        }
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame) Activate();
    }

    void Activate()
    {
        if (cooldownTimer > 0f || current >= abilities.Count) return;
        var ability = abilities[current];
        cooldownTimer = ability.Cooldown;
        ability.Activate(new AbilityContext
        {
            owner = transform,
            facing = facing,
            animator = animator,
            projectilePool = projectilePool,
            damage = baseDamage,
        });
    }

    // Picked up a new weapon
    public void AddAbility(AbilityType type)
    {
        foreach (var a in abilities) if (a.Name == Create(type).Name) return; // no duplicates
        abilities.Add(Create(type));
        current = abilities.Count - 1;   // auto-equip the new weapon
        OnAbilitiesChanged?.Invoke();
    }

    void Switch(int index)
    {
        if (index < 0 || index >= abilities.Count) return;
        current = index;
        OnAbilitiesChanged?.Invoke();
    }

    IAbility Create(AbilityType type) => type switch
    {
        AbilityType.Gun => new GunAbility(),
        AbilityType.Fists => new FistsAbility(),
        _ => new SwordAbility(),
    };
}
