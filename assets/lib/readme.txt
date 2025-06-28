BeachedUnityBridge.dll
contains shared classes between the Unity Project used for custom assets and the main mod dll
it has to be loaded manually so the assembly qualified name remains untouched by ilmerge, and Unity can understand what it's looking at