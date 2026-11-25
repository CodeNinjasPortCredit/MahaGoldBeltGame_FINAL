# Unused scenes

These two scenes are not part of the game. They were moved out of `Assets/`
so Unity no longer imports them, and so the project only contains the six
scenes that are actually in the build:

```
MainMenu -> MahaInterior -> level 2 -> Spaceship Level -> circus -> ChiikawaLAND
```

| Scene | Why it is here |
| --- | --- |
| `cockpit.unity` | An empty placeholder. Never finished, never reached from the game. |
| `SampleScene.unity` | Unity's default scene from when the project was created. |

They are kept rather than deleted in case `cockpit` is built out later. To use
one again, drag it back into `Assets/Scenes/` from here.
