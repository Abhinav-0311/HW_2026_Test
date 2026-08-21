# Doofus Adventure

Doofus Adventure is a 3D Unity game where the player guides Doofus, a yellow cube, across timed green Pulpits. Each Pulpit disappears after a short random lifetime, so the player must reach newly spawned adjacent Pulpits before falling. Every new Pulpit reached increases the score.

## Features

- 3D movement with WASD or arrow keys
- JSON driven player speed and Pulpit timing
- Random 4 to 5 second Pulpit lifetime
- Successor Pulpit appears when 2.5 seconds remain
- Score tracking for each new Pulpit reached
- Start, Game Over, and Restart flow
- Background music and gameplay timers

## Configuration

Gameplay values are loaded from `Assets/StreamingAssets/doofus_diary.json`.

```json
{
  "player_data": { "speed": 3 },
  "pulpit_data": {
    "min_pulpit_destroy_time": 4,
    "max_pulpit_destroy_time": 5,
    "pulpit_spawn_time": 2.5
  }
}
```

## Run the project

Open the project with Unity 6 or later, open `Assets/Scenes/Main.unity`, and press Play.

## Gameplay media

- [Final gameplay video](Media/final-gameplay.mp4)
- [Gameplay screenshot 1](Media/gameplay-01.png)
- [Gameplay screenshot 2](Media/gameplay-02.png)
