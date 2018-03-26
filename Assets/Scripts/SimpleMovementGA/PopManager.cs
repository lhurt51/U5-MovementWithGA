using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PopManager : MonoBehaviour {

	public GameObject botPrefab;
	public int popSize = 50;
	public static float elapsed = 0.0f;
	public float trialTime = 5.0f;

	int generation = 1;
	List<GameObject> population = new List<GameObject>();

	GUIStyle guiStyle = new GUIStyle();

	void OnGUI() {
		guiStyle.fontSize = 25;
		guiStyle.normal.textColor = Color.white;
		GUI.BeginGroup (new Rect (10, 10, 250, 150));
		GUI.Box (new Rect (0, 0, 140, 140), "Stats", guiStyle);
		GUI.Label (new Rect (10, 25, 200, 30), "Gen: " + generation, guiStyle);
		GUI.Label (new Rect (10, 50, 200, 30), string.Format ("Time: {0:0.00}", elapsed), guiStyle);
		GUI.Label (new Rect (10, 75, 200, 30), "Population: " + population.Count, guiStyle);
		GUI.EndGroup ();
	}

	// Use this for initialization
	void Start () {
		for (int i = 0; i < popSize; i++) {
			Vector3 startingPos = new Vector3 (this.transform.position.x + Random.Range (-2.0f, 2.0f), this.transform.position.y, this.transform.position.z + Random.Range (-2.0f, 2.0f));
			GameObject bot = Instantiate (botPrefab, startingPos, this.transform.rotation);

			bot.GetComponent<Brain> ().Init ();
			population.Add (bot);
		}
	}

	GameObject Breed(GameObject P1, GameObject P2) {
		Vector3 startingPos = new Vector3 (this.transform.position.x + Random.Range (-2.0f, 2.0f), this.transform.position.y, this.transform.position.z + Random.Range (-2.0f, 2.0f));
		GameObject offspring = Instantiate (botPrefab, startingPos, this.transform.rotation);
		Brain brain = offspring.GetComponent<Brain> ();

		if (Random.Range (0.0f, 100.0f) == 1.0f) {
			brain.Init ();
			brain.GA.Mutate ();
		} else {
			brain.Init ();
			brain.GA.Combine (P1.GetComponent<Brain> ().GA, P2.GetComponent<Brain> ().GA);
		}

		return offspring;
	}

	void BreedNewPop() {
		List<GameObject> sortedList = population.OrderBy(o => o.GetComponent<Brain>().timeAlive).ThenBy(o => o.GetComponent<Brain>().dstTraveled).ToList();

		population.Clear ();
		// Breed upper half of sorted list
		for (int i = (int)(sortedList.Count / 2.0f) - 1; i < sortedList.Count - 1; i++) {
			population.Add (Breed (sortedList [i], sortedList [i + 1]));
			population.Add (Breed (sortedList [i + 1], sortedList [i]));
		}

		// Destory all previous population
		for (int i = 0; i < sortedList.Count; i++) {
			Destroy (sortedList [i]);
		}

		generation++;
	}
	
	// Update is called once per frame
	void Update () {
		elapsed += Time.deltaTime;
		if (elapsed >= trialTime) {
			BreedNewPop ();
			elapsed = 0;
		}
	}
}
