# Tilemap
A simple yet highly customisable tile mapping tool made for the Unity Engine

## Help

### How do I make my own tiles?
In the project window click on _Create > Tilemap > Tiles_ and select a scriptable tile type to create. You can edit the tiles variables by just clicking on it, like anyother asset.

If you want to create your own scriptable tiles (Like the **Tile** or **AutoTile**) then click on _Create > Tilemap > Create C# Tile Script_ and edit like anyother script. Alternatively create a C# class that inherits **ScriptableTile**

### Can I make custom brushes?
Yes. If you want to create your own brushes (Like the **Pencil** or **Eyedropper**) then click on _Create > Tilemap > Create C# Stencil Script_ and edit like anyother script. Since all brushes (even the included ones) inherit from **ScriptableBrush** you can also simply create a C# class that inherits from **ScriptableBrush**. Any class that inherits **ScriptableBrush** will automatically be able to use in the Edit Mode tile editor, any public variables will also be exposed in the tile editor toolbar.