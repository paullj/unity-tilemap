# Tilemap
A simple yet highly customisable tile mapping tool made for the Unity Engine

## Help

### How do I make my own tiles?
In the project window click on _Create > Tilemap > Tiles_ and select a scriptable tile type to create. You can edit the tiles variables by just clicking on it, like anyother asset.

If you want to create your own scriptable tiles (Like the **Tile** or **AutoTile**) then click on _Create > Tilemap > Create C# Tile Script_ and edit like anyother script. Alternatively create a C# class that inherits **ScriptableTile**

### Can I make custom tools?
Yes. If you want to create your own tools (Like the **Pencil** or **Eyedropper**) then click on _Create > Tilemap > Create C# Tool Script_ and edit like anyother script. Since all tools (even the included ones) inherit from **ScriptableTool** you can also simply create a C# class that inherits from **ScriptableTool**. Any class that inherits **ScriptableTool** will automatically be able to use in the Edit Mode tile editor, any public variables will also be exposed in the tile editor toolbar.