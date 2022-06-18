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

   
##  Task list

- [x] Add basic functionality
- [x] Implement interface to allow different options for writing and reading of data (interface and implementation)
- [x] Implement example of writing and reading data to / from file in CSV format
- [x] Implement example of writing and reading data to / from file in JSON format
- [ ] Refactor/Rename/Simplify BaseEvent, EventData and EventPosition classes
- [ ] Implement example of writing and reading data to / from DB
- [ ] Make setup of package easier and write setup guide
