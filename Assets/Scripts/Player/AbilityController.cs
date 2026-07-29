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
    private CameraFollow cam;
    private PlayerController playerController;

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
        playerController = GetComponent<PlayerController>();
        cam = Camera.main != null ? Camera.main.GetComponent<CameraFollow>() : null;
        if (projectilePrefab != null) projectilePool = new ObjectPool<Projectile>(projectilePrefab, 10);

        // Everyone starts with the basic Slash
        abilities.Add(Create(AbilityType.Slash));

        // If the class special was already picked up (saved), start with it too
        if (SaveManager.Instance.Data.specialUnlocked)
        {
            var loader = GetComponent<PlayerModelLoader>();
            AbilityType special = loader != null ? loader.Ability : AbilityType.Gun;
            abilities.Add(Create(special));
        }
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

        // Aim where the camera looks, and turn the player to face it
        Vector3 aim = cam != null ? cam.FlatForward : (facing != null ? facing.forward : transform.forward);
        if (playerController != null) playerController.FaceAim(aim);

        ability.Activate(new AbilityContext
        {
            owner = transform,
            facing = facing,
            aimDirection = aim,
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
        AbilityType.SpinSlash => new SpinSlashAbility(),
        AbilityType.Gun => new GunAbility(),
        AbilityType.SuperPunch => new SuperPunchAbility(),
        _ => new SwordAbility(),
    };
}
