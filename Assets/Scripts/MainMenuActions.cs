using UnityEngine;

public class MainMenuActions : MonoBehaviour
{
    public void newGame()
    {
        GameStatus.coins = 0;
        GameStatus.healthBarAmount = 0;
        GameStatus.GoToDiary();
    }
    public void loadCredits()
    {
        GameStatus.GoToCredits();
    }
	public void quitGame()
	{
		Application.Quit ();
	}
}
