﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyGrowth : MonoBehaviour {

	/* Limiters */

	[SerializeField]
    private float GrowthGoal = 30.0f;

	[SerializeField]
    private float WaterMax = 10.0f;

	/* During Life */

    private float waterLevel = 0.0f;
    private float growthLevel = 0.0f;

	private bool growing = false;

	public enum GrowingState { Dormant, GrowingHealthy, GrowingWilting, GrowingDying, Dead };
	private GrowingState state = GrowingState.Dormant;

	public GrowingState CurrentState { get { return state; } }

	// Use this for initialization
	void Start () {
		ChangeState(GrowingState.Dormant);
		Reset();
	}
	
	// Update is called once per frame
	void Update () {
		TestInputs();

		if (!growing) {
			return;
		}

		UpdateTimeDeltas();

		if (growthLevel >= GrowthGoal) {
			PopoutABaby();
			return;
		}

		UpdateState();
	}

	private void TestInputs() {
		if (Input.GetButton("Fire1")) {
			PlantSeed();
		}

		if (Input.GetButton("Fire2")) {
			FillWater();
		}
	}

	public void PlantSeed() {
		if (growing) {
			return;
		}

		growing = true;
		waterLevel = WaterMax;
		ChangeState(GrowingState.GrowingHealthy);
	}

	public void FillWater() {
		waterLevel = WaterMax;
	}

	private void PopoutABaby() {
		// TODO - Create a baby object
		GetComponent<SpriteRenderer>().color = Color.yellow;

		//ChangeState(State.Dormant);
		Reset();
	}

	public bool IsGrowing() {
		return growing;
	}

	private void UpdateTimeDeltas()
	{
		waterLevel -= Time.deltaTime;
		growthLevel += Time.deltaTime;
	}

	private void UpdateState() {
		if (waterLevel > (WaterMax * 0.6)) {
			ChangeState(GrowingState.GrowingHealthy);
		}
		else if (waterLevel > (WaterMax * 0.25)) {
			ChangeState(GrowingState.GrowingWilting);
		}
		else if (waterLevel > 0) {
			ChangeState(GrowingState.GrowingDying);
		}
		else {
			Kill();
		}
	}

	private void ChangeState(GrowingState state)
	{
		state = state;

		switch (state)
		{
			case GrowingState.Dormant:
				//GetComponent<SpriteRenderer>().color = Color.magenta;
				break;
			case GrowingState.GrowingHealthy:
				GetComponent<SpriteRenderer>().color = Color.green;
				break;
			case GrowingState.GrowingWilting:
				GetComponent<SpriteRenderer>().color = Color.grey;
				break;
			case GrowingState.GrowingDying:
				GetComponent<SpriteRenderer>().color = Color.red;
				break;
			case GrowingState.Dead:
				GetComponent<SpriteRenderer>().color = Color.black;
				break;
		}
	}

	private void Kill() {
		ChangeState(GrowingState.Dead);
		Reset();
	}

	private void Reset() {
		waterLevel = 0.0f;
		growthLevel = 0.0f;
		growing = false;
	}
}
