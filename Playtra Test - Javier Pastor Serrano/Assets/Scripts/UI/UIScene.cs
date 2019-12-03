using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScene : MonoBehaviour
{
  // Loads a new scene.
  public void ChangeScene(int scene)
  {
    SceneManager.LoadScene(scene);
  }
  // Exits the game.
  public void ExitGame()
  {
    Application.Quit();
  }
}
