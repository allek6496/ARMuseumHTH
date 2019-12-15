using UnityEngine;
using System.Collections;
using System.Linq;

public class Play_animations : MonoBehaviour {
	// Use this for initialization

	static class random {
		public static System.Random rand;
	}

	static class move {
		public static string[] moves = new string[] {"resist", "victory", "salute", "die", "diehard", "attack", "idlebattle"};
	}

	public bool updateOn;
	public bool walking;
	public bool turning;
	public string play;
	public Vector3 origin;
	public Vector3 maxPosition;
	public Vector3 minPosition;
	public Vector3 randomPosition;
	//public float xDiff;
	//public float zDiff;

	void Start () {
		random.rand = new System.Random();
		this.GetComponent<Animation>().Play("idle");

		origin = transform.position;
		maxPosition = new Vector3(origin.x+1f, 0, origin.z);
		minPosition = new Vector3(origin.x, 0, origin.z-1f);
		updateOn = false;
		walking = false;
		turning = true;
		randomPosition = new Vector3(Random.Range(minPosition.x, maxPosition.x), 0, Random.Range(minPosition.z, maxPosition.z));
		this.GetComponent<Animation>().Play("walk");
	}

	
	// Update is called once per frame
	void Update () { 
		//var randomPosition = new Vector3(Random.Range(minPosition.x, maxPosition.x), 1, Random.Range(minPosition.z, maxPosition.z) );
		var choice = random.rand.Next(0, 6);
		if (!updateOn && !walking) { //dino does something
			play = move.moves[choice];
			this.GetComponent<Animation>().Play(play);
			//StartCoroutine(wait(1.5f));

			if (new []{"resist", "die", "salute", "idlebattle"}.Contains(play)) { //NOT WALKING
				StartCoroutine(wait(2f));
			} else {
				StartCoroutine(wait(0.5f));
			}

			randomPosition = new Vector3(Random.Range(minPosition.x, maxPosition.x), 0
, Random.Range(minPosition.z, maxPosition.z));

		} else if (!updateOn && walking) { //WALKING
			this.GetComponent<Animation>().Play("walk");
			StartCoroutine(waitWalk(1.5f)); //make this a variable
			if (turning) {
				Debug.Log("***");
				var zDiff = System.Math.Abs(randomPosition.z-transform.position.z);
				var xDiff = System.Math.Abs(randomPosition.x-transform.position.x);
				var angle = System.Math.Atan(xDiff/zDiff)*(180 / System.Math.PI);

				Vector3 targetDirection = (randomPosition - transform.position)/24;

				Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, 0.125f, 0.0f);
				transform.rotation = Quaternion.LookRotation(newDirection);
				Debug.Log(targetDirection);
				Debug.Log(transform.forward);
				if (transform.forward == targetDirection) {
				//if (true) {
					Debug.Log("WAS HERE");
					turning = false;
				}
			} else {
				//transform.position = Vector3.MoveTowards(transform.position, new Vector3((float) maxPosition.x, 1, (float) maxPosition.z), 0.25f);
				transform.position = Vector3.MoveTowards(transform.position, randomPosition, 0.125f);
			}
		}

		if (turning) {
			var zDiff = System.Math.Abs(randomPosition.z-transform.position.z);
			var xDiff = System.Math.Abs(randomPosition.x-transform.position.x);
			var angle = System.Math.Atan(xDiff/zDiff)*(180 / System.Math.PI);

			Vector3 targetDirection = (randomPosition - transform.position)/24;

			Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, 0.125f, 0.0f);
			transform.rotation = Quaternion.LookRotation(newDirection);
			Debug.Log(targetDirection);
			Debug.Log(transform.forward);
			if (transform.forward == targetDirection) {
			//if (true) {
				Debug.Log("WAS HERE");
				turning = false;
			}
		}

		if (walking) { //WALKING
			transform.position = Vector3.MoveTowards(transform.position, randomPosition, 0.125f);
			if (transform.position == randomPosition) {
				//got there
				walking = false;
				updateOn = false;
				turning = true;
			}
		}
	}

	IEnumerator wait(float seconds) {
		updateOn = true;
		yield return new WaitForSeconds(seconds);
		updateOn = false;
		walking = true;
	}

	IEnumerator waitWalk(float seconds) {
		updateOn = true;
		yield return new WaitForSeconds(seconds);
		//updateOn = false;
		//walking = false;
	}
}
