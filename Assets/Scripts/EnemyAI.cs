using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private GameObject opponentGO;

    private GameObject monster;
    SkillManager skillManager;
    PlayerStats ps, os;
    MonsterStats ms;
    private float epsilon = 1f, metaChoice = 0.5f;

    private Action tempAct = null;

    private List<Action>[] states;
    private List<Action>[] metaGame;

    public float getEpsilon()
    {
        return epsilon;
    }

    public void setEpsilon(float value)
    {
        epsilon = value;
    }

    public void setSkillInPath()
    {
        if (tempAct != null)
        {
            tempAct.setIsUsed(true);
        }
    }

    public Action getTemp()
    {
        return tempAct;
    }

    public void enemyAI()
    {
        float rnd = Random.Range(0f, 1f);

        if (rnd > epsilon)
        {
            if (os.getCurrentHP() <= 10)
            {
                choosePriorities(0);
            }
            else if (os.getCurrentHP() <= 20)
            {
                choosePriorities(1);
            }
            else if (monsterExist("Goblin"))
            {
                choosePriorities(2);
            }
            else if (ps.getCurrentHP() <= 50)
            {
                choosePriorities(3);
            }
            else if (ps.getCurrentMP() <= 50)
            {
                choosePriorities(4);
            }
            else
            {
                choosePriorities(5);
            }
        }
        else
        {
            if (os.getCurrentHP() <= 10)
            {
                randomSkill(0);
            }
            else if (os.getCurrentHP() <= 20)
            {
                randomSkill(1);
            }
            else if (monsterExist("Goblin"))
            {
                randomSkill(2);
            }
            else if (ps.getCurrentHP() <= 50)
            {
                randomSkill(3);
            }
            else if (ps.getCurrentMP() <= 50)
            {
                randomSkill(4);
            }
            else
            {
                randomSkill(5);
            }
        }
    }

    public void sortPriorities()
    {
        for (int i = 0; i < states.Length; i++)
        {
            states[i].Sort((x, y) => y.getSummaryReward().CompareTo(x.getSummaryReward()));
        }
    }

    public void resetPriorities()
    {
        for (int i = 0; i < states.Length; i++)
        {
            states[i].Clear();
        }
    }

    public int endReward(PlayerStats tmpPS, string str, bool winGame)
    {
        int rewardValue = 0;
        float summary = 0;
        if (monsterExist(str))
        {
            ms = monster.GetComponent<MonsterStats>();
            summary = tmpPS.getCurrentHP() + ms.getHP();
        }
        else
        {
            summary = tmpPS.getCurrentHP();
        }
        if (winGame)
        {
            while (summary > 0)
            {
                rewardValue++;
                summary -= 10;
            }
        }
        else
        {
            while (summary > 0)
            {
                rewardValue--;
                summary -= 10;
            }
        }
        return rewardValue;
    }

    public void endGameReward(bool winGame)
    {
        int rewardValue = 0;

        if (winGame)
        {
            rewardValue = endReward(ps, "Terrex", true);
        }
        else
        {
            rewardValue = endReward(os, "Goblin", false);
        }

        for (int i = 0; i < states.Length; i++)
        {
            foreach (Action var in states[i])
            {
                if (var.getIsUsed())
                {
                    var.setSummaryReward(rewardValue);
                }

                var.setIsUsed(false);
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

    // szuka dostepnej umiejetnosci wg priorytetu z wyuczonej dotad sciezki
    public void choosePriorities(int stateNumber)
    {
        bool skillUsed = false;

        foreach (Action var in states[stateNumber])
        {
            if (skillToUse(var.getSkill(), var.getTarget()))
            {
                tempAct = var;
                skillUsed = true;
                break;
            }
        }

        if (!skillUsed)
        {
            randomSkill(stateNumber);
        }
    }

    // uzywa umiejetnosci wybranej z wyuczonej dotad sciezki
    public bool skillToUse(string skillName, string target)
    {
        bool isUsed = false;

        switch (skillName)
        {
            case "basicAttack":
                if (skillManager.basicAttack())
                {
                    if (target.Equals("opponent"))
                    {
                        skillManager.attackOpponent();
                        isUsed = true;
                    }
                    else if (target.Equals("monster"))
                    {
                        if (skillManager.attackMonster())
                        {
                            isUsed = true;
                        }
                    }
                }
                break;
            case "strongAttack":
                if (skillManager.strongAttack())
                {
                    if (target.Equals("opponent"))
                    {
                        skillManager.attackOpponent();
                        isUsed = true;
                    }
                    else if (target.Equals("monster"))
                    {
                        if (skillManager.attackMonster())
                        {
                            isUsed = true;
                        }
                    }
                }
                break;
            case "areaAttack":
                if (skillManager.areaAttack())
                {
                    isUsed = true;
                }
                break;
            case "heal":
                if (skillManager.heal())
                {
                    isUsed = true;
                }
                break;
            case "mana":
                if (skillManager.mana())
                {
                    isUsed = true;
                }
                break;
            case "summon":
                if (skillManager.summon())
                {
                    isUsed = true;
                }
                break;
            case "strengthenSummon":
                if (skillManager.strengthenSummon())
                {
                    isUsed = true;
                }
                break;
            case "freeze":
                if (skillManager.freeze())
                {
                    if (target.Equals("opponent"))
                    {
                        skillManager.attackOpponent();
                        isUsed = true;
                    }
                    else if (target.Equals("monster"))
                    {
                        if (skillManager.attackMonster())
                        {
                            isUsed = true;
                        }
                    }
                }
                break;
            default:
                isUsed = false;
                break;
        }

        if (isUsed)
            return true;
        else
            return false;
    }

    public void randomSkill(int stateNumber)
    {
        bool skillUsed = false;

        while (!skillUsed)
        {
            float rndMetaChoice = Random.Range(0f, 1f);

            if (rndMetaChoice < metaChoice)
            {
                foreach (Action var in metaGame[stateNumber])
                {
                    if (skillToUse(var.getSkill(), var.getTarget()))
                    {
                        Action tmpAct = isSkillKnown(stateNumber, var.getSkill(), var.getTarget());
                        if (tmpAct == null)
                        {
                            states[stateNumber].Add(new Action(var.getSkill(), var.getTarget()));
                            tempAct = states[stateNumber].Last();
                        }
                        else
                        {
                            tempAct = tmpAct;
                        }
                        skillUsed = true;
                        break;
                    }
                }
            }
            else
            {
                int rndSkill = Random.Range(0, 8);

                switch (rndSkill)
                {
                    case 0:
                        if (skillManager.basicAttack())
                        {
                            if (randomAttack())
                            {
                                Action tmpAct = isSkillKnown(stateNumber, "basicAttack", "opponent");
                                if (tmpAct == null)
                                {
                                    states[stateNumber].Add(new Action("basicAttack", "opponent"));
                                    tempAct = states[stateNumber].Last();
                                }
                                else
                                {
                                    tempAct = tmpAct;
                                }
                            }
                            else
                            {
                                Action tmpAct = isSkillKnown(stateNumber, "basicAttack", "monster");
                                if (tmpAct == null)
                                {
                                    states[stateNumber].Add(new Action("basicAttack", "monster"));
                                    tempAct = states[stateNumber].Last();
                                }
                                else
                                {
                                    tempAct = tmpAct;
                                }
                            }
                            skillUsed = true;
                        }
                        break;
                    case 1:
                        if (skillManager.strongAttack())
                        {
                            if (randomAttack())
                            {
                                Action tmpAct = isSkillKnown(stateNumber, "strongAttack", "opponent");
                                if (tmpAct == null)
                                {
                                    states[stateNumber].Add(new Action("strongAttack", "opponent"));
                                    tempAct = states[stateNumber].Last();
                                }
                                else
                                {
                                    tempAct = tmpAct;
                                }
                            }
                            else
                            {
                                Action tmpAct = isSkillKnown(stateNumber, "strongAttack", "monster");
                                if (tmpAct == null)
                                {
                                    states[stateNumber].Add(new Action("strongAttack", "monster"));
                                    tempAct = states[stateNumber].Last();
                                }
                                else
                                {
                                    tempAct = tmpAct;
                                }
                            }
                            skillUsed = true;
                        }
                        break;
                    case 2:
                        if (skillManager.areaAttack())
                        {
                            Action tmpAct = isSkillKnown(stateNumber, "areaAttack", null);
                            if (tmpAct == null)
                            {
                                states[stateNumber].Add(new Action("areaAttack", null));
                                tempAct = states[stateNumber].Last();
                            }
                            else
                            {
                                tempAct = tmpAct;
                            }
                            skillUsed = true;
                        }
                        break;
                    case 3:
                        if (skillManager.heal())
                        {
                            Action tmpAct = isSkillKnown(stateNumber, "heal", null);
                            if (tmpAct == null)
                            {
                                states[stateNumber].Add(new Action("heal", null));
                                tempAct = states[stateNumber].Last();
                            }
                            else
                            {
                                tempAct = tmpAct;
                            }
                            skillUsed = true;
                        }
                        break;
                    case 4:
                        if (skillManager.mana())
                        {
                            Action tmpAct = isSkillKnown(stateNumber, "mana", null);
                            if (tmpAct == null)
                            {
                                states[stateNumber].Add(new Action("mana", null));
                                tempAct = states[stateNumber].Last();
                            }
                            else
                            {
                                tempAct = tmpAct;
                            }
                            skillUsed = true;
                        }
                        break;
                    case 5:
                        if (skillManager.summon())
                        {
                            Action tmpAct = isSkillKnown(stateNumber, "summon", null);
                            if (tmpAct == null)
                            {
                                states[stateNumber].Add(new Action("summon", null));
                                tempAct = states[stateNumber].Last();
                            }
                            else
                            {
                                tempAct = tmpAct;
                            }
                            skillUsed = true;
                        }
                        break;
                    case 6:
                        if (skillManager.strengthenSummon())
                        {
                            Action tmpAct = isSkillKnown(stateNumber, "strengthenSummon", null);
                            if (tmpAct == null)
                            {
                                states[stateNumber].Add(new Action("strengthenSummon", null));
                                tempAct = states[stateNumber].Last();
                            }
                            else
                            {
                                tempAct = tmpAct;
                            }
                            skillUsed = true;
                        }
                        break;
                    case 7:
                        if (skillManager.freeze())
                        {
                            if (randomAttack())
                            {
                                Action tmpAct = isSkillKnown(stateNumber, "freeze", "opponent");
                                if (tmpAct == null)
                                {
                                    states[stateNumber].Add(new Action("freeze", "opponent"));
                                    tempAct = states[stateNumber].Last();
                                }
                                else
                                {
                                    tempAct = tmpAct;
                                }
                            }
                            else
                            {
                                Action tmpAct = isSkillKnown(stateNumber, "freeze", "monster");
                                if (tmpAct == null)
                                {
                                    states[stateNumber].Add(new Action("freeze", "monster"));
                                    tempAct = states[stateNumber].Last();
                                }
                                else
                                {
                                    tempAct = tmpAct;
                                }
                            }
                            skillUsed = true;
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public bool randomAttack()
    {
        int rndAttack = Random.Range(0, 2);

        if (rndAttack == 0)
        {
            skillManager.attackOpponent();
            return true;
        }
        else
        {
            if (skillManager.attackMonster())
            {
                return false;
            }
            else
            {
                skillManager.attackOpponent();
                return true;
            }
        }
    }

    // Sprawdza czy w danym stanie jest dana umiejetnosc
    public Action isSkillKnown(int stateNumber, string skill, string target)
    {
        Action act = null;

        foreach (Action var in states[stateNumber])
        {
            if (skill.Equals("basicAttack") || skill.Equals("strongAttack") || skill.Equals("freeze"))
            {
                if (var.getSkill().Equals(skill) && var.getTarget().Equals(target))
                {
                    act = var;
                    break;
                }
            }
            else if (var.getSkill().Equals(skill))
            {
                act = var;
                break;
            }
        }

        if (act != null)
            return act;
        else
            return null;
    }

    public void getQueue()
    {
        for (int i = 0; i < states.Length; i++)
        {
            Debug.Log(states[i].Count);
            foreach (Action var in states[i])
            {
                Debug.Log(var.getSkill() + ", " + var.getTarget() + ", " + var.getSummaryReward());
            }
        }
    }

    public void metaGameInit()
    {
        metaGame[0].Add(new Action("areaAttack", null));
        metaGame[0].Add(new Action("strongAttack", "opponent"));
        metaGame[0].Add(new Action("freeze", "opponent"));
        metaGame[0].Add(new Action("strengthenSummon", null));
        metaGame[0].Add(new Action("summon", null));
        metaGame[0].Add(new Action("strongAttack", "monster"));
        metaGame[0].Add(new Action("basicAttack", "opponent"));
        metaGame[0].Add(new Action("basicAttack", "monster"));
        metaGame[0].Add(new Action("freeze", "monster"));
        metaGame[0].Add(new Action("heal", null));
        metaGame[0].Add(new Action("mana", null));

        metaGame[1].Add(new Action("strongAttack", "opponent"));
        metaGame[1].Add(new Action("strongAttack", "monster"));
        metaGame[1].Add(new Action("freeze", "opponent"));
        metaGame[1].Add(new Action("strengthenSummon", null));
        metaGame[1].Add(new Action("summon", null));
        metaGame[1].Add(new Action("areaAttack", null));
        metaGame[1].Add(new Action("freeze", "monster"));
        metaGame[1].Add(new Action("basicAttack", "opponent"));
        metaGame[1].Add(new Action("basicAttack", "monster"));
        metaGame[1].Add(new Action("heal", null));
        metaGame[1].Add(new Action("mana", null));

        metaGame[2].Add(new Action("strongAttack", "monster"));
        metaGame[2].Add(new Action("strengthenSummon", null));
        metaGame[2].Add(new Action("summon", null));
        metaGame[2].Add(new Action("freeze", "opponent"));
        metaGame[2].Add(new Action("areaAttack", null));
        metaGame[2].Add(new Action("strongAttack", "opponent"));
        metaGame[2].Add(new Action("freeze", "monster"));
        metaGame[2].Add(new Action("basicAttack", "opponent"));
        metaGame[2].Add(new Action("basicAttack", "monster"));
        metaGame[2].Add(new Action("heal", null));
        metaGame[2].Add(new Action("mana", null));

        metaGame[3].Add(new Action("heal", null));
        metaGame[3].Add(new Action("strengthenSummon", null));
        metaGame[3].Add(new Action("summon", null));
        metaGame[3].Add(new Action("freeze", "opponent"));
        metaGame[3].Add(new Action("strongAttack", "opponent"));
        metaGame[3].Add(new Action("basicAttack", "opponent"));
        metaGame[3].Add(new Action("mana", null));
        metaGame[3].Add(new Action("areaAttack", null));
        metaGame[3].Add(new Action("strongAttack", "monster"));
        metaGame[3].Add(new Action("basicAttack", "monster"));
        metaGame[3].Add(new Action("freeze", "monster"));

        metaGame[4].Add(new Action("mana", null));
        metaGame[4].Add(new Action("freeze", "opponent"));
        metaGame[4].Add(new Action("strengthenSummon", null));
        metaGame[4].Add(new Action("summon", null));
        metaGame[4].Add(new Action("strongAttack", "opponent"));
        metaGame[4].Add(new Action("areaAttack", null));
        metaGame[4].Add(new Action("basicAttack", "opponent"));
        metaGame[4].Add(new Action("heal", null));
        metaGame[4].Add(new Action("strongAttack", "monster"));
        metaGame[4].Add(new Action("basicAttack", "monster"));
        metaGame[4].Add(new Action("freeze", "monster"));

        metaGame[5].Add(new Action("strengthenSummon", null));
        metaGame[5].Add(new Action("summon", null));
        metaGame[5].Add(new Action("areaAttack", null));
        metaGame[5].Add(new Action("freeze", "opponent"));
        metaGame[5].Add(new Action("basicAttack", "opponent"));
        metaGame[5].Add(new Action("strongAttack", "opponent"));
        metaGame[5].Add(new Action("heal", null));
        metaGame[5].Add(new Action("mana", null));
        metaGame[5].Add(new Action("strongAttack", "monster"));
        metaGame[5].Add(new Action("basicAttack", "monster"));
        metaGame[5].Add(new Action("freeze", "monster"));
    }

    private void Awake()
    {
        skillManager = gameObject.GetComponent<SkillManager>();
        ps = gameObject.GetComponent<PlayerStats>();
        os = opponentGO.GetComponent<PlayerStats>();

        states = new List<Action>[6];
        metaGame = new List<Action>[6];

        for (int i = 0; i < states.Length; i++)
        {
            states[i] = new List<Action>();
        }

        for (int i = 0; i < metaGame.Length; i++)
        {
            metaGame[i] = new List<Action>();
        }

        metaGameInit();
    }
}