using UnityEngine;
using System.Collections;


	public class Wall : MonoBehaviour
	{
		public AudioClip chopSound1;				//音效
		public AudioClip chopSound2;				
		public Sprite dmgSprite;					//破坏障碍时障碍的渲染
		public int hp = 3;							//墙壁的hp值
		
		
		private SpriteRenderer spriteRenderer;		
		
		
		void Awake ()
		{
			//获取到sprite渲染
			spriteRenderer = GetComponent<SpriteRenderer> ();
		}
		
		
		//每次破坏墙壁执行的函数
		public void DamageWall (int loss)
		{
			//播放音效
			SoundManager.instance.RandomizeSfx (chopSound1, chopSound2);
			
			//渲染被破坏的障碍
			spriteRenderer.sprite = dmgSprite;
			
			//减去hp一个
			hp -= loss;
			
			//如果障碍完全被破坏
			if(hp <= 0)
				//不再渲染此物体
				gameObject.SetActive (false);
		}
	}
