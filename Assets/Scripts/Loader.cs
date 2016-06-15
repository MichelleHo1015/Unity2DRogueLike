using UnityEngine;
using System.Collections;


	
	public class Loader : MonoBehaviour 
	{
		public GameObject gameManager;			//prefabs中的gamemanager预设
		public GameObject soundManager;			//prefabs中的soundmanager预设
		
		
		void Awake ()
		{
			//检测游戏场景中是否有gamemanager的实例
			if (GameManager.instance == null)
				
				//没有的话为其添加gamemanager这个实例
				Instantiate(gameManager);
			
			//检测游戏场景中是否有soundmanager的实例
			if (SoundManager.instance == null)
				
				//没有的话为其添加gamemanager这个实例
				Instantiate(soundManager);
		}
	}
