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
        redScoreText.text = "R = " + red.TotalScore;
        blueScoreText.text = "B = " + blue.TotalScore;
        purpleScoreText.text = "P = " + purple.TotalScore;
        greenScoreText.text = "G = " + green.TotalScore;
    }
}