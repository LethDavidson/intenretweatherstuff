using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//displays image downladoed from itnernet
public class WebLoadingBillboard : MonoBehaviour
{

    public void Operate(){
        //calls the method in the image manager
        Debug.Log("use recieved");
        Managers.Images.GetWebImage(OnWebImage);
    }

//OH, right, the public facing methods are all "tell the privte method to do the hting", abosultely no letting outside shit direclty touch importnat stuff.
    private void OnWebImage(Texture2D image){
        Debug.Log("changing terxure");
        GetComponent<Renderer>().material.mainTexture = image; // downled image is applied to the matieral in the callback
    }

}
