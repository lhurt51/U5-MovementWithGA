using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MazePopManager : PopManager {

	// Use this for initialization
	protected override void Start () {
		for (int i = 0; i < popSize; i++) {
			GameObject bot = Instantiate (botPrefab, this.transform.position, this.transform.rotation);

			bot.GetComponent<MazeBrain> ().Init ();
			population.Add (bot);
		}
	}

	protected override GameObject Breed (GameObject P1, GameObject P2)
	{
		GameObject offspring = Instantiate (botPrefab, this.transform.position, this.transform.rotation);
		MazeBrain brain = offspring.GetComponent<MazeBrain> ();

		if (Random.Range (0.0f, 100.0f) == 1.0f) {
			brain.Init ();
			brain.GA.Mutate ();
		} else {
			brain.Init ();
			brain.GA.Combine (P1.GetComponent<MazeBrain> ().GA, P2.GetComponent<MazeBrain> ().GA);
		}

		return offspring;
	}

	protected override void BreedNewPop ()
	{
		List<GameObject> sortedList = population.OrderBy(o => o.GetComponent<MazeBrain>().dstTravelled).ToList();

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
