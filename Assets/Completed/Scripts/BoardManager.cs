using UnityEngine;
using System;
using System.Collections.Generic; 	
using Random = UnityEngine.Random; 	

namespace Completed
	
{
	
	public class BoardManager : MonoBehaviour
	{
		[Serializable]
		public class Count
		{
			public int minimum; 
			public int maximum; 
			
			public Count (int min, int max)
			{
				minimum = min;
				maximum = max;
			}
		}
		
		
		public int columns = 8; 
		public int rows = 8;	
		public Count wallCount = new Count (5, 9);	
		public Count foodCount = new Count (1, 5);	
		public GameObject exit;		
		public GameObject[] floorTiles;	
		public GameObject[] wallTiles;	
		public GameObject[] foodTiles;	
		public GameObject[] enemyTiles;		
		public GameObject[] outerWallTiles;	
		
		private Transform boardHolder;	
		private List <Vector3> gridPositions = new List <Vector3> ();
		
		void InitialiseList ()
		{
			gridPositions.Clear ();
			for(int x = 1; x < columns-1; x++)
			{
				for(int y = 1; y < rows-1; y++)
				{
					gridPositions.Add (new Vector3(x, y, 0f));
				}
			}
		}
		
		//Sets up the outer walls and floor (background) of the game board.
		void BoardSetup ()
		{
			boardHolder = new GameObject ("Board").transform;
			for(int x = -1; x < columns + 1; x++)
			{
				for(int y = -1; y < rows + 1; y++)
				{
					GameObject toInstantiate = floorTiles[Random.Range (0,floorTiles.Length)];
					if(x == -1 || x == columns || y == -1 || y == rows)
						toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];
					GameObject instance =
						Instantiate (toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
					    instance.transform.SetParent (boardHolder);
				}
			}
		}
		
		
		//RandomPosition returns a random position from our list gridPositions.
		Vector3 RandomPosition ()
		{
			int randomIndex = Random.Range (0, gridPositions.Count);
			Vector3 randomPosition = gridPositions[randomIndex];
			
			//Remove the entry at randomIndex from the list so that it can't be re-used.
			gridPositions.RemoveAt (randomIndex);
			
			//Return the randomly selected Vector3 position.
			return randomPosition;
		}
	
		void LayoutObjectAtRandom (GameObject[] tileArray, int minimum, int maximum)
		{
			int objectCount = Random.Range (minimum, maximum+1);
			//Instantiate objects until the randomly chosen limit objectCount is reached
			for(int i = 0; i < objectCount; i++)
			{
				Vector3 randomPosition = RandomPosition();
				GameObject tileChoice = tileArray[Random.Range (0, tileArray.Length)];
				Instantiate(tileChoice, randomPosition, Quaternion.identity);
			}
		}
		
		
		//SetupScene initializes our level and calls the previous functions to lay out the game board
		public void SetupScene (int level)
		{
			BoardSetup ();
			//Reset list of gridpositions.
			InitialiseList ();
			LayoutObjectAtRandom (wallTiles, wallCount.minimum, wallCount.maximum);
			LayoutObjectAtRandom (foodTiles, foodCount.minimum, foodCount.maximum);
			int enemyCount = (int)Mathf.Log(level, 2f);
			LayoutObjectAtRandom (enemyTiles, enemyCount, enemyCount);
			//Instantiate the exit tile in the upper right hand corner of our game board
			Instantiate (exit, new Vector3 (columns - 1, rows - 1, 0f), Quaternion.identity);
		}
	}
}
