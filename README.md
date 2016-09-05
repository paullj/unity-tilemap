# Unity Tilemap

[![Join the chat at https://gitter.im/unity-tilemap/Lobby](https://badges.gitter.im/unity-tilemap/Lobby.svg)](https://gitter.im/unity-tilemap/Lobby?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)
A simple yet highly customisable tile mapping tool made for the Unity Engine.

[About](#about)    
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[Features](#features)    
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[Things I Would Like](#like)    
[Help](#help)    
[Contributing](#contributing)    
[Contact](#contact)

![alt tag](https://github.com/toinfiniityandbeyond/unity-tilemap/blob/master/images/banner.gif)

## About
This tilemap system is a robust system that allows users to customise the tile types and brushes to enable them to be able to easily paint tiles in the Unity Editor as well as in realtime.
### Features
* Scriptable tiles, allows custom behaviour. Including:
	* SimpleTile
	* RandomTile
	* AutoTile
	* **TODO:** AnimatedTile
	* **TODO:** ConditionalTile
* Tile Editor
	* Scriptable tools, allows custom behaviour. Including:
		*  Pencil
		*  Brush
		*  Fill
		*  Eraser
		*  Eyedropper
		*  Line
		*  Rectangle
		*  Ellipse
	* Proper Undo/Redo ([@nickgirardo](https://github.com/nickgirardo))
	* Export/Import to ScriptableObject
* Different rendering modes
	* Single Sprites
	* Multiple Quads
* **TODO:** TileMapCollider2D (Use PolygonCollider2D underneath)

### Things I Would Like (But have been put on the back burner)<a name="like"></a>
* Good documentation
* More rendering modes
	* Single Quad
	* Chunked Quads
* Infinite or limited tilemap size (only available on chunked quads?)
* Layers
* In-Game tilemap editor

## Help ([More on the wiki](../../wiki))<a name="help"></a>

### How do I make my own tiles?
In the project window click on _Create > Tilemap > Tiles_ and select a scriptable tile type to create. You can edit the tiles variables by just clicking on it, like anyother asset.

If you want to create your own scriptable tiles (Like the **RandomTile** or **AutoTile**) then click on _Create > Tilemap > Create C# Tile Script_ and edit like anyother script. Alternatively create a C# class that inherits **ScriptableTile**

### Can I make custom tools?
Yes. If you want to create your own tools (Like the **Pencil** or **Eyedropper**) then click on _Create > Tilemap > Create C# Tool Script_ and edit like anyother script. Since all tools (even the included ones) inherit from **ScriptableTool** you can also simply create a C# class that inherits from **ScriptableTool**. Any class that inherits **ScriptableTool** will automatically be able to use in the Edit Mode tile editor, any public variables will also be exposed in the tile editor toolbar.

## Contributing
Feel free to contribute, I will accept pull requests if I think it will benefit the tool. If you want to help but are not sure with what have a look at the **TODOs** as well as the **Things I Would Like** sections for ideas. If you have any questions contact me (details below).

Special thanks to the following for their contribution:
* [@nickgirardo](https://github.com/nickgirardo)

## Contact
If you would like to contact me please do so over twitter below or through github directly :)    
[@toinfiniityandb](https://www.twitter.com/toinfiniityandb)
