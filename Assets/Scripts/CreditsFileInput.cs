using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[ExecuteInEditMode]
public class CreditsFileInput : MonoBehaviour {
    public TextAsset credits;

	void Update () {
        gameObject.GetComponent<Text>().text = credits.text;
	}
}
