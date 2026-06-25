# Escape Days — Artist & Designer Role Guide
> **Role:** 2D Artist, UI Designer, Level Designer  
> **Scope:** Asset creation standards, import settings, naming conventions, tilemap rules, UI guidelines, and sorting layers  
> **Last Updated:** 2025 | **Version:** 2.0 (2D)

---

## Table of Contents
1. [Art Direction Overview](#1-art-direction-overview)
2. [Asset Naming Conventions](#2-asset-naming-conventions)
3. [Sprite Import Settings](#3-sprite-import-settings)
4. [Texture & Atlas Settings](#4-texture--atlas-settings)
5. [Animation Standards](#5-animation-standards)
6. [Level Design — Tilemap Rules](#6-level-design--tilemap-rules)
7. [Sorting Layers & Order in Layer](#7-sorting-layers--order-in-layer)
8. [Lighting — URP 2D Light System](#8-lighting--urp-2d-light-system)
9. [UI Design Rules](#9-ui-design-rules)
10. [Audio Asset Standards](#10-audio-asset-standards)
11. [Do Not — Artist Hard Rules](#11-do-not--artist-hard-rules)

---

## 1. Art Direction Overview

### 1.1 Visual Style

**Escape Days** uses a **2D Top-Down perspective** with the following art direction targets:

| Attribute | Specification |
|---|---|
| **Art Style** | Gritty military survival — detailed, desaturated, realistic proportions |
| **Perspective** | Top-Down 2D (45° axonometric or strict vertical overhead — decide and lock early) |
| **Resolution** | Designed for 1920×1080 at 100 PPU base. Sprites scale to 1 world unit = 1 meter |
| **Pixel Art?** | [TEAM DECISION: Lock this early] If pixel art → Point filter, 16–32px characters. If HD → Bilinear, high-res sprites |
| **Color Palette** | Desaturated greens, grays, browns for world. Accent colors for UI vitals (red HP, blue water, etc.) |
| **Camera** | Orthographic. Size = 9 units (for 1080p at 100 PPU). No perspective distortion. |

### 1.2 Reference Games

| Game | What to Reference |
|---|---|
| **Hotline Miami** | Top-down 2D character proportions, readability |
| **Green Hell** | Environmental color grading, survival mood |
| **XCOM 2** (tactical map) | Sorting layer logic, character vs environment overlap |
| **Hades** | HUD restraint, information density |

---

## 2. Asset Naming Conventions

**All filenames use underscores, no spaces, no camelCase. Use prefixes to identify type at a glance.**

### 2.1 Sprite / Image Assets

| Asset Type | Prefix | Example |
|---|---|---|
| Sprite (character) | `SPR_CHAR_` | `SPR_CHAR_Player_Idle_00.png` |
| Sprite (enemy) | `SPR_ENEMY_` | `SPR_ENEMY_Soldier_Walk_01.png` |
| Sprite (companion) | `SPR_COMP_` | `SPR_COMP_Maya_Idle_00.png` |
| Sprite (item/icon) | `SPR_ITEM_` | `SPR_ITEM_MedKit.png` |
| Sprite (environment) | `SPR_ENV_` | `SPR_ENV_Tree_01.png` |
| Sprite (building) | `SPR_BUILD_` | `SPR_BUILD_Wall_Horizontal.png` |
| Sprite (VFX) | `SPR_VFX_` | `SPR_VFX_Explosion_00.png` |
| Tile (ground) | `TILE_GND_` | `TILE_GND_Grass_01.png` |
| Tile (wall) | `TILE_WALL_` | `TILE_WALL_Stone_01.png` |
| Tile (water) | `TILE_WTR_` | `TILE_WTR_Shallow_01.png` |
| UI element | `UI_` | `UI_HPBar_Fill.png` |
| UI icon | `UI_ICO_` | `UI_ICO_Hunger.png` |
| Background | `BG_` | `BG_ForestCanopy.png` |

### 2.2 Animation Assets

| Asset Type | Prefix | Example |
|---|---|---|
| Animator Controller | `AC_` | `AC_Player.controller` |
| Animation Clip | `ANIM_` | `ANIM_Player_Idle.anim` |
| Avatar Mask | `MASK_` | `MASK_UpperBody.mask` |

**Animation clip naming convention:**
```
ANIM_[Character]_[Action]_[Variant].anim

Examples:
  ANIM_Player_Idle.anim
  ANIM_Player_Walk.anim
  ANIM_Player_Sprint.anim
  ANIM_Player_MeleeAttack_01.anim
  ANIM_Player_Death.anim
  ANIM_Enemy_Soldier_Patrol.anim
  ANIM_Enemy_Soldier_Alert.anim
  ANIM_Enemy_Soldier_Death.anim
```

### 2.3 Prefab Assets

| Asset Type | Prefix | Example |
|---|---|---|
| Character prefab | `PFB_CHAR_` | `PFB_CHAR_Player.prefab` |
| Enemy prefab | `PFB_ENEMY_` | `PFB_ENEMY_Soldier.prefab` |
| Item pickup | `PFB_ITEM_` | `PFB_ITEM_MedKit.prefab` |
| VFX prefab | `PFB_VFX_` | `PFB_VFX_BloodSplatter.prefab` |
| Building piece | `PFB_BUILD_` | `PFB_BUILD_WoodWall.prefab` |
| UI panel | `PFB_UI_` | `PFB_UI_HUD.prefab` |
| Environment prop | `PFB_PROP_` | `PFB_PROP_BarrelRed.prefab` |

### 2.4 Audio Assets

| Asset Type | Prefix | Example |
|---|---|---|
| SFX (player) | `SFX_PLR_` | `SFX_PLR_Footstep_Grass_01.ogg` |
| SFX (combat) | `SFX_CMB_` | `SFX_CMB_GunShot_Rifle.ogg` |
| SFX (UI) | `SFX_UI_` | `SFX_UI_ButtonClick.ogg` |
| SFX (ambient) | `SFX_AMB_` | `SFX_AMB_Forest_Day_Loop.ogg` |
| SFX (enemy) | `SFX_ENM_` | `SFX_ENM_Alert_01.ogg` |
| Music (menu) | `MUS_MENU_` | `MUS_MENU_MainTheme.ogg` |
| Music (gameplay) | `MUS_GAME_` | `MUS_GAME_Combat_Tense.ogg` |

### 2.5 Data & Material Assets

| Asset Type | Prefix | Example |
|---|---|---|
| Material | `MAT_` | `MAT_Player_Shadow.mat` |
| Sprite Atlas | `ATLAS_` | `ATLAS_Player.spriteatlas` |
| Tile Palette | `PAL_` | `PAL_Forest_Ground.asset` |
| Tile Asset | `TILE_` | `TILE_GND_Grass_RuleTile.asset` |
| Physics Material 2D | `PHYS2D_` | `PHYS2D_Slippery.physicsMaterial2D` |

---

## 3. Sprite Import Settings

> Apply these settings in the Unity Inspector for each sprite or texture type. Consistent settings prevent bugs.

### 3.1 Character Sprites (Player, Enemy, Companion)

```
Texture Type:       Sprite (2D and UI)
Sprite Mode:        Single (if single sprite) / Multiple (if sprite sheet)
Pixels Per Unit:    100                    ← GLOBAL STANDARD. Must be 100 for all game sprites.
Pivot:              Bottom                  ← Pivot at feet for correct floor alignment
Filter Mode:        [See 3.4 — depends on art style]
Compression:        None                    ← Characters need full quality
Max Size:           512 (small chars), 1024 (large chars)
Generate Mipmaps:   ✗ OFF                  ← 2D game, no 3D perspective scaling needed
Read/Write:         ✗ OFF                  ← Unless runtime texture modification needed
```

### 3.2 Environment / World Sprites (Props, Decorations)

```
Texture Type:       Sprite (2D and UI)
Pixels Per Unit:    100
Pivot:              Center (default)
Filter Mode:        [See 3.4]
Compression:        Normal Quality
Max Size:           512
Generate Mipmaps:   ✗ OFF
```

### 3.3 UI Sprites (Icons, Bars, Panels)

```
Texture Type:       Sprite (2D and UI)
Pixels Per Unit:    100
Pivot:              Center
Filter Mode:        Bilinear                ← Always Bilinear for UI (pixel-perfect not needed)
Compression:        None                    ← UI must be crisp
Max Size:           512 (icons), 2048 (large panels)
Generate Mipmaps:   ✗ OFF
Mesh Type:          Full Rect (for 9-slice UI), Tight (for small icons)
```

### 3.4 Filter Mode Decision Table

**This must be decided once and applied consistently across all game sprites.**

| Art Style | Filter Mode | When to Use |
|---|---|---|
| **Pixel Art** | `Point (no filter)` | Character sprites are ≤ 64px wide. Crisp, no blur at any zoom. |
| **HD / Illustrated** | `Bilinear` | High-res sprites (256px+). Smooth scaling needed. |
| **UI Always** | `Bilinear` | Regardless of art style, UI sprites use Bilinear |

> ⚠️ **LOCK THIS DECISION EARLY.** Mixing Point and Bilinear on game sprites causes visual inconsistency that is very difficult to fix later.

### 3.5 Pixels Per Unit (PPU) — Golden Rule

```
ALL game sprites MUST use PPU = 100.

This means:
- A 100×100px sprite = 1×1 Unity world units
- A 200×100px sprite = 2×1 Unity world units
- Player sprite at 64×64px = 0.64×0.64 world units (scale in prefab if needed)

NEVER mix PPU values (e.g., some sprites at 16, others at 100).
This causes objects to appear at wildly wrong sizes in the scene.
```

---

## 4. Texture & Atlas Settings

### 4.1 Sprite Atlas (Required for Performance)

All character animation sprites MUST be packed into a `SpriteAtlas`. This reduces draw calls by batching sprites that share the same atlas.

**Atlas structure:**
```
Assets/_Project/Art/Sprites/
├── Characters/
│   ├── Player/
│   │   ├── ATLAS_Player.spriteatlas        ← Contains all player frames
│   │   ├── SPR_CHAR_Player_Idle_00.png
│   │   ├── SPR_CHAR_Player_Idle_01.png
│   │   └── ...
│   └── Enemy_Soldier/
│       ├── ATLAS_Enemy_Soldier.spriteatlas
│       └── ...
└── UI/
    ├── ATLAS_HUD.spriteatlas               ← All HUD elements in one atlas
    └── ATLAS_Icons.spriteatlas             ← All item icons in one atlas
```

**Atlas Settings:**
```
Include in Build:   ✓ ON
Allow Rotation:     ✗ OFF                  ← Rotation can break pivot alignment
Tight Packing:      ✓ ON (for irregular shapes), ✗ OFF for pixel art
Padding:            2px                    ← Prevents bleeding between sprites
Max Size:           2048                   ← Keep atlases under 2048×2048
Format:             RGBA32 (for sprites with transparency), RGB24 (no alpha)
```

---

## 5. Animation Standards

### 5.1 Required Animation States (Player)

Every character needs these **minimum animation states** in their Animator Controller:

| State Name | Condition | Loop |
|---|---|---|
| `Idle` | Default state | ✓ |
| `Walk` | `isMoving == true` | ✓ |
| `Sprint` | `isSprinting == true` | ✓ |
| `Crouch_Idle` | `isCrouching == true` | ✓ |
| `Crouch_Walk` | `isCrouching && isMoving` | ✓ |
| `MeleeAttack` | Trigger: `attackMelee` | ✗ |
| `Shoot` | Trigger: `attackRanged` | ✗ |
| `Hit` | Trigger: `takeDamage` | ✗ |
| `Death` | Trigger: `die` | ✗ |

### 5.2 Animator Parameter Naming

```csharp
// Use these EXACT parameter names in Animator and C# code
// Bool parameters
"isMoving"
"isSprinting"
"isCrouching"
"isAlive"

// Trigger parameters
"attackMelee"
"attackRanged"
"takeDamage"
"die"

// Float parameters
"moveSpeed"          // 0.0 to 1.0 normalized speed
"moveX"              // Horizontal component for 8-directional movement
"moveY"              // Vertical component for 8-directional movement
```

### 5.3 8-Directional Sprite Sheets

For top-down movement showing the character from multiple angles:

```
Frame layout for 8-directional walk:
Row 0: Walk South (toward camera)
Row 1: Walk Southwest
Row 2: Walk West
Row 3: Walk Northwest
Row 4: Walk North (away from camera)
Row 5: Walk Northeast
Row 6: Walk East
Row 7: Walk Southeast

Minimum frames per direction: 4 (can use flip for mirrored directions)
```

---

## 6. Level Design — Tilemap Rules

### 6.1 Tilemap Layer Structure

Each scene must have Tilemap objects organized in this hierarchy:

```
LevelRoot (GameObject)
└── Grid (Grid component)
    ├── TM_Ground          ← Terrain: grass, dirt, sand, stone floors
    │   └── Tilemap + TilemapRenderer (Sorting Layer: Ground, Order: 0)
    │
    ├── TM_Ground_Detail   ← Puddles, dirt patches, decals on ground
    │   └── Tilemap + TilemapRenderer (Sorting Layer: Ground, Order: 1)
    │
    ├── TM_Water           ← Water tiles (uses water shader)
    │   └── Tilemap + TilemapRenderer + TilemapCollider2D
    │   └── (Sorting Layer: Ground, Order: 2)
    │
    ├── TM_Obstacles       ← Low obstacles player can walk behind (bushes, crates)
    │   └── Tilemap + TilemapRenderer + TilemapCollider2D + CompositeCollider2D
    │   └── (Sorting Layer: Objects, Order: 0)
    │
    ├── TM_Walls           ← Solid walls, rocks, high obstacles (block movement)
    │   └── Tilemap + TilemapRenderer + TilemapCollider2D + CompositeCollider2D
    │   └── (Sorting Layer: Objects, Order: 10)
    │
    ├── TM_Roof            ← Building roofs (hidden when player is inside)
    │   └── Tilemap + TilemapRenderer (Sorting Layer: Roof, Order: 0)
    │
    └── TM_Overlay         ← Overlaid elements (footprints, blood decals)
        └── Tilemap + TilemapRenderer (Sorting Layer: Overlay, Order: 0)
```

### 6.2 Tilemap Collider Setup

For performance, always use **Composite Collider 2D** with `TilemapCollider2D`:

```
TilemapCollider2D settings:
  Used by Composite:    ✓ ON
  
CompositeCollider2D settings:
  Geometry Type:        Polygons (for irregular shapes), Outlines (for thin walls)
  Rigidbody2D:          Attached automatically (set to Static)
```

This merges all tile colliders into a single optimized polygon mesh, drastically reducing physics calculations.

### 6.3 Rule Tiles

Use **Rule Tiles** for any terrain type that auto-connects (grass, water edges, walls):

```
Assets/_Project/Art/Tilemaps/
├── RuleTiles/
│   ├── TILE_GND_Grass_RuleTile.asset
│   ├── TILE_WALL_Stone_RuleTile.asset
│   └── TILE_WTR_River_RuleTile.asset
└── Palettes/
    ├── PAL_Forest.asset
    └── PAL_Military_Base.asset
```

**Rule Tile naming:** `TILE_[Type]_[Name]_RuleTile.asset`

### 6.4 Level Design Principles

- **Readability First:** Player must always be able to distinguish walkable from non-walkable at a glance. Use value contrast (dark walls, lighter floors).
- **Cover Placement:** Every 8–12 tiles of open space should have at least 1 cover object (barrel, crate, low wall) for combat viability.
- **Resource Pacing:** Don't cluster resources. Spread critical resources (water sources, food nodes) with at least 60-tile gaps.
- **Lighting Sockets:** Plan positions for `Light2D` sources before building. Campfires, windows, torches should align with tilemap cells.
- **Enemy Patrol Paths:** Patrol waypoints should be placed on Tilemap grid intersections for A\* compatibility.

---

## 7. Sorting Layers & Order in Layer

This is the **single most important system** for visual correctness in top-down 2D. Define this once and never change it without team approval.

### 7.1 Sorting Layer Stack (Top to Bottom on Screen)

Configure in: **Edit → Project Settings → Tags and Layers → Sorting Layers**

```
Sorting Layer (rendered bottom to top):
──────────────────────────────────────
  1. Background       ← Sky, distant mountains, parallax backgrounds
  2. Ground           ← Terrain tiles (grass, dirt, stone, water)
  3. GroundDetail     ← Decals, puddles, tile overlays
  4. Shadow           ← Character drop shadows (rendered before characters)
  5. Objects          ← Props, crates, bushes, low obstacles
  6. Characters       ← Player, enemies, companions, NPCs
  7. Projectiles      ← Bullets, arrows, thrown items
  8. Effects          ← VFX particles (blood, fire, smoke at ground level)
  9. AboveObjects     ← Tall trees, building walls that overlap characters
 10. Roof             ← Building roofs (hidden via alpha when player inside)
 11. WeatherEffects   ← Rain, snow particles (above everything)
 12. UI_World         ← In-world UI (health bars, exclamation marks)
 13. UI               ← [Reserved — managed by Canvas, not SpriteRenderer]
──────────────────────────────────────
Higher number = rendered on top (closer to camera)
```

### 7.2 Order in Layer — Y-Axis Sorting (Most Important Rule)

For objects on the **same Sorting Layer** (Characters, Objects), use **Y-axis sorting** so that objects lower on the screen appear in front of objects higher on the screen.

**Setup (required):**
- In the Unity Project Settings → Graphics → Camera Settings → Transparency Sort Mode: **Custom Axis**
- Set Transparency Sort Axis to: `X=0, Y=1, Z=0`

This makes Unity automatically sort sprites by their Y position.

**Additionally, for characters and props, set `Sprite Renderer → Order in Layer`:**
```
Approach A — Automatic Y-Sort (Unity built-in):
  Enable "Transparency Sort Mode: Custom Axis" (above) and leave Order in Layer = 0.
  Unity handles it automatically.

Approach B — Dynamic Order via Script (for precise control):
  Set Order in Layer dynamically based on Y position:
```

```csharp
// YSortController.cs — attach to any sprite that needs Y-sorting
[RequireComponent(typeof(SpriteRenderer))]
public class YSortController : MonoBehaviour
{
    private SpriteRenderer _renderer;
    private const int YSortMultiplier = 100; // Converts world Y to sort integer

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    // FixedUpdate is fine since position changes are physics-driven
    private void LateUpdate()
    {
        // Invert Y: lower Y position = higher order (in front)
        _renderer.sortingOrder = -(int)(transform.position.y * YSortMultiplier);
    }
}
```

### 7.3 Quick Reference — What Goes Where

| Object | Sorting Layer | Order in Layer |
|---|---|---|
| Ground tiles | Ground | 0 |
| Water tiles | Ground | 2 |
| Ground decals (blood, mud) | GroundDetail | 0 |
| Player shadow | Shadow | 0 |
| Small props (crates, barrels) | Objects | Y-sorted |
| Player character | Characters | Y-sorted |
| Enemy character | Characters | Y-sorted |
| Companion character | Characters | Y-sorted |
| Bullets | Projectiles | 0 |
| Tree trunk (base) | Objects | Y-sorted |
| Tree canopy (top) | AboveObjects | 0 |
| Building wall (bottom) | Objects | Y-sorted |
| Building wall (top/overhang) | AboveObjects | 0 |
| Rain particles | WeatherEffects | 0 |
| Enemy HP bar | UI_World | 0 |
| Interact prompt | UI_World | 1 |

---

## 8. Lighting — URP 2D Light System

### 8.1 Global Light

Every scene must have exactly **1 Global Light 2D** as the ambient base:

```
Global Light 2D:
  Color:          (0.3, 0.3, 0.35, 1.0) — cool dark ambient for night
  Intensity:      0.15 at night, 0.9 at day
  Target Sorting Layers: All
  
DayNightCycle.cs controls Global Light intensity over time.
```

### 8.2 Point Lights (Campfire, Torches, Windows)

```
Point Light 2D settings for Campfire:
  Light Type:         Point
  Color:              (1.0, 0.6, 0.2) — warm orange
  Intensity:          1.5
  Inner Radius:       1.0
  Outer Radius:       4.0
  Falloff:            Enable Falloff Strength = 0.8
  Target Sorting Layers: Ground, Objects, Characters
  Shadow Caster:      Enable on walls and trees (Shadow Caster 2D component)
```

### 8.3 Spot Lights (Flashlights, Enemy Searchlights)

```
Spot Light 2D settings for Flashlight:
  Light Type:         Spot / Freeform (use Freeform for cone shape)
  Color:              (1.0, 1.0, 0.9) — cool white
  Intensity:          2.0
  Range:              8.0 units
  Inner Spot Angle:   20°
  Outer Spot Angle:   40°
  
Attach to Player or Enemy weapon socket. Rotate with aim direction.
```

### 8.4 Shadow Caster 2D Rules

- Add `Shadow Caster 2D` to: Walls, Trees, Buildings, Large Crates.
- Do NOT add `Shadow Caster 2D` to: Characters (they use a separate shadow sprite on the Shadow sorting layer), small props, ground tiles.
- Set `Cast Shadows` on each `Light2D` that should produce shadows.
- Enable `Self Shadows` only on lights that should illuminate the back of their own casting object.

---

## 9. UI Design Rules

### 9.1 HUD Architecture Decision

**Escape Days uses UI Toolkit (UXML/USS) for the main HUD.** UGUI Canvas is used only for in-world elements (health bars above enemies).

| UI Type | System | Reason |
|---|---|---|
| Main HUD (vitals, inventory shortcut) | **UI Toolkit** | Retained mode, CSS-like styling, performant |
| Pause / Main Menu | **UI Toolkit** | Consistent with HUD |
| In-world health bars | **UGUI Canvas** (World Space) | Follows enemy position in world |
| Damage numbers | **UGUI Canvas** (Screen Space) | Easier animation with DOTween |

### 9.2 UI Toolkit (USS) Styling Rules

**File locations:**
```
Assets/_Project/UI/
├── UXML/
│   ├── HUD_Main.uxml           ← Root HUD document
│   ├── HUD_Vitals.uxml         ← Vitals panel (HP, Hunger, etc.)
│   ├── Menu_Pause.uxml
│   └── Menu_Main.uxml
└── USS/
    ├── Variables.uss           ← CSS custom properties (colors, font sizes)
    ├── Common.uss              ← Shared utility classes
    ├── HUD.uss                 ← HUD-specific styles
    └── Menus.uss               ← Menu-specific styles
```

**Variables.uss (Required — define all design tokens here):**
```css
:root {
    /* Colors */
    --color-hp:         rgb(220, 50, 50);
    --color-stamina:    rgb(50, 200, 100);
    --color-hunger:     rgb(210, 150, 50);
    --color-thirst:     rgb(80, 160, 220);
    --color-sleep:      rgb(150, 100, 200);
    --color-bg-panel:   rgba(0, 0, 0, 0.6);
    --color-text-main:  rgb(240, 235, 220);
    --color-text-dim:   rgb(160, 155, 145);

    /* Spacing */
    --spacing-xs:   4px;
    --spacing-sm:   8px;
    --spacing-md:   16px;
    --spacing-lg:   24px;

    /* Typography */
    --font-size-sm:   12px;
    --font-size-md:   16px;
    --font-size-lg:   24px;
    --font-size-xl:   32px;

    /* Border */
    --border-radius-sm: 4px;
    --border-radius-md: 8px;
}
```

**USS Naming Convention (BEM-style):**
```css
/* Block */
.vitals-panel { }

/* Element */
.vitals-panel__bar { }
.vitals-panel__icon { }
.vitals-panel__label { }

/* Modifier */
.vitals-panel__bar--critical { }
.vitals-panel__bar--full { }
```

### 9.3 HUD Vitals Layout

The HUD vitals display must follow this order (customizable, but consistent):

```
[Top-Left Corner]
  [HP Bar]      ████████████████░░░░  85/100
  [Stamina]     ████████████░░░░░░░░  60/100
  [Hunger]      ██████████░░░░░░░░░░  50/100
  [Thirst]      ████████████████████ 100/100
  [Sleep]       ████████████████░░░░  80/100

[Top-Right Corner]
  [Companion portrait + morale bar]

[Bottom-Center]
  [Hotbar: 6 inventory slots]

[Bottom-Left]
  [Mini-map — optional, design phase]
```

**Design Rules:**
- Bar fill color changes at 25% critical threshold (pulse animation).
- Icons to the left of each bar use `UI_ICO_` sprites.
- Bars use 9-slice `background-image` for scalable fill.
- Never show raw numbers to the player — bars only. (Green Hell style: you feel the depletion, not read it.)

### 9.4 UGUI Canvas Rules (In-World Elements)

For in-world UI (enemy health bars):

```
Canvas Settings:
  Render Mode:        World Space
  Event Camera:       None (no interaction needed)
  Dynamic Pixels Per Unit: 1

Canvas Scaler:
  (Not applicable in World Space)

Enemy Health Bar prefab structure:
  WorldUI_EnemyHealth (Canvas - World Space)
  └── BarBackground (Image, anchored center)
      └── BarFill (Image, Image Type: Filled, Fill Method: Horizontal)

Size:       0.8 × 0.1 world units
Position:   0.5 units above enemy sprite pivot
Sorting Layer: UI_World, Order: 0
```

### 9.5 Font Rules

```
Approved Fonts:
  Primary:    [Project font choice — e.g., "Share Tech Mono" for military aesthetic]
  Secondary:  [Readable sans-serif for body text]
  
Font Asset Creation:
  - Create TextMeshPro font assets from .ttf source files
  - Naming: FONT_[Name]_[Weight].asset (e.g., FONT_ShareTechMono_Regular.asset)
  - Store in: Assets/_Project/Art/UI/Fonts/
  
Size Hierarchy:
  XL (32px):  Section headers
  LG (24px):  Panel titles
  MD (16px):  Body / labels
  SM (12px):  Tooltips / metadata
```

---

## 10. Audio Asset Standards

### 10.1 Import Settings by Type

| Audio Type | Format | Load Type | Compression | Quality |
|---|---|---|---|---|
| Short SFX (< 1 sec) | `.ogg` | Decompress on Load | Vorbis | 70% |
| Medium SFX (1–5 sec) | `.ogg` | Compressed in Memory | Vorbis | 70% |
| Long ambient / music | `.ogg` | Streaming | Vorbis | 80% |
| Footstep variations | `.ogg` | Decompress on Load | Vorbis | 60% |

**Rule:** Never import `.wav` files directly into Unity without conversion to `.ogg` first. Raw WAV files are uncompressed and bloat build size.

### 10.2 Footstep System

Footstep sounds must have **minimum 3 variations per surface** to avoid repetition:

```
SFX_PLR_Footstep_Grass_01.ogg
SFX_PLR_Footstep_Grass_02.ogg
SFX_PLR_Footstep_Grass_03.ogg
SFX_PLR_Footstep_Concrete_01.ogg
SFX_PLR_Footstep_Concrete_02.ogg
SFX_PLR_Footstep_Concrete_03.ogg
SFX_PLR_Footstep_Water_01.ogg
...
```

### 10.3 Audio Mixer Structure

```
Master Mixer
├── Music Bus           ← All music tracks
├── SFX Bus
│   ├── Player SFX
│   ├── Enemy SFX
│   ├── Ambient SFX
│   └── UI SFX
└── Voice Bus           ← Companion / NPC dialogue (future)
```

Each bus has its own Volume parameter exposed (e.g., `MusicVolume`, `SFXVolume`) for player settings.

---

## 11. Do Not — Artist Hard Rules

### ❌ Import Rules

| Forbidden | Correct Action |
|---|---|
| Sprites with inconsistent PPU (e.g., mix of 16, 32, 100) | Set ALL game sprites to PPU = 100 |
| Raw `.psd` files committed without LFS tracking | Track with Git LFS; export to `.png` for non-reference sprites |
| Sprites larger than 2048×2048px | Split into sub-sprites or use SpriteAtlas |
| Uncompressed `.wav` audio committed | Convert to `.ogg` at appropriate quality |
| `Generate Mipmaps: ON` for 2D sprites | Always OFF — mipmaps are for 3D textures |
| `Filter Mode: Point` on UI sprites | Always use Bilinear for UI |
| `Filter Mode: Bilinear` on pixel art game sprites | Always use Point for pixel art |

### ❌ Naming Rules

| Forbidden | Correct |
|---|---|
| `sprite1.png`, `image_final.png`, `sprite_FINAL_v2_USE_THIS.png` | `SPR_CHAR_Player_Idle_00.png` |
| Spaces in filenames: `Player Walk.png` | `SPR_CHAR_Player_Walk.png` |
| CamelCase filenames: `playerWalk.png` | Use underscores: `SPR_CHAR_Player_Walk.png` |
| Assets outside `Assets/_Project/` | All custom assets in `_Project/` subdirectories |

### ❌ Sorting Layer Rules

| Forbidden | Reason |
|---|---|
| Using `Order in Layer` manually as a substitute for the correct Sorting Layer | Creates fragile, hard-to-maintain ordering |
| Characters on the `Ground` sorting layer | Characters must be on `Characters` layer for Y-sorting to work |
| Setting `Order in Layer` to large arbitrary numbers (e.g., 9999) | Use the defined layer system; large numbers indicate wrong approach |
| Placing all sprites on the `Default` sorting layer | Default layer is forbidden. Assign the correct layer from the list |
| `Shadow Caster 2D` on characters | Characters use a sprite shadow on the Shadow sorting layer, not shadow casters |

### ❌ Scene Rules

| Forbidden | Correct Action |
|---|---|
| Placing raw sprites directly in scene without a Prefab | Create Prefab first, then place in scene |
| Building scenes without the Grid/Tilemap hierarchy | Always use the defined Tilemap layer structure |
| Missing references in scene (pink sprites, broken prefab links) | Resolve before committing the scene file |
| Committing scenes with `TilemapCollider2D` not set to "Used by Composite" | Always pair with CompositeCollider2D |

### ❌ UI Rules

| Forbidden | Reason |
|---|---|
| Hardcoding color values directly in USS (e.g., `color: rgb(220, 50, 50)`) | Use CSS variables from `Variables.uss` |
| Using pixel values for layout that should be percentage-based | Breaks at non-standard resolutions |
| Creating UI with UGUI Canvas for elements that should be in UI Toolkit | HUD and menus use UI Toolkit; only in-world elements use UGUI |
| Showing raw number values for vitals to the player | Vitals are bars only (no numbers shown to player) |

---

*This guide is the visual and design standard for Escape Days. Consistency in assets = consistency in the game world. When in doubt, ask before importing.*
