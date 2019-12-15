using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeManager : MonoBehaviour
{
    GameManager gm;
    Image wallToggle;
    Image pathToggle;
    Image heightToggle;
    public Sprite wall, wallPressed, path, pathPressed, height, heightPressed;

    private void Start()
    {
        gm = GameObject.FindObjectOfType<GameManager>();
        wallToggle = GameObject.Find("WallMode").GetComponent<Image>();
        pathToggle = GameObject.Find("PathMode").GetComponent<Image>();
        heightToggle = GameObject.Find("HeightMode").GetComponent<Image>();
    }

    public void SetWall()
    {
        gm.wallMode = true;
        gm.pathMode = false;
        gm.heightMode = false;
        wallToggle.sprite = wallPressed;
        pathToggle.sprite = path;
        heightToggle.sprite = height;
    }

    public void SetPath()
    {
        gm.pathMode = true;
        gm.wallMode = false;
        gm.heightMode = false;
        pathToggle.sprite = pathPressed;
        wallToggle.sprite = wall;
        heightToggle.sprite = height;

    }

    public void SetHeight()
    {
        gm.pathMode = false;
        gm.wallMode = false;
        gm.heightMode = true;
        pathToggle.sprite = path;
        wallToggle.sprite = wall;
        heightToggle.sprite = heightPressed;

    }
}
