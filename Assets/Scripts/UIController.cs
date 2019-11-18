using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private SettingsPopup popup; // refeence popuip object in the scene


    void Start(){
        popup.gameObject.SetActive(false); // initalizes the hidden popup
    }

    void Update(){
        //toggle the popup with the M key
        if (Input.GetButtonDown("Settings")){
            bool isShowing = popup.gameObject.activeSelf; // check if you're up or not
            popup.gameObject.SetActive(!isShowing); // toggle it, setting yourself to the opposite of waht ou currently are,. 
            
            //toggle the cursor lock state alongside the popup. 
            if (isShowing){
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Debug.Log("Locked cursor");
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Debug.Log("Unlocked curosr");
            }
        }

    }


}
