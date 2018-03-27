using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ComplexPopManager : PopManager {

	protected override void Start () {
		for (int i = 0; i < popSize; i++) {
			Vector3 startingPos = new Vector3 (this.transform.position.x + Random.Range (-2.0f, 2.0f), this.transform.position.y, this.transform.position.z + Random.Range (-2.0f, 2.0f));
			GameObject bot = Instantiate (botPrefab, startingPos, this.transform.rotation);

			bot.GetComponent<ComplexBrain> ().Init ();
			population.Add (bot);
		}
	}

	protected override GameObject Breed (GameObject P1, GameObject P2) {
		Vector3 startingPos = new Vector3 (this.transform.position.x + Random.Range (-2.0f, 2.0f), this.transform.position.y, this.transform.position.z + Random.Range (-2.0f, 2.0f));
		GameObject offspring = Instantiate (botPrefab, startingPos, this.transform.rotation);
		ComplexBrain brain = offspring.GetComponent<ComplexBrain> ();

		if (Random.Range (0.0f, 100.0f) == 1.0f) {
			brain.Init ();
			brain.GA.Mutate ();
		} else {
			brain.Init ();
			brain.GA.Combine (P1.GetComponent<ComplexBrain> ().GA, P2.GetComponent<ComplexBrain> ().GA);
		}

		return offspring;
	}

	protected override void BreedNewPop ()
	{
		List<GameObject> sortedList = population.OrderBy(o => o.GetComponent<ComplexBrain>().timeAlive).ThenBy(o => o.GetComponent<ComplexBrain>().timeWalking).ToList();

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

}
