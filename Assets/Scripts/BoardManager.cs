using UnityEngine;
using System;
using System.Collections.Generic; 		
using Random = UnityEngine.Random; 		

	
public class BoardManager : MonoBehaviour
{
	[Serializable]
	public class Count
	{
		public int minimum;             //设置最小值
		public int maximum;             //设置最大值
		
		
		//设置一个公用的表示随机数量范围的函数
		public Count (int min, int max)
		{
			minimum = min;
			maximum = max;
		}
	}
	
	
	public int columns = 8;                                         //设置floor的宽度
	public int rows = 8;                                            //设置floor的长度
	public Count wallCount = new Count (5, 9);                      //每局出现障碍的个数范围
	public Count foodCount = new Count (2, 5);                      //每局出现食物的个数范围
	public GameObject exit;                                         //获取出口预设
	public GameObject[] floorTiles;                                 //获取地板预设
	public GameObject[] wallTiles;                                  //获取障碍预设
	public GameObject[] foodTiles;                                  //获取食物预设
	public GameObject[] enemyTiles;                                 //获取敌人预设
	public GameObject[] outerWallTiles;                             //获取外墙预设

	private Transform boardHolder;                                  //为渲染在游戏中的各个预设建议一个父类容器
	private List <Vector3> gridPositions = new List <Vector3> ();   //将得出的随机位置存储在list中
	
	
	
	void InitialiseList ()
	{
		//清空上次选出的坐标
		gridPositions.Clear ();
		
		//当x在1到列数-1时
		for(int x = 1; x < columns-1; x++)
		{
			//当y在1到行数-1时
			for(int y = 1; y < rows-1; y++)
			{
				//得到坐标，添加在List中
				gridPositions.Add (new Vector3(x, y, 0f));
			}
		}
	}
	
	
	//创建绘制主地图的函数
	void BoardSetup ()
	{
		//新建一个游戏对象以存放所有的地图物体
		boardHolder = new GameObject ("Board").transform;
		
		//当x坐标在0至列数-1时，绘制地板
		for(int x = -1; x < columns + 1; x++)
		{
			//当y坐标在0至行数-1时，绘制地板
			for(int y = -1; y < rows + 1; y++)
			{
				//随机获取floor数组中的某个预设，并将其渲染到在坐标中
				GameObject toInstantiate = floorTiles[Random.Range (0,floorTiles.Length)];
				
				//此时绘制外围墙
				if(x == -1 || x == columns || y == -1 || y == rows)
					toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
				
				//记录下预设所在坐标，并渲染完成
				GameObject instance =
					Instantiate (toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;

				//将之前所有渲染的组件包存放在以boardHolder为父级的游戏对象中
				instance.transform.SetParent (boardHolder);
			}
		}
	}
	
	
	
	Vector3 RandomPosition ()
	{
		//在可备选的位置List中随机获取某个位置的序号
		int randomIndex = Random.Range (0, gridPositions.Count);
		
		//存下这个随机的位置
		Vector3 randomPosition = gridPositions[randomIndex];
		
		//避免重复选择同一个位置
		gridPositions.RemoveAt (randomIndex);
		
		return randomPosition;
	}
	
	
	
	void LayoutObjectAtRandom (GameObject[] tileArray, int minimum, int maximum)
	{
		//随机选定一个数
		int objectCount = Random.Range (minimum, maximum+1);
		
		//循环这个选定数的次数进行创建坐标
		for(int i = 0; i < objectCount; i++)
		{
			//选定一个坐标
			Vector3 randomPosition = RandomPosition();
			
			//进行某一个预设数组的渲染
			GameObject tileChoice = tileArray[Random.Range (0, tileArray.Length)];
			
			//将这个预设创建到游戏中去
			Instantiate(tileChoice, randomPosition, Quaternion.identity);
		}
	}
	
	
	//m每次开局前的绘制地图
	public void SetupScene (int level)
	{
		//外墙和地板的渲染
		BoardSetup ();
		
		//重置存放随机位置的list
		InitialiseList ();
		
		//渲染随机数个障碍物
		LayoutObjectAtRandom (wallTiles, wallCount.minimum, wallCount.maximum);
		
		//渲染随机数个food
		LayoutObjectAtRandom (foodTiles, foodCount.minimum, foodCount.maximum);
		
		//以log形式确定每局出现的敌人数
		int enemyCount = (int)Mathf.Log(level, 2f);
		
		//渲染随机数个敌人
		LayoutObjectAtRandom (enemyTiles, enemyCount, enemyCount);
		
		//渲染在（7，7）位置的出口
		Instantiate (exit, new Vector3 (columns - 1, rows - 1, 0f), Quaternion.identity);
	}
}
