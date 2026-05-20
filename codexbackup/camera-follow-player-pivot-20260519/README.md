# Camera follow player pivot backup

Date: 2026-05-19

Backed up files:
- `CameraFollow.cs`
- `SampleScene.unity`

Why these files were modified:
- `CameraFollow.cs` was changed so the main camera uses the player position as its rotation pivot instead of rotating only around the camera's own transform.
- The camera position was calculated from the player pivot, distance, yaw, and pitch, then pointed back at the player with `Quaternion.LookRotation`.
- Automatic target lookup was added so the camera can find `PlayerManger`, a `Player` tagged object, or `unitychan` if inspector references are missing.
- `SampleScene.unity` was changed to attach the `CameraFollow` component to the `Main Camera` and serialize its default camera-follow settings.

Reason for backup:
- The original request was to make the main camera keep the player centered when rotating.
- These files are saved here as a reference copy before reverting the project files back to their previous state.
