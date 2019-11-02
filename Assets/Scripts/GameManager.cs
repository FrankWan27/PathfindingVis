using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    FloorManager fm;

    void Start()
    {
        fm = GetComponent<FloorManager>();
        //Create Ground
        fm.Create();
        fm.Instantiate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
