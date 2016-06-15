using UnityEngine;
using System.Collections;


	//抽象关键字使您能够创建不完整的类和类成员，并且必须在派生类中实现。
	public abstract class MovingObject : MonoBehaviour
	{
		public float moveTime = 0.1f;			//移动时间.1s
		public LayerMask blockingLayer;			//检测碰撞的层
		
		
		private BoxCollider2D boxCollider; 		//2D碰撞器
		private Rigidbody2D rb2D;				//2D刚体
		private float inverseMoveTime;			
		
		
		protected virtual void Start ()
		{
			//获取2D碰撞器
			boxCollider = GetComponent <BoxCollider2D> ();
			
			//获取2D刚体
			rb2D = GetComponent <Rigidbody2D> ();
			
			inverseMoveTime = 1f / moveTime;
		}
		
		
		
		protected bool Move (int xDir, int yDir, out RaycastHit2D hit)
		{
			//角色当前位置
			Vector2 start = transform.position;
			
			// 计算出角色将要到达的目标
			Vector2 end = start + new Vector2 (xDir, yDir);
			
			//禁用自身与自身的碰撞
			boxCollider.enabled = false;
			
			hit = Physics2D.Linecast (start, end, blockingLayer);
			
			//重新开启自己的碰撞器
			boxCollider.enabled = true;
			
			//检测碰撞
			if(hit.transform == null)
			{
				//无碰撞触发
				StartCoroutine (SmoothMovement (end));
				
				return true;
			}
			
			//有碰撞出发，无法移动
			return false;
		}
		
		
		
		protected IEnumerator SmoothMovement (Vector3 end)
		{
			//使得运动平滑
			float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
			
			while(sqrRemainingDistance > float.Epsilon)
			{
				
				Vector3 newPostion = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
				
				
				rb2D.MovePosition (newPostion);
				
				sqrRemainingDistance = (transform.position - end).sqrMagnitude;
				
				yield return null;
			}
		}
		
		//实现轮转
		protected virtual void AttemptMove <T> (int xDir, int yDir)
			where T : Component
		{

			RaycastHit2D hit;
			
			//如果可以行进
			bool canMove = Move (xDir, yDir, out hit);
			
			//不会触发被袭击
			if(hit.transform == null)
				
				return;
			

			T hitComponent = hit.transform.GetComponent <T> ();
			
			//如果被袭击以及不可行动都不为空
			if(!canMove && hitComponent != null)
				
				//角色被袭击
				OnCantMove (hitComponent);
		}
		
		protected abstract void OnCantMove <T> (T component)
			where T : Component;
	}
