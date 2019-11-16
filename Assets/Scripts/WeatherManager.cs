using System.Collections;
using System.Collections.Generic;
using MiniJSON;
using UnityEngine; // adding the namespace we downloaded, setupin that script
using System.Xml; // namespace for reading xml shit

// weathermanager handles the getting of data and managing what'll be fed to the controller, and the ocntrolelr is ht ehting that's direclty fuckign with the werather
public class WeatherManager : MonoBehaviour, IGameManager {
    public ManagerStatus status { get; private set; }

    //add cloud value here, the time incrimental stuff. 
    private NetworkService _network;

    public float cloudValue { get; private set; } // cloudiness is modified interanally but read onl elsewerae . what we're gonna put the cdonvverted cloud node data into fromthe parsed xml. 

    private const string cloudNode = "clouds";
    private const string cloudValueNode = "value";
    private const string allNodeValue= "all";

    public void Startup (NetworkService service) {
        Debug.Log ("Weather manager starting...");

        _network = service; // store the injehcted networkservice object
        StartCoroutine(_network.GetWeatherXML(OnXMLDataLoaded));

        //StartCoroutine(_network.GetWeatherJSON(OnJSONDataLoaded));

        status = ManagerStatus.Initializing;
        //makes the status initalizing becuase this startup method will likely finish before the internet request gets back
        //this is one of htose longer running tasks that coroutines are good for, the rest of ht estattup can get goig
        // but another part will finish up later. that's rad. 
    }

    //parse allt aht xml weather data
    public void OnXMLDataLoaded (string data) {
        Debug.Log("XML started");
        XmlDocument doc = new XmlDocument ();
        doc.LoadXml (data); // parse xml into a searchble strucure, filled as this is ran in network services's corotuine via a callback
        XmlNode root = doc.DocumentElement;

        XmlNode node = root.SelectSingleNode (cloudNode); // pull out a single node form the data, this here assuming you know waht the nodes are called
        string value = node.Attributes[cloudValueNode].Value;
        cloudValue = XmlConvert.ToInt32 (value) / 100f; // convert the value to a 0-1 flaot. so we can feed it to the sky blend i guess. 
        Debug.Log ("XML weather Value: " + cloudValue);

        Messenger.Broadcast (GameEvent.WEATHER_UPDATED); // boradcast to inform other scripts to do stuff now that the weather data is loaded parsed. 

        status = ManagerStatus.Started; //once this is ifnished, this module will ahve finished all it's laoding and thus THIS is where it hits started and not in the startup
    }

// parse json data, put into a dict instead of cusotm xml contianers
    public void OnJSONDataLoaded(string data){
        Dictionary<string, object> dict; // name, kind of object
        dict = Json.Deserialize(data) as Dictionary<string, object>; // format and type the json string data autoamtically into a dictionary build to hold string/ object key values.

        Dictionary<string, object> clouds = (Dictionary<string, object>) dict[cloudNode];// clouds is a dict now, code still does all the same stuf,f it's just sorted/held in a dif way
        // OH oka, so we have dict sotring that info, and this new dict is speicfically a dict made out of the name/stirng of that... dict, and the object is the dict cloud
        // okay so it's a dictionary made up fo dicitonaries. okay yeah that's rad. i love those.
        // ig uess that's what the ndoe and stirng values up top are doing, sorting and saving all this info into custom formats that amount to aidcitoanry holding dictionaries, a reference object for several compelx objects. i lvoe dicts in dicts. DOCKING

        cloudValue = (long) clouds [allNodeValue] / 100f; // dif synatx,same as xml
        Debug.Log("JSON weather Value: " + cloudValue);

        Messenger.Broadcast(GameEvent.WEATHER_UPDATED);

        status = ManagerStatus.Started;
    }

}