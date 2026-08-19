using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("Players")]
    public playerMovement redPlayer;
    public playerMovement greenPlayer;
    public playerMovement bluePlayer;
    public playerMovement purplePlayer;
    public GameScoreUI gameScoreUI;
    public CameraController cameraController;
    private List<playerMovement> activePlayers = new List<playerMovement>();
    private List<playerMovement> turnOrder = new List<playerMovement>();

    private int currentPlayerIndex = 0;
    private int currentTurn = 1;
    private int currentRound = 1;

    private const int turnsPerRound = 3;


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

        cameraController.RotateToPlayer(currentPlayer);

        currentPlayer.StartPlayerTurn();
    }


    public void PlayerFinishedTurn(playerMovement player)
    {
        Debug.Log(
            player.playerName +
            " FINISHED THEIR SHOT."
        );
        gameScoreUI.UpdateScores(
            redPlayer,
            bluePlayer,
            purplePlayer,
            greenPlayer
        );
        currentPlayerIndex++;

        StartNextPlayer();
    }


    void EndTurn()
    {
        Debug.Log(
            "TURN " + currentTurn +
            " FINISHED"
        );

        // Reset every player's coin
        ResetAllCoins();

        if (currentTurn < turnsPerRound)
        {
            // There is another turn in this round.
            currentTurn++;

            StartTurn();
        }
        else
        {
            // All 3 turns are complete.
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

        // We will add score processing here.
        // We will add elimination here.
        // We will add next-round ordering here.

        currentRound++;

        StartRound();
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