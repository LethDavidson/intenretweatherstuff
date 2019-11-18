using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the shit that the settigns menu itself direclty controls
//ie, WHAT DO THE BUTTONS DO? THIS SENDS TO COMMANDS TO THE THINGS THAT THE BUTTONS ARE SUPPOSED TO DO, THIS DOENS"T HANDLE UI STUFF SO MUCH AS BE THE TELEPHONE FOR THE UI BUTTONS TO DO WAHT THEY SHOULD BE
public class SettingsPopup : MonoBehaviour {
    [SerializeField] private AudioClip sound; // inspector slot to reference the sound clip
    //a better version fo this would be called like, UI click button sound. or ui highilight option sound (like hl2's purrs and pats of it's menu)

    //called when using onclick event fromt he gui button.
    public void OnSoundToggle () {
        Debug.Log("About to mute the sfx");
        Managers.Audio.soundMute = !Managers.Audio.soundMute; //toggle the bool of soundMute
        Debug.Log("Muted the sfx");
        Managers.Audio.PlaySound (sound); // play the sound effect when the button is pressed so you know that it worked. so yeah mute one/toehr main streams of audio, and jjst play this isolated one. 
        // if it were like, the sound of the xclick was based on the effects volime, you'd play one of the sounds stored and controlled by the effects slider. this is idp atm.
    }

    //read the slider value and change the volume accoridngly using the onclccik/onchange stuff. 
    public void OnSoundValue (float volume) { // this argument here is gotten from the slider, it's passed fromt he slider script/data into this, passing it's core data along. neato. 
        Managers.Audio.soundVolume = volume;
    }

    public void OnMusicToggle(){
        Debug.Log("about to mute the music");
        Managers.Audio.musicMute = !Managers.Audio.musicMute;
        Debug.Log("muted the music");
        Managers.Audio.PlaySound (sound);
    }

    public void onMusicValue(float volume){
        Managers.Audio.musicVolume = volume;
    }

    //SWITCHES :D
    public void OnPlayMusic (int selector) { // which option fromt he switch do we pick?
        Managers.Audio.PlaySound (sound);

        switch (selector) { // a differen tmusic function in audiomanager for each fucntion
            case 1: // play the intro music
                Managers.Audio.PlayIntroMusic ();
                break;

            case 2: // play the level music
                Managers.Audio.PlayLevelMusic ();
                break;

            default: // the default option is to pause music
                Managers.Audio.StopMusic();
                break;
        }
    }

}