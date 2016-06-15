using UnityEngine;
using System.Collections;
using UnityEngine.UI;	

	
	public class Player : MovingObject
	{
		public float restartLevelDelay = 1f;		//每次进入新关卡的遮罩展示延时
		public int pointsPerFood = 10;				//每个樱桃的食物值
		public int pointsPerSoda = 20;				//每个可乐的食物值
		public int wallDamage = 1;					//破坏障碍一次减去的食物值
		public Text foodText;						//UI层中的食物值显示
		public AudioClip moveSound1;				//音效
		public AudioClip moveSound2;				
		public AudioClip eatSound1;					
		public AudioClip eatSound2;					
		public AudioClip drinkSound1;				
		public AudioClip drinkSound2;				
		public AudioClip gameOverSound;				
		public AudioClip byebye;
		
		private Animator animator;					//定义动画
		private int food;							//定义食物值
		
		
		protected override void Start ()
		{
			//获取动画组件
			animator = GetComponent<Animator>();
			
			//存储关卡切换时的食物值
			food = GameManager.instance.playerFoodPoints;
			
			//设置UI层显示的食物值
			foodText.text = "Food: " + food;
			
			//调用父类中的base方法
			base.Start ();
		}
		
		
		//角色到达exit之后不可移动的方法
		private void OnDisable ()
		{
			//将当前的食物值赋值到gamemanager中以供下一轮游戏开始后的调用
			GameManager.instance.playerFoodPoints = food;
		}
		
		
		private void Update ()
		{
			//如果当前不是角色的移动回合
			if(!GameManager.instance.playersTurn) return;
			
			int horizontal = 0;  	//初始的x坐标
			int vertical = 0;		//初始的y坐标
			

			
			//获取到键盘输入的x坐标（左还是右）
			horizontal = (int) (Input.GetAxisRaw ("Horizontal"));
			
			//获取到键盘输入的x坐标（前还是后）
			vertical = (int) (Input.GetAxisRaw ("Vertical"));
			
			//如果水平移动，将y轴方向设置为0
			if(horizontal != 0)
			{
				vertical = 0;
			}

			//如果y方向不是0或者x方向不是0
			if(horizontal != 0 || vertical != 0)
			{
				
				//以水平和垂直作为参数看是否可移动
				AttemptMove<Wall> (horizontal, vertical);
			}
		}
		
		
		protected override void AttemptMove <T> (int xDir, int yDir)
		{
			//每次行走倒要减去食物一个
			food--;
			
			//实时显示当前food值
			foodText.text = "Food: " + food;
			
			//以水平和垂直作为参数看是否可移动
			base.AttemptMove <T> (xDir, yDir);
			
			//记录下检测碰撞的结果
			RaycastHit2D hit;
			
			//可以移动且无碰撞
			if (Move (xDir, yDir, out hit)) 
			{
				//播放音效
				SoundManager.instance.RandomizeSfx (moveSound1, moveSound2);
			}
	
			//检查游戏是否结束
			CheckIfGameOver ();
			
			//如果游戏结束禁用角色移动
			GameManager.instance.playersTurn = false;
		}
		
		
		
		protected override void OnCantMove <T> (T component)
		{
			Wall hitWall = component as Wall;
			
			//调用破坏障碍函数
			hitWall.DamageWall (wallDamage);
			
			//开启破坏障碍的动画
			animator.SetTrigger ("playerChop");
		}
		
		
		private void OnTriggerEnter2D (Collider2D other)
		{
			//如果是出口
			if (other.tag == "Exit") {
				//开始下一关
				Invoke ("Restart", restartLevelDelay);
				
				//禁用角色移动
				enabled = false;
			}
			
			//如果是食物
			else if (other.tag == "Food") {
				//加上食物值
				food += pointsPerFood;
				
				//展示当前食物值
				foodText.text = "+" + pointsPerFood + " Food: " + food;
				
				//播放音效
				SoundManager.instance.RandomizeSfx (eatSound1, eatSound2);
				
				//不再渲染食物
				other.gameObject.SetActive (false);
		
				
			}
			
			
			else if (other.tag == "Soda") {
				
				food += pointsPerSoda;
				
				foodText.text = "+" + pointsPerSoda + " Food: " + food;
				
				SoundManager.instance.RandomizeSfx (drinkSound1, drinkSound2);

				other.gameObject.SetActive (false);

				
			} 
		}

		//每次开始下一关卡
		private void Restart ()
		{
			Application.LoadLevel (Application.loadedLevel);
		}
		
		

		//减去食物
		public void LoseFood (int loss)
		{
			//被袭击时
			animator.SetTrigger ("playerHit");
			
			//减去食物值
			food -= loss;
			
			//渲染新的食物值
			foodText.text = "-"+ loss + " Food: " + food;
			
			//检测游戏是否结束
			CheckIfGameOver ();
		}
		
		
		//检测游戏是否结束
		private void CheckIfGameOver ()
		{
			//如果食物值小于等于0
			if (food <= 0) 
			{
				//播放音效
//				SoundManager.instance.PlaySingle (gameOverSound);
				SoundManager.instance.PlaySingle (byebye);
				
				//关闭背景音乐
				SoundManager.instance.musicSource.Stop();
				
				//调用游戏结束函数
				GameManager.instance.GameOver ();	

			}
		}
		
	//重置食物值
		public void ResetFood(){
			//初始状态食物值为100
			food = 100;					
			
			//重新渲染食物值显示
			foodText.text = "Food: " + food;
		}

	}

