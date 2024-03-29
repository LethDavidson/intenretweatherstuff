﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (WeatherManager))]
[RequireComponent (typeof(ImagesManager))]
[RequireComponent (typeof(AudioManager))]
public class Managers : MonoBehaviour {
	public static WeatherManager Weather { get; private set; }
	public static ImagesManager Images {get; private set;}
	public static AudioManager Audio {get; private set;}

	private List<IGameManager> _startSequence;

	void Awake () {
		Weather = GetComponent<WeatherManager> ();
		Images = GetComponent<ImagesManager> (); // forgot to add this, bork emy shit :p
		Audio = GetComponent<AudioManager> ();

		_startSequence = new List<IGameManager> ();
		_startSequence.Add (Weather);
		_startSequence.Add (Images);
		_startSequence.Add (Audio);

		StartCoroutine (StartupManagers ());
	}

	private IEnumerator StartupManagers () {

		NetworkService network = new NetworkService(); //create a new netwroksservice to give to each othe rhte managers

		foreach (IGameManager manager in _startSequence) {
			manager.Startup(network);
		}
 
		yield return null;

		int numModules = _startSequence.Count;
		int numReady = 0;

		while (numReady < numModules) {
			int lastReady = numReady;
			numReady = 0;

			foreach (IGameManager manager in _startSequence) {
				if (manager.status == ManagerStatus.Started) {
					numReady++;
				}
			}

			if (numReady > lastReady)
				Debug.Log ("Progress: " + numReady + "/" + numModules);

			yield return null;
		}

		Debug.Log ("All managers started up");
	}
}