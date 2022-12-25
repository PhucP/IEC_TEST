using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PipeType
{
	easy,
	hard
}

public class ObstacleSpawner : MonoBehaviour {

	[SerializeField] private float waitTime;
	[SerializeField] private GameObject obstaclePrefabs;
	[SerializeField] private float offset;
	private List<PipeType> listPipe;
	private float tempTime;
	private System.Random rand = new System.Random();
	private int obsIdex;
	[SerializeField] private ObstacleBehaviour previousPipe;

	void Start(){
		tempTime = waitTime - Time.deltaTime;
		listPipe = new List<PipeType> {PipeType.easy, PipeType.easy, PipeType.easy, PipeType.hard, PipeType.hard};
		obsIdex = 0;
	}

	void LateUpdate () {
		if(GameManager.Instance.GameState()){
			tempTime += Time.deltaTime;
			if(tempTime > waitTime){
				// Wait for some time, create an obstacle, then set wait time to 0 and start again
				tempTime = 0;
				var tempIndex = obsIdex%listPipe.Count;
				var newPos = ChooseNextPipe(tempIndex);

				GameObject pipeClone = Instantiate(obstaclePrefabs, new Vector2(2.5f, newPos.x * newPos.y * offset), transform.rotation);
				previousPipe = pipeClone.GetComponent<ObstacleBehaviour>();
				obsIdex++;
			}
		}
	}

	private Vector2Int ChooseNextPipe(int tempIndex)
	{
		Vector2 result;
		if(previousPipe == null)
		{
			return Vector2Int.zero;
		}
		else
		{
			result.y = (tempIndex < 3) ? rand.Next(4) : rand.Next(4, 7);
			result.x = rand.Next(2);
		}

		if(previousPipe.colIndex + result.x * result.y > 8)
		{
			result.x = 1;
			result.y = 8 - previousPipe.colIndex;
		}
		else if(previousPipe.colIndex + result.x * result.y < 0)
		{
			result.x = -1;
			result.y = previousPipe.colIndex;
		}

		
		return new Vector2Int((int)result.x, (int)result.y);
	}

	void OnTriggerEnter2D(Collider2D col){
		if(col.gameObject.transform.parent != null){
			Destroy(col.gameObject.transform.parent.gameObject);
		}else{
			Destroy(col.gameObject);
		}
	}

}
