using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgo {

	private List<int> genes = new List<int>();
	private int geneLength = 0;
	private int maxValue = 0;

	public GeneticAlgo(int l, int v) {
		geneLength = l;
		maxValue = v;
		SetRandom();
	}

	public int GetGene(int pos) {
		return genes [pos];
	}

	public void SetGene(int pos, int value) {
		genes[pos] = value;
	}

	public void SetRandom() {
		genes.Clear ();
		for (int i = 0; i < geneLength; i++) {
			genes.Add (Random.Range (0, maxValue));
		}
	}

	public void Combine(GeneticAlgo GA1, GeneticAlgo GA2) {
		for (int i = 0; i < geneLength; i++) {
			if (i < geneLength / 2) {
				int c = GA1.genes [i];
				genes [i] = c;
			} else {
				int c = GA2.genes [i];
				genes [i] = c;
			}
		}
	}

	public void Mutate() {
		genes [Random.Range (0, geneLength)] = Random.Range(0, maxValue);
	}
}
