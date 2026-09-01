using UnityEngine.SceneManagement;
using UnityEngine;

public class winnerCoinScript : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color HexColor(string hex)
    {
        Color color;

        if (ColorUtility.TryParseHtmlString(hex, out color))
        {
            return color;
        }

        return Color.black;
    }
    public void Update()
    {
        if (GameManager.winnerName == "Red Player")
        {
            spriteRenderer.color = HexColor("#C00000");
        }
        else if (GameManager.winnerName == "Blue Player")
        {
            spriteRenderer.color = HexColor("#3BBCC8");
        }
        else if (GameManager.winnerName == "Green Player")
        {
            spriteRenderer.color = HexColor("#13B807");
        }
        else if (GameManager.winnerName == "Purple Player")
        {
            spriteRenderer.color = HexColor("#800080");
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }
}
