using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkService {
    private const string xmlApi = "http://api.openweathermap.org/data/2.5/weather?q=Chicago,us&mode=xml&APPID=0e033b969b12be2186e4a4b1f5bbbb39"; // the url to send the request to
    // putting it in const cuz it's never gonan change. 
    private const string jsonApi = "http://api.openweathermap.org/data/2.5/weather?q=Chicago,us&APPID=0e033b969b12be2186e4a4b1f5bbbb39";

//addreess of th eserver side script, change to wherever it is. 
    private const string localApi = "http://localhost/uia/api.php";

    private const string webImage = "http://upload.wikimedia.org/wikipedia/commons/c/c5/Moraine_Lake_17092005.jpg"; 



    //create unitywebrequest in get mode, dor claling api weather hsit
    private IEnumerator CallAPI (string url, WWWForm form, Action<string> callback) {
        //using (UnityWebRequest request = UnityWebRequest.Get(url)){ //getmode, ready go go get info
        using (UnityWebRequest request = (form == null) ? UnityWebRequest.Get (url) : UnityWebRequest.Post(url, form)) { //getmode, ready go go get info
// if the form is empty,  get the url. if not, then post the post using the wwwform format

            yield return request.SendWebRequest (); // hold this function untl i ge the info

            //check for errors inthe response we get
            if (request.isNetworkError) { // if it's fucked
                Debug.LogError ("network problem: " + request.error); // get whatever error there is 
            } else if (request.responseCode != (long) System.Net.HttpStatusCode.OK) { // if our code does not caontain an "All good" code
                Debug.LogError ("response error: " + request.responseCode); // so storrwe whatever the fucked code is so we can look at it later
            } else {
                callback (request.downloadHandler.text); // if all good, run the delegate method using the new string info as it's passed parameter
            }
        }
    }
    

//corotujnes other shit calls to retrieve and return internet data
    public IEnumerator GetWeatherXML (Action<string> callback) {
        return CallAPI (xmlApi, null, callback); //yield cascades threough the cortoune methods that call eachtoehr. 
        // so... so i guess that as one thign is called and it's waiting for info, other sutff is called or can be  called and get itself togehter?
    }

    // get and return the weather data as a json instead of xml. 
    public IEnumerator GetWeatherJSON (Action<string> callback) {
        return CallAPI (jsonApi, null, callback);
    }// form can be null, so send a null if you're first grabbing it. don't forget youc an just pass nulls

//turn info we've extracted for xml/json into a wwwform format for easier posting to server, 
public IEnumerator LogWeather(string name, float cloudValue, Action<string> callback){
    WWWForm form = new WWWForm(); // define a form with values to send. wwwForm puts data we give itno a format that's easier for unitywebrequst to post to servers.
    //construct the form that you're gonna send, defining fields wih a kind of dicitoanry key/value sturcutrre
    form.AddField("message", name);
    form.AddField("cloud_value", cloudValue.ToString());
    form.AddField("timestamp", DateTime.UtcNow.Ticks.ToString()); // get the local time, turn it to stirng, then add it to this format. importatn for debugging/loggign stuff.

    return CallAPI(localApi, form, callback); // then run... the weather api thing, now passing along the form info we made ontop of the callback
    //OKAY so what wants the weather will call THIS, this cortuone will make the form, then just keep rolling by returning the callapi, passing the callback directly to it and thus api can then 
    // talk to... the hting that orignally called it, and we're now like 2-3 degrees away from eachother but info is being requested, now it's passing through emdiator prep the weather to be able
    // to properly log what's happening, then the weather can then get the data, and send thedata, or just... keep it? 
    
}


//callback will require a texture instead of string
    public IEnumerator DownloadImage(Action<Texture2D> callback){
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(webImage); // special emthod for turning dl'd images into texures
        yield return request.SendWebRequest (); // yield this cortuine until you get the data back from the intenret
        callback(DownloadHandlerTexture.GetContent(request)); // do callback using a function built to handled dl'd images as texture, and pass the speific webimage we dl as the thing to GET content from.
        //reirve downlaoded image using the downloadhandeler utility 
    }

}