using UnityEngine;
using UnityEngine.UI;

public class GameScoreUI : MonoBehaviour
{
    public Text redScoreText;
    public Text blueScoreText;
    public Text purpleScoreText;
    public Text greenScoreText;

    public void UpdateScores(
        playerMovement red,
        playerMovement blue,
        playerMovement purple,
        playerMovement green)
    {
        if (GameManager.selectedPlayersCount == 4)
        {
            redScoreText.text = "R = " + red.DisplayScore;
            blueScoreText.text = "B = " + blue.DisplayScore;
            purpleScoreText.text = "P = " + purple.DisplayScore;
            greenScoreText.text = "G = " + green.DisplayScore;
        }

        if (GameManager.selectedPlayersCount == 3)
        {
            redScoreText.text = "R = " + red.DisplayScore;
            blueScoreText.text = "B = " + blue.DisplayScore;
            purpleScoreText.text = "P = " + purple.DisplayScore;
            greenScoreText.text = " ";
        }

        if (red.isPlayerEliminated)
        {
            redScoreText.text = " ";
        }
        if (blue.isPlayerEliminated)
        {
            blueScoreText.text = " ";
        }
        if (purple.isPlayerEliminated)
        {
            purpleScoreText.text = " ";
        }
        if (green.isPlayerEliminated)
        {
            greenScoreText.text = " ";
        }
    }
}