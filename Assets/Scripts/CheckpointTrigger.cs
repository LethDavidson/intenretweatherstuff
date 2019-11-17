using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//save data when hit, atm it tells weather and network systems to log and upload weather data to a server.
public class CheckpointTrigger : MonoBehaviour
{
    public string identifier = "Checkpoint";

    private bool _triggered; // track if the checkpoint has alread been triggered

    public void OnTriggerEnter(Collider other){
        if (_triggered) {return;}

        Managers.Weather.LogWeather(identifier); // call to send data
        _triggered = true; // don't send again if entered
    }
}
