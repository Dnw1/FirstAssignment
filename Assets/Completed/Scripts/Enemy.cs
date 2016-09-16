using UnityEngine;
using System.Collections;

namespace Completed
{
	public class Enemy : MovingObject
	{
		public int playerDamage; 	
		public AudioClip attackSound1;	
		public AudioClip attackSound2;
		
		
		private Animator animator;	
		private Transform target;	
		private bool skipMove;	
		
		protected override void Start ()
		{
			//Register this enemy with our instance of GameManager by adding it to a list of Enemy objects. 
			GameManager.instance.AddEnemyToList (this);
			animator = GetComponent<Animator> ();
			target = GameObject.FindGameObjectWithTag ("Player").transform;
			base.Start ();
		}
		
		protected override void AttemptMove <T> (int xDir, int yDir)
		{
			if(skipMove)
			{
				skipMove = false;
				return;
				
			}
			base.AttemptMove <T> (xDir, yDir);
			skipMove = true;
		}

		public void MoveEnemy ()
		{
			int xDir = 0;
			int yDir = 0;
			if(Mathf.Abs (target.position.x - transform.position.x) < float.Epsilon)
				yDir = target.position.y > transform.position.y ? 1 : -1;
			else
				xDir = target.position.x > transform.position.x ? 1 : -1;
			    AttemptMove <Player> (xDir, yDir);
		}

		//OnCantMove is called if Enemy attempts to move into a space occupied by a Player, it overrides the OnCantMove function of MovingObject 
		protected override void OnCantMove <T> (T component)
		{
			Player hitPlayer = component as Player;
			hitPlayer.LoseFood (playerDamage);
			animator.SetTrigger ("enemyAttack");
			SoundManager.instance.RandomizeSfx (attackSound1, attackSound2);
		}
	}
}
