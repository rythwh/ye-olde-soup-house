using System.Collections.Generic;

public class CookingPot {
	
	private readonly List<Ingredient> addedIngredients = new List<Ingredient>();
	
	private bool cooking;

	private Recipe selectedRecipe;
	public float remainingCookingTime;

	public CookingPot() {

	}

	public void AddIngredient(Ingredient ingredient) {
		if (!addedIngredients.Contains(ingredient)) {
			addedIngredients.Add(ingredient);
			GameManager.uiM.AddIngredientToCookingPot(ingredient);
			GameManager.restaurantM.bank -= Ingredient.typeToPrice[ingredient.type];
		}

		FindValidRecipe();
	}

	public void Dump() {
		addedIngredients.Clear();
		FindValidRecipe();
		GameManager.uiM.DumpCookingPot();
	}

	public void ToggleCooking(bool setState = false, bool state = false) {
		if (setState) {
			cooking = state;
		} else {
			cooking = !cooking;
		}
		GameManager.uiM.SetCookingState();
	}

	public void FindValidRecipe() {

		List<Recipe> candidateRecipes = new List<Recipe>();
		candidateRecipes.AddRange(GameManager.restaurantM.recipes);

		List<Recipe> removeRecipes = new List<Recipe>();

		foreach (Ingredient addedIngredient in addedIngredients) {
			foreach (Recipe recipe in candidateRecipes) {
				if (!Recipe.recipeToIngredients[recipe.type].Contains(addedIngredient.type)) {
					removeRecipes.Add(recipe);
				}
			}
			foreach (Recipe removeRecipe in removeRecipes) {
				candidateRecipes.Remove(removeRecipe);
			}
		}
		if (candidateRecipes.Count == 1) {
			selectedRecipe = candidateRecipes[0];
		} else if (candidateRecipes.Count > 0) {
			selectedRecipe = candidateRecipes.Find(recipe => Recipe.recipeToIngredients[recipe.type].Count == addedIngredients.Count);
		} else {
			selectedRecipe = null;
		}

		if (selectedRecipe != null) {
			remainingCookingTime = Recipe.recipeToCookingSecondsAtTemperature[selectedRecipe.type];
		} else {
			remainingCookingTime = 0;
		}

		GameManager.uiM.SetCookingPotSelectedRecipe(selectedRecipe);
	}

	public Recipe GetSelectedRecipe() {
		return selectedRecipe;
	}

	public bool IsCooking() {
		return cooking;
	}
}