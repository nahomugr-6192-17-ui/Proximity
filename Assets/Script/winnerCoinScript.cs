using UnityEngine.SceneManagement;
using UnityEngine;

public class winnerCoinScript : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Debug.Log("Winner Name: " + GameManager.winnerName);
        ColorChange(GameManager.winnerName);
    }
    private static Color HexColor(string hex)
    {
        Color color;

        if (ColorUtility.TryParseHtmlString(hex, out color))
        {
            return color;
        }

        return Color.black;
    }
    public void ColorChange(string winnerName)
    {
        if (winnerName == "Red")
        {
            spriteRenderer.color = HexColor("#C00000");
        }
        else if (winnerName == "Blue")
        {
            spriteRenderer.color = HexColor("#3BBCC8");
        }
        else if (winnerName == "Green")
        {
            spriteRenderer.color = HexColor("#13B807");
        }
        else if (winnerName == "Purple")
        {
            spriteRenderer.color = HexColor("#800080");
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }
}
