using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeBrain : MonoBehaviour {

	public float dstTravelled = 0.0f;
	public GeneticAlgo GA;
	public GameObject eyes;

	int geneLength = 2;
	Vector3 startPos;
	bool seeWall = true;
	bool alive = true;

	public void Init() {
		// Initialize the genes
		// 0 = Forward
		// 1 = Angled Turn
		GA = new GeneticAlgo(geneLength, 360);
		startPos = this.transform.position;
	}

	void OnCollisionEnter(Collision col) {
		if (col.gameObject.tag == "dead") {
			dstTravelled = 0.0f;
			alive = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!alive) return;

		seeWall = false;
		RaycastHit hit;
		Debug.DrawRay (eyes.transform.position, eyes.transform.forward * 0.5f, Color.red);
		if (Physics.SphereCast (eyes.transform.position, 0.1f, eyes.transform.forward, out hit, 0.5f)) {
			if (hit.collider.gameObject.tag == "wall") seeWall = true;
		}
	}

	void FixedUpdate() {
		if (!alive) return;

		// read genes
		float h = 0.0f;
		float v = GA.GetGene (0);

		if (seeWall) h = GA.GetGene (1);

		this.transform.Translate (0.0f, 0.0f, v * 0.0002f);
		this.transform.Rotate (0.0f, h, 0.0f);
		dstTravelled = Vector3.Distance (startPos, this.transform.position);
	}
}
