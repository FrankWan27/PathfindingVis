using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderText : MonoBehaviour
{
    Text sliderText;
    FloorManager fm;
    // Start is called before the first frame update
    void Start()
    {
        fm = FindObjectOfType<FloorManager>();
    }

    // Update is called once per frame
    void Update()
    {
        sliderText = GetComponent<Text>();
        sliderText.text = fm.floor.GetLength(0) + " x " + fm.floor.GetLength(1);
    }
}
