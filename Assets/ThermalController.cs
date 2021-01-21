using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThermalController : MonoBehaviour
{

    public Slider Power;
    public Slider Cooling;
    public Slider Pressure;
    public Slider Flow;

    public GameObject Metal;
    public GameObject Water;


    public float temperature;

    float maxFlutter = 0.18f;
    float flutter;

    public float filmSize = 1.0f;

    public float Tsat;
    float filmRate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //set power apperence of metal
        Metal.GetComponent<Renderer>().material.SetFloat("Power", Power.value);

        //update fluiod temperature
        temperature += (Power.value - Cooling.value) * Time.deltaTime*0.1f;

        //anti temperature wind up
        if (temperature > 1)
            temperature = 1;
        if (temperature < 0)
            temperature = 0;

        //set nucleate boiling
        flutter = (temperature*maxFlutter)*(1.2f-Pressure.value);
        Water.GetComponent<Renderer>().material.SetFloat("TurbulenceStrength", flutter);

        //Tsat calc
        Tsat = Mathf.Log(Pressure.value+1.1f)+0.1f;

        //set film size
       // filmRate = (Tsat - temperature) * Time.deltaTime;

        if(temperature > Tsat)
            filmRate = (Tsat - temperature) * Time.deltaTime;
        else
            filmRate = (Tsat - temperature) * Time.deltaTime*10;

        filmSize += filmRate;

        if (filmSize > 1)
            filmSize = 1;
        if (filmSize < 1.0f-Power.value*0.6f )
            filmSize = 1.0f - Power.value * 0.6f;


        Metal.GetComponent<Renderer>().material.SetFloat("HTC", filmSize);

        Water.GetComponent<Renderer>().material.SetFloat("FlowWidth", filmSize);
    }
}
