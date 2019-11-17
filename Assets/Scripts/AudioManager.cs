using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//handle all the audio
public class AudioManager : MonoBehaviour, IGameManager {
    public ManagerStatus status { get; private set; }

    private NetworkService _network;

//volume control propties, speicific dif get/set repsosnes, not jsut reutning self. 
    public float soundVolume {
        // get whatever audipo source hti sis tied to, or modify it based on passed values
        get {return AudioListener.volume;}
        set {AudioListener.volume = value;}
    }

// check/set if sound is paused/muted, audip playback muted
    public bool soundMute {
        get {return AudioListener.pause;}
        set {AudioListener.pause = value;}
    }

    public void Startup (NetworkService network) {
        Debug.Log ("Audio Manager starting up...");

        _network = network;

        // init msuic sources here, long running tasks go here afterall. 
        soundVolume = 1.0f; // call the function, looks like this cuz it's an internal get, and becuase it does verry dif things for it's get/set, this is working jsut fine. 
        
        

        status = ManagerStatus.Started; // if there are long running tasks (like when we grab the music, switch this to initalzing and make the confirmn status show up in the other ting when done)
    }

}