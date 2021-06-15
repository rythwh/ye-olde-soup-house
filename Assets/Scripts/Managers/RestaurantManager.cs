using System;
using System.Collections.Generic;

using UnityEngine;

public class RestaurantManager : BaseManager {

	public readonly List<Ingredient> ingredients = new List<Ingredient>();

	public readonly List<Recipe> recipes = new List<Recipe>();

	public readonly CookingPot cookingPot = new CookingPot();

	public readonly List<Customer> customers = new List<Customer>();

	public int bank;

	public override void Awake() {
		foreach (Ingredient.Type ingredientType in Enum.GetValues(typeof(Ingredient.Type))) {
			ingredients.Add(new Ingredient(ingredientType));
		}
		foreach (Recipe.Type recipeType in Enum.GetValues(typeof(Recipe.Type))) {
			recipes.Add(new Recipe(recipeType));
		}

		bank = 300;
	}

	private float temperature;
	public static readonly int maxTemperature = 400;

	public float customerTimer = 0;
	public float secondTimer = 1;
	public static readonly int minCustomerTimer = 30;
	public static readonly int maxCustomerTimmer = 120;

	public override void Start() {
		base.Start();

		Customer customer = new Customer();
		customers.Add(customer);
		GameManager.uiM.AddCustomer(customer);
	}

	public override void Update() {
		base.Update();

		if (customers.Count < 4) {
			customerTimer += 1 * Time.deltaTime;
			if (customerTimer >= minCustomerTimer && customerTimer <= maxCustomerTimmer) {
				secondTimer -= 1 * Time.deltaTime;
				if (secondTimer <= 0) {
					if (customerTimer >= UnityEngine.Random.Range(minCustomerTimer, maxCustomerTimmer)) {
						Customer customer = new Customer();
						customers.Add(customer);
						GameManager.uiM.AddCustomer(customer);
						customerTimer = 0f;
					}
					secondTimer = 1f;
				}
			}
		}

		if (cookingPot.IsCooking()) {
			temperature += 10 * 2 * Time.deltaTime;
		} else {
			temperature -= 10 * Time.deltaTime;
		}

		if (temperature >= maxTemperature) {
			temperature = maxTemperature;
		} else if (temperature <= 0) {
			temperature = 0;
		}

		if (cookingPot.GetSelectedRecipe() != null) {
			if (temperature >= Recipe.recipeToTemperature[cookingPot.GetSelectedRecipe().type]) {
				cookingPot.remainingCookingTime -= 1 * Time.deltaTime;
				if (cookingPot.remainingCookingTime <= 0) {
					cookingPot.remainingCookingTime = 0;
				}
			}
		}

		foreach (Customer customer in customers) {
			if (customer.satisfaction <= 0) {
				RemoveCustomer(customer);
				break;
			}
		}
	}

	public float GetTemperature() {
		return temperature;
	}

	public Ingredient GetIngredientByType(Ingredient.Type type) {
		return ingredients.Find(ingredient => ingredient.type == type);
	}

	public Recipe GetRecipeByType(Recipe.Type type) {
		return recipes.Find(recipe => recipe.type == type);
	}

	public void Serve() {

		Customer satisfiedCustomer = null;
		foreach (Customer customer in customers) {
			if (customer.desiredRecipe == cookingPot.GetSelectedRecipe().type) {
				satisfiedCustomer = customer;
				break;
			}
		}

		if (satisfiedCustomer != null) {

			foreach (Ingredient.Type ingredientType in Recipe.recipeToIngredients[satisfiedCustomer.desiredRecipe]) {
				bank += Mathf.RoundToInt(Ingredient.typeToPrice[ingredientType] * (1 + (satisfiedCustomer.satisfaction / Customer.maxSatisfaction)));
			}

			RemoveCustomer(satisfiedCustomer);
		}

		cookingPot.Dump();
	}

	public void RemoveCustomer(Customer customer) {
		customer.Destroy();
		customers.Remove(customer);
	}
}
