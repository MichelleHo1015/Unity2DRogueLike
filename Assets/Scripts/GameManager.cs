using UnityEngine;
using System.Collections;
using System.Collections.Generic;	
using UnityEngine.UI;					

public class GameManager : MonoBehaviour
{
	public float levelStartDelay = 2f;					//开局的遮罩时间为2s		
	public float turnDelay = 0.1f;						//每次进入新关卡的遮罩时间为.1s	
	public int playerFoodPoints = 100;					//角色初始食物值为100
	public static GameManager instance = null;			//一开始gamemanager为空	

	
	[HideInInspector] 
	public bool playersTurn = true;		
	private Text levelText;									
	private Text dayText;					
	public Text highScoreText;					//显示最高分
	public int highScoreCount;					//定义最高分
	private GameObject levelImage;							
	private GameObject buttonCanvas;
	private BoardManager boardScript;						
	private int level = 1;									
	private List<Enemy> enemies;							
	private bool enemiesMoving;								
	private bool doingSetup = true;						
	
	
	
	//Awake is always called before any Start functions
	void Awake()
	{
		//检验instance是否已存在
		if (instance == null) {
			
		//如果不存在，当前创建的实例就是自己
			instance = this;

		}
		//如果存在且不是自己
		else if (instance != this) 
			
		//摧毁当前存在的gamemanager实例
			Destroy (gameObject);


		
		DontDestroyOnLoad(gameObject);
		
	//实例化敌人列表
		enemies = new List<Enemy>();
		
	//获取到boardmanager
		boardScript = GetComponent<BoardManager>();
		
	//调用init函数
		InitGame();
	}

	//开始时检测是否有上一次游戏留存下的最高分
	void Start(){

		if(PlayerPrefs.GetInt("HighScore") != null){
			//有的话就渲染进最高分
//			PlayerPrefs.SetInt("HighScore",0);H
			
			highScoreCount = PlayerPrefs.GetInt("HighScore");
		
		}
	}

//每次开局都会调用此函数
	void OnLevelWasLoaded(int index)
	{

		level++;

		InitGame();
	}

	void InitGame()
	{
	//当doingstep为真时，角色不可移动
		doingSetup = true;

		//获取场景中的游戏物体
		levelImage = GameObject.Find("LevelImage");
		buttonCanvas = GameObject.Find("ButtonCanvas");

		buttonCanvas.SetActive(false);
		//获取场景中的游戏物体的text属性
		levelText = GameObject.Find("LevelText").GetComponent<Text>();
		dayText = GameObject.Find ("DayText").GetComponent<Text> ();
		

		levelText.text = "第 " + level + " 天";
		dayText.text = "Day " + level ;
		
		//打开遮罩层
		levelImage.SetActive(true);
		
		//调用delay方法，使得开局时遮罩有延时效果
		Invoke("HideLevelImage", levelStartDelay);
		
		//清空敌人列表，为下一关做准备
		enemies.Clear();
		
		//传当前关卡数，调用boardmanager里的setupscene函数
		boardScript.SetupScene(level);
		
	}
	
	
	//隐藏遮罩的函数
	void HideLevelImage()
	{
		levelImage.SetActive(false);
		buttonCanvas.SetActive(false);
		
		//禁止角色在设置地图时移动
		doingSetup = false;
	}
	
	//Update is called every frame.
	void Update()
	{
		HighScore ();
		//三者任何一个为真，敌人都不可移动
		if(playersTurn || enemiesMoving || doingSetup)
			
			return;
		
		//三者皆不为真，敌人才可移动
		StartCoroutine (MoveEnemies ());
		

	}
	
	//可被调用去增加敌人数
	public void AddEnemyToList(Enemy script)
	{
		enemies.Add(script);
	}
	
	
	//角色食物值小于等于0时，游戏结束
	public void GameOver()
	{

		levelText.text = "你挺过了 " + level + " 天.";
		
		//开启遮罩
		levelImage.SetActive(true);
		buttonCanvas.SetActive(true);
		//禁用gamemanager，使游戏结束
		enabled = false;

	}


	//当敌人数大于1时，协调敌人移动顺序
	IEnumerator MoveEnemies()
	{
		//敌人移动时，角色不可移动
		enemiesMoving = true;
		
		//敌人每次移动都会延时0.1s(看起来就像是一步一步走)
		yield return new WaitForSeconds(turnDelay);
		
		//在第一关无敌人时
		if (enemies.Count == 0) 
		{
			//实例一个给角色的行走的延时
			yield return new WaitForSeconds(turnDelay);
		}
		
		//遍历敌人列表
		for (int i = 0; i < enemies.Count; i++)
		{
			//为列表中的每一个敌人都添加移动方法
			enemies[i].MoveEnemy ();
			
			//为每一个敌人行走前都添加一个等待
			yield return new WaitForSeconds(enemies[i].moveTime);
		}
		//敌人行走完毕后就是角色的回合
		playersTurn = true;
		
		//禁用敌人的行走回合
		enemiesMoving = false;
	}

	//重新开始关卡
	public void StartDay(int day){
		level = day - 1; 
		Application.LoadLevel (Application.loadedLevel);
	}
	
	//暂停遮罩中的重新开始
	public void RestartDay(){
		StartDay (level);
	}

	//结束遮罩中的重新开始
	public void Retry(){

		StartDay (1);
		GameObject.Find("Player").GetComponent<Player>().ResetFood();
		enabled = true;
		SoundManager.instance.musicSource.Play();
		
	}

	//展示最高分
	public void HighScore(){

		//获取游戏中的gameobject
		highScoreText = GameObject.Find("HighScore").GetComponent<Text>();

		//如果当前level值大于最高分
		if (level > highScoreCount) {

			highScoreCount = level;

			//将当前的最高分存在playerprefs中
			PlayerPrefs.SetInt("HighScore",highScoreCount);

		}
		//展示最高分
		highScoreText.text = "HighScore: " + highScoreCount;
	}
	
}

