using UnityEngine;
using System.Collections;

public class CreditsKeyboardInputHandler : MonoBehaviour {
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameStatus.GoToMainMenuScreen();
        }
	}
}
