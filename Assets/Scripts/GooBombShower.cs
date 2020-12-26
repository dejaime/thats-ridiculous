using UnityEngine;
using Unity.Mathematics;

public class GooBombShower : MonoBehaviour {
	private static GooBombShower _instance;

	public static GooBombShower Instance { get { return _instance; } }


	private void Awake() {
		if (_instance != null && _instance != this) {
			Destroy(this.gameObject);
		} else {
			_instance = this;
		}
	}

	public static void Shower(float3 position, float size) {
		//Increase goo cubes' heights around position using size as a scale
	}
}
