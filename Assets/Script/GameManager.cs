using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Players")]
    public playerMovement redPlayer;
    public playerMovement greenPlayer;
    public playerMovement bluePlayer;
    public playerMovement purplePlayer;
    public int coinCounter = 3;
    public Text coinCounterText;
    public GameScoreUI gameScoreUI;
    public CameraController cameraController;
    public goldenCoin goldCoinScript;
    private List<playerMovement> activePlayers = new List<playerMovement>();
    private List<playerMovement> turnOrder = new List<playerMovement>();

    private int currentPlayerIndex = 0;
    private int currentTurn = 1;
    private int currentRound = 1;

    private const int turnsPerRound = 3;
    private bool isFirstPlayerOfGame = true;


    void Start()
    {
        activePlayers.Add(redPlayer);
        activePlayers.Add(greenPlayer);
        activePlayers.Add(bluePlayer);
        activePlayers.Add(purplePlayer);
        gameScoreUI.UpdateScores(
            redPlayer,
            bluePlayer,
            purplePlayer,
            greenPlayer
        );
        StartRound();
    }


    void StartRound()
    {
        Debug.Log("================================");
        Debug.Log("ROUND " + currentRound + " STARTED");
        Debug.Log("================================");
        currentTurn = 1;
        ShuffleTurnOrder();
        StartTurn();
    }


    void StartTurn()
    {
        Debug.Log("--------------------------------");
        Debug.Log(
            "ROUND " + currentRound +
            " | TURN " + currentTurn + " STARTED"
        );
        Debug.Log("--------------------------------");

        currentPlayerIndex = 0;
        StartNextPlayer();
    }

    void StartNextPlayer()
    {
        if (currentPlayerIndex >= turnOrder.Count)
        {
            EndTurn();
            return;
        }

        playerMovement currentPlayer =
            turnOrder[currentPlayerIndex];

        Debug.Log(
            "Round " + currentRound +
            " | Turn " + currentTurn +
            " | Current Player: " +
            currentPlayer.playerName
        );

        cameraController.RotateToPlayer(
            currentPlayer,
            isFirstPlayerOfGame
        );

        isFirstPlayerOfGame = false;

        currentPlayer.StartPlayerTurn();
    }

    public void PlayerFinishedTurn(playerMovement player)
    {
        Debug.Log(
            player.playerName +
            " FINISHED THEIR SHOT."
        );

        // Recalculate all players because this shot
        // may have moved other player coins.
        RecalculateAllScores();

        currentPlayerIndex++;

        StartNextPlayer();
    }

    public playerMovement CurrentPlayer
    {
        get
        {
            if (currentPlayerIndex < 0 ||
                currentPlayerIndex >= turnOrder.Count)
            {
                return null;
            }

            return turnOrder[currentPlayerIndex];
        }
    }

    void RecalculateAllScores()
    {
        foreach (playerMovement player in activePlayers)
        {
            int newScore = player.CalculateCurrentScore();

            player.UpdateCurrentScore(newScore);

            Debug.Log(
                player.playerName +
                " | Current Score = " +
                player.CurrentScore +
                " | Total Score = " +
                player.TotalScore
            );
        }

        gameScoreUI.UpdateScores(
            redPlayer,
            bluePlayer,
            purplePlayer,
            greenPlayer
        );
    }


    void EndTurn()
    {
        Debug.Log(
            "TURN " + currentTurn + " FINISHED"
        );

        // Scores have already been updated
        // after every player's shot.

        ResetAllCoins();

        goldCoinScript.ResetToStartPosition();

        coinCounter--;

        coinCounterText.text =
            "Coins: " + coinCounter;

        if (currentTurn < turnsPerRound)
        {
            currentTurn++;

            StartTurn();
        }
        else
        {
            EndRound();
        }
    }

    void CalculateTurnScores()
    {
        foreach (playerMovement player in activePlayers)
        {
            int score = player.CalculateCurrentScore();

            player.UpdateCurrentScore(score);

            Debug.Log(
                player.playerName +
                " FINAL SCORE FOR TURN " +
                currentTurn +
                " = " +
                score
            );
        }
    }
    void EndRound()
    {
        Debug.Log("================================");
        Debug.Log(
            "ROUND " + currentRound +
            " COMPLETE"
        );
        Debug.Log("================================");

        // Add this round's scores to total scores.
        ResetRoundScores();
        // Elimination will happen here.
        // Next-round order will happen here.

        currentRound++;

        StartRound();
    }

    void ResetRoundScores()
    {
        foreach (playerMovement player in activePlayers)
        {
            player.ResetRoundScoreForNewRound();
        }

        gameScoreUI.UpdateScores(
            redPlayer,
            bluePlayer,
            purplePlayer,
            greenPlayer
        );
    }


    void ResetAllCoins()
    {
        Debug.Log("RESETTING ALL PLAYER COINS");

        foreach (playerMovement player in activePlayers)
        {
            player.ResetCoin();
        }
    }


    void ShuffleTurnOrder()
    {
        turnOrder = new List<playerMovement>(activePlayers);

        for (int i = 0; i < turnOrder.Count; i++)
        {
            int randomIndex =
                Random.Range(i, turnOrder.Count);

            playerMovement temp = turnOrder[i];

            turnOrder[i] =
                turnOrder[randomIndex];

            turnOrder[randomIndex] =
                temp;
        }

        Debug.Log("Turn Order:");

        foreach (playerMovement player in turnOrder)
        {
            Debug.Log(player.playerName);
        }
    }
}