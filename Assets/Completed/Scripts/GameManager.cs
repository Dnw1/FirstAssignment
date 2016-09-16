using UnityEngine;
using System.Collections;

namespace Completed
{
	using System.Collections.Generic;
	using UnityEngine.UI;	
	
	public class GameManager : MonoBehaviour
	{
		public float levelStartDelay = 2f;	
		public float turnDelay = 0.1f;		
		public int playerFoodPoints = 100;		
		public static GameManager instance = null;		
		[HideInInspector] public bool playersTurn = true;	
		
		
		private Text levelText;		
		private GameObject levelImage;		
		private BoardManager boardScript;	
		private int level = 1;	
		private List<Enemy> enemies;	
		private bool enemiesMoving;		
		private bool doingSetup = true;		

		void Awake()
		{
			if (instance == null)
				instance = this;
			else if (instance != this)
				Destroy(gameObject);	
			//Sets this to not be destroyed when reloading scene
			DontDestroyOnLoad(gameObject);
			enemies = new List<Enemy>();
			//Call the InitGame function to initialize the first level 
			InitGame();
		}
		//This is called each time a scene is loaded.
		void OnLevelWasLoaded(int index)
		{
			level++;
			InitGame();
		}

		void InitGame()
		{
			doingSetup = true;
			levelImage = GameObject.Find("LevelImage");
			levelText = GameObject.Find("LevelText").GetComponent<Text>();
			levelText.text = "Day " + level;
			levelImage.SetActive(true);
			Invoke("HideLevelImage", levelStartDelay);
			enemies.Clear();
			//Call the SetupScene function of the BoardManager script, pass it current level number.
			
		}

		void HideLevelImage()
		{
			levelImage.SetActive(false);
			doingSetup = false;
		}
		void Update()
		{
			if(playersTurn || enemiesMoving || doingSetup)
				return;
			    StartCoroutine (MoveEnemies ());
		}
		public void AddEnemyToList(Enemy script)
		{
			enemies.Add(script);
		}
		
		public void GameOver()
		{
			levelText.text = "After " + level + " days, you starved.";
			levelImage.SetActive(true);
			enabled = false;
		}

		IEnumerator MoveEnemies()
		{
			enemiesMoving = true;
			yield return new WaitForSeconds(turnDelay);
			if (enemies.Count == 0) 
			{
				yield return new WaitForSeconds(turnDelay);
			}
			
			for (int i = 0; i < enemies.Count; i++)
			{
				enemies[i].MoveEnemy ();
				yield return new WaitForSeconds(enemies[i].moveTime);
			}
			playersTurn = true;
			enemiesMoving = false;
		}
	}
}

