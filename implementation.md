Simplified Mechanics
---

DOTS Features: 
- ECS Core
- Burst Compiler
- Job System
- Entities Graphics

**Player**
- Input: Movement only (WASD or Joystick).
- Combat: Automatic. The player has one weapon that fires a projectile at the nearest enemy within a fixed radius.
- Objective: Collect XP orbs to increase a score counter. leveling up simply increases the fire rate or projectile count.

**Enemy Behavior**
- Movement: Every enemy uses a simple "Move Toward Player" logic. They do not avoid each other or use complex pathfinding.
- Logic Loop:
    - Calculate direction to player.
    - Move at a constant speed.
    - If within a specific distance of the player, deal damage (deplete a health bar).
- Death: When health reaches zero, the enemy is destroyed and replaced by an XP orb entity at that location.

**Simplified Mechanics**
- SpawningEnemies - spawn in a ring just outside the camera view
- Projectiles - Linear movement. They use the spatial grid to check for collisions with enemies.
- XP Collection - Orbs stay stationary. When the player is near, they fly toward the player and are destroyed.
- Performance Toggle - A UI button switches the active "World." One world runs a standard MonoBehaviour script for 500 units; the other runs the ECS systems for 50,000 units.
    - Mode Switcher: A standard MonoBehaviour that handles the toggle UI.
    - GameObject Path: A simple script that uses Instantiate() and Update() for 500 units.
    - DOTS Path: Uses EntityManager.Instantiate() for 50,000 units.
    - Metric System: A UI system that captures 1 / World.Time.DeltaTime to display live FPS for both modes side by side.

**Key Technical Goals for the Project**
- Grid Partitioning: Instead of using Unity Physics, you will write a Job that sorts all enemy positions into a grid. Projectiles will only check the grid cell they are currently in to find a target.
- Burst Optimized: Ensure the movement and grid-search jobs are fully compatible with the Burst Compiler for maximum CPU throughput.
- Entity Graphics: Use the BatchRendererGroup API (via Entities Graphics) to draw all 50,000 enemies in very few draw calls.

**System Architecture**
1. Movement and Input
    - PlayerInputSystem: Reads standard input and updates Player movement components.
    - EnemyAIsystem: Calculates direction vectors from Enemy entities toward the Player transform.
    - TransformSystem: Applies movement vectors to LocalTransform using SystemAPI.Query.
2. Spatial Partitioning and Collision
    - HashGridSystem: Clears and populates a NativeParallelMultiHashMap using enemy positions. This is the heart of your performance showcase.
    - CollisionSystem: Projectiles and the Player query the HashGrid for nearby entities. This replaces expensive physics raycasts or triggers.
3. Lifecycle and Spawning
    - SpawnerSystem: Monitors time and instantiates Enemy prefabs based on the current Wave configuration.
    - DespawnSystem: Removes Projectiles when Lifetime expires or Health reaches zero.

<!-- Data
    Units
        EnemyConfig
        PlayerConfig
Runtime
    Player -->

TODO
- [ ] 