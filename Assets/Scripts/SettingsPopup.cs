using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsPopup : MonoBehaviour
{
    //called when using onclick event fromt he gui button.
    public void OnSoundToggle(){
        Managers.Audio.soundMute = !Managers.Audio.soundMute; //toggle the bool of soundMute
    }

//read the slider value and change the volume accoridngly using the onclccik/onchange stuff. 
    public void OnSoundValue (float volume){ // this argument here is gotten from the slider, it's passed fromt he slider script/data into this, passing it's core data along. neato. 
        Managers.Audio.soundVolume = volume;
    }
}
