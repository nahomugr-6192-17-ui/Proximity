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
    public int coinCounter = 2;
    public Text coinCounterText;
    public GameScoreUI gameScoreUI;
    public CameraController cameraController;
    public goldenCoin goldCoinScript;
    private List<playerMovement> activePlayers = new List<playerMovement>();
    private List<playerMovement> turnOrder = new List<playerMovement>();

    private int currentPlayerIndex = 0;
    private int currentTurn = 1;
    public int currentRound = 1;
    public Text roundCounterText;
    private const int turnsPerRound = 2;
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
        coinCounter = 2;
        coinCounterText.text = "COINS: " + coinCounter;
        roundCounterText.text = "ROUNDS: " + currentRound;
        StartTurn();
    }


    void StartTurn()
    {
        Debug.Log("--------------------------------");
        Debug.Log("ROUND " + currentRound + " | TURN " + currentTurn + " STARTED");
        Debug.Log("--------------------------------");

        foreach (playerMovement player in activePlayers)
        {
            player.ResetForNewTurn();
        }

        gameScoreUI.UpdateScores(redPlayer, bluePlayer, purplePlayer, greenPlayer);

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

        if (goldCoinScript.IsGoldCoinSafe())
        {
            Debug.Log("GOLD COIN IS SAFE");
        }
        else
        {
            Debug.Log("GOLD COIN IS NOT SAFE");
            goldCoinScript.ResetToStartPosition();
        }
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
            int newScore =
                player.CalculateCurrentScore();

            player.UpdateCurrentScore(newScore);

            Debug.Log(
                player.playerName +
                " Current Score = " +
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
        Debug.Log("TURN " + currentTurn + " FINISHED");

        foreach (playerMovement player in activePlayers)
        {
            player.CommitCurrentScoreToTotal();
        }

        gameScoreUI.UpdateScores(redPlayer, bluePlayer, purplePlayer, greenPlayer);

        ResetAllCoins();
        goldCoinScript.ResetToStartPosition();

        if (currentTurn < turnsPerRound)
        {
            currentTurn++;
            coinCounter--;
            coinCounterText.text = "COINS: " + coinCounter;
            StartTurn();
        }
        else
        {
            EndRound();
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

        Elimination();
        // Add this round's scores to total scores.
        ResetRoundScores();
        // Elimination will happen here.
        // Next-round order will happen here.

        coinCounter = 2;
        coinCounterText.text = "COINS: " + coinCounter;
        currentRound++;
        roundCounterText.text = "ROUNDS: " + currentRound;

        if (currentRound > 4)
        {
            Debug.Log("GAME OVER");
            return;
        }

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

    void Elimination()
    {
        if (activePlayers.Count <= 1)
        {
            Debug.Log("The Winner is: " + activePlayers[0].playerName);
            return;
        }
        activePlayers.Sort((a, b) => a.TotalScore.CompareTo(b.TotalScore));
        playerMovement lowestScoringPlayer = activePlayers[0];

        foreach (playerMovement player in activePlayers)
        {
            if (player.TotalScore < lowestScoringPlayer.TotalScore && coinCounter <= 0)
            {
                lowestScoringPlayer = player;
            }
        }

        activePlayers.Remove(lowestScoringPlayer);
        Debug.Log("Player " + lowestScoringPlayer.playerName + " has been eliminated.");
        activePlayers.Remove(lowestScoringPlayer.DisplayScore);
    }
}