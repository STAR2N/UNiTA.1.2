using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycle : MonoBehaviour {
 
    public bool StartSun;
    public Color32 DawnFogColor;
    public float DawnFogIntensity;
    public Color32 MorningFogColor;
    public float MorningFogIntensity;
    public Color32 NightFogColor;
    public float NightFogIntensity;

    public Color32 DawnSunColor;
    public Color32 MorningSunColor;
    public Color32 SunsetSunColor;
    public Color32 NightSunColor;
    public Color32 SunsetFogColor;
    public float SunsetFogIntensity;
   
    public float DawnTime = 0.2f;
    public float MorningTime= 0.4f;
    public float SunsetTime = 0.75f;
    public float NightTime = 0.85f;
    public float CurrentTimeOfDay=0;
    public float SecondsInFullDay=120f;
    public float TimeMultiplier=1f;
    
	// Use this for initialization
	void Start () {
     
        

    }
	
	// Update is called once per frame
	void Update () {

        //If you want to start the Day and night Cycle
        if (StartSun)
        {
            CurrentTimeOfDay += (Time.deltaTime / SecondsInFullDay) * TimeMultiplier;
            gameObject.transform.localRotation = Quaternion.Euler((CurrentTimeOfDay * 360f) - 90, 170, 0);

            // If the cycle of the day has ended restart it
            if (CurrentTimeOfDay >= 1)
                CurrentTimeOfDay = 0;

            // Is Dawn?
            if(CurrentTimeOfDay > DawnTime && CurrentTimeOfDay < MorningTime)
            {
                gameObject.GetComponent<Light>().color = Color.Lerp(gameObject.GetComponent<Light>().color, DawnSunColor, Time.deltaTime * TimeMultiplier/6);
                RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, DawnFogColor, Time.deltaTime * TimeMultiplier/6);
                RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, DawnFogIntensity, Time.deltaTime * TimeMultiplier / 6);
            }

            // Is Morning?
            if (CurrentTimeOfDay > MorningTime && CurrentTimeOfDay < SunsetTime)
            {
                gameObject.GetComponent<Light>().color = Color.Lerp(gameObject.GetComponent<Light>().color, MorningSunColor,Time.deltaTime * TimeMultiplier/6);
                RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, MorningFogColor, Time.deltaTime * TimeMultiplier/6);
                RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, MorningFogIntensity, Time.deltaTime * TimeMultiplier / 6);
            }
            // Is Sunset?
            if (CurrentTimeOfDay > SunsetTime && CurrentTimeOfDay < NightTime)
            {
                gameObject.GetComponent<Light>().color = Color.Lerp(gameObject.GetComponent<Light>().color, SunsetSunColor, Time.deltaTime * TimeMultiplier/6);
                RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, SunsetFogColor, Time.deltaTime * TimeMultiplier/6);
                RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, SunsetFogIntensity, Time.deltaTime * TimeMultiplier / 6);
            }
            // Is NightTime?
            if (CurrentTimeOfDay > NightTime )
            {
                gameObject.GetComponent<Light>().color = Color.Lerp(gameObject.GetComponent<Light>().color, NightSunColor, Time.deltaTime * TimeMultiplier / 6);
                RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, NightFogColor, Time.deltaTime * TimeMultiplier / 6);
                RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, NightFogIntensity, Time.deltaTime * TimeMultiplier / 6);
            }
        }
	}
}
