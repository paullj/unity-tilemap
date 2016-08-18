# Tilemap
A simple yet highly customisable tile mapping tool made for the Unity Engine

## Help

### How do I make my own tiles?
In the project window click on _Create > Tilemap > Tiles_ and select a scriptable tile type to create. You can edit the tiles variables by just clicking on it, like anyother asset.

If you want to create your own scriptable tiles (Like the **AutoTile**) then click on _Create > Tilemap > Tiles > Create C# Tile Script_ and edit like anyother script. Alternativly create a C# class that inherits **ScriptableTile**

### What is a stencil ?
A **Stencil** (or in other words a brush) is a script that controls the way tiles are painted on the tilemap.

### How do I make custom stencils?
If you want to create your own Stencils (Like the **AutoTile**) then click on _Create > Tilemap > Create C# Stencil Script_ and edit like anyother script. Alternativly create a C# class that inherits **Stencil**