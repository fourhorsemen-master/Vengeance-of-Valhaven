# Danki2

## Development

### Unity

The unity verion that should be used is: `2019.4.14f1`.

### Requirements

Blender must be installed for the project to load.

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
  - There should be an empty parent game object with default transform (can click "Reset" on the menu in the transform).
  - Child game object should also have default transform, but scale can be tweaked if we want to uniformly adjust the proportions.
  - The game object with the mesh should have a "Mesh Filter", a "Mesh Renderer" and a "Mesh Collider" (if collidable).
  - If the prop should be part of the navmesh, it should have "Navigation Static" selected and an appropriate "Navigation Area".
