using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComplexBrain : MonoBehaviour {

	public float timeAlive;
	public GeneticAlgo GA;
	public GameObject eyes;

	int geneLength = 2;
	bool alive = true;
	bool seeGround = true;

	void OnCollisionEnter(Collision obj) {
		if (obj.gameObject.tag == "dead") alive = false;
	}

	public void Init() {
		// Initialize our genes
		// 0 = Forward
		// 1 = Left
		// 2 = Right
		GA = new GeneticAlgo(geneLength, 3);
		timeAlive = 0.0f;
		alive = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (!alive) return;

		Debug.DrawRay (eyes.transform.position, eyes.transform.forward * 10.0f, Color.red, 10.0f);
		seeGround = false;
		RaycastHit hit;

		if (Physics.Raycast (eyes.transform.position, eyes.transform.forward * 10.0f, out hit)) {
			if (hit.collider.gameObject.tag == "platform") seeGround = true;
		}
		timeAlive = PopManager.elapsed;

		// Read genes
		float h = 0.0f;
		float v = 0.0f;
		if (seeGround) {
			// Make v relative to the character and always move forward
			if (GA.GetGene (0) == 0) v = 1.0f;
			else if (GA.GetGene (0) == 1) h = -90.0f;
			else if (GA.GetGene (0) == 2) h = 90.0f;
		} else {
			if (GA.GetGene (1) == 0) v = 1.0f;
			else if (GA.GetGene (1) == 1) h = -90.0f;
			else if (GA.GetGene (1) == 2) h = 90.0f;
		}

		this.transform.Translate (0.0f, 0.0f, v * 0.1f);
		this.transform.Rotate (0, h, 0);
	}
}
