# Escape Days — Agent & DevOps Rules
> **Role:** AI Assistants (Cursor, Claude Code), DevOps, All Developers  
> **Scope:** Git workflow, CI/CD, testing standards, and global "do not" rules  
> **Last Updated:** 2025 | **Version:** 2.0

---

## Table of Contents
1. [AI Assistant Rules](#1-ai-assistant-rules)
2. [Repository Structure](#2-repository-structure)
3. [Git Workflow & Branch Rules](#3-git-workflow--branch-rules)
4. [Commit Message Standard](#4-commit-message-standard)
5. [Git LFS Configuration](#5-git-lfs-configuration)
6. [CI/CD Pipeline](#6-cicd-pipeline)
7. [Testing Standards](#7-testing-standards)
8. [Global "Do Not" Rules](#8-global-do-not-rules)

---

## 1. AI Assistant Rules

> These rules apply to any AI coding assistant (Cursor AI, Claude Code, GitHub Copilot) operating on this codebase.

### 1.1 Context Loading Order

When starting a new task, the AI assistant MUST read documents in this order:

```
1. project.md          → Understand what the game is and its 2D architecture
2. agen.md             → Understand workflow rules (this file)
3. role-programmer.md  → If writing C# code
4. role-artist-designer.md → If working with assets, scenes, or UI
```

### 1.2 AI Behavior Rules

**ALWAYS:**
- Use 2D Unity components exclusively. This is a **2D Top-Down** game.
- Follow C# naming conventions defined in `role-programmer.md`.
- Prefer `ScriptableObject` for data, `UnityEvent` or C# events for communication.
- Write code in the correct folder as defined in `role-programmer.md`.
- Ask for clarification before making architectural changes (adding new managers, changing save structure).
- Add `// TODO: [reason]` comments for incomplete sections rather than leaving empty methods.

**NEVER:**
- Use any 3D component: `Rigidbody`, `Collider`, `NavMeshAgent`, `MeshRenderer`, `Camera` (perspective).
- Use `FindObjectOfType<T>()` — this is banned. Use dependency injection or Singleton pattern.
- Use `GetComponent<T>()` inside `Update()` or `FixedUpdate()`.
- Use `Camera.main` inside `Update()`.
- Create `MonoBehaviour` scripts without a corresponding folder according to project structure.
- Modify `ProjectSettings/` files without explicit instruction.
- Edit `.unity` scene files directly via text — always use the Unity Editor.

### 1.3 When to Stop and Ask

The AI assistant must **stop and ask the developer** before:
- Refactoring a system that touches more than 3 files.
- Changing any public API (method signatures on manager classes).
- Adding new third-party packages not listed in `project.md`.
- Modifying the save data schema (`SaveData.cs`).
- Creating new scenes.

---

## 2. Repository Structure

```
EscapeDays/ (Unity project root)
├── Assets/
│   └── _Project/         ← All custom project content
├── Packages/
├── ProjectSettings/
├── .gitignore
├── .gitattributes         ← Git LFS rules (see Section 5)
├── .github/
│   └── workflows/
│       ├── unity-test.yml
│       └── unity-build.yml
└── Docs/
    ├── project.md
    ├── agen.md
    ├── role-programmer.md
    └── role-artist-designer.md
```

### .gitignore (Required Entries)

```gitignore
# Unity generated
[Ll]ibrary/
[Tt]emp/
[Oo]bj/
[Bb]uild/
[Bb]uilds/
[Ll]ogs/
[Uu]ser[Ss]ettings/
MemoryCaptures/
Recordings/

# JetBrains Rider / Visual Studio
.idea/
.vs/
*.csproj
*.sln

# OS
.DS_Store
Thumbs.db

# Never commit local user settings
Assets/Settings/Resources/
```

---

## 3. Git Workflow & Branch Rules

### 3.1 Branch Hierarchy

```
main
  └── develop
        ├── feature/player-movement
        ├── feature/vitals-system
        ├── feature/enemy-ai
        ├── bugfix/hunger-not-depleting
        └── hotfix/critical-save-corruption   ← branches from main directly
```

### 3.2 Branch Naming Convention

| Type | Pattern | Example |
|---|---|---|
| Feature | `feature/<short-description>` | `feature/companion-morale` |
| Bug Fix | `bugfix/<short-description>` | `bugfix/stamina-regen-overflow` |
| Hotfix (prod) | `hotfix/<short-description>` | `hotfix/save-null-ref` |
| Experiment | `experiment/<short-description>` | `experiment/procedural-tilemap` |
| Release | `release/<version>` | `release/0.3.0` |

### 3.3 Branch Rules

- `main` — **Protected.** Only accepts merges from `release/*` or `hotfix/*`. Requires PR + 1 reviewer approval.
- `develop` — **Protected.** Only accepts merges via Pull Request from `feature/*` or `bugfix/*`. Requires PR + 1 reviewer.
- `feature/*` — Created from `develop`. Deleted after merge.
- `hotfix/*` — Created from `main`. Merged into both `main` AND `develop`.

### 3.4 Merge Strategy

- Use **Squash and Merge** for `feature/*` → `develop` (keeps develop history clean).
- Use **Merge Commit** for `release/*` → `main` (preserves release boundary).
- **Never use rebase on shared branches** (`develop`, `main`).

---

## 4. Commit Message Standard

Follow the **Conventional Commits** specification.

### Format

```
<type>(<scope>): <short summary in imperative mood>

[Optional body: explain WHY, not WHAT]

[Optional footer: BREAKING CHANGE / Refs #issue]
```

### Types

| Type | When to Use |
|---|---|
| `feat` | A new feature or game mechanic |
| `fix` | A bug fix |
| `refactor` | Code restructuring without behavior change |
| `perf` | Performance improvement |
| `test` | Adding or updating tests |
| `docs` | Documentation changes only |
| `art` | Art asset additions or updates |
| `audio` | Audio asset additions or updates |
| `chore` | Build scripts, CI, dependency updates |
| `scene` | Unity scene additions or layout changes |

### Scopes (Common)

`player`, `enemy`, `companion`, `vitals`, `crafting`, `building`, `combat`, `ai`, `save`, `ui`, `tilemap`, `audio`, `camera`, `input`

### Examples

```
feat(vitals): add sleep deprivation debuff stack system

fix(enemy): fix patrol path NaN when waypoint list is empty

refactor(player): extract movement logic into PlayerMovementController

perf(combat): replace FindObjectOfType with cached reference in BulletPool

art(tilemap): add forest biome tile palette assets

test(vitals): add unit tests for hunger depletion rate calculation

docs: update agen.md with new branch protection rules

chore: upgrade A* Pathfinding Project to 5.1.4
```

### Rules

- Summary line: **max 72 characters**, lowercase, no period at end.
- Use **imperative mood** ("add", not "added" or "adding").
- Reference issues: `Refs #42` or `Closes #17` in footer.
- Never write `wip`, `stuff`, `fix`, or `update` as the entire message.

---

## 5. Git LFS Configuration

All binary asset files must be tracked via **Git LFS**. Add these rules to `.gitattributes`:

```gitattributes
# ── Sprites & Textures ────────────────────────────────
*.png filter=lfs diff=lfs merge=lfs -text
*.psd filter=lfs diff=lfs merge=lfs -text
*.aseprite filter=lfs diff=lfs merge=lfs -text
*.jpg filter=lfs diff=lfs merge=lfs -text
*.jpeg filter=lfs diff=lfs merge=lfs -text
*.tga filter=lfs diff=lfs merge=lfs -text
*.bmp filter=lfs diff=lfs merge=lfs -text
*.webp filter=lfs diff=lfs merge=lfs -text
*.gif filter=lfs diff=lfs merge=lfs -text
*.svg filter=lfs diff=lfs merge=lfs -text

# ── Sprite Atlas ──────────────────────────────────────
*.spriteatlas filter=lfs diff=lfs merge=lfs -text

# ── Audio ─────────────────────────────────────────────
*.wav filter=lfs diff=lfs merge=lfs -text
*.mp3 filter=lfs diff=lfs merge=lfs -text
*.ogg filter=lfs diff=lfs merge=lfs -text
*.flac filter=lfs diff=lfs merge=lfs -text
*.aif filter=lfs diff=lfs merge=lfs -text

# ── Video ─────────────────────────────────────────────
*.mp4 filter=lfs diff=lfs merge=lfs -text
*.mov filter=lfs diff=lfs merge=lfs -text

# ── Unity Binaries ────────────────────────────────────
*.unitypackage filter=lfs diff=lfs merge=lfs -text
*.asset filter=lfs diff=lfs merge=lfs -text

# ── Fonts ─────────────────────────────────────────────
*.ttf filter=lfs diff=lfs merge=lfs -text
*.otf filter=lfs diff=lfs merge=lfs -text

# ── Text files: normal Git diff ───────────────────────
*.cs text eol=lf
*.md text eol=lf
*.json text eol=lf
*.yaml text eol=lf
*.yml text eol=lf
*.uxml text eol=lf
*.uss text eol=lf
*.asmdef text eol=lf
*.unity merge=unityyamlmerge eol=lf
*.prefab merge=unityyamlmerge eol=lf

# ── Unity Scene / Prefab smart merge ─────────────────
*.unity -text merge=unityyamlmerge
*.prefab -text merge=unityyamlmerge
*.asset -text merge=unityyamlmerge
```

**Setup LFS after cloning:**
```bash
git lfs install
git lfs pull
```

---

## 6. CI/CD Pipeline

### 6.1 GitHub Actions — Test Runner

File: `.github/workflows/unity-test.yml`

```yaml
name: Unity Tests

on:
  pull_request:
    branches: [develop, main]
  push:
    branches: [develop]

jobs:
  test:
    name: Run Unity Tests
    runs-on: ubuntu-latest
    steps:
      - name: Checkout repository
        uses: actions/checkout@v4
        with:
          lfs: true

      - name: Cache Unity Library
        uses: actions/cache@v3
        with:
          path: Library
          key: Library-${{ hashFiles('Assets/**', 'Packages/**', 'ProjectSettings/**') }}
          restore-keys: Library-

      - name: Run Edit Mode & Play Mode Tests
        uses: game-ci/unity-test-runner@v4
        env:
          UNITY_LICENSE: ${{ secrets.UNITY_LICENSE }}
          UNITY_EMAIL: ${{ secrets.UNITY_EMAIL }}
          UNITY_PASSWORD: ${{ secrets.UNITY_PASSWORD }}
        with:
          unityVersion: 6000.0.23f1  # Update to actual LTS version
          testMode: all
          artifactsPath: test-results

      - name: Upload Test Results
        uses: actions/upload-artifact@v4
        if: always()
        with:
          name: test-results
          path: test-results
```

### 6.2 GitHub Actions — Headless Build

File: `.github/workflows/unity-build.yml`

```yaml
name: Unity Build

on:
  push:
    branches: [main]
  workflow_dispatch:

jobs:
  build:
    name: Build for ${{ matrix.targetPlatform }}
    runs-on: ubuntu-latest
    strategy:
      matrix:
        targetPlatform:
          - StandaloneWindows64
          - StandaloneLinux64

    steps:
      - name: Checkout repository
        uses: actions/checkout@v4
        with:
          lfs: true

      - name: Cache Unity Library
        uses: actions/cache@v3
        with:
          path: Library
          key: Library-${{ matrix.targetPlatform }}-${{ hashFiles('Assets/**', 'Packages/**', 'ProjectSettings/**') }}

      - name: Build Unity project
        uses: game-ci/unity-builder@v4
        env:
          UNITY_LICENSE: ${{ secrets.UNITY_LICENSE }}
          UNITY_EMAIL: ${{ secrets.UNITY_EMAIL }}
          UNITY_PASSWORD: ${{ secrets.UNITY_PASSWORD }}
        with:
          unityVersion: 6000.0.23f1
          targetPlatform: ${{ matrix.targetPlatform }}
          buildName: EscapeDays

      - name: Upload Build Artifact
        uses: actions/upload-artifact@v4
        with:
          name: Build-${{ matrix.targetPlatform }}
          path: build/${{ matrix.targetPlatform }}
```

### 6.3 Manual Build Commands (Local)

```bash
# Run all tests (headless)
/path/to/Unity -batchmode -nographics \
  -projectPath "$(pwd)" \
  -runTests \
  -testPlatform EditMode \
  -testResults TestResults/EditMode.xml \
  -logFile TestResults/edit-mode.log

# Build for Windows (headless)
/path/to/Unity -batchmode -nographics \
  -projectPath "$(pwd)" \
  -buildTarget StandaloneWindows64 \
  -executeMethod BuildScript.BuildGame \
  -logFile build.log \
  -quit
```

---

## 7. Testing Standards

### 7.1 Test Folder Structure

```
Assets/
└── _Project/
    └── Tests/
        ├── EditMode/               ← Pure logic tests (no scene required)
        │   ├── VitalsSystemTests.cs
        │   ├── CraftingSystemTests.cs
        │   ├── SaveSystemTests.cs
        │   └── PathfindingTests.cs
        ├── PlayMode/               ← Tests that require game runtime
        │   ├── PlayerMovementTests.cs
        │   ├── CombatTests.cs
        │   └── EnemyAITests.cs
        ├── EditMode.asmdef
        └── PlayMode.asmdef
```

### 7.2 Assembly Definition Files

`EditMode.asmdef`:
```json
{
    "name": "EscapeDays.Tests.EditMode",
    "references": ["EscapeDays.Runtime", "UnityEngine.TestRunner", "UnityEditor.TestRunner"],
    "includePlatforms": ["Editor"],
    "optionalUnityReferences": ["TestAssemblies"]
}
```

### 7.3 Edit Mode Test Template

```csharp
using NUnit.Framework;
using EscapeDays.Vitals;

namespace EscapeDays.Tests.EditMode
{
    /// <summary>
    /// Tests for VitalsSystem — pure logic, no MonoBehaviour.
    /// </summary>
    [TestFixture]
    public class VitalsSystemTests
    {
        private VitalsData _vitalsData;

        [SetUp]
        public void SetUp()
        {
            // Arrange: Create ScriptableObject instance for testing
            _vitalsData = ScriptableObject.CreateInstance<VitalsData>();
            _vitalsData.maxHunger = 100f;
            _vitalsData.hungerDepletionRate = 1f;
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_vitalsData);
        }

        [Test]
        public void Hunger_Depletes_AtCorrectRate()
        {
            // Arrange
            float initialHunger = 100f;
            float deltaTime = 60f; // 1 in-game minute

            // Act
            float result = initialHunger - (_vitalsData.hungerDepletionRate * deltaTime);

            // Assert
            Assert.AreEqual(40f, result, 0.01f, "Hunger should deplete by 60 over 60 seconds");
        }

        [Test]
        public void Hunger_CannotGoBelowZero()
        {
            float hunger = Mathf.Max(0f, -10f);
            Assert.AreEqual(0f, hunger, "Hunger cannot be negative");
        }
    }
}
```

### 7.4 Play Mode Test Template

```csharp
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

namespace EscapeDays.Tests.PlayMode
{
    /// <summary>
    /// Tests for player movement — requires scene with PlayerController.
    /// </summary>
    [TestFixture]
    public class PlayerMovementTests
    {
        [UnitySetUp]
        public IEnumerator SetUp()
        {
            SceneManager.LoadScene("TestScene_Player");
            yield return null; // Wait one frame for scene to load
        }

        [UnityTest]
        public IEnumerator Player_MovesRight_WhenRightInputApplied()
        {
            // Arrange
            var playerObj = GameObject.FindWithTag("Player");
            Assert.IsNotNull(playerObj, "Player object must exist in TestScene_Player");
            var startPos = playerObj.transform.position;

            // Act — Simulate right input for 0.5 seconds
            // Note: Use InputSystem.QueueStateEvent for new Input System
            yield return new WaitForSeconds(0.5f);

            // Assert
            Assert.Greater(playerObj.transform.position.x, startPos.x,
                "Player should have moved right");
        }
    }
}
```

### 7.5 Testing Rules

- **Edit Mode tests** must not use `MonoBehaviour.Start/Update`. Test pure C# classes and ScriptableObjects only.
- **Play Mode tests** must use a dedicated test scene (`TestScene_*`), never a production scene.
- Minimum **1 test per manager class**.
- Tests must be **deterministic** — no `Random` calls without a seeded `System.Random`.
- All tests must **pass** before merging to `develop`.
- Test names follow: `[SystemUnderTest]_[Condition]_[ExpectedResult]`.

---

## 8. Global "Do Not" Rules

> These are **hard rules**. Violations require an immediate fix before the PR can be merged.

### 8.1 Git Rules

| ❌ FORBIDDEN | ✅ CORRECT ACTION |
|---|---|
| `git push --force` on `develop` or `main` | Use `git push --force-with-lease` only on personal feature branches, never shared branches |
| Committing `.unity` files with unresolved merge conflicts | Resolve using UnityYAMLMerge or manually in Unity Editor, then commit |
| Committing `Library/`, `Temp/`, `Logs/` folders | Ensure `.gitignore` is correct; remove from tracking with `git rm --cached` |
| Committing large binaries without LFS | Run `git lfs track "*.png"` and re-add the file |
| Committing directly to `main` or `develop` | Always open a Pull Request |
| Merge commit from `develop` into a feature branch (forward-merge) | Use `git rebase develop` on your local feature branch |
| WIP/broken code committed to `develop` | Use `git stash` or feature branch. `develop` must always run |

### 8.2 Code Rules (Enforced here, detailed in role-programmer.md)

| ❌ FORBIDDEN | Reason |
|---|---|
| `FindObjectOfType<T>()` | Performance — O(n) every call; breaks with multiple instances |
| `Camera.main` in `Update()` | Expensive tag-based lookup per frame |
| `GetComponent<T>()` in `Update()`/`FixedUpdate()` | Expensive reflection per frame |
| `new` allocation in `Update()` (strings, lists, etc.) | Causes GC spikes and frame drops |
| 3D Physics components (`Rigidbody`, `Collider`) | This is a 2D game — use `Rigidbody2D`, `Collider2D` |
| `using UnityEngine.AI` (NavMesh) | NavMesh is 3D. Use A\* Pathfinding Project |
| Hard-coded magic numbers in gameplay code | Use constants or ScriptableObject fields |
| `public` fields on MonoBehaviour without `[SerializeField]` | Breaks encapsulation; use `[SerializeField] private` |

### 8.3 Asset Rules (Enforced here, detailed in role-artist-designer.md)

| ❌ FORBIDDEN | Reason |
|---|---|
| Committing `.psd` files without LFS tracking | Bloats repo history |
| Sprites without consistent Pixels Per Unit (PPU) | Causes scale inconsistencies in-game |
| Audio files above 5MB uncompressed in the repo | Use compressed `.ogg` for in-game audio |
| Placing assets outside `Assets/_Project/` | Breaks project organization |
| Scenes with missing script references | All `MonoBehaviour` references must be valid before commit |

### 8.4 CI/CD Rules

- **PRs cannot be merged if CI tests fail.** Period.
- Build artifacts are not manually uploaded to any platform — only CI builds are used for distribution.
- Never store `UNITY_LICENSE`, API keys, or secrets in code or documentation. Use GitHub Secrets only.

---

*This document governs the workflow of all contributors — human and AI. Treat it as the ground truth for how work flows through this project.*
