using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turn : MonoBehaviour {

    private int turn = 1, move = 1, firstMove;
    private bool playerTurn, turnChanged = false;

    public int getTurn()
    {
        return turn;
    }

    public int getMove()
    {
        return move;
    }

    public bool getPlayerTurn()
    {
        return playerTurn;
    }

    public bool getTurnChanged()
    {
        return turnChanged;
    }

    public void setTurnChanged(bool value)
    {
        turnChanged = value;
    }

    public int getFirstMove()
    {
        return firstMove;
    }

    public void nextTurn()
    {
        turn++;
        turnChanged = true;
    }

    public void switchMove()
    {
        playerTurn = !playerTurn;
        move++;
        if(move % 2 == 1)
        {
            nextTurn();
        }
    }

    public void randomFirstMove()
    {
        firstMove = Random.Range(0, 2);

        if (firstMove == 0)
        {
            playerTurn = true;
        }
        else if (firstMove == 1)
        {
            playerTurn = false;
        }
    }

    public void defaultValues()
    {
        turn = 1;
        move = 1;
        randomFirstMove();
        turnChanged = false;
    }

    private void Awake()
    {
        randomFirstMove();
    }
}
