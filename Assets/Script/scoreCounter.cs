using UnityEngine;
using UnityEngine.UI;

public class scoreCounter : MonoBehaviour
{
    public Text scoreText;
    private int score = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreText.text = "Score: " + score;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
