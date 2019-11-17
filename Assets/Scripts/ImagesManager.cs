using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ImagesManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status {get; private set;}

    private NetworkService _network;
    
    private Texture2D _webImage; // variable to store the downloaded image

    public void Startup(NetworkService service){
        Debug.Log("Images Manager starting...");

        _network = service;

        status = ManagerStatus.Started;
    }

    public void GetWebImage(Action<Texture2D> callback){
        // check if the image is already stored
        if (_webImage == null){ // is there's no web image
            StartCoroutine(_network.DownloadImage((Texture2D image) =>{ _webImage = image; callback(_webImage); } )); //use lambda to store image and create a callback to return it 
        } // store the downlaoded image (_webimage = iamge), callback is used in lmba fucntion instead of sent directy to network service. 
        else // if there IS a web image already stored
        {
            callback(_webImage); // just go ahead and send it. 
        }//invoke callback right away (dont' download) if there'sa stored image. 
    }
}
