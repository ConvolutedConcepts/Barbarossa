using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class FuelProgressBar : MonoBehaviour {

	[SerializeField]
	private float fillAmount;

	[SerializeField]
	private Image blackRect;
	// Use this for initialization
	public int decSpeed = 0; // Set it to whatever. Min 100 for now
		
	void Start () {
		blackRect.fillAmount = GameStatus.healthBarAmount;
	}
	
	// Update is called once per frame
	void Update () {
		HandleBar ();
	}
	private void HandleBar() {
		blackRect.fillAmount += Time.deltaTime / decSpeed;
		GameStatus.healthBarAmount = blackRect.fillAmount;
			
	}
}
