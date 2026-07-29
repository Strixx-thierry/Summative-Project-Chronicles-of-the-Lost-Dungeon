# Chronicles of the Lost Dungeon

A 5-level dungeon adventure game made in **Unity 6.3**. Pick a class, fight through the levels, collect coins, grab a special weapon, and reach the glowing exit. Built to show clean, reusable game code.

## How to Play

| Action | Keyboard | Mobile |
|---|---|---|
| Move | WASD | Joystick (bottom-left) |
| Aim | Mouse | — |
| Attack | Left click | ATTACK |
| Swap weapon | Q | SWAP |
| Inspect weapon | I | INSPECT |
| Pause | Esc | — |

Each level shows its goals on screen (defeat enemies, collect coins, find the weapon). Finish them all and the exit turns blue — walk in to complete the level. Beaten levels are saved and unlock the next one.

## Classes
- **Adventurer** – sword + spin slash
- **Gunner** – SMG that shoots bullets
- **Brawler** – axe + heavy punch

You start with a basic slash; your special weapon is found in Level 2.

## How It's Built (quick version)

The game keeps systems separate so new things are easy to add:
- **Managers** (`GameManager`, `AudioManager`, `SaveManager`) are single global objects.
- **Events** (`GameEvents`) let systems react to things like "enemy defeated" without knowing about each other.
- **Interfaces** let us swap in new weapons, enemies, and objectives without touching old code.

### Design patterns used
- **Singleton** – the manager classes
- **Observer** – `GameEvents` + C# events (health, enemy defeated, item collected)
- **Strategy** – weapons (`IAbility`) and enemy attacks (`IEnemyBehaviour`)
- **State** – enemy AI states (idle → chase → attack)
- **Object Pooling** – reused bullet projectiles

### Interfaces
`IDamageable`, `IAbility`, `IEnemyState`, `IEnemyBehaviour`, `IObjective`, `ICollectable`

### Algorithms
1. **Damage calculation** – `damage = base × crit × (100 / (100 + defence))`, minimum 1. Handles crits and defence in one place so combat stays balanced.
2. **Enemy targeting** – enemies measure distance to the player to decide when to chase or attack, and move along the direction toward the player. The Maw locks its direction and charges.
3. **Object pooling** – bullets are kept in a reusable queue instead of being created and destroyed every shot, which keeps the game smooth.

## Saving & Online Data
- **Save:** progress (name, class, unlocked levels, best times, coins) is saved as **JSON** in the device's save folder, so it stays after closing the game. JSON is used because it's simple, readable, and works on every platform.
- **Online:** press **I** to see real weapon stats pulled from the free **D&D 5e API** (`dnd5eapi.co`). If there's no internet it just says "unavailable" and the game keeps going.

## Platforms
Runs on **WebGL, Windows, and Android**. The mobile joystick/buttons only appear on phones, using Unity conditional compilation (`#if UNITY_ANDROID`).

## Tests
11 unit tests (NUnit) check the core logic — damage, saving, level unlocking, and the object pool.
Run them in Unity: **Window → General → Test Runner → EditMode → Run All**.

## Run & Build
1. Open in **Unity 6.3**, press Play on `Assets/Scenes/MainMenu.unity`.
2. To build: **File → Build Profiles**, pick Windows / WebGL / Android.

## Build Links
- WebGL (Unity Play): _add link_
- Windows: _add link_
- Android: _add link_

## Credits
All models, audio and fonts are listed in [CREDITS.md](CREDITS.md). Animations from Mixamo; weapon data from dnd5eapi.co.
