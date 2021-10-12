# Danki2

## Development

### Unity

The unity verion that should be used is: `2020.3.3f1`.

### Requirements

Blender must be installed for the project to load.

### Fmod
Danki2 uses FMOD as audio middleware - this is the official user guide https://fmod.com/resources/documentation-unity?version=2.0&page=user-guide.html#using-source-control

Any additions to fmod source code are preceeded by a comment:
// \*\*Added to original source code\*\*

Any removals to fmod source code are preceeded by a comment:
// \*\*Removed from original source code\*\*

Note - we have implemented a workaround for a bug where the sound wasn't stopping correctly when exiting play mode (this happened if and only if any text component in scene had a non-default font - it wasn't clear why this was happening). This involved adding a "state == PlayModeStateChange.ExitingPlayMode" case on line 698 of Danki2\Assets\Plugins\FMOD\src\Runtime\RuntimeManager.cs (ie. in the fmod source code).

### Smart Merge

To use [Unity Smart Merge](https://docs.unity3d.com/Manual/SmartMerge.html), for resolving merge conflicts in non-code files, you must first check that the file path to your verion of UnityYamlMerge is the same as that in the .gitconfig file (if not then change the file path in the .gitconfig file). Then run the following command:

```
git config --local include.path ../.gitconfig
```

This will apply the settings in the .gitconfig file to your local version of this repository, setting up Unity Smart Merge. To use Unity Smart Merge:
* Create the merge conflict as normal by running `git merge <branch-name>`.
* Run `git mergetool`, unity smart merge will then attempt to resolve the conflict.
* Check back in unity that the files have been merged correctly and type `y` if so.
* If you see any files with either `_BACKUP_`, `_BASE_`, `_LOCAL_` or `_REMOTE_` in the name then wait a moment, these should clean themselves up.
* Clean up any files left over from the smart merge. These will have a `.orig` in
the file name.
* Add the merged files as normal and run `git commit`.

### Making Prop Prefabs

Things to be sure to do when making prop prefabs:

- On the mesh:
  - "Material Creation Mode" should be set to "None" on the mesh.
  - "Convert Units", "Import BlendShapes", "Import Visibility", "Import Cameras" and "Import Lights" should all be off.
  - "Scale Factor" should be so that meshes appear at the correct size when their transforms have scale (1, 1, 1).
- On the prefab:
  - The name should be the same as the mesh's name, but without the "_mesh" suffix.
  - There should be an empty parent game object with default transform (can click "Reset" on the menu in the transform). The easiest way to achieve this is:
    - Create an empty game object in scene
    - Drag the desired model in as a child (and rename the child gameobject to Mesh)
    - Right click on the child game object and click "Unpack Prefab" to convert it into standard gameobject with no prefab links
    - Drag the parent game object into the desired prefab location
  - Child game object should also have default transform, but scale can be tweaked if we want to uniformly adjust the proportions.
  - The game object with the mesh should have a "Mesh Filter", a "Mesh Renderer" and a "Mesh Collider" (if collidable).
    - If a collider is present, it should have a physic material assigned to it (determines which collision sound will play if struck)
  - The game object layer should be set to "Props" (including all children)
  - If the prop should be part of the navmesh, it should have "Navigation Static" selected and an appropriate "Navigation Area".

### Making Enemy Prefabs

- Make sure that "Depth Write" and "Double-Sided" are checked on any appropriated materials, for example, the Forest Bear leaves.

### Making New Scenes

To create a new scene:

- Create a new folder and scene inside that folder,
- Open the new scene and delete the camera and directional light so that it completely empty,
- Open an existing scene and copy the main camera and Meta game objects into your new scene,
- Add an empty game object called "Room" to the scene (ideally with the default transform). This will contain everything else related to the level, including:
  - The floor,
  - Enemy spawners,
  - Props,
  - Etc...
- Add a terrain as a child of Room and call it "Floor", you'll probably want to make it a lot smaller and raise it up using the "Set Height" tool so that you have room for sunken terrain. Be sure to set the layer to "Floor", if adding water then set the water's layer to "Water". Make sure that the "Rendering Layer Mask" is set to include the "Light Layer Default" and the "Decal Layer Default", otherwise decals won't appear on the terrain,
- Adding the terrain will have created a new terrain asset in the root of the "Assets" folder. Rename this to "\<your-scene-name\>_TerrainData" and move it into your scenes folder,
- Bake the navmesh and save (Ctrl+S),
- This is a good time to make a commit, as you have all of the new files required for a new scene,
- The next step is to build the scene in whichever way is required. This will include adding the following, as well as any kind of props (see other scenes for examples of hierarchy structure):
  - *Entrances*. To create an entrance, add an instance of the PlayerSpawner prefab. This will need an ID which must be unique from any other player spawners in the scene,
  - *Exits*. To create an exit, add an instance of the RoomTransitioner prefab. This will need an ID which must be unique from any other room transitioners in the scene,
  - *Transition Sockets*. To add a transition socket to an exit (a doorway which will only be there when the exit is in use), add an instance of the TransitionSocket prefab. This will need an ID which must be unique from any other transition sockets in the scene. It also needs an associated exit ID, this is the ID of the room transitioner that is to be associated with this socket. When the exit is active in a scene, the transition socket will pick an appropriate doorway to instantiate. When the exit is not active, the transition socket is destroyed when the scene loads,
  - *Spawners*. To create an enemy spawner, add an instance of the EnemySpawner prefab. There must be exactly 3 spawners in the scene with IDs: 0, 1, 2. This number is given by the MapGenerationLookup,
  - *Sockets*. To create a socket, add an instance of one of the socket prefabs. This will need an ID which must be unique from any other sockets in the scene. Tags added will be required on any module that gets put in that socket, tags to exclude will ensure that none of the tags in that list are on any of the modules that get put in that socket. Sockets should be rotated so that they face a direction that generally makes sense within the scene, some modules will take this rotation into account and some modules will still allow free rotation,
  - *Transition Dependent Objects*. To create objects that are either destroyed or left depending on the status of an entrance or exit (for example, entrances and exits should be blocked off when they are not active), add the "Transition Dependent" component to it. This can have an associated entrance and/or an associated exit (Note that this means that we can have entrances and exits and the exact same points in a scene). If the entrance or exit is active and the type is set to `ActiveWithTransition`, then the game object will be left alone. If it is set to `ActiveWithoutTransition` then the game object will be destroyed. Similarly, if the entrance or exit is not active and the the type is set to `ActiveWithTransition`, then the game object will be destroyed. If it is set to `ActiveWithoutTransition` then the game object will be left alone.
  - *Camera Volumes*. To create a camera volume, add an instance of either the CircularCameraVolume or SquareCameraVolume prefabs. The volume can be resized in any way. When the player is inside the volume, the camera will lerp towards the appropriate transform depending on the camera orientation. The transforms are determined by the 4 children of the prefabs, which can be freely moved around in scene. Aspects of the UI can also be turned off when inside the volume using the editor.
- Before a scene is finished, data about it must be added to the SceneLookup which can be found in the EntryScene:
  - Add a new entry to the Scene enum for your new scene,
  - Open your new scene, then go to the build settings and click "Add Open Scenes". You should see a change in the EditorBuildSettings.asset file,
  - In the SceneLookup, fill out all of the data required.
- The scene should now be finished and will come up in random scene selection.

### Making New Modules

To create a new module:

- Create a prefab in the relevant folder,
- Add props to the prefab as required (you can add an instance of the socket you're creating the module for temporarily to show you the required size),
- Make sure the prefab has the default transform and that it is facing the -ve z-axis (if the direction is important to the module),
- In the ModuleLookup, add a new entry for your module, drag in the prefab, set the appropriate tags and data about available rotations,
- The module will now come up in random module selection.

### Creating an Enemy Prefab

This assumes a mesh and materials have been created and a blank enemy prefab is desired:

- Create an empty prefab in the scene, reset the transform and turn this into a prefab appropriately named under the Enemies folder.
- Place the mesh under the prefab and rename it to Mesh.
- Take a copy of the SpeedTrail object from another enemy. Place this under the prefab and set the height.
- Create an Empty under the prefab and name it Sockets. Under Sockets create 3 Empties called: Centre, AbilitySource, and CollisionTemplateSource.
- Move the 3 Sockets into position.
- From the UI folder drag the EnemyDiegetic onto the prefab.
- Zoom out and adjust the Y-position of the Diegetic to be slightly above the enemies height.
- Create an Enemy script. Open the ActorType enum and add the enemy to that list.
- Inside the enemies script firstly inheret from the Enemy class, then write `public override ActorType Type => ActorType.Example;`.
- Note: The enemies script can be left at this unless further details are going to be added immediately.
- Create an enemy AI script and inheret from EnemyAi. Within the class start by writing: `[SerializeField] private Example example = null;`.
- Then write: `protected override Actor Actor => example;`.
- Then ctrl + . the class to create an empty state machine:

```
protected override IStateMachineComponent BuildStateMachineComponent()
{
    throw new System.NotImplementedException();
}
```

- Within the Enemy script folder we must also create an enemy editor script.
- Within this script write:

```
[CustomEditor(typeof(Example))]
public class ExampleEditor : EnemyEditor
{
}
```

- Make sure the scripts are saved, open Unity and the prefab. Attach the enemy script and the enemy ai scipt, making sure to drop a reference to the enemy script in the enemy ai scripts serialized field.
- We now want to complete the enemy script. Start by adding a nav mesh agent component to the prefab. Check the component, but most of the time the component can be left as default. Reference this in the enemy script.
- Next reference the speed trail child object in the heirarchy.
- Next open the mesh renderer dropdown box on the enemy script. Add as many lines as there are child objects of the prefabs Mesh, each child of the Mesh must be manually placed into this list.
- Next enter an appropriate weight, for reference the bear weighs 2, the wolf weighs 1.
- Enter a rotation smoothing of 0.1.
- Open the collidors dropdown, create a single line and reference the collidor component from the prefab.
- Open the sockets dropdown in the heirarchy and 1 by 1 reference each of the sockets.
- Enter appropriate Stats.
- Add 2 more components to the prefab: Flash on hit & Player targetable. Reference the enemy in each of these.
- The EnemyDiegetic child of the prefab now needs a reference to the enemy.
- Finally open the Spawner prefab and reference the new enemy.
- Test the enemy in scene to ensure it is appropriately sized.