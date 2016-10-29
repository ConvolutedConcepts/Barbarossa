using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class GameStatus : MonoBehaviour {

	//Turn this off for PC/Mac Build
	public static bool isDeployedToMobile = true;


	public static int score = 0;
	public static int currentLevel = 1;
	public static int lives = 3;
	//Jet Pack
	public static bool isJetPackOn = false;
	public static bool isJetPackPickedUp = false;

	public static bool isGunPickedUp = false;

	public static float fillAmount = 1;

	public static float fireRate = 0.5f; 
	public static float nextFire = 0.1f;

	public static bool allowToFire = false;

	//UIButtonsControls for JetPack
	public static bool isJetUpButtonPressed = false;
	public static bool isJetDownButtonPressed = false;

	public static bool isJetLeftButtonPressed = false;
	public static bool isJetRightButtonPressed = false;

	public static bool isAltButtonPressed = false;
	//Check if we on the ground to prevent double jump
	public static bool isGrounded = false;

	public void NewGameButtonPressed() {
		SceneManager.LoadScene ("Level1");
	}

	public void GoToMainMenuScreen() {
		SceneManager.LoadScene ("Main_Menu");
	}

	public void GoToLevel_2() {
		GameStatus.currentLevel = 2;
		GameStatus.lives = 3;
		SceneManager.LoadScene ("Level2");
	}

	public void GoToLevel_3() {
		GameStatus.lives = 3;
		GameStatus.currentLevel = 3;
		GameStatus.fillAmount = 1;
		GameStatus.isGunPickedUp = false;
		SceneManager.LoadScene ("Level3");
	}

	public void GoToLevelSelectionScreen() {
		SceneManager.LoadScene ("LevelChoiceScreen");
	}

	public void ExitGame() {
		Application.Quit();
	}



}
