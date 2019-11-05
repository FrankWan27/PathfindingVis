using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeManager : MonoBehaviour
{
    GameManager gm;
    Image wallToggle;
    Image pathToggle;
    public Sprite wall, wallPressed, path, pathPressed;
    
    private void Start()
    {
        gm = GameObject.FindObjectOfType<GameManager>();
        wallToggle = GameObject.Find("WallMode").GetComponent<Image>();
        pathToggle = GameObject.Find("PathMode").GetComponent<Image>();
    }

    public void SetWall()
    {
        gm.wallMode = true;
        gm.pathMode = false;
        wallToggle.sprite = wallPressed;
        pathToggle.sprite = path;
    }

    public void SetPath()
    {
        gm.pathMode = true;
        gm.wallMode = false;
        pathToggle.sprite = pathPressed;
        wallToggle.sprite = wall;

    }
}
