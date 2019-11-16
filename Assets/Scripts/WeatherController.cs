using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Manage the sky/weather stuff
public class WeatherController : MonoBehaviour {
    [Tooltip ("What skybox are we fucking with?")]
    [SerializeField] private Material sky; // reference the material in a project view, not only pobjects in scnee

    [Tooltip ("What's the main light source we're fucking with?")]
    [SerializeField] private Light sun; // the main light source of the scene

    private float _fullIntensity; // store the maximumamount of brightness to owrk from

    // private float _cloudValue = 0f; // for incrimenting weather stuff

    // [Tooltip ("How quickly does it get dark? aka how much does _cloudValue de/incriment per frame?")]
    // public float cloudIncrimentationSpeed = .005f;

// consts
    private const string shaderBlendSlider = "_Blend";
//

    void Awake(){
        Messenger.AddListener(GameEvent.WEATHER_UPDATED, OnWeatherUpdated);
    }

    void OnDestroy() {
        Messenger.RemoveListener(GameEvent.WEATHER_UPDATED, OnWeatherUpdated);
    }

    

    void Start () {
        //initial intnesity of th elight, whatever it starets with, is what we consider Full intensity, and it'll dim from there currently as it turns ot "night" over time. 
        _fullIntensity = sun.intensity;
    }

    private void OnWeatherUpdated(){
        SetOvercast(Managers.Weather.cloudValue); // use the clouniess value fromthe intenret sotred in weathermanger to update our... clouds. 
    }

    // void Update () {
    //     //adjust the weather each frame by passing it this newly updated cloudvalue each frame
    //     if (_cloudValue < 1) {
    //         SetOvercast (_cloudValue);
    //         _cloudValue += cloudIncrimentationSpeed; // incriment this valuye every frame for a contius change, 
    //         //Debug.Log ("_cloudValue = " + _cloudValue);

    //     }
    //     if (_cloudValue > 1.0f) {
    //         _cloudValue = 1.0f;
    //         //Debug.Log("capping");
    //     }

    // }

    // adjust both the mateiral's blend value and th elight's intensity, only adjsutable within this so it's a private
    private void SetOvercast (float value) {
        sky.SetFloat (shaderBlendSlider, value);
        sun.intensity = _fullIntensity - (_fullIntensity * value);
        Debug.Log("weather value?: " + value);
    }

}