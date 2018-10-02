using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;

public class GameManager : MonoBehaviour {

    [SerializeField] private GameObject playerGO, cpuGO;
    [SerializeField] private int numberOfGames = 100, wins = 0, numberOfRepetitions = 10;
    [SerializeField] private string fileName;

    private GameObject monster;

    PlayerStats player, cpu;
    SkillManager skillManagerPlayer, skillManagerCpu;
    Turn turn;
    PlayerAI playerAI;
    EnemyAI enemyAI;
    MonsterAI mAI;
    MonsterStats ms;

    private int gameIndex = 1, startNumberOfGames;
    private bool playerMoved, cpuMoved;

    public void switchMove()
    {
        if (!isGameOver())
        {
            if (playerMoved)
            {
                playerMoved = false;
                if (monsterExist("Terrex") && !monsterFreezed())
                {
                    mAI = monster.GetComponent<MonsterAI>();
                    mAI.attack();
                }
                if (!playerFreezed(cpu))
                {
                    enemyAI.enemyAI();
                }
                cpuMoved = true;
                cpu.setIsFreezed(false);
                if (ms != null && ms.getIsFreezed())
                    ms.setIsFreezed(false);
                turn.switchMove();
            }
            else if (cpuMoved)
            {
                cpuMoved = false;
                if (monsterExist("Goblin") && !monsterFreezed())
                {
                    mAI = monster.GetComponent<MonsterAI>();
                    mAI.attack();
                }
                if (!playerFreezed(player))
                {
                    playerAI.playerAI();
                }
                playerMoved = true;
                player.setIsFreezed(false);
                if (ms != null && ms.getIsFreezed())
                    ms.setIsFreezed(false);
                turn.switchMove();
            }

            if (turn.getMove() % 2 == 1 && turn.getTurnChanged())
            {
                enemyAI.setSkillInPath();
                skillManagerPlayer.countdown();
                skillManagerCpu.countdown();
                turn.setTurnChanged(false);
            }
        }
    }

    public bool monsterExist(string str)
    {
        monster = GameObject.FindGameObjectWithTag(str);
        if (monster)
        {
            return true;
        }
        return false;
    }

    public bool playerFreezed(PlayerStats ps)
    {
        if (ps.getIsFreezed())
            return true;
        else
            return false;
    }

    public bool monsterFreezed()
    {
        ms = monster.GetComponent<MonsterStats>();
        if (ms.getIsFreezed())
            return true;
        else
            return false;
    }

    public bool isGameOver()
    {
        if (player.getIsDead())
        {
            return true;
        }else if (cpu.getIsDead())
        {
            return true;
        }
        return false;
    }

    public void resetGame()
    {
        if (monsterExist("Goblin"))
        {
            ms = monster.GetComponent<MonsterStats>();
            ms.destroyMonster();
        }
        if (monsterExist("Terrex"))
        {
            ms = monster.GetComponent<MonsterStats>();
            ms.destroyMonster();
        }
        skillManagerPlayer.defaultValues();
        skillManagerCpu.defaultValues();
        turn.defaultValues();
        player.defaultValues();
        cpu.defaultValues();
        setFirstMove();
    }

    public void exitGame()
    {
        Application.Quit();
    }

    public void setFirstMove()
    {
        if (!turn.getPlayerTurn())
        {
            playerMoved = true;
        }
        else if (turn.getPlayerTurn())
        {
            cpuMoved = true;
        }
    }

    public void statistics()
    {
        int x = 2;

        if (player.getIsDead())
        {
            x = 0;
            wins++;
            enemyAI.endGameReward(true);
        }
        else if (cpu.getIsDead())
        {
            x = 1;
            enemyAI.endGameReward(false);
        }

        using(StreamWriter sw = new StreamWriter(fileName, true))
        {
            sw.WriteLine(buildString(x));
        }

        gameIndex++;
    }

    public string buildString(int value)
    {
        StringBuilder sb = new StringBuilder(15);

        sb.Append(gameIndex + "-");
        sb.Append(value + "-");
        sb.Append(turn.getFirstMove() + "-");
        sb.Append(turn.getTurn());
        return sb.ToString();
    }

    private void Awake()
    {
        player = playerGO.GetComponent<PlayerStats>();
        cpu = cpuGO.GetComponent<PlayerStats>();
        skillManagerPlayer = playerGO.GetComponent<SkillManager>();
        skillManagerCpu = cpuGO.GetComponent<SkillManager>();
        turn = gameObject.GetComponent<Turn>();
        enemyAI = cpuGO.GetComponent<EnemyAI>();
        playerAI = playerGO.GetComponent<PlayerAI>();
    }

    // Use this for initialization
    void Start () {
        setFirstMove();
        startNumberOfGames = numberOfGames;
    }

    public void changeEpsilon()
    {
        if (wins % 2 == 0 && wins != 0 && wins < 10 && enemyAI.getEpsilon() > 0.5f)
        {
            enemyAI.setEpsilon(enemyAI.getEpsilon() - 0.1f);
        }
        else if (wins % 10 == 0 && wins != 0 && enemyAI.getEpsilon() > 0.2f)
        {
            enemyAI.setEpsilon(enemyAI.getEpsilon() - 0.1f);
        }
        else if (numberOfGames % 10 == 0 && numberOfGames != startNumberOfGames && enemyAI.getEpsilon() > 0.4f)
        {
            enemyAI.setEpsilon(enemyAI.getEpsilon() - 0.1f);
        }
        else if (wins == 100 && enemyAI.getEpsilon() == 0.2f)
        {
            enemyAI.setEpsilon(0.1f);
        }
    }

    // Update is called once per frame
    void Update () {
        switchMove();

        if (isGameOver() && numberOfRepetitions > 1)
        {
            if (numberOfGames > 1)
            {
                changeEpsilon();
                statistics();
                resetGame();
                numberOfGames--;
                enemyAI.sortPriorities();
            }
            else if (numberOfGames == 1)
            {
                statistics();
                resetGame();
                enemyAI.setEpsilon(1f);
                wins = 0;
                gameIndex = 1;
                numberOfGames = startNumberOfGames;
                numberOfRepetitions--;
                enemyAI.resetPriorities();
            }
        }
        else if (isGameOver() && numberOfRepetitions == 1)
        {
            if (numberOfGames > 1)
            {
                changeEpsilon();
                statistics();
                resetGame();
                numberOfGames--;
                enemyAI.sortPriorities();
            }
            else if (numberOfGames == 1)
            {
                statistics();
                numberOfGames = 0;
                numberOfRepetitions = 0;
                //enemyAI.getQueue();
                Debug.Log("The end");
            }
        }
    }
}
