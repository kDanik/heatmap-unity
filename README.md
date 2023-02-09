# Unity Heatmap


## About 
Unity Heatmap is a Unity3D package built with C#. It allows recording of different Vector3 positions for different events(for example movement, some in-game events, or positions of points on another object where to player’s camera is facing), storing it, calculating, and then visualizing it with the usage of the heatmap.

A heatmap (or heat map) is a data visualization technique that shows the magnitude of a phenomenon as color in two dimensions (or sometimes in three dimensions ).
This package offers a heatmap that is highly configurable and can deal with big amounts of data.
<p align="center">
    <img src="https://github.com/kDanik/heatmap-unity/blob/main/Dist/Assets/heatmap-screenshot2.png" width=450 height=220/>
  &nbsp; &nbsp;
    <img src="https://github.com/kDanik/heatmap-unity/blob/main/Dist/Assets/heatmap-screenshot1.png" width=350 height=220/>
</p>

##  Settings
  <img width="1712" alt="Screenshot 2023-01-01 at 18 04 59" src="https://user-images.githubusercontent.com/86569730/210179068-2789e1ce-960a-4fcd-94b5-662c570693f0.png">


### Button - Load events from a file
Loads events from file with provided path and stores data in memory. After reading the event you will see options in the "Events" setting.

### Button - Initialize particle system
Creates and configures particle system for heatmap on this game object. You should activate it if you changed the configuration of the heatmap (such as particle distance or particle size) in run-time.

### Button - Generate heatmap
Adds events to heatmap and calculates color for its particles.

### Button - Reset heatmap values
Resets the color of particles in the heatmap to default

### Events
Here you can choose, which events from the provided file should be visualized

### Distance between particles
This setting changes the distance between particles. A smaller distance will result in more particles in total and worse performance, but a smoother heatmap.



**After changing this setting "Initialize particle system" MUST be called.**

### Particle size
Changes the size of each particle. Mostly changes how smooth the heatmap looks and if doesn't have gaps between particles. If performance is important for big heatmaps particle size and distance between particles should be increased (fewer particles in total).



**After changing this setting "Initialize particle system" MUST be called.**

### Coloring Multiplier
Changes how much each event changes the color of particles near it (depending on distance, but this value is the maximum value). For bigger data sets should be decreased, and for a smaller increases.

### Coloring Distance
Changes distance in which each event changes particle color.

### Gradient
The gradient is used after the calculation of the color value of the particle (from 0 to 1) to determine its real color.

You can use your gradients, different colors, different transparency, etc.
   
### Color Cutoff
This setting is used to determine which particles with which color value should be displayed, and which not.

If the value here is 0.5 all particles with a color value less than 0.5 will not be displayed.

Setting this value at 0 will display ALL particles and particles with 0 color value (no event data nearby).

### Height in particles 

By default, heatmap uses the size of its game objects collider to calculate the height of the heatmap in particles. 

If you want to override it this setting can be used (also should be used, when the "Ignore height for color calculations" setting is on)



**After changing this setting "Initialize particle system" MUST be called.**

### Ignore height for color calculations
If this setting is on, the y-axis of events data will be ignored while calculating the heatmap. That will result in a flat(2D heatmap).

If this setting is off, generated heatmap will be 3D.


## TODO at some point
- Add in memory recorder and reader (to record and visualize data without writing to file = live)
- Maybe debug and test performance to allow even bigger data sets / faster visualization
- ...... more than just heatmap as visualization option ?
