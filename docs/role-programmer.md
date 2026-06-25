# Escape Days — Programmer Role Guide
> **Role:** Core Systems Programmer, Gameplay Programmer  
> **Scope:** C# standards, architecture patterns, performance rules, and system design  
> **Last Updated:** 2025 | **Version:** 2.0 (2D)

---

## Table of Contents
1. [Project Scripts Structure](#1-project-scripts-structure)
2. [Naming & Code Conventions](#2-naming--code-conventions)
3. [Architecture Patterns](#3-architecture-patterns)
4. [2D Physics & Performance Rules](#4-2d-physics--performance-rules)
5. [Object Pooling](#5-object-pooling)
6. [State Management (FSM)](#6-state-management-fsm)
7. [Save System](#7-save-system)
8. [ScriptableObject Data Pattern](#8-scriptableobject-data-pattern)
9. [Input System (New)](#9-input-system-new)
10. [Do Not — C# Hard Rules](#10-do-not--c-hard-rules)

---

## 1. Project Scripts Structure

All scripts live under `Assets/_Project/Scripts/`. Structure by **domain (feature)**, not by type.

```
Assets/_Project/Scripts/
│
├── Core/                           ← Engine-level utilities, bootstrapping
│   ├── Bootstrap/
│   │   └── GameBootstrapper.cs     ← Entry point: initializes all managers
│   ├── Events/
│   │   ├── GameEventSO.cs          ← ScriptableObject event channel
│   │   └── GameEventListener.cs
│   ├── Pool/
│   │   └── ObjectPool.cs           ← Generic Object Pool
│   └── Extensions/
│       └── Vector2Extensions.cs
│
├── Player/
│   ├── PlayerController.cs
│   ├── PlayerMovementController.cs
│   ├── PlayerCombatController.cs
│   └── PlayerInteractionController.cs
│
├── Vitals/
│   ├── VitalsSystem.cs             ← MonoBehaviour on player, manages all vitals
│   ├── VitalsData.cs               ← ScriptableObject: rates, thresholds
│   └── VitalEffect.cs              ← Effect applied when vital is critical
│
├── Companion/
│   ├── CompanionController.cs
│   ├── CompanionAI.cs
│   ├── CompanionData.cs            ← ScriptableObject: name, skills, stats
│   └── CompanionCommandHandler.cs
│
├── Enemy/
│   ├── EnemyController.cs
│   ├── EnemyAI.cs
│   ├── EnemyDetection.cs           ← Sight cone + noise detection
│   ├── EnemyData.cs                ← ScriptableObject: stats, patrol points
│   └── States/
│       ├── EnemyPatrolState.cs
│       ├── EnemyInvestigateState.cs
│       ├── EnemyAlertState.cs
│       └── EnemyAttackState.cs
│
├── Combat/
│   ├── WeaponController.cs
│   ├── ProjectileController.cs     ← Managed via Object Pool
│   ├── MeleeAttackHandler.cs
│   ├── DamageReceiver.cs           ← Interface implementation on any damageable
│   └── NoiseEmitter.cs
│
├── Crafting/
│   ├── CraftingSystem.cs
│   ├── RecipeData.cs               ← ScriptableObject: ingredients + output
│   └── WorkbenchInteractable.cs
│
├── Building/
│   ├── BuildingSystem.cs
│   ├── StructureController.cs
│   ├── GridPlacementValidator.cs
│   └── RaidManager.cs
│
├── Inventory/
│   ├── InventorySystem.cs
│   ├── ItemData.cs                 ← ScriptableObject: item definition
│   └── ItemInstance.cs            ← Runtime item with quantity, durability
│
├── World/
│   ├── DayNightCycle.cs
│   ├── ResourceNode.cs             ← World interactable resources
│   └── EnvironmentHazard.cs
│
├── Save/
│   ├── SaveSystem.cs               ← Singleton, handles read/write
│   ├── SaveData.cs                 ← [Serializable] struct — the save schema
│   └── ISaveable.cs               ← Interface for saveable components
│
├── UI/
│   ├── HUD/
│   │   ├── HUDController.cs        ← Manages all HUD panels
│   │   ├── VitalsHUDView.cs        ← Binds VitalsSystem to UI Toolkit
│   │   └── CompanionHUDView.cs
│   └── Menus/
│       ├── MainMenuController.cs
│       └── PauseMenuController.cs
│
├── Audio/
│   ├── AudioManager.cs             ← Singleton
│   └── FootstepController.cs
│
└── Infrastructure/
    ├── GameManager.cs              ← Master game state (Playing, Paused, Dead)
    ├── ServiceLocator.cs           ← Alternative to singleton abuse
    └── SceneLoader.cs
```

---

## 2. Naming & Code Conventions

### 2.1 General C# Standards

```csharp
// ── Classes & Structs ────────────────────────────────
public class PlayerMovementController : MonoBehaviour { }  // PascalCase
public struct VitalSnapshot { }                             // PascalCase

// ── Interfaces ────────────────────────────────────────
public interface IDamageable { }        // Prefix with 'I'
public interface ISaveable { }
public interface IInteractable { }

// ── Enums ─────────────────────────────────────────────
public enum GameState { Playing, Paused, Dead, Loading }   // PascalCase values

// ── Public Properties (exposed, not raw fields) ───────
public float CurrentHealth { get; private set; }           // PascalCase

// ── Private Fields (Unity-serialized) ─────────────────
[SerializeField] private float _moveSpeed = 5f;            // _camelCase prefix
[SerializeField] private Rigidbody2D _rb;
[SerializeField] private VitalsData _vitalsData;

// ── Private Fields (not serialized) ───────────────────
private float _currentHunger;
private bool _isGrounded;                                  // _camelCase prefix

// ── Constants ─────────────────────────────────────────
private const float MaxStamina = 100f;                     // PascalCase
private const string SaveFileName = "escape_days_save.json";

// ── Local Variables ───────────────────────────────────
float depletionAmount = _vitalsData.hungerRate * Time.deltaTime;  // camelCase
int enemyCount = 0;

// ── Methods ───────────────────────────────────────────
public void TakeDamage(float amount) { }                   // PascalCase, verb
private void UpdateHunger(float deltaTime) { }
private bool IsPlayerVisible() { return false; }           // IsX for booleans
```

### 2.2 MonoBehaviour Order Convention

Always write MonoBehaviour members in this order:

```csharp
public class ExampleController : MonoBehaviour
{
    // 1. Serialized Fields (Inspector-visible)
    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private Rigidbody2D _rb;

    // 2. Private Runtime Fields
    private Vector2 _moveInput;
    private bool _isMoving;

    // 3. Public Properties
    public bool IsMoving => _isMoving;

    // 4. Unity Lifecycle (in execution order)
    private void Awake()    { /* Cache refs, init data */ }
    private void OnEnable() { /* Subscribe to events */ }
    private void Start()    { /* Post-Awake init */ }
    private void Update()   { /* Input read ONLY */ }
    private void FixedUpdate() { /* Physics movement */ }
    private void LateUpdate()  { /* Camera follow, etc. */ }
    private void OnDisable() { /* Unsubscribe events */ }
    private void OnDestroy() { /* Cleanup */ }

    // 5. Public Methods
    public void TakeDamage(float amount) { }

    // 6. Private Methods
    private void HandleMovement(Vector2 input) { }

    // 7. Unity Physics Callbacks
    private void OnTriggerEnter2D(Collider2D other) { }
    private void OnCollisionEnter2D(Collision2D collision) { }

    // 8. Gizmos (Editor-only)
    private void OnDrawGizmosSelected() { }
}
```

### 2.3 ScriptableObject Naming

```csharp
// File naming: [Name]Data.cs or [Name]SO.cs
// Asset naming: [PREFIX]_[Name] — e.g., ITEM_MedKit, RECIPE_Bandage, ENEMY_Soldier

[CreateAssetMenu(fileName = "ItemData_New", menuName = "EscapeDays/Items/Item Data")]
public class ItemData : ScriptableObject
{
    [Header("Identity")]
    public string itemName;
    public Sprite icon;
    
    [Header("Properties")]
    public float weight;
    public bool isStackable;
    public int maxStack = 1;
}
```

---

## 3. Architecture Patterns

### 3.1 Singleton Pattern (Manager Classes Only)

Use Singleton **exclusively** for manager-level classes that are globally unique and must persist across scenes.

**Approved Singletons:** `GameManager`, `AudioManager`, `SaveSystem`, `ObjectPoolManager`

```csharp
/// <summary>
/// Generic Singleton base class. Inherit for manager classes only.
/// </summary>
public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError($"[Singleton] {typeof(T).Name} instance not found in scene!");
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Debug.LogWarning($"[Singleton] Duplicate {typeof(T).Name} destroyed.");
            Destroy(gameObject);
            return;
        }
        _instance = this as T;
        DontDestroyOnLoad(gameObject);
    }
}

// Usage:
public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    public void PlaySFX(AudioClip clip) { /* ... */ }
}

// Calling:
AudioManager.Instance.PlaySFX(myClip);
```

### 3.2 Observer Pattern (Events)

Use C# events or ScriptableObject event channels. **Never poll with Update to detect state changes.**

#### Option A — C# Static Events (Simple, intra-domain)

```csharp
// In VitalsSystem.cs
public class VitalsSystem : MonoBehaviour
{
    // Events — subscribe from any listener
    public static event Action<float> OnHealthChanged;
    public static event Action OnPlayerDied;
    public static event Action<float> OnHungerChanged;

    private float _currentHealth;

    public void TakeDamage(float amount)
    {
        _currentHealth = Mathf.Max(0f, _currentHealth - amount);
        OnHealthChanged?.Invoke(_currentHealth);

        if (_currentHealth <= 0f)
            OnPlayerDied?.Invoke();
    }
}

// In VitalsHUDView.cs
public class VitalsHUDView : MonoBehaviour
{
    private void OnEnable()
    {
        VitalsSystem.OnHealthChanged += UpdateHealthBar;
        VitalsSystem.OnPlayerDied += ShowDeathScreen;
    }

    private void OnDisable()
    {
        // CRITICAL: Always unsubscribe to prevent memory leaks
        VitalsSystem.OnHealthChanged -= UpdateHealthBar;
        VitalsSystem.OnPlayerDied -= ShowDeathScreen;
    }

    private void UpdateHealthBar(float newHealth) { /* update UI */ }
    private void ShowDeathScreen() { /* show UI panel */ }
}
```

#### Option B — ScriptableObject Event Channels (Cross-scene, decoupled)

```csharp
// GameEventSO.cs — ScriptableObject event with no parameters
[CreateAssetMenu(menuName = "EscapeDays/Events/Game Event")]
public class GameEventSO : ScriptableObject
{
    private readonly List<GameEventListener> _listeners = new();

    public void Raise()
    {
        for (int i = _listeners.Count - 1; i >= 0; i--)
            _listeners[i].OnEventRaised();
    }

    public void Register(GameEventListener listener) => _listeners.Add(listener);
    public void Deregister(GameEventListener listener) => _listeners.Remove(listener);
}

// GameEventListener.cs — MonoBehaviour that responds to SO events
public class GameEventListener : MonoBehaviour
{
    [SerializeField] private GameEventSO _event;
    [SerializeField] private UnityEvent _response;

    private void OnEnable() => _event.Register(this);
    private void OnDisable() => _event.Deregister(this);
    public void OnEventRaised() => _response.Invoke();
}
```

### 3.3 Dependency Injection (Constructor or Inspector)

Prefer **Inspector injection** (drag references in Unity Editor) over runtime lookup.

```csharp
// ✅ CORRECT — injected via Inspector
public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyData _data;           // ScriptableObject
    [SerializeField] private EnemyAI _ai;               // Sibling component
    [SerializeField] private DamageReceiver _damageReceiver;
    
    private void Awake()
    {
        // Cache — done ONCE in Awake, never in Update
        _ai.Initialize(_data);
    }
}

// ❌ WRONG — runtime lookup in Start
public class EnemyController : MonoBehaviour
{
    private void Start()
    {
        var player = FindObjectOfType<PlayerController>(); // BANNED
    }
}
```

---

## 4. 2D Physics & Performance Rules

### 4.1 Rigidbody2D Rules

```csharp
public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _moveSpeed = 5f;

    private Vector2 _moveInput;

    // ✅ Read input in Update (runs every frame, captures input accurately)
    private void Update()
    {
        _moveInput = InputManager.Instance.MoveInput; // Cached InputAction value
    }

    // ✅ Apply physics in FixedUpdate (fixed timestep, deterministic)
    private void FixedUpdate()
    {
        _rb.linearVelocity = _moveInput * _moveSpeed;
    }
}
```

**Rules:**
- All physics movement via `Rigidbody2D.linearVelocity`, `AddForce2D`, or `MovePosition` — never via `Transform.position` on physics objects.
- Use `FixedUpdate` for all physics. Never `Update`.
- `Rigidbody2D.Collision Detection Mode` = **Continuous** for fast-moving objects (bullets, player at sprint).

### 4.2 Collision Layer Matrix

Define these layers in Unity (Edit → Project Settings → Physics 2D → Layer Collision Matrix):

| Layer | Player | Enemy | Companion | Projectile | World | Trigger |
|---|---|---|---|---|---|---|
| **Player** | ✗ | ✓ | ✗ | ✗ | ✓ | ✓ |
| **Enemy** | ✓ | ✗ | ✓ | ✓ | ✓ | ✓ |
| **Companion** | ✗ | ✓ | ✗ | ✗ | ✓ | ✓ |
| **Projectile** | ✓ | ✓ | ✓ | ✗ | ✓ | ✗ |

Use `LayerMask` constants in code:

```csharp
// Define in a static class
public static class Layers
{
    public static readonly int Player = LayerMask.NameToLayer("Player");
    public static readonly int Enemy = LayerMask.NameToLayer("Enemy");
    public static readonly int World = LayerMask.NameToLayer("World");
    
    // Masks (for Physics2D queries)
    public static readonly int EnemyMask = LayerMask.GetMask("Enemy");
    public static readonly int PlayerMask = LayerMask.GetMask("Player");
    public static readonly int WorldMask = LayerMask.GetMask("World");
}

// Usage:
var hit = Physics2D.Raycast(origin, direction, range, Layers.EnemyMask);
```

### 4.3 Avoiding Allocations in Update

```csharp
// ❌ WRONG — allocates every frame → GC spikes
private void Update()
{
    var enemies = FindObjectsByType<EnemyController>(FindObjectsSortMode.None); // BAD
    var list = new List<Collider2D>(); // Allocation in Update = BAD
    string debug = "Health: " + _health.ToString(); // String concat = BAD
}

// ✅ CORRECT — pre-allocate, reuse
private Collider2D[] _overlapResults = new Collider2D[16]; // Pre-allocated buffer
private ContactFilter2D _contactFilter;

private void Awake()
{
    _contactFilter.SetLayerMask(Layers.EnemyMask);
    _contactFilter.useTriggers = false;
}

private void FixedUpdate()
{
    // Non-allocating overlap check
    int count = Physics2D.OverlapCircle(
        transform.position, _detectionRadius, _contactFilter, _overlapResults);
        
    for (int i = 0; i < count; i++)
    {
        // Process _overlapResults[i]
    }
}
```

### 4.4 Sight Cone Detection (2D Raycast)

```csharp
/// <summary>
/// 2D line-of-sight check. Returns true if target is visible.
/// </summary>
private bool CanSeeTarget(Transform target)
{
    Vector2 directionToTarget = (target.position - transform.position).normalized;
    float distanceToTarget = Vector2.Distance(transform.position, target.position);

    // Check angle (sight cone)
    float angle = Vector2.Angle(transform.up, directionToTarget);
    if (angle > _sightAngle * 0.5f) return false;

    // Check distance
    if (distanceToTarget > _sightRange) return false;

    // Raycast for obstacles
    RaycastHit2D hit = Physics2D.Raycast(
        transform.position,
        directionToTarget,
        distanceToTarget,
        Layers.WorldMask | Layers.PlayerMask
    );

    return hit.collider != null && hit.collider.CompareTag("Player");
}
```

---

## 5. Object Pooling

**Required for:** Projectiles, Enemy spawns, Damage numbers, Particle effects, Loot drops.

### 5.1 Generic Object Pool

```csharp
/// <summary>
/// Generic Object Pool. Attach to a manager GameObject.
/// </summary>
public class ObjectPool<T> where T : MonoBehaviour
{
    private readonly T _prefab;
    private readonly Transform _parent;
    private readonly Queue<T> _pool = new();

    public ObjectPool(T prefab, int initialSize, Transform parent = null)
    {
        _prefab = prefab;
        _parent = parent;

        // Pre-warm the pool
        for (int i = 0; i < initialSize; i++)
            _pool.Enqueue(CreateInstance());
    }

    public T Get()
    {
        T instance = _pool.Count > 0 ? _pool.Dequeue() : CreateInstance();
        instance.gameObject.SetActive(true);
        return instance;
    }

    public void Return(T instance)
    {
        instance.gameObject.SetActive(false);
        _pool.Enqueue(instance);
    }

    private T CreateInstance()
    {
        T instance = Object.Instantiate(_prefab, _parent);
        instance.gameObject.SetActive(false);
        return instance;
    }
}
```

### 5.2 Bullet Pool Example

```csharp
public class BulletPoolManager : SingletonMonoBehaviour<BulletPoolManager>
{
    [SerializeField] private ProjectileController _bulletPrefab;
    [SerializeField] private int _poolSize = 30;

    private ObjectPool<ProjectileController> _pool;

    protected override void Awake()
    {
        base.Awake();
        _pool = new ObjectPool<ProjectileController>(_bulletPrefab, _poolSize, transform);
    }

    public ProjectileController GetBullet() => _pool.Get();
    public void ReturnBullet(ProjectileController bullet) => _pool.Return(bullet);
}

// In ProjectileController.cs:
public class ProjectileController : MonoBehaviour
{
    [SerializeField] private float _speed = 15f;
    [SerializeField] private float _lifetime = 3f;
    private Rigidbody2D _rb;
    private float _spawnTime;

    private void Awake() => _rb = GetComponent<Rigidbody2D>(); // Cached in Awake ✅

    private void OnEnable()
    {
        _spawnTime = Time.time;
        // Direction set by caller after GetBullet()
    }

    private void Update()
    {
        if (Time.time - _spawnTime >= _lifetime)
            BulletPoolManager.Instance.ReturnBullet(this); // Return to pool
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IDamageable>(out var damageable))
            damageable.TakeDamage(10f);

        BulletPoolManager.Instance.ReturnBullet(this);
    }

    public void Launch(Vector2 direction)
    {
        _rb.linearVelocity = direction * _speed;
    }
}
```

---

## 6. State Management (FSM)

Use a **lightweight FSM** for any object with 3+ distinct behaviors (enemies, companions, game states).

### 6.1 Interface-Based FSM

```csharp
// IState.cs
public interface IState
{
    void Enter();
    void Tick();          // Called each Update/FixedUpdate
    void Exit();
}

// StateMachine.cs
public class StateMachine
{
    private IState _currentState;

    public IState CurrentState => _currentState;

    public void ChangeState(IState newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState?.Enter();
    }

    public void Tick() => _currentState?.Tick();
}
```

### 6.2 Enemy FSM Usage

```csharp
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private EnemyData _data;

    private StateMachine _stateMachine;

    // States — instantiated once, reused
    private EnemyPatrolState _patrolState;
    private EnemyInvestigateState _investigateState;
    private EnemyAttackState _attackState;

    private void Awake()
    {
        _stateMachine = new StateMachine();
        _patrolState = new EnemyPatrolState(this, _data);
        _investigateState = new EnemyInvestigateState(this, _data);
        _attackState = new EnemyAttackState(this, _data);
    }

    private void Start()
    {
        _stateMachine.ChangeState(_patrolState);
    }

    private void Update()
    {
        _stateMachine.Tick();
    }

    public void OnPlayerSpotted() => _stateMachine.ChangeState(_attackState);
    public void OnPlayerLost() => _stateMachine.ChangeState(_investigateState);
}

// EnemyPatrolState.cs
public class EnemyPatrolState : IState
{
    private readonly EnemyAI _enemy;
    private readonly EnemyData _data;

    public EnemyPatrolState(EnemyAI enemy, EnemyData data)
    {
        _enemy = enemy;
        _data = data;
    }

    public void Enter()  { /* Set patrol path */ }
    public void Tick()   { /* Move along waypoints, check for player */ }
    public void Exit()   { /* Stop movement */ }
}
```

---

## 7. Save System

### 7.1 SaveData Schema

```csharp
// SaveData.cs — the save file schema
// NEVER remove or rename fields without a migration strategy.

[Serializable]
public struct SaveData
{
    // ── World ──────────────────────────────
    public float worldTimestamp;        // In-game time
    public string currentSceneName;

    // ── Player ─────────────────────────────
    public SerializableVector2 playerPosition;
    public float health;
    public float stamina;
    public float hunger;
    public float thirst;
    public float sleep;

    // ── Inventory ──────────────────────────
    public List<SerializedItemInstance> inventoryItems;

    // ── Companions ─────────────────────────
    public List<SerializedCompanionState> companions;

    // ── Base ───────────────────────────────
    public List<SerializedStructure> placedStructures;

    // ── Meta ───────────────────────────────
    public string saveVersion;      // For future migration
    public long saveTimestamp;      // System.DateTime.UtcNow.Ticks
}

// Unity's Vector2 is not serializable by JsonUtility in all cases
[Serializable]
public struct SerializableVector2
{
    public float x, y;
    public SerializableVector2(Vector2 v) { x = v.x; y = v.y; }
    public Vector2 ToVector2() => new Vector2(x, y);
}
```

### 7.2 SaveSystem Implementation

```csharp
public class SaveSystem : SingletonMonoBehaviour<SaveSystem>
{
    private const string SaveFileName = "escape_days_save.json";
    private const string SaveVersion = "1.0.0";

    private string SavePath => Path.Combine(Application.persistentDataPath, SaveFileName);

    public void Save(SaveData data)
    {
        data.saveVersion = SaveVersion;
        data.saveTimestamp = DateTime.UtcNow.Ticks;

        string json = JsonUtility.ToJson(data, prettyPrint: false);
        File.WriteAllText(SavePath, json);
        Debug.Log($"[SaveSystem] Game saved to {SavePath}");
    }

    public bool TryLoad(out SaveData data)
    {
        if (!File.Exists(SavePath))
        {
            data = default;
            return false;
        }

        string json = File.ReadAllText(SavePath);
        data = JsonUtility.FromJson<SaveData>(json);
        return true;
    }

    public void DeleteSave()
    {
        if (File.Exists(SavePath))
            File.Delete(SavePath);
    }
}
```

### 7.3 ISaveable Interface

```csharp
public interface ISaveable
{
    void OnSave(ref SaveData data);
    void OnLoad(SaveData data);
}

// In VitalsSystem.cs:
public class VitalsSystem : MonoBehaviour, ISaveable
{
    public void OnSave(ref SaveData data)
    {
        data.health = _currentHealth;
        data.hunger = _currentHunger;
        data.thirst = _currentThirst;
    }

    public void OnLoad(SaveData data)
    {
        _currentHealth = data.health;
        _currentHunger = data.hunger;
        _currentThirst = data.thirst;
    }
}
```

---

## 8. ScriptableObject Data Pattern

ScriptableObjects (SOs) define **static configuration data**. Never store runtime mutable state in SOs.

```csharp
// ✅ CORRECT — SO contains only config data
[CreateAssetMenu(menuName = "EscapeDays/Items/Item Data")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public string description;
    public Sprite icon;
    public float weight;
    public bool isStackable;
    public int maxStack;
    public ItemCategory category;
}

// ✅ CORRECT — Runtime state lives in a separate class
[Serializable]
public class ItemInstance
{
    public ItemData data;    // Reference to SO
    public int quantity;
    public float durability;
}

// ❌ WRONG — mutable runtime state on an SO
[CreateAssetMenu(menuName = "Bad Example")]
public class ItemData : ScriptableObject
{
    public int currentQuantity; // WRONG — SOs are shared assets, this mutates globally
}
```

### SO Asset Naming Convention

| Type | Prefix | Example |
|---|---|---|
| Item | `ITEM_` | `ITEM_MedKit.asset` |
| Recipe | `RECIPE_` | `RECIPE_Bandage.asset` |
| Enemy | `ENEMY_` | `ENEMY_Soldier.asset` |
| Companion | `COMP_` | `COMP_Maya.asset` |
| Vital Config | `VITALS_` | `VITALS_Default.asset` |
| Event Channel | `EVT_` | `EVT_PlayerDied.asset` |

---

## 9. Input System (New)

**All input must use Unity's new Input System.** Legacy `Input.GetKey`, `Input.GetAxis` are banned.

```csharp
// InputManager.cs — Single entry point for input
public class InputManager : SingletonMonoBehaviour<InputManager>
{
    private PlayerInputActions _actions; // Generated from InputActionAsset

    // Cached input values — read by other systems
    public Vector2 MoveInput { get; private set; }
    public bool IsSprintHeld { get; private set; }
    public bool IsCrouchHeld { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        _actions = new PlayerInputActions();
        _actions.Enable();
    }

    private void Update()
    {
        MoveInput = _actions.Player.Move.ReadValue<Vector2>();
        IsSprintHeld = _actions.Player.Sprint.IsPressed();
        IsCrouchHeld = _actions.Player.Crouch.IsPressed();
    }

    private void OnDestroy() => _actions.Disable();
}

// In PlayerMovementController.cs — reads from InputManager
private void Update()
{
    _moveInput = InputManager.Instance.MoveInput;
    _isSprinting = InputManager.Instance.IsSprintHeld;
}
```

---

## 10. Do Not — C# Hard Rules

> Violations of these rules will block PR merges. These are non-negotiable.

### ❌ Banned API Calls

| Banned | Why | Replacement |
|---|---|---|
| `FindObjectOfType<T>()` | O(n) scan of all scene objects; breaks with multiple instances | Inspector injection or `ServiceLocator` |
| `FindObjectsByType<T>()` in Update | Same as above, in a hot path | Pre-cache in `Awake` |
| `Camera.main` in `Update()` | Tag-based lookup per frame | Cache in `Awake`: `_cam = Camera.main` |
| `GetComponent<T>()` in `Update/FixedUpdate` | Reflection per frame → massive perf cost | Cache in `Awake` or use `[SerializeField]` |
| `SendMessage()` / `BroadcastMessage()` | Reflection, no compile-time safety | Use interfaces or C# events |
| `Resources.Load<T>()` at runtime | Synchronous disk I/O; breaks on mobile | Use `Addressables` or serialize via Inspector |
| `Input.GetKey` / `Input.GetAxis` (legacy) | Legacy input system, to be removed | New Input System (`InputActionAsset`) |
| `NavMeshAgent` | 3D-only component | A\* Pathfinding Project |
| `Rigidbody` (3D) | 3D physics | `Rigidbody2D` |
| `Collider` (3D) | 3D physics | `Collider2D` |

### ❌ Anti-Patterns

```csharp
// ❌ WRONG — Don't use public fields
public float health;                // Exposed raw field — no encapsulation

// ✅ CORRECT
[SerializeField] private float _health;
public float Health => _health;

// ❌ WRONG — Logic in property getters that should be cached
public int EnemyCount => FindObjectsByType<EnemyController>(FindObjectsSortMode.None).Length;

// ✅ CORRECT — Track count via events
private int _enemyCount;
public int EnemyCount => _enemyCount;

// ❌ WRONG — Coroutine for every frame
private IEnumerator UpdateHungerEveryFrame()
{
    while (true)
    {
        UpdateHunger();
        yield return null; // Just use Update() instead!
    }
}

// ❌ WRONG — new allocation in Update
private void Update()
{
    var results = new List<Collider2D>(); // GC allocation every frame
    Physics2D.OverlapCircle(pos, radius, results);
}

// ✅ CORRECT — pre-allocated array (see Section 4.3)

// ❌ WRONG — Hardcoded strings for tags, layers
if (other.tag == "Player") { }
gameObject.layer = 8;

// ✅ CORRECT — use constants
if (other.CompareTag("Player")) { }
gameObject.layer = Layers.Player;
```

### ❌ Architecture Rules

- **No more than 1 Singleton per system domain.** If you're adding a 5th Singleton, you have a design problem.
- **Never call `SaveSystem.Save()` from a MonoBehaviour's `Update()`** — save triggers must be intentional (sleep, exit, autosave timer).
- **ScriptableObjects are read-only at runtime.** Never write to SO fields during gameplay.
- **No cross-domain direct calls** (e.g., `EnemyAI` directly calling `VitalsSystem.TakeDamage`). Use events/interfaces.
- **All `IEnumerator` Coroutines** must be stoppable — store the `Coroutine` reference and `StopCoroutine` in `OnDisable`.

---

*This document is the law for all C# code in Escape Days. When in doubt, write less code, not more.*
