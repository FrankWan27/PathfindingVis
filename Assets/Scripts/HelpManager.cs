using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HelpManager : MonoBehaviour
{
	public GameObject helpPanel;
	public Text algorithmName;
	public Text algorithmInfo;
	public Text mapName;
	public Text mapInfo;



	GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        UpdateHelp();
    }

    public void OpenHelp()
    {
    	helpPanel.SetActive(true);
    }

    public void CloseHelp()
    {
    	helpPanel.SetActive(false);
    }

    public void UpdateHelp()
    {
    	switch(gm.algorithm)
    	{
    		case 0: //floodfill
    			algorithmName.text = "Selected Algorithm: Floodfill";
    			algorithmInfo.text = floodfillInfo;
    			break;
    		case 1: //floodfill3D
    			algorithmName.text = "Selected Algorithm: Floodfill3D";
    			algorithmInfo.text = floodfill3DInfo;
    			break;
    		case 2: //greedy
    			algorithmName.text = "Selected Algorithm: Greedy";
    			algorithmInfo.text = greedyInfo;
    			break;
			case 3: //AStar		
    			algorithmName.text = "Selected Algorithm: A*";
    			algorithmInfo.text = AStarInfo;
    			break;
    		default:
    			break;
    	}

    	switch(gm.mapMode)
    	{
    		case 0: //clear
    			mapName.text = "Selected Map: Clear";
    			mapInfo.text = clearInfo;
    			break;
    		case 1: //wall maze
    			mapName.text = "Selected Map: Wall Maze";
    			mapInfo.text = wallMazeInfo;
    			break;
    		case 2: //tall maze
    			mapName.text = "Selected Map: Tall Maze";
    			mapInfo.text = tallMazeInfo;
    			break;
    		case 3: //2D noise
    			mapName.text = "Selected Map: 2D Noise";
    			mapInfo.text = Noise2DInfo;
    			break;
    		case 4: //3D noise
    			mapName.text = "Selected Map: 3D Noise";
    			mapInfo.text = Noise3DInfo;
    			break;    	
    		case 5: //pillars
    			mapName.text = "Selected Map: Pillars";
    			mapInfo.text = pillarsInfo;
    			break;
    		default:
    			break;
     	}
    }


    string floodfillInfo = "This algorithm simply checks every adjacent tile using a queue until it reaches the destination. This algorithm is guarenteed to return the shortest path in a 2D map, but is not guarenteed to return the shortest path in a 3D map.";
    string floodfill3DInfo = "This algorithm is a modification of the floodfill algorithm, and it checks every tile on the map and will backtrack if there is a better path availiable. This modification will guarentee the algorithm to return the shortest path even in a 3D map, but it is terribly inefficient.";
	string greedyInfo = "This algorithm will always pick the next best adjacent tile, only backtracking when it hits a dead end. This algorithm is very fast, but it is not guarenteed to return the shortest path.";
	string AStarInfo = "This algorithm assigns heuristics to each tile, and then ...... updated later. This algorithm runs slower than the greedy algorithm, but is still quick and is also guarenteed to return the shortest path.";

	string clearInfo = "This option will simply generate a blank map.";
	string wallMazeInfo = "This option will generate a random maze, using the recursive backtracking algorithm. The walls of this maze are wall tiles, which means they are not traversable.";
	string tallMazeInfo = "This option will generate a random maze, using the recursive backtracking algorithm. The walls of this maze are tiles of height 5, which means they are traversable, but it will cost 5 distance to climb the wall, and 5 distance to climb down the wall.";
	string Noise2DInfo = "This option will randomly generate wall tiles, at the rate of 1 wall tile per 2 normal tiles.";
	string Noise3DInfo = "This option will generate a map with randomized heights for each tile, ranging from 0 to 20.";
	string pillarsInfo = "This option will randomly generate tiles with heights ranging from 14 to 20. These pillars will spawn at the rate of 1 pillar tile per 15 normal tiles.";
}
