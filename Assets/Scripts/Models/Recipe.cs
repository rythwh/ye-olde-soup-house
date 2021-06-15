using System.Collections.Generic;

using UnityEngine;

public class Recipe {
	
	public enum Type {
		CreamOfMushroom,
		Tomato,
		PotatoAndBroccoli
	}

	public static readonly Dictionary<Type, List<Ingredient.Type>> recipeToIngredients = new Dictionary<Type, List<Ingredient.Type>>() {
		{ Type.CreamOfMushroom, new List<Ingredient.Type>() { Ingredient.Type.Cream, Ingredient.Type.Mushrooms, Ingredient.Type.Butter, Ingredient.Type.Flour } },
		{ Type.Tomato, new List<Ingredient.Type>() { Ingredient.Type.VegetableBroth, Ingredient.Type.Tomatoes, Ingredient.Type.Butter } },
		{ Type.PotatoAndBroccoli, new List<Ingredient.Type>() { Ingredient.Type.ChickenBroth, Ingredient.Type.Potatoes, Ingredient.Type.Broccoli, Ingredient.Type.Butter, Ingredient.Type.Flour } }
	};

	public static readonly Dictionary<Type, int> recipeToTemperature = new Dictionary<Type, int>() {
		{ Type.CreamOfMushroom, 200 },
		{ Type.Tomato, 300 },
		{ Type.PotatoAndBroccoli, 400 }
	};

	public static readonly Dictionary<Type, int> recipeToCookingSecondsAtTemperature = new Dictionary<Type, int>() {
		{ Type.CreamOfMushroom, 10 },
		{ Type.Tomato, 20 },
		{ Type.PotatoAndBroccoli, 40 }
	};

	public Type type;

	public Sprite baseSprite;

	public Recipe(Type type) {
		this.type = type;

		baseSprite = Resources.Load<Sprite>(@"Sprites/Recipes/" + type.ToString() + "/" + type.ToString().ToLower());
	}
}
