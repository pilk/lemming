--- Dependencies ---
UnityHelpers/Editor


--- Purpose ---
AssetManifest is a system to track the contents of an asset folder in Unity and make that list available at runtime.

--- Usage ---
To begin tracking an asset folder in your project, simply right click on the folder in Unity and select "Create/Asset Manifest". A new AssetManifest object will be created inside that folder. From now on, any time files are added or removed from that folder or its subfolders, the AssetManifest object's "assets" array is updated to reflect the change. Note that it's not necessary to commit the modifications to this file (but you still need to add it to the depot) since everyone else's machines, including build machines, will rebuild this list when they also update those secondary assets.

To access this list at runtime, either 
A) assign the AssetManifest to a direct reference of an object in the build (a prefab, etc) OR
B) Load the AssetManifest through a standard Resources.Load() call.



Author: Ben Sheftel