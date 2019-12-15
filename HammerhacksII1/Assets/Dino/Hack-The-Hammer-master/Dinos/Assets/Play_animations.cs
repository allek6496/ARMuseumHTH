using UnityEngine;
using System.Collections;
using System.Linq;

public class Play_animations : MonoBehaviour {
	// Use this for initialization

	static class random {
		public static System.Random rand;
	}

	static class move {
		public static string[] moves = new string[] {"Eat", "Roar", "Snap", "hit", "Dodge", "Jump", "Death", "Talk", "Fart", "Dance", "Burp"};
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
		maxPosition = new Vector3(origin.x+9.2f, (float) origin.y, origin.z);
		minPosition = new Vector3(origin.x, (float) origin.y, origin.z-10.11f);
		updateOn = false;
		walking = false;
		turning = true;
		randomPosition = new Vector3(Random.Range(minPosition.x, maxPosition.x), (float) origin.y, Random.Range(minPosition.z, maxPosition.z));
		this.GetComponent<Animation>().Play("walk_loop");
	}

	
	// Update is called once per frame
	void Update () { 
		//var randomPosition = new Vector3(Random.Range(minPosition.x, maxPosition.x), (float) origin.y, Random.Range(minPosition.z, maxPosition.z) );
		var choice = random.rand.Next(0, 10);
		if (!updateOn && !walking) { //dino does something
			play = move.moves[choice];
			this.GetComponent<Animation>().Play(play);
			//StartCoroutine(wait(1.5f));

			if (new []{"Eat", "Talk", "Death", "Dance"}.Contains(play)) { //NOT WALKING
				StartCoroutine(wait(2f));
			} else if (play == "Dodge") {
				StartCoroutine(wait(1f));
			} else {
				StartCoroutine(wait(0.5f));
			}

			randomPosition = new Vector3(Random.Range(minPosition.x, maxPosition.x), (float) origin.y, Random.Range(minPosition.z, maxPosition.z));

		} else if (!updateOn && walking) { //WALKING
			this.GetComponent<Animation>().Play("walk_loop");
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
				transform.position = Vector3.MoveTowards(transform.position, new Vector3(-2.17f, (float) origin.y, 7.18f), 0.25f);
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

	/*public void OnGUI() {
		if (GUI.Button(new Rect((float)50, (float)50, (float)80, (float)20), "Idle")) {
			this.GetComponent<Animation>().Play("idle");
		}

		if (GUI.Button(new Rect((float)50, (float)75, (float)80, (float)20), "Walk")) {
			this.GetComponent<Animation>().Play("walk_loop");
		}

		if (GUI.Button(new Rect((float)50, (float)100, (float)80, (float)20), "Run")) {
			this.GetComponent<Animation>().Play("Run");
		}

		if (GUI.Button(new Rect((float)50, (float)125, (float)80, (float)20), "Eat")) {
			this.GetComponent<Animation>().Play("Eat");
		}

		if (GUI.Button(new Rect((float)50, (float)150, (float)80, (float)20), "Roar")) {
			this.GetComponent<Animation>().Play("Roar");
		}

		if (GUI.Button(new Rect((float)50, (float)175, (float)80, (float)20), "Snap")) {
			this.GetComponent<Animation>().Play("Snap");
		}

		if (GUI.Button(new Rect((float)50, (float)200, (float)80, (float)20), "Hit")) {
			this.GetComponent<Animation>().Play("hit");
		}

		if (GUI.Button(new Rect((float)50, (float)225, (float)80, (float)20), "Dodge")) {
			this.GetComponent<Animation>().Play("Dodge");
		}

		if (GUI.Button(new Rect((float)50, (float)250, (float)80, (float)20), "Jump")) {
			this.GetComponent<Animation>().Play("Jump");
		}

		if (GUI.Button(new Rect((float)50, (float)275, (float)80, (float)20), "Look Right")) {
			this.GetComponent<Animation>().Play("Look_right");
		}

		if (GUI.Button(new Rect((float)50, (float)300, (float)80, (float)20), "Look Left")) {
			this.GetComponent<Animation>().Play("Look_left");
		}

		if (GUI.Button(new Rect((float)50, (float)325, (float)80, (float)20), "Death")) {
			this.GetComponent<Animation>().Play("Death");
		}

		if (GUI.Button(new Rect((float)50, (float)350, (float)80, (float)20), "Talk")) {
			this.GetComponent<Animation>().Play("Talk");
		}

		if (GUI.Button(new Rect((float)50, (float)375, (float)80, (float)20), "Fart")) {
			this.GetComponent<Animation>().Play("Fart");
		}

		if (GUI.Button(new Rect((float)50, (float)400, (float)80, (float)20), "Dance")) {
			this.GetComponent<Animation>().Play("Dance");
		}

		if (GUI.Button(new Rect((float)50, (float)425, (float)80, (float)20), "Burp")) {
			this.GetComponent<Animation>().Play("Burp");
		}

	}*/
}
