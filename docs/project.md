# Escape Days — Project Documentation
> **Role:** All Team Members | **Scope:** Game Design Document & Technical Overview  
> **Last Updated:** 2025 | **Version:** 2.0 (2D Pivot)

---

## Table of Contents
1. [Project Overview](#1-project-overview)
2. [Game Design Document (GDD)](#2-game-design-document-gdd)
3. [Tech Stack](#3-tech-stack)
4. [Features Checklist](#4-features-checklist)
5. [Project Structure (Root)](#5-project-structure-root)
6. [Milestone Roadmap](#6-milestone-roadmap)

---

## 1. Project Overview

**Game Title:** Escape Days  
**Genre:** 2D Top-Down Survival  
**Platform (Target):** PC (Windows/Linux), with potential mobile port  
**Engine:** Unity 6 LTS  
**Team Size:** [Define]  
**Repository:** [Your Git URL Here]

### Synopsis

> *"You are a soldier — abandoned, wounded, and hunted. Surrounded by hostile terrain and enemies who won't stop until you're dead. Your only objective: survive long enough to escape."*

**Escape Days** is a **2D Top-Down Survival** game where the player controls a stranded soldier navigating a hostile open-world environment. The core experience blends:

- **Green Hell-style survival mechanics:** Every second matters. Manage hunger, thirst, body temperature, stamina, and sleep deprivation. Ignore your vitals and you will die — slowly, painfully, and alone.
- **Far Cry-style Companion System:** Recruit and command AI companions discovered in the field. Each companion has unique skills (medic, scout, heavy weapons). They follow orders, provide cover fire, and can be lost permanently if you fail to protect them.
- **Tactical 2D Top-Down Combat:** Real-time combat with noise detection, line-of-sight stealth, and weapon management. Enemies patrol, investigate disturbances, and call for backup.

### Core Pillars

| Pillar | Description |
|---|---|
| **Desperate Survival** | The world is trying to kill you through biology as much as bullets |
| **Meaningful Relationships** | Companions have names, traits, and permadeath — losing them matters |
| **Tactical Decisions** | Every encounter can be avoided, ambushed, or negotiated around |
| **Earned Progression** | No level-ups. Survival skill comes from player knowledge, not stats |

---

## 2. Game Design Document (GDD)

### 2.1 Core Gameplay Loop

```
[Explore World]
      │
      ▼
[Gather Resources] ──► [Manage Vitals]
      │                       │
      ▼                       ▼
[Craft / Build Base] ◄── [Rest & Recover]
      │
      ▼
[Engage or Evade Enemies]
      │
      ▼
[Unlock New Areas / Find Escape Route]
      │
      ▼
[ESCAPE] ──► Victory
```

### 2.2 Vitals System

The player has **5 core vitals** displayed on the HUD. Each vital degrades over time and has cascading effects when critically low.

| Vital | Range | Critical Effect | Depletion Rate |
|---|---|---|---|
| **Health (HP)** | 0–100 | Death at 0 | From damage only |
| **Stamina** | 0–100 | Cannot sprint/dodge below 10 | Regens when idle |
| **Hunger** | 0–100 | -HP regen, stamina penalty below 20 | -1/min baseline |
| **Thirst** | 0–100 | Debuffs stack faster than hunger; death at 0 after 3 in-game days | -2/min baseline |
| **Sleep** | 0–100 | Hallucinations, reduced accuracy, stamina cap reduced | -1/min (accelerates at night) |

**Design Rule:** Vitals must feel like a *clock*, not a punishment. Players should feel urgency, not frustration.

### 2.3 Companion System

- **Recruitment:** Companions are found in the world — injured, trapped, or hiding. Player must rescue them.
- **Permadeath:** Companions who are downed can be revived within a 30-second window. If not revived, they die permanently.
- **Commands:** Follow Me, Hold Position, Attack Target, Scout Ahead, Loot Area.
- **Companion Stats:** Each companion has: `Health`, `Morale`, `Specialization` (Medic / Scout / Fighter).
- **Morale System:** Morale degrades when companions witness deaths or player abandons them. Low morale = disobedience.

### 2.4 Combat System (2D Top-Down)

- **Melee:** Short-range, high stamina cost. Weapons include knife, machete, improvised clubs.
- **Ranged:** Line-of-sight projectile system. Ammo is scarce. Shooting generates noise.
- **Stealth:** Enemy sight cones (2D raycasts). Crouching reduces noise and sight profile.
- **AI Behavior:** Patrol → Investigate → Alert → Attack → Retreat (if outnumbered).

### 2.5 Crafting System

Crafting uses a **recipe + workbench** model. No magic inventory crafting — player must be near a campfire or workbench.

```
Resources (Sticks, Stones, Cloth, Metal Parts)
    │
    ▼
[Campfire / Workbench Proximity]
    │
    ▼
[Crafting Menu → Select Recipe]
    │
    ▼
[Output Item in Inventory]
```

**Crafting Categories:** Tools, Weapons, Medicine, Food/Water, Base Components.

### 2.6 Base Building (2D Grid System)

- Building uses a **grid-snapped placement system** rendered via Tilemaps or a custom 2D grid.
- Structures: Walls, Floors, Doors, Storage Boxes, Campfire, Watchtower, Traps.
- Structures have durability and can be destroyed by enemies during raids.
- **Raid System:** At set intervals (or triggered by player actions), enemy squads attack the base.

---

## 3. Tech Stack

> All technology choices are finalized for the **2D Top-Down** architecture. Do NOT use 3D components.

### 3.1 Engine & Runtime

| Component | Technology | Version / Notes |
|---|---|---|
| **Engine** | Unity | 6 LTS (Unity 6000.x) |
| **Language** | C# | .NET Standard 2.1 |
| **Render Pipeline** | Universal Render Pipeline (URP) — **2D Renderer** | URP 17.x |
| **Target Resolution** | 1920×1080 (16:9 base), scalable | |

### 3.2 2D-Specific Systems

| System | Unity Feature | Notes |
|---|---|---|
| **Physics** | `Physics2D`, `Rigidbody2D`, `BoxCollider2D`, `CircleCollider2D`, `PolygonCollider2D` | Zero use of 3D Physics |
| **Rendering** | `SpriteRenderer`, `SpriteMask` | All visuals are sprites |
| **Lighting** | URP 2D Lights (`Light2D`), Shadow Caster 2D | Dynamic night/day lighting |
| **Level / Map** | Unity Tilemap System (`Tilemap`, `TilemapRenderer`, `TilemapCollider2D`) | Grid-based world |
| **Pathfinding** | **A\* Pathfinding Project (Free/Pro)** by Aron Granberg | No NavMesh — Unity NavMesh is 3D only |
| **Animation** | `Animator`, `AnimationClip`, `SpriteAtlas` | 2D skeletal optional via PSD Importer |
| **Input** | Unity Input System (new) — `InputActionAsset` | No legacy `Input.GetKey` |
| **UI** | **UI Toolkit** (USS + UXML) for HUD; **UGUI Canvas** for in-world elements | |
| **Audio** | Unity Audio Mixer | FMOD optional for advanced needs |
| **Saving** | JSON serialization (`JsonUtility` / `Newtonsoft.Json`) | Saved to `Application.persistentDataPath` |

### 3.3 Third-Party Packages (Approved)

| Package | Purpose | Source |
|---|---|---|
| A\* Pathfinding Project | 2D enemy pathfinding with grid graphs | [arongranberg.com](https://arongranberg.com/astar/) |
| DOTween (Free) | Smooth UI/world transitions and tweens | Asset Store |
| Newtonsoft.Json (Unity) | Robust JSON save/load | Unity Package Manager |
| PSD Importer | Photoshop layer → Sprite breakdown | Unity Package Manager |
| 2D Pixel Perfect (URP) | Maintain crisp pixel art at any resolution | Unity Package Manager |

### 3.4 Development Tools

| Tool | Purpose |
|---|---|
| **Unity Editor 6 LTS** | Primary development environment |
| **JetBrains Rider / VS Code** | C# IDE |
| **Git + Git LFS** | Version control (see `agen.md`) |
| **GitHub Actions** | CI/CD pipeline |
| **Unity Test Framework** | Edit Mode & Play Mode testing |
| **Aseprite / Photoshop** | 2D art creation |
| **Tiled (optional)** | External tilemap editor (exports to Unity) |

---

## 4. Features Checklist

Use this as the single source of truth for feature status. Update during sprint planning.

### 4.1 Core Vitals System
- [ ] HP system with damage intake and healing
- [ ] Stamina — sprint drain, regen logic
- [ ] Hunger — depletion over time, food consumption
- [ ] Thirst — depletion over time, water sources
- [ ] Sleep — fatigue meter, sleep action at campfire/shelter
- [ ] Vital cascade effects (debuffs when critical)
- [ ] HUD display for all 5 vitals (UI Toolkit)

### 4.2 Player
- [ ] 2D Top-Down movement with `Rigidbody2D`
- [ ] Sprinting (stamina drain)
- [ ] Crouching (reduced noise/sight)
- [ ] Inventory system (grid or slot-based)
- [ ] Equipment slots (weapon, armor, tool)
- [ ] Interaction system (press E to interact with world objects)

### 4.3 Combat
- [ ] Melee attack with hitbox (`Physics2D.OverlapCircle`)
- [ ] Ranged attack — projectile with `Rigidbody2D` velocity
- [ ] Object Pooling for projectiles
- [ ] Noise system (sound radius triggers AI)
- [ ] Stealth — 2D sight cone raycast
- [ ] Player death and respawn/game-over logic

### 4.4 AI & Enemies
- [ ] A\* Pathfinding integration (grid graph)
- [ ] Enemy states: Patrol, Investigate, Alert, Attack, Retreat
- [ ] Sight cone detection (2D raycast)
- [ ] Noise-based detection
- [ ] Enemy object pooling
- [ ] Group coordination (reinforcement calls)

### 4.5 Companion System
- [ ] Companion recruit / find flow
- [ ] Companion AI (follow, hold, attack commands)
- [ ] Companion health & morale stats
- [ ] Permadeath + revive window
- [ ] Companion HUD indicators

### 4.6 Crafting
- [ ] Resource item definitions (`ScriptableObject`)
- [ ] Recipe definitions (`ScriptableObject`)
- [ ] Crafting menu UI
- [ ] Workbench / campfire proximity check
- [ ] Output item generation

### 4.7 Base Building
- [ ] 2D grid system for placement
- [ ] Ghost preview on placement
- [ ] Structure placement / destruction
- [ ] Storage box interaction
- [ ] Raid system trigger

### 4.8 World & Level
- [ ] Tilemap world (ground, walls, water layers)
- [ ] Sorting layer setup (see `role-artist-designer.md`)
- [ ] Day/Night cycle (`Light2D` global intensity)
- [ ] Environmental hazards (poison plants, traps)
- [ ] World resource nodes (berry bush, water puddle, scrap pile)

### 4.9 Save/Load
- [ ] SaveData struct with all persistent state
- [ ] JSON serialization to persistent path
- [ ] Auto-save trigger (on sleep / on exit)
- [ ] Load on game start

### 4.10 Audio
- [ ] Footstep system (surface-dependent)
- [ ] Ambient soundscapes (day/night)
- [ ] Combat SFX
- [ ] UI SFX

---

## 5. Project Structure (Root)

```
EscapeDays/                         ← Unity project root
├── Assets/
│   └── _Project/                   ← ALL custom project files go here
│       ├── Art/
│       │   ├── Sprites/
│       │   │    ├── Map
│       │   │    ├── Karakter
│       │   │    ├── Weapon
│       │   │    ├── Pohon
│       │   ├── Animations/
│       │   ├── Tilemaps/
│       │   └── UI/
│       ├── Audio/
│       │   ├── Music/
│       │   └── SFX/
│       ├── Data/                   ← ScriptableObject assets (.asset files)
│       ├── Prefabs/
│       ├── Scenes/
│       ├── Scripts/                ← See role-programmer.md for sub-structure
│       ├── Settings/               ← URP, Input Actions, Audio Mixer
│       └── UI/                     ← UXML / USS files
├── Packages/
│   └── manifest.json
├── ProjectSettings/
├── .gitignore
├── .gitattributes                  ← Git LFS tracking rules
└── Docs/
    ├── project.md                  ← THIS FILE
    ├── agen.md
    ├── role-programmer.md
    └── role-artist-designer.md
```

**Rule:** No custom files outside `Assets/_Project/`. Third-party assets go in `Assets/Plugins/` or `Assets/[PackageName]/`.

---

## 6. Milestone Roadmap

| Milestone | Deliverable | Target |
|---|---|---|
| **M0 — Foundation** | Project setup, tech stack configured, empty scene with player movement | Week 1–2 |
| **M1 — Prototype** | Player movement, basic combat, 1 enemy type, tilemap world | Week 3–5 |
| **M2 — Core Loop** | All 5 vitals, crafting (3 recipes), campfire rest, basic save/load | Week 6–9 |
| **M3 — AI & World** | Enemy AI with A\*, companion recruit, day/night cycle, world resources | Week 10–13 |
| **M4 — Systems Complete** | Base building, raid system, full companion system, full crafting table | Week 14–18 |
| **M5 — Polish & Ship** | Art pass, audio, UI polish, performance profiling, playtesting | Week 19–24 |

---

*This document is the source of truth for all team members. Any architectural changes must be reflected here first.*
