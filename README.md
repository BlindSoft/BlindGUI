BlindGUI - powerful and flexible GUI Framework
=====================================================

blindGUI is powerful and flexible GUI framework which support translation, rotation and scale of elements in hierarchy or without it. 

This framework gives full control under graphical elements through the code. 

In blindGUI you can control element with pixel-perfect quality.

blindGUI is ideal for fast and simple GUI creation.

Examples:
---------

Layout control: http://blind-soft.info/blindgui/examples/topic/example1

2D selection of 3D objects: http://blind-soft.info/blindgui/examples/topic/example2

Dynamical indicators: http://blind-soft.info/blindgui/examples/topic/example3

Scripts:
--------
2D pointers to 2D and 3D objects: http://blind-soft.info/blindgui/scripts/pointers


Installation
------------
Just copy blindGUI library to assets/Plugins folder.


Version history
---------------

v1.2.4
------
- Fixed bug with fonts.

v1.2.3 [ 29.02.2012 ]
---------------------

- Text area now has "Word Warp" propertie. Long lines of text will be warped automatically.
- blindGUIButton now has new pair of delegates with mouse button information.

- New section on website: scripts. There will be some useful blindGUI cases.
2D pointers to 2D and 3D objects already there. 

v1.2.0 [ 09.12.2011 ]
---------------------

- The scale controls of GUIController were updated. 
Now BlindGUI contains 4 scale modes (Fit all, Fill all, Fit width, Fit height) and 3 scale directions (Shrink, Grow, Both)

v1.1.0 [ 29.11.2011 ]
---------------------

- Fixed bug with import blindGUI to large projects
- blindGUIColorTiledTexturedContainer now works with 1x1 texture and renamed to blindGUIColorTexturedContainer
- blindGUIMovieTexturedContainer moved to separate .cs file, because of MovieTexture conflict on mobile platforms
- Windowsless property of blindGUILayer was removed due to incorrect clipping at rotation with windows. Now evrything is windowless.

v1.0.0 [ 29.10.2011 ]
---------------------

- Base version uploaded