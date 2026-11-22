# Response to Round 1 Feedback

**Ninja:** Maha Bashir (Mississauga, ON)
**Project:** XYZ Game
**Feedback round:** Round 1, returned November 5, 2026
**This response:** November 22, 2026

This document answers the Round 1 Fix List point by point. Every claim below was
checked against the project as it stands today, not from memory — where a number
appears (how many canvases, which scene has which sound), it came from opening
the scenes and reading the components.

Items that are **not** finished are marked as such rather than left out.

| Status | Meaning |
| --- | --- |
| **Fixed** | The item is done and was verified. |
| **Fixed, with a note** | The item is done, but there is something the committee should know. |
| **Not fixed** | Still outstanding. |

---

## Must fix

### 1. Space never jumps — `Jump.cs` holds a class called `PlayerController`

**Status: Fixed**

`Assets/_Scripts/Jump.cs` now reads:

```csharp
public class Jump : MonoBehaviour
```

The class name matches the file name, so Unity loads it on the Player and Space
jumps. The script also plays a jump sound through an `AudioSource`.

---

### 2. The "restart?" buttons call a function named `restart` that does not exist

**Status: Fixed**

`Assets/_Scripts/ResetLevel.cs` now has the public function, and it is the one
thing that reloads the level:

```csharp
public void restart()
{
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}
```

The game over screen no longer has buttons on it — it is a message, **"YOU DIED
oop / Press space to restart"**, and restarting is done with a key:

- **Space** on the game over screen (`ResetLevel.Update`)
- **R** at any time during play, including while dead (added this round, see below)

So the dead-button problem is gone because there is no longer a button that can
do nothing. If the committee would rather see a clickable button, that is a
small change to make.

---

### 3. There is no title or start screen

**Status: Fixed**

`Assets/Scenes/MainMenu.unity` is **build index 0**, so it is what opens when the
game starts. The full build order is now:

```
0  MainMenu
1  MahaInterior
2  level 2
3  Spaceship Level
4  circus
5  ChiikawaLAND
```

The menu shows the title **"Escape Big Rats Spaceship!"**, "Made by Maha Bashir",
and has Play, Options and Credits. The Controls panel inside Options lists:

```
WASD  - To Move
Space - To Jump
Shift - To Run
Tab   - Look Forward
R     - Reset Level
```

Tab and R are new controls added since Round 1: **Tab** swings the camera around
to the front of the player so you can see what is chasing you, and **R** restarts
the level you are in.

---

### 4. There is no credits screen

**Status: Fixed, with a note**

The main menu has a Credits panel. It currently reads:

```
Senseis: Magdalena, Scott, Jacky
Assets: SlimUI, N91 Music, Southport Studio, Andrew Sitkov Music, Sound Bits
Chiikawa by its creators
AI assistant: bug fixes
```

**The note:** the requirement asks for *every* asset pack used, and this list is
not yet complete. The project also uses, among others:

- Sci-Fi Styled Modular Pack (the spaceship corridors)
- LittleFriends – Cartoon Animals Lite (the squirrel enemy body)
- Hatogame BossMonsterPack1 (the wolf enemy body)
- Stylized3DMonster / True_Horror (the circus clowns)
- Action Soundtrack Pack FREE (the level music)
- free horror ambience 2 (the circus and menu ambience)
- SoundBits Free SFX 2020 (sound effects)

This is listed here honestly so the committee can see it is known, and the
credits panel can be extended to name all of them.

---

### 5. There is no music or sound effects

**Status: Fixed, with a note**

Every scene now has audio. This is what is actually in each one:

| Scene | Music / ambience | Sound effects |
| --- | --- | --- |
| MainMenu | `SFX_MusicSample` (loops) + `ha-distillery` ambience | menu click and whoosh |
| MahaInterior | `Drive With Me` | jump whoosh, enemy chase voice ×2 |
| level 2 | `The Superiority` | jump whoosh, enemy chase voice |
| Spaceship Level | `Hidden Threat` (loops, volume 0.30) | jump whoosh, enemy chase voice ×3 |
| circus | `ha-distillery` ×2 | jump whoosh, enemy chase voice ×7 |
| ChiikawaLAND | `Pure Dreamer_93 BPM` | — |

The **enemy chase voice** is new: every enemy plays an "I love you" clip while it
can see the player. It is 3D sound, so it gets louder as the enemy closes in and
you can tell which direction it is coming from. It repeats every 9 seconds, and
each enemy in a level starts a little after the last so a pack does not chant in
unison. It is driven by `Assets/_Scripts/EnemyVoice.cs`, which only asks
`EnemyAI.CanSeePlayer()` — `EnemyAI.cs` itself did not have to change.

**The note:** two things from the original wording are not done. Four of the six
music tracks are set to play once rather than loop, so they stop when the track
ends; and there are not yet dedicated sounds for *picking up a key*, *getting
caught* or *escaping*. Jumping, the menu, and being chased all have sound.

---

### 6. 3 of 4 canvases use Constant Pixel Size, and the instructions float in the room

**Status: Fixed, with a note**

Canvases were counted across all six scenes in the build:

```
23 canvases total
21 set to Scale With Screen Size
 2 not  -  Canv_Main and Canv_Options in MainMenu
```

The two exceptions are the SlimUI menu's own canvases, and they are **World
Space**, not Screen Space. A World Space canvas does not use the Canvas Scaler's
screen-size mode at all; it is positioned in 3D in front of the menu camera and
scales with that camera's view. They are listed here so the count is honest.

Every canvas that belongs to gameplay — `PlayerUI`, `GameOverScreen`,
`ObjectiveHUD`, `ScreenFader` — is Screen Space Overlay and set to Scale With
Screen Size.

Also new since Round 1: an **objective line in the top-left corner of every
level**, reading `Objective: Get the key` and changing to `Objective: Find the
exit` once the key is collected. It is on its own screen canvas
(`Assets/_Prefabs/ObjectiveHUD.prefab`).

**The note:** the original instruction sign in **MahaInterior** — "wasd to move /
Shift to run! / Find keys to escape the chiikawa ships!" — is **still a world
object**, not on a screen canvas, so it can still be blocked by the player. The
controls now live on the start screen instead, which is where the feedback asked
for them, but this particular text has not been moved.

---

## Should fix

### 7. Empty and unused scripts

**Status: Fixed**

`Loader.cs`, `RegularEnemy.cs`, `gameOver.cs` and `EnemyScript.cs` are all gone
from the project. A search for each filename returns nothing.

The scripts folder is now 10 files, all of them in use:

```
EnemyAI.cs   EnemyVoice.cs   FadeScreen.cs   Jump.cs      Keyandgate.cs
Loadscene.cs LookBehind.cs   MainMenu1.cs    PlayerControllerTopDown.cs  ResetLevel.cs
```

---

### 8. Unclear names, and a level saved inside an asset pack folder

**Status: Partly fixed**

- **Fixed:** every scene in the build now lives in `Assets/Scenes/`, including
  the first level. Nothing is saved inside an asset pack folder.
- **Fixed:** the names "cool scene" and "Player Smalleeer" no longer appear in
  any scene in the build.
- **Not fixed:** `EnemyAI.cs` still has the field `NONtransformingPlayer`
  (lines 13 and 165). Renaming it means updating it everywhere it is used, so it
  has been left for now rather than changed carelessly.

---

## Nice to have

### 9. Three `Debug.Log` lines print every frame

**Status: Fixed**

The per-frame stamina logs are gone. The `Debug.Log` lines left in the project
only run when something actually happens — the player dies, the game exits, an
interaction starts or ends — not once per frame.

The one remaining warning, `"Stamina Bar is not assigned!"`, sits in an `else`
branch, and the stamina bar is assigned in all four gameplay levels (checked), so
it never fires.


---

## Summary

| # | Item | Status |
| --- | --- | --- |
| 1 | Space never jumps | Fixed |
| 2 | Restart buttons do nothing | Fixed |
| 3 | No start screen | Fixed |
| 4 | No credits screen | Fixed — asset list not yet complete |
| 5 | No music or sound effects | Fixed — some tracks do not loop; no key/caught/escape sounds |
| 6 | Canvases and floating instructions | Fixed for canvases — MahaInterior's instruction sign still in the world |
| 7 | Empty and unused scripts | Fixed |
| 8 | Unclear names, level in asset folder | Partly — `NONtransformingPlayer` remains |
| 9 | Per-frame `Debug.Log` lines | Fixed |
| — | Your own code (*Discuss*) | Partly — credits name the AI; no per-script header comments |
| — | Going Further (optional) | Not done — not required at Gold Belt |

**Still outstanding, in one list:**

1. Credits panel does not name every asset pack used.
2. Four music tracks play once instead of looping.
3. No sounds for picking up a key, getting caught, or escaping.
4. MahaInterior's instruction sign is still a world object, not a screen canvas.
5. `NONtransformingPlayer` has not been renamed.
