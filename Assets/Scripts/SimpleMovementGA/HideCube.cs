﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideCube : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<MeshRenderer> ().enabled = false;
	}
}