using UnityEngine;

public class MainMenuActions : MonoBehaviour
{
    public void newGame()
    {
        GameStatus.GoToLevel_1();
    }
    public void loadCredits()
    {
        GameStatus.GoToCredits();
    }
}
