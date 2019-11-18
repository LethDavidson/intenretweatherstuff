using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//handle all the audio
public class AudioManager : MonoBehaviour, IGameManager {
    [SerializeField] private AudioSource soundSource; // variable slot in the isnpectro r ref the ne audio source, ie it's child that's gonna handle the 2d stuff
    [SerializeField] private AudioSource music1Source;
    [SerializeField] private AudioSource music2Source;

    private AudioSource _activeMusic; // keep track fo whic si active/inactive
    private AudioSource _inactiveMusic;

    public float crossFadeRate = 1.5f;
    private bool _crossFading; // a toggle to avoid bugs while a crossfade is happening, ensure we're in a fading state, like so we can maqke sure w edon't do a fade when antoehr is goign on.

    //string values holding the names of the speicifc songs we're gonna make shit play
    [SerializeField] private string introBGMusic;
    [SerializeField] private string levelBGMusic;

    public string currentMusicName;

    private float _musicVolume; // private var that won't be accessed directly, only through the prop's getter. 
    public float musicVolume { // the public facing getter/setter for the music volume. again, gatekeeping. 
        get {
            return _musicVolume; // return the private music volume stat
        }
        set {
            _musicVolume = value; // change the private msuic volume, split get/set here due to wanting to keep it consoldiated.

//if we exist adn we aren't currently crossfading, adjsut the music on both to ensure we're awlays keeping where we wnat 
            if (music1Source != null && !_crossFading) { //
                music1Source.volume = _musicVolume;
                music2Source.volume = _musicVolume; // adjust the volme on both music

                Debug.Log ("Current music volume: " + _musicVolume);
            }
            else {
            Debug.Log ("music1Source doesn't exist, cannot fuckw ith volume. ohno");
        }
        
        }

    }

    public bool musicMute { // pass the music1store audio source a mute true/false directly, rather than actually STOPPING the music ala the current onclick button
        get {
            if (music1Source != null) { // if we exist
                return music1Source.mute; // used to check if we're muted or not. possible for like, a !mute or to ensure we arn't on mute by mistake or something. making sure, like to have a tickbox check adn fill or not fill if it is or not
            } //return if we're muted or not. 
            Debug.Log ("music1Source doesn't exist. Cannot get the mute value. ohno");
            return false; // retrun false if we don't eixst actually, like if the hitng palying the audio isnt' arind. 
        }
        set {
            if (music1Source != null) { // alwyus check to make sure we eixst
                music1Source.mute = value; // this isa bool function cuz this sertter needs to tkae a bool. value is what's passed when it's called, jsut like musicVolume's value is a float cuz it's pased that
                music2Source.mute = value; // if we're mutignthe musc, we want both music sterams to be muted.
                if (music1Source.mute == true) {
                    Debug.Log ("music1 muted");
                } else {
                    Debug.Log ("music1 unmuted");
                }
            } else {
                Debug.Log ("msuic1source doens't exist, can't tell tell it to mute.");
            }
        }
    }

    //setup playing intor, level and music in general
    //we;re treating intor and level dif cuz i guess we wnat o be able to fade. so like defning channel 1/channel 2 like we would ina bigger project
    //load intro music then stream it to playmusic
    public void PlayIntroMusic () {
        PlayMusic (Resources.Load ("Music/" + introBGMusic) as AudioClip);
    }

    //load and sream level music then play to lpakyixc
    public void PlayLevelMusic () {
        PlayMusic (Resources.Load ("Music/" + levelBGMusic) as AudioClip); //convert to audioclip type so that the playmusic function can figure it out
    }

    // play the actual music we're streaming, the playinto/loading stuff passes to this and this acutally plays
    public void PlayMusic (AudioClip clip) { // play music by setting the aduisouce. 
        if (_crossFading) {return;} // if we're already fading, just reutn that value and fuck off. 
        StartCoroutine(CrossFadeMusic(clip));

        //old code when there was only 1 msusic
        // Debug.Log ("about to play this music: " + clip.name);
        // music1Source.clip = clip;
        // music1Source.Play ();
        // currentMusicName = music1Source.clip.name;
        // Debug.Log ("Playhing" + currentMusicName);
        // //i guss we could just do music1Source.play(newSong)? but then we'd need to hold onto this and other calls wmight get fucked up and lose that info, plus i might wanan track WHAT it's playing.
    }

    public void StopMusic () {
        Debug.Log ("pausing music: " + currentMusicName);
        _activeMusic.Stop ();
        Debug.Log ("stopped: " + currentMusicName);
        _inactiveMusic.Stop (); 
    }

    public ManagerStatus status { get; private set; }

    private NetworkService _network;

    //play sounds that don't have any other source
    public void PlaySound (AudioClip clip) {
        soundSource.PlayOneShot (clip); // just play whatever is passed. so in this case, 2d sounds like ui stuff that settings/ect passes to it, source would be the child object.
        Debug.Log (clip.name);
    }

    //volume control propties, speicific dif get/set repsosnes, not jsut reutning self. 
    public float soundVolume {
        // get whatever audipo source hti sis tied to, or modify it based on passed values
        get { return AudioListener.volume; }
        set { AudioListener.volume = value; }
    }

    // check/set if sound is paused/muted, audip playback muted
    public bool soundMute {
        get { return AudioListener.pause; }
        set { AudioListener.pause = value; }
    }

    public void Startup (NetworkService network) {
        Debug.Log ("Audio Manager starting up...");

        _network = network;

        // these proepries tell the audiosource (thing plahing the sounds) to oingore the settings on any audiolistnee picking up on it, we're running netierly on OUR rules 
        music1Source.ignoreListenerVolume = true;
        music1Source.ignoreListenerPause = true;
        music2Source.ignoreListenerVolume = true;
        music2Source.ignoreListenerPause = true;

        //just going ina nd altering the code to all accocunt for 2 music sources instead of 1.

        soundVolume = 1.0f; // call the function, looks like this cuz it's an internal get, and becuase it does verry dif things for it's get/set, this is working jsut fine. 
        musicVolume = 1.0f;

//initalize one as th eactive and other as backup, so we can swap
        _activeMusic = music1Source;
        _inactiveMusic = music2Source;

//will this still work?
        currentMusicName = "none";

        status = ManagerStatus.Started; // if there are long running tasks (like when we grab the music, switch this to initalzing and make the confirmn status show up in the other ting when done)
    }

//swap which audio source is considered active adn inactive. 
//handle the volume alteration of both the active and backup, as to gradually shift their important adn swap their roles. 
    private IEnumerator CrossFadeMusic(AudioClip clip){
        _crossFading = true; // temporarly make _crossfading true so that nothing tries to fuck with you duirn iot

        _inactiveMusic.clip = clip; // pass the fadetotrack
        _inactiveMusic.volume = 0; // maek sure it's set to 0 so we can start it without interupiton
        _inactiveMusic.Play(); // get it rolling

//how quickly will we scale, detemined by our rate setting and the volume (to know how much it neeeds to scale, distanct traveled, ect. )
        float scaledRate = crossFadeRate * _musicVolume;
        while (_activeMusic.volume >0){ // so continue to scale until the active music hits 0 and inacitve hits the value we require of it, down active and up inactive
            _activeMusic.volume -= scaledRate * Time.deltaTime;
            _inactiveMusic.volume += scaledRate * Time.deltaTime;

            yield return null; // ah, of course, the wait that corotouens needsoemwhere, this is here ot wait a frame between this incrimenting. agian, goign too fast seems like a error waiting to happen/too much cpuy draw
        }

        AudioSource temp = _activeMusic; // temp var to swap active and inactive. 

// swap the active and inactiv, and bring up the active vol (whci was at 0 given it was inactive)
        _activeMusic = _inactiveMusic;
        _activeMusic.volume = _musicVolume;

        _inactiveMusic = temp; // use the temp we made holding actiuve to now fill with inactive. 
        _inactiveMusic.Stop(); // stop playing it. it will being playing again at 0 once corssfade is calle again

        _crossFading = false; // finished crossfading 
    }

}