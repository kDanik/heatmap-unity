# Unity Heatmap


## About 
Unity Heatmap is Unity3D package built with C#. It allows recording of different Vector3 positions for different events(for example movement, some in-game events or positions of points on other object where to players camera is facing), storing it, calculating and then visualising it with usage of heatmap.

A heatmap (or heat map) is a data visualization technique that shows magnitude of a phenomenon as color in two dimensions (or sometimes in three dimensions ).
This package offers heatmap that is higly configurable and can deal with big ammounts of data.
<p align="center">
    <img src="https://github.com/kDanik/heatmap-unity/blob/main/Dist/Assets/heatmap-screenshot2.png" width=450 height=220/>
  &nbsp; &nbsp;
    <img src="https://github.com/kDanik/heatmap-unity/blob/main/Dist/Assets/heatmap-screenshot1.png" width=350 height=220/>
</p>

##  Settings
   <img width="1395" alt="Settings" src="https://user-images.githubusercontent.com/86569730/209441771-3eca3d47-9ed7-43c6-86d4-a87fc9fd59bb.png">


### Button - Load events from file
Loads events from file with provided path and stores data in memory. After reading event you will see options in "Events" setting.

### Button - Initialize particle system
Creates and configures particle system for heatmap on this gameobject. You should activate it if you changed configuration of heatmap (such as particle distance or particle size) in run-time.

### Button - Generate heatmap
Adds events to heatmap and calculates color for its particles.

### Button - Update heatmap
Updates / Reapplies changes to color settings in heatmap, without reloading events data. Can be a little faster for small changes, than "Generate heatmap".

### Button - Reset heatmap
Resets color of particles in heatmap

### Events
Here you can choose which events from provided file should be visualised

### Distance between particles
This setting changes distance between particles. Smaller distance will result into more particles in total and worse performance, but smoother heatmap.



**After changing this setting "Initialize particle system" MUST be called.**

### Particle size
Changes size of each particle. Mostly changes how smooth heatmap looks like and if doesn't have gaps between particles. If performance is important for big heatmaps particle size and distance between particles should be increased (less particles in total).



**After changing this setting "Initialize particle system" MUST be called.**

### Coloring Multiplier
Changes how much each event changes color of particles near it (depending on distance, but this value is maximum value). For bigger data sets should be decreased, for smaller increased.

### Coloring Distance
Changes distance in which each event changes particle color.

### Gradient
Gradient that is used after calculation of color value of particle (from 0 to 1) to determine its real color.

You can use your own gradients, different colors, different transparency and etc.
   
### Color Cutoff
This setting is used to determine which particles with which color value should be displayed, and which not.

If the value here is 0.5 all particles with color value less than 0.5 will not be displayed.

Setting this value at 0 will display ALL particles, also particles that have 0 color value (no event data nearby).

### Height in particles 

By default heatmap uses size of its gameobjects collider to calculate height of heatmap in particles. 

If you want to override it this setting can be used (also should be used, when "Ignore height for color calculations" setting is on)



**After changing this setting "Initialize particle system" MUST be called.**

### Ignore height for color calculations
If this setting is on, y axis of events data will be ignored while calculating heatmap. That will result into flat(2D heatmap).

If this setting is off, generated heatmap will be 3D.
