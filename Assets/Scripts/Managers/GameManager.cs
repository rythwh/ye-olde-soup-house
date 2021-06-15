using System.Collections.Generic;

using UnityEngine;

public class GameManager : MonoBehaviour {

	public static readonly RestaurantManager restaurantM = new RestaurantManager();
	public static readonly UIManager uiM = new UIManager();

	public static readonly List<BaseManager> managers = new List<BaseManager>() {
		restaurantM,
		uiM
	};

	public void Awake() {
		foreach (BaseManager manager in managers) {
			manager.Awake();
		}
	}

	public void Start() {
		foreach (BaseManager manager in managers) {
			manager.Start();
		}
	}

	public void Update() {
		foreach (BaseManager manager in managers) {
			manager.Update();
		}
	}
}
