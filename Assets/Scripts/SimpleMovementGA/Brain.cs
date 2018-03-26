using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof(ThirdPersonCharacter))]
public class Brain : MonoBehaviour {

	public float timeAlive;
	public float dstTraveled;
	public GeneticAlgo GA;

	private int geneLength = 1;
	private ThirdPersonCharacter m_Character;
	private Vector3 m_Move;
	private Vector3 startingPos;
	private bool m_Jump;
	private bool alive = true;

	void OnCollisionEnter(Collision obj) {
		if (obj.gameObject.tag == "dead") alive = false;
	}

	public void Init() {
		// Init GeneticAlgo
		// 0 = Forward
		// 1 = Back
		// 2 = Left
		// 3 = Right
		// 4 = Jump
		// 5 = Crouch
		GA = new GeneticAlgo(geneLength, 6);
		m_Character = GetComponent<ThirdPersonCharacter> ();
		startingPos = this.transform.position;
		timeAlive = 0;
		alive = true;
	}

	// Fixed update is called in sync with physics
	private void FixedUpdate() {
		// Read GA
		float h = 0;
		float v = 0;
		bool crouch = false;

		if (GA.GetGene (0) == 0) v = 1;
		else if (GA.GetGene (0) == 1) v = -1;
		else if (GA.GetGene (0) == 2) h = -1;
		else if (GA.GetGene (0) == 3) h = 1;
		else if (GA.GetGene (0) == 4) m_Jump = true;
		else if (GA.GetGene (0) == 5) crouch = true;

		m_Move = v * Vector3.forward + h * Vector3.right;
		m_Character.Move(m_Move, crouch, m_Jump);
		m_Jump = false;
		if (alive) {
			timeAlive += Time.deltaTime;
			dstTraveled = Vector3.Distance(this.transform.position, startingPos);
		}
	}
}
