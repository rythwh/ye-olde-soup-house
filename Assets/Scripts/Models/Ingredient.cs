using System.Collections.Generic;

using UnityEngine;

public class Ingredient {

	public enum Class {
		Meat,
		Vegetable,
		Broth,
		Extra
	}

	public enum Type {
		Butter,
		Flour,
		Chicken,
		Beef,
		Tomatoes,
		Potatoes,
		Broccoli,
		Carrots,
		Mushrooms,
		Cream,
		ChickenBroth,
		BeefBroth,
		TurkeyBroth,
		VegetableBroth
	}

	public static readonly Dictionary<Class, List<Type>> classToTypes = new Dictionary<Class, List<Type>>() {
		{ Class.Meat, new List<Type>() { 
			Type.Chicken, 
			Type.Beef } },
		{ Class.Vegetable, new List<Type>() { 
			Type.Tomatoes, 
			Type.Potatoes, 
			Type.Broccoli, 
			Type.Carrots, 
			Type.Mushrooms } },
		{ Class.Broth, new List<Type>() { 
			Type.Cream, 
			Type.ChickenBroth, 
			Type.Beef, 
			Type.TurkeyBroth, 
			Type.VegetableBroth } },
		{ Class.Extra, new List<Type>() { 
			Type.Butter, 
			Type.Flour } }
	};

	public static readonly Dictionary<Type, int> typeToPrice = new Dictionary<Type, int>() {
		{ Type.Butter, 10 },
		{ Type.Flour, 2 },
		{ Type.Chicken, 135 },
		{ Type.Beef, 180 },
		{ Type.Tomatoes, 45 },
		{ Type.Potatoes, 25 },
		{ Type.Broccoli, 35 },
		{ Type.Carrots, 30 },
		{ Type.Mushrooms, 60 },
		{ Type.Cream, 15 },
		{ Type.ChickenBroth, 50 },
		{ Type.BeefBroth, 65 },
		{ Type.TurkeyBroth, 55 },
		{ Type.VegetableBroth, 35 }
	};

	public Type type;

	public Sprite baseSprite;
	public Sprite addedSprite;

	public Ingredient(Type type) {
		this.type = type;

		baseSprite = Resources.Load<Sprite>(@"Sprites/Ingredients/" + type.ToString() + "/" + type.ToString().ToLower());
		addedSprite = Resources.Load<Sprite>(@"Sprites/Ingredients/" + type.ToString() + "/" + type.ToString().ToLower() + "-added");
	}
}