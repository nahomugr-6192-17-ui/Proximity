using UnityEngine;
using UnityEngine.SceneManagement;

public class choiceScript : MonoBehaviour
{
    public void Choose3Players()
    {
        GameManager.selectedPlayersCount = 3;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex + 1
        );
    }

    public void Choose4Players()
    {
        GameManager.selectedPlayersCount = 4;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex + 1
        );
    }
}