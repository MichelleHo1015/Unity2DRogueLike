using UnityEngine;
using System.Collections;

	using System.Collections.Generic;		
	using UnityEngine.UI;				

	public class Pause : MonoBehaviour {
		public bool isPaused ;				
		public string quitGame;
		public GameObject PauseCanvas;


		// Update is called once per frame
		void Update () {
			if (isPaused) {						//暂停bool为真
				Time.timeScale = 0;				//实现暂停
				PauseCanvas.SetActive (true);	//遮罩层出现
			} else { 
				PauseCanvas.SetActive (false);
				Time.timeScale = 1;
			}
			if(Input.GetButtonDown("Pause"))	//奇数次点击出现遮罩，偶数次点击关闭遮罩
			   isPaused = !isPaused;
		}

		public void Resume(){ 			//暂停时点击继续游戏可继续
			isPaused = false;
		}

		public void Quit(){			//结束时点击退出游戏可退出
			Application.Quit();
		}

//		public void Restart(){
//
////			gameManagerScript = GetComponent<GameManager>();
////			InitGame ();
////			gameManagerScript.ResetLevel ();
////			Application.LoadLevel (Application.loadedLevel);
//
//		}
		public void Restart(){			//调用gamamanager脚本中的重新开始类
			
			GameManager.instance.RestartDay ();
			
		}

		public void Retry(){			//游戏结束后的重新开始游戏

			GameManager.instance.Retry ();
	
		}

		public void ClearHighScore(){
			PlayerPrefs.SetInt("HighScore",0);
		}
		

	}
