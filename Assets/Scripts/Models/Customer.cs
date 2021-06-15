using System;
using System.Collections.Generic;

using UnityEngine;

public class Customer {

	public enum Temper {
		Relaxed,
		Normal,
		Angry
	}

	private static readonly Dictionary<Temper, float> temperToSatisfactionRate = new Dictionary<Temper, float>() {
		{ Temper.Relaxed, 0.5f },
		{ Temper.Normal, 1f },
		{ Temper.Angry, 1.5f }
	};

	public Recipe.Type desiredRecipe;

	public Temper temper;
	public static readonly int maxSatisfaction = 120;
	public float satisfaction;

	public GameObject uiRepresentation;

	public Customer() {
		satisfaction = maxSatisfaction;

		desiredRecipe = ((Recipe.Type[])Enum.GetValues(typeof(Recipe.Type)))[UnityEngine.Random.Range(0, Enum.GetValues(typeof(Recipe.Type)).Length)];
		temper = ((Temper[])Enum.GetValues(typeof(Temper)))[UnityEngine.Random.Range(0, Enum.GetValues(typeof(Temper)).Length)];
	}

	public void Update() {
		satisfaction -= 1f * temperToSatisfactionRate[temper] * Time.deltaTime;
	}

	public void Destroy() {
		UnityEngine.Object.Destroy(uiRepresentation);
	}
}