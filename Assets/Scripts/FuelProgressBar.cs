using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class FuelProgressBar : MonoBehaviour {

	[SerializeField]
	private float fillAmount;

	[SerializeField]
	private Image blackRect;
	// Use this for initialization
	void Start () {
		blackRect.fillAmount = 0;
	}
	
	// Update is called once per frame
	void Update () {
		HandleBar ();
	}
	private void HandleBar() {
		blackRect.fillAmount += Time.deltaTime / 100 ;
	}
}
