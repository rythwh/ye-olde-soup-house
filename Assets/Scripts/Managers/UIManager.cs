using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using UnityEngine;
using UnityEngine.UI;

public class UIManager : BaseManager {
	public enum Colours {
		White,
		Orange,
		Blue,
		Red
	}

	private static readonly Dictionary<Colours, Color> colourMap = new Dictionary<Colours, Color>() {
		{ Colours.White, new Color(255f, 255f, 255f, 255f) / 255f },
		{ Colours.Orange, new Color(230f, 126f, 34f, 255f) / 255f },
		{ Colours.Blue, new Color(52f, 152f, 219f, 255f) / 255f },
		{ Colours.Red, new Color(231f, 76f, 60f, 255f) / 255f }
	};

	public static Color GetColour(Colours colourEnum) {
		return colourMap[colourEnum];
	}

	public static Color ChangeAlpha(Color colour, float alpha) {
		return new Color(colour.r, colour.g, colour.b, alpha);
	}

	public static string SplitByCapitals(string combinedString) {
		var r = new Regex(
			@"(?<=[A-Z])(?=[A-Z][a-z]) | (?<=[^A-Z])(?=[A-Z]) | (?<=[A-Za-z])(?=[^A-Za-z])",
			RegexOptions.IgnorePatternWhitespace);
		return r.Replace(combinedString, " ");
	}

	private Transform canvas;

	private Transform mainMenuPanel;
	private Transform gamePanel;

	public override void Awake() {
		base.Awake();

		SetupUI();
	}

	public override void Update() {
		base.Update();

		if (mainMenuPanel.gameObject.activeSelf) {
			Time.timeScale = 0;
		} else {
			Time.timeScale = 1;
		}

		UpdateGameUI();
	}

	private void SetupUI() {
		canvas = GameObject.Find("Canvas").transform;

		mainMenuPanel = canvas.Find("MainMenu-Panel");
		gamePanel = canvas.Find("Game-Panel");

		SetupMainMenuUI();
		SetupGameUI();
	}

	private void SetupMainMenuUI() {

		Button continueButton = mainMenuPanel.Find("SoupHouseLogo-Image/Buttons-Panel/ButtonsLayoutGroup-Panel/Continue-Button").GetComponent<Button>();
		continueButton.onClick.AddListener(delegate {
			mainMenuPanel.gameObject.SetActive(false);
			gamePanel.gameObject.SetActive(true);
		});

		Button beginAnewButton = mainMenuPanel.Find("SoupHouseLogo-Image/Buttons-Panel/ButtonsLayoutGroup-Panel/BeginAnew-Button").GetComponent<Button>();
		beginAnewButton.onClick.AddListener(delegate {
			mainMenuPanel.gameObject.SetActive(false);
			gamePanel.gameObject.SetActive(true);
		});

		Button loadButton = mainMenuPanel.Find("SoupHouseLogo-Image/Buttons-Panel/ButtonsLayoutGroup-Panel/Load-Button").GetComponent<Button>();
		loadButton.onClick.AddListener(delegate {
			mainMenuPanel.gameObject.SetActive(false);
			gamePanel.gameObject.SetActive(true);
		});

		Button configureButton = mainMenuPanel.Find("SoupHouseLogo-Image/Buttons-Panel/ButtonsLayoutGroup-Panel/Configure-Button").GetComponent<Button>();

		Button leaveButton = mainMenuPanel.Find("SoupHouseLogo-Image/Buttons-Panel/ButtonsLayoutGroup-Panel/Leave-Button").GetComponent<Button>();
		leaveButton.onClick.AddListener(delegate {
			Application.Quit();
		});

		mainMenuPanel.gameObject.SetActive(true);
	}

	private readonly List<Transform> countertopSide = new List<Transform>();
	private readonly Dictionary<Transform, List<GameObject>> countertopToBowls = new Dictionary<Transform, List<GameObject>>();

	private readonly int numSides = 2;
	private readonly int numBowlsPerSide = 7;

	private Text bankText;

	private void SetupGameUI() {

		gamePanel.transform.Find("Menu-Button").GetComponent<Button>().onClick.AddListener(delegate {
			mainMenuPanel.gameObject.SetActive(true);
			gamePanel.gameObject.SetActive(false);
		});

		SetupIngredientBowls();
		SetupCookingPot();
		SetupRecipeBook();
		SetupBankUI();

		gamePanel.gameObject.SetActive(false);
	}

	private void SetupIngredientBowls() {
		Transform leftBowlsPanel = gamePanel.Find("Countertop-Panel/Countertop-Image/LeftBowls-Panel");
		Transform rightBowlsPanel = gamePanel.Find("Countertop-Panel/Countertop-Image/RightBowls-Panel");

		countertopSide.Add(leftBowlsPanel);
		countertopSide.Add(rightBowlsPanel);

		for (int sideIndex = 0; sideIndex < numSides; sideIndex++) {
			List<GameObject> bowls = new List<GameObject>();
			for (int bowlIndex = 0; bowlIndex < numBowlsPerSide; bowlIndex++) {
				GameObject bowl = MonoBehaviour.Instantiate(Resources.Load<GameObject>(@"UI/Prefabs/Bowl"), countertopSide[sideIndex], false);
				bowls.Add(bowl);

				int ingredientIndex = (sideIndex * numBowlsPerSide) + bowlIndex;

				if (ingredientIndex < Enum.GetValues(typeof(Ingredient.Type)).Length) {

					Ingredient ingredient = GameManager.restaurantM.ingredients[ingredientIndex];

					bowl.transform.Find("Contents-Image").GetComponent<Image>().sprite = ingredient.baseSprite;
					bowl.transform.Find("Contents-Text").GetComponent<Text>().text = SplitByCapitals(ingredient.type.ToString());

					bowl.transform.Find("Contents-Image").GetComponent<Button>().onClick.AddListener(delegate {
						GameManager.restaurantM.cookingPot.AddIngredient(ingredient);
					});
				}
			}
			countertopToBowls.Add(countertopSide[sideIndex], new List<GameObject>());
		}
	}

	private Transform cookingPot;
	private Image recipeImage;

	private Button serveButton;

	private Button lightFireButton;
	private GameObject fire;

	private void SetupCookingPot() {
		cookingPot = gamePanel.Find("Countertop-Panel/Countertop-Image/CookingPot-Image");

		Button dumpButton = cookingPot.Find("Dump-Button").GetComponent<Button>();
		dumpButton.onClick.AddListener(delegate {
			GameManager.restaurantM.cookingPot.Dump();
		});

		serveButton = cookingPot.Find("Serve-Button").GetComponent<Button>();
		serveButton.onClick.AddListener(delegate {
			GameManager.restaurantM.Serve();
		});
		serveButton.gameObject.SetActive(false);

		lightFireButton = cookingPot.Find("LightFire-Button").GetComponent<Button>();

		fire = cookingPot.Find("Fire-Image").gameObject;
		fire.SetActive(false);

		lightFireButton.onClick.AddListener(delegate {
			GameManager.restaurantM.cookingPot.ToggleCooking();
		});

		recipeImage = cookingPot.Find("Recipe-Image").GetComponent<Image>();
	}

	public void SetCookingState() {
		fire.SetActive(GameManager.restaurantM.cookingPot.IsCooking());
		if (GameManager.restaurantM.cookingPot.IsCooking()) {
			lightFireButton.GetComponent<Image>().color = GetColour(Colours.Blue);
			lightFireButton.transform.Find("Text").GetComponent<Text>().text = "Extinguish";
		} else {
			lightFireButton.GetComponent<Image>().color = GetColour(Colours.Orange);
			lightFireButton.transform.Find("Text").GetComponent<Text>().text = "Light Fire";
		}
	}

	public void SetCookingPotSelectedRecipe(Recipe recipe) {
		if (recipe != null) {
			recipeImage.sprite = recipe.baseSprite;
		} else {
			recipeImage.sprite = null;
			recipeImage.color = ChangeAlpha(GetColour(Colours.White), 0f);
		}
	}

	private Transform recipeBookOpen;

	private int currentRecipeIndex = 0;

	private Transform leftPageRecipe;
	private Transform rightPageRecipe;

	private void SetupRecipeBook() {

		recipeBookOpen = gamePanel.transform.Find("RecipeBookOpen-Image");

		Button recipeBook = gamePanel.transform.Find("Countertop-Panel/Countertop-Image/RecipeBook-Button").GetComponent<Button>();
		recipeBook.onClick.AddListener(delegate {
			ToggleRecipeBookOpen();
		});
		recipeBookOpen.Find("RecipeBookOpenFlippable-Image/CloseRecipeBook-Button").GetComponent<Button>().onClick.AddListener(delegate {
			ToggleRecipeBookOpen();
		});

		recipeBookOpen.Find("RecipeBookOpenFlippable-Image/PreviousPage-Button").GetComponent<Button>().onClick.AddListener(delegate {
			ChangeRecipePages(-1);
		});
		recipeBookOpen.Find("RecipeBookOpenFlippable-Image/NextPage-Button").GetComponent<Button>().onClick.AddListener(delegate {
			ChangeRecipePages(1);
		});

		leftPageRecipe = recipeBookOpen.Find("RecipeBookOpenFlippable-Image/LeftPageRecipe-Panel");
		rightPageRecipe = recipeBookOpen.Find("RecipeBookOpenFlippable-Image/RightPageRecipe-Panel");

		currentRecipeIndex = Enum.GetValues(typeof(Recipe.Type)).Length / 2 - 1;

		recipeBookOpen.gameObject.SetActive(false);
	}

	private void ToggleRecipeBookOpen() {
		recipeBookOpen.gameObject.SetActive(!recipeBookOpen.gameObject.activeSelf);

		if (recipeBookOpen.gameObject.activeSelf) {
			ChangeRecipePages(0);
		}
	}

	private readonly List<GameObject> recipeIngredientElements = new List<GameObject>();

	private void ChangeRecipePages(int changeDirection) {

		int numRecipes = Enum.GetValues(typeof(Recipe.Type)).Length;

		currentRecipeIndex += changeDirection * 2;
		if (currentRecipeIndex < 0) {
			currentRecipeIndex = 0;
		} else if (currentRecipeIndex > numRecipes - 1) {
			currentRecipeIndex = numRecipes - 1;
		}

		foreach (GameObject recipeIngredientElement in recipeIngredientElements) {
			UnityEngine.Object.Destroy(recipeIngredientElement);
		}
		recipeIngredientElements.Clear();

		if (leftPageRecipe.gameObject.activeSelf) {
			SetRecipePage(leftPageRecipe, GameManager.restaurantM.recipes[currentRecipeIndex]);
		}

		rightPageRecipe.gameObject.SetActive(currentRecipeIndex + 1 < numRecipes);
		if (rightPageRecipe.gameObject.activeSelf) {
			SetRecipePage(rightPageRecipe, GameManager.restaurantM.recipes[currentRecipeIndex + 1]);
		}
	}

	private void SetRecipePage(Transform recipePage, Recipe recipe) {
		recipePage.Find("RecipeName-Text").GetComponent<Text>().text = SplitByCapitals(recipe.type.ToString());
		recipePage.Find("Bowl-Image/Recipe-Image").GetComponent<Image>().sprite = recipe.baseSprite;

		foreach (Ingredient.Type ingredientType in Recipe.recipeToIngredients[recipe.type]) {
			Ingredient ingredient = GameManager.restaurantM.GetIngredientByType(ingredientType);
			GameObject ingredientElement = UnityEngine.Object.Instantiate(Resources.Load<GameObject>(@"UI/Prefabs/RecipeIngredient"), recipePage.Find("IngredientList-Panel"), false);
			ingredientElement.transform.Find("Name").GetComponent<Text>().text = SplitByCapitals(ingredient.type.ToString());
			ingredientElement.transform.Find("Bowl-Image/Ingredient-Image").GetComponent<Image>().sprite = ingredient.baseSprite;
			recipeIngredientElements.Add(ingredientElement);
		}
	}

	private void SetupBankUI() {
		bankText = gamePanel.transform.Find("Bank-Panel/BankAmount-Text").GetComponent<Text>();
	}

	private readonly List<GameObject> addedIngredients = new List<GameObject>();

	public void AddIngredientToCookingPot(Ingredient ingredient) {
		GameObject addedIngredient = UnityEngine.Object.Instantiate(Resources.Load<GameObject>(@"UI/Prefabs/AddedIngredient"), cookingPot, false);
		addedIngredient.GetComponent<Image>().sprite = ingredient.addedSprite;
		addedIngredients.Add(addedIngredient);
	}

	public void DumpCookingPot() {
		foreach (GameObject addedIngredient in addedIngredients) {
			UnityEngine.Object.Destroy(addedIngredient);
		}
		addedIngredients.Clear();
	}

	public void AddCustomer(Customer customer) {
		customer.uiRepresentation = UnityEngine.Object.Instantiate(Resources.Load<GameObject>(@"UI/Prefabs/Customer"), gamePanel.transform.Find("HouseBackground-Image"), false);
		customer.uiRepresentation.GetComponent<Image>().sprite = Resources.Load<Sprite>(@"Sprites/Customers/customer-" + UnityEngine.Random.Range(0, 3));
		customer.uiRepresentation.transform.Find("Bowl-Image/Recipe-Image").GetComponent<Image>().sprite = GameManager.restaurantM.GetRecipeByType(customer.desiredRecipe).baseSprite;
		customer.uiRepresentation.transform.Find("Bowl-Image/RecipeName-Text").GetComponent<Text>().text = SplitByCapitals(customer.desiredRecipe.ToString());
	}

	private void UpdateGameUI() {
		cookingPot.GetComponent<Image>().color = Color.Lerp(GetColour(Colours.White), GetColour(Colours.Orange), GameManager.restaurantM.GetTemperature() / RestaurantManager.maxTemperature);

		if (GameManager.restaurantM.cookingPot.GetSelectedRecipe() != null) {
			float alphaLerpTime = GameManager.restaurantM.cookingPot.remainingCookingTime / Recipe.recipeToCookingSecondsAtTemperature[GameManager.restaurantM.cookingPot.GetSelectedRecipe().type];
			recipeImage.color = ChangeAlpha(GetColour(Colours.White), Mathf.Lerp(1f, 0f, alphaLerpTime));
			foreach (GameObject addedIngredient in addedIngredients) {
				addedIngredient.GetComponent<Image>().color = ChangeAlpha(GetColour(Colours.White), Mathf.Lerp(0f, 1f, alphaLerpTime));
			}
		}

		serveButton.gameObject.SetActive(GameManager.restaurantM.cookingPot.GetSelectedRecipe() != null && GameManager.restaurantM.cookingPot.remainingCookingTime <= 0);
		if (serveButton.gameObject.activeSelf) {
			GameManager.restaurantM.cookingPot.ToggleCooking(true, false);
		}

		foreach (Customer customer in GameManager.restaurantM.customers) {
			customer.Update();
			customer.uiRepresentation.GetComponent<Image>().color = Color.Lerp(GetColour(Colours.Red), GetColour(Colours.White), customer.satisfaction / Customer.maxSatisfaction);
		}

		bankText.text = GameManager.restaurantM.bank.ToString();
	}
}
