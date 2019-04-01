# RTS_4
3D RTS game in unity. UI Improvements from the engine I made with SDL(RTS_3). 

Download And Play (Legacy, not as stable 0.5)
-----------------------------------------------

Windows:
https://www.dropbox.com/s/dg9mabis9nelmip/RTS4_0.5_win_release.rar?dl=0

MacOSX(I don't have a mac anymore so this is completely untested, and probably won't even unzip correctly):
https://www.dropbox.com/s/sfsqvnzdgc8wz7t/RTS4_0.5_mac_release.7z?dl=0

Download And Play (Stable 0.4)
-------------------------------------

Windows:
https://www.dropbox.com/s/d7simemkf90u0ny/RTS4_0.4_win_release.rar?dl=0

MacOSX:
https://www.dropbox.com/s/xivoze9woksrcbj/RTS4_0.4_mac_release.zip?dl=0

Controls
---------

alt + rightmouse + mouse) tilt the camera

left click) select units and select menu items

right click) Open the PopMenu for giving an action to an entity

ctrl + leftclick) In the PopMenu ctrl + left click one of the options to queue actions, the action queue is shown in one of the side bar tabs.

mouse wheel) zoom in and out

Rick click outside map) Unit will move to the nearest unexplored tile and explore it.

The controls take a bit of getting used to because some of the actions are continuous and won't be overriden if you're holding ctrl when you select the action. If in doubt look at the Action info tab in the sidebar.

Actions
-------------
* Construct
* Collect
* Procreate
* Garrison
* Attack
* Move
* Construct(New town)
* Exchange

Notes
-------------
* If a new town is started, the units usually die of hunger before they have time to build the town. To avoid this, give them hand carts full of food before sending them out.

Issues
---------
* In the mac version a green temporary plane that I use as a visual aid when making models is visible as a glitch below some of the map. This is not visible in the Windows version 
* Garrisoned Units or Procreating Units that get hungry and leave their Garrison to get food at the stockpile often collide with the stockpile model instead of moving beside it. Also the Procreate Action gets cancelled and their Action queue gets cleared, the Units are supposed to return to the Action they were doing before they went to Eat.
* There is a display bug where the cation symbols also display in the sky above the camera. If there was a 
limit on the camera tilt then this wouldn't be visible, but the camera can tilt in any direction, which 
creates lots of visual problems.

