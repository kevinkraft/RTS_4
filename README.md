# RTS_4
3D RTS game in unity. UI Improvements from the engine I made with SDL(RTS_3). 

Download And Play
----------------------

Windows:
https://www.dropbox.com/s/kuohv550o7uu6br/RTS4_0.3_win_release.rar?dl=0

MacOSX:
https://www.dropbox.com/s/4co4hfb74bajh6f/RTS4_0.3_mac_release.zip?dl=0

In case of problems with the Mac versions, let me know, then try these versions instead:

MacOSX V2:
https://www.dropbox.com/s/ljb1oubtot5umx2/RTS4_0.3_mac_universal_release.zip?dl=0

MacOSX V3:
https://www.dropbox.com/s/zm4zq4mww6pmtln/RTS4_0.3_mac_x86_64_release.zip?dl=0

Controls
---------

alt + rightmouse + mouse) tilt the camera

left click) select units and select menu items

right click) Open the PopMenu for giving an action to an entity

ctrl + leftclick) In the PopMenu ctrl + left click one of the options to queue actions, the action queue is shown in one of the side bar tabs.

The controls take a bit of getting used to because some of the actions are continuous and won't be overriden if you're holding ctrl when you select the action. If in doubt look at the Action info tab in the sidebar.

Issues
---------

* When multiple Units are Constructing a Building some of the Units often get stuck Exchanging Items at the stockpile. The fix is probably simple.
* Garrisoned Units or Procreating Units that get hungry and leave their Garrison to get food at the stockpile often collide with the stockpile model instead of moving beside it. Also the Procreate Action gets cancelled and their Action queue gets cleared, the Units are supposed to return to the Action they were doing before they went to Eat.
