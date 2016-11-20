using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


/// <summary>
/// Script is used to keep track of all static values used across multiple scripts and manage level transitions
/// </summary>

public class GameStatus {

	//Turn this off for PC/Mac Build
	public static bool isDeployedToMobile = true;


	public static int score = 0;
	public static int currentLevel = 1;
	public static int lives = 3;

	public static float fillAmount = 1;

	//Check if we on the ground to prevent double jump
	public static bool isGrounded = false;

	public void NewGameButtonPressed() {
		SceneManager.LoadScene ("Level1");
	}

	public static void GoToMainMenuScreen() {
		SceneManager.LoadScene ("Main_Menu");
	}

	public static void GoToCredits() {
		SceneManager.LoadScene ("Credits");
	}

    public static void loadNextLevel()
    {
        if (GameStatus.currentLevel == 1)
        {
            GoToLevel_2();
        }

        //Temp until decided where player will go after the end of level 2
        else if (GameStatus.currentLevel == 2)
        {
            GoToLevel_1();
        }
    }

    public static void GoToLevel_1()
    {
        GameStatus.currentLevel = 1;
        GameStatus.lives = 3;
        SceneManager.LoadScene("Level1");
    }

    public static void GoToLevel_2() {
		GameStatus.currentLevel = 2;
		GameStatus.lives = 3;
		SceneManager.LoadScene ("Level2");
	}

	public static void GoToLevel_3() {
		GameStatus.lives = 3;
		GameStatus.currentLevel = 3;
		SceneManager.LoadScene ("Level3");
	}

	public void GoToLevelSelectionScreen() {
		SceneManager.LoadScene ("LevelChoiceScreen");
	}

	public void ExitGame() {
		Application.Quit();
	}



}
