using UnityEngine;
using System.Collections;
using System.Linq;

public class play_spartan_animation : MonoBehaviour {
	// Use this for initialization

	static class random {
		public static System.Random rand;
	}

	static class move {
		public static string[] moves = new string[] {"resist", "resist", "victory", "victory", "salute", "salute", "die", "diehard", "attack", "attack", "idlebattle"};
	}

	public bool updateOn;
	public bool walking;
	public bool turning;
	public string play;
	public Vector3 origin;
	public double difference;
	public Vector3 maxPosition;
	public Vector3 minPosition;
	public Vector3 randomPosition;
	//public float xDiff;
	//public float zDiff;

	void Start () {
		random.rand = new System.Random();
		this.GetComponent<Animation>().Play("idle");

		difference = 9.655;
		origin = transform.position;
		maxPosition = new Vector3(origin.x+9.2f, 18.26f, origin.z);
		minPosition = new Vector3(origin.x, 18.26f, origin.z-10.11f);
		updateOn = false;
		walking = false;
		turning = true;
		randomPosition = new Vector3(Random.Range(minPosition.x, maxPosition.x), 18.26f, Random.Range(minPosition.z, maxPosition.z));
		this.GetComponent<Animation>().Play("walk");
	}

	
	// Update is called once per frame
	void Update () { 
		//var randomPosition = new Vector3(Random.Range(minPosition.x, maxPosition.x), 18.26f, Random.Range(minPosition.z, maxPosition.z) );
		var choice = random.rand.Next(0,10);
		if (!updateOn && !walking) { //dino does something
			play = move.moves[choice];
            Debug.Log(play);
			this.GetComponent<Animation>().Play(play);
			//StartCoroutine(wait(1.5f));

			if (new []{"die", "idlebattle"}.Contains(play)) { //NOT WALKING
				StartCoroutine(wait(3f));
			} else {
				StartCoroutine(wait(2f));
			}

			randomPosition = new Vector3(Random.Range(minPosition.x, maxPosition.x), 18.26f, Random.Range(minPosition.z, maxPosition.z));

		} else if (!updateOn && walking) { //WALKING
			this.GetComponent<Animation>().Play("walk");
			StartCoroutine(waitWalk(1.5f)); //make this a variable
			if (turning) {
				var zDiff = System.Math.Abs(randomPosition.z-transform.position.z);
				var xDiff = System.Math.Abs(randomPosition.x-transform.position.x);
				var angle = System.Math.Atan(xDiff/zDiff)*(180 / System.Math.PI);

				Vector3 targetDirection = (randomPosition - transform.position)/24;

				Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, 0.125f, 0.0f);
				transform.rotation = Quaternion.LookRotation(newDirection);
				if (transform.forward == targetDirection) {
				//if (true) {
					turning = false;
				}
			} else {
				transform.position = Vector3.MoveTowards(transform.position, new Vector3(-2.17f, 18.26f, 7.18f), 0.25f);
			}
		}

		if (turning) {
			var zDiff = System.Math.Abs(randomPosition.z-transform.position.z);
			var xDiff = System.Math.Abs(randomPosition.x-transform.position.x);
			var angle = System.Math.Atan(xDiff/zDiff)*(180 / System.Math.PI);

			Vector3 targetDirection = (randomPosition - transform.position)/24;

			Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, 0.125f, 0.0f);
			transform.rotation = Quaternion.LookRotation(newDirection);
			if (transform.forward == targetDirection) {
			//if (true) {
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
