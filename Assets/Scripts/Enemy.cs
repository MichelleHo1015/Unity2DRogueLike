using UnityEngine;
using System.Collections;

//继承MovingObject类
public class Enemy : MovingObject
{
	public int playerDamage; 							//敌人的攻击值
	public AudioClip attackSound1;						//第一次攻击时的音效
	public AudioClip attackSound2;						//第二次攻击时的音效
	
	
	private Animator animator;							//角色动画控制
	private Transform target;							//敌人每次移动的目标坐标
	private bool skipMove;								//判断此回合是否是自己的回合
	

	protected override void Start ()
	{
		//将敌人加入到gamemanager里，可通过gamemanager去控制
		GameManager.instance.AddEnemyToList (this);
		
		//获取animator组件
		animator = GetComponent<Animator> ();
		
		//通过tag获取到角色物体并存储它的坐标变换
		target = GameObject.FindGameObjectWithTag ("Player").transform;
		
		//调用movingobject里的base类
		base.Start ();
	}
	
	

	protected override void AttemptMove <T> (int xDir, int yDir)
	{
		//当角色移动时，将敌人不可移动的bool变量设为true
		if (skipMove)
//			{							此操作可使的角色走两步敌人走一步
//				skipMove = false;
//				return;
//			}
			skipMove = false;		//此操作可使角色走一步敌人走一步
		//敌人移动
		base.AttemptMove <T> (xDir, yDir);
		
		//此时将敌人不可移动的bool值设为真，此回合该角色走
		skipMove = true;
	}
	
	
	//敌人每次都向角色的方向移动
	public void MoveEnemy ()
	{

		//敌人将移动的位置坐标设置为（0，0）
		int xDir = 0;
		int yDir = 0;
		
		//如果碰撞器确定二者碰撞
		if(Mathf.Abs (target.position.x - transform.position.x) < float.Epsilon)
			//如果角色当前位置的Y坐标大于敌人的Y坐标,敌人向上移动
			//否则敌人向下移动
			yDir = target.position.y > transform.position.y ? 1 : -1;
		
		//如果碰撞器未检测到二者碰撞
		else
			//如果角色当前位置的x坐标大于敌人的x坐标,敌人向右移动
			//否则敌人向下移动
			xDir = target.position.x > transform.position.x ? 1 : -1;
		
		//开始移动
		AttemptMove <Player> (xDir, yDir);
	}
	
	

	protected override void OnCantMove <T> (T component)
	{
		//声明hitPlayer，将它设置为遇到的组件。
		Player hitPlayer = component as Player;
		
		//获取敌人的伤害值，角色损失的食物值等于敌人伤害值
		hitPlayer.LoseFood (playerDamage);
		
		//开启敌人伤害动画
		animator.SetTrigger ("enemyAttack");
		
		//播放攻击声音
		SoundManager.instance.RandomizeSfx (attackSound1, attackSound2);
	}
}
