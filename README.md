

# Pathfinding Visualizer

Simple pathfinding visualizer created in Unity 

## Installing

Download the latest standalone executable file from the [releases](https://github.com/FrankWan27/PathfindingVis/releases) page (Windows only).

If you have a Mac or Linux computer that can run Unity, just download the entire project and import it as a Unity project.


## About this project
I created this project because I wanted to learn a bit more about pathfinding algorithms and thought it would be cool to see them visualized in 3d space. It's quite useful to see the difference in accuracy and efficiency of different algorithms. For example, while the A* algorithm explored a much greater number of nodes (the blue/green colored nodes), it resulted in a shorter path than the greedy algorithm (the purple line).
![A* on the left, Greedy on the right](https://i.imgur.com/fe5ftqr.png)
## Screenshots

Example of Maze Generation + Path Finding (Red Tile = Start/End, Purple Tile = Path)
![Maze](https://i.imgur.com/8jQHbPr.png)
Example of Random Noise + Path Finding
![2D Noise](https://i.imgur.com/wJXaPoK.png)
Example of Random 3D Noise + Path Finding
![3D Noise](https://i.imgur.com/jroz2QZ.png)
Example of path finding in a maze with walls of height 10. Sometimes it's shorter to follow the path, sometimes it's shorter to jump over walls. (Start is at bottom left, end is at top right)
![3D Maze](https://i.imgur.com/nAaXHeT.png)


## Built With

* [Unity](https://unity.com/) - Version 2018.3.0f2

## Authors

* **Frank Wan** - [Github](https://github.com/FrankWan27)