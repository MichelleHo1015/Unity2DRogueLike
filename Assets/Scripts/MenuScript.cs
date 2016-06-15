using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour {

	public void Quit(){
		Application.Quit ();
	}

	public void StartGame(){
//		GameManager.instance.RestartDay ();
		GameObject.Find("GameManager").GetComponent<GameManager>().RestartDay();
//		Application.LoadLevel("main");
	}
}