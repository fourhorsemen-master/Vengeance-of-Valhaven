# Editor Assembly

This folder is just to define the EditorAssembly. Editor folders in /Scripts should all have an Assembly Definition Reference which points to this assembly. This allows us to keep our editor scripts alongside our regular scripts, without them being part of the ScriptsAssembly.

Usually files in folders called "Editor" will be part of a predefined editor assembly. However the ScriptsAssembly will override this, causing the editor scripts to be part of the ScriptsAssembly. This causes all kinds of problems relating to testing and building, so that's why we do this.
