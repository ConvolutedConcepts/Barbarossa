using UnityEngine;

public class MainMenuActions : MonoBehaviour
{
    public void newGame()
    {
        GameStatus.GoToDiary();
    }
    public void loadCredits()
    {
        GameStatus.GoToCredits();
    }
}
