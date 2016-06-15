using UnityEngine;
using System.Collections;

	public class SoundManager : MonoBehaviour 
	{
		public AudioSource efxSource;					
		public AudioSource musicSource;					
		public static SoundManager instance = null;					
		public float lowPitchRange = .95f;				
		public float highPitchRange = 1.05f;			
		
		
		void Awake ()
		{
			//是否已存在soundmanager
			if (instance == null)
				
				//没有的话设置为自己
				instance = this;
			
			else if (instance != this)
				
				Destroy (gameObject);
			
			DontDestroyOnLoad (gameObject);
		}
		
		

		public void PlaySingle(AudioClip clip)
		{
			
			efxSource.clip = clip;
			
			//播放音效
			efxSource.Play ();
		}
		
		
		//随意播放某一段音效
		public void RandomizeSfx (params AudioClip[] clips)
		{
			//随意截取某一段音效
			int randomIndex = Random.Range(0, clips.Length);

			//随意进行音效变调
			float randomPitch = Random.Range(lowPitchRange, highPitchRange);
			
			//d得到随意改变后的音效
			efxSource.pitch = randomPitch;
			
			efxSource.clip = clips[randomIndex];
			
			efxSource.Play();
		}
	}
