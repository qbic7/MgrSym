using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAI : MonoBehaviour
{
    [SerializeField] private GameObject opponentGO;

    SkillManager skillManager;
    PlayerStats ps, os;
    private string str = "Terrex";

    public void playerAI()
    {
        if (os.getCurrentHP() <= 5)
        {
            skillManager.basicAttack();
            skillManager.attackOpponent();
        }
        else if (os.getCurrentHP() <= 10)
        {
            if (skillManager.areaAttack()) { }
            else if (skillManager.strongAttack())
            {
                skillManager.attackOpponent();
            }
            else if (skillManager.freeze())
            {
                skillManager.attackOpponent();
            }
            else if (skillManager.strengthenSummon()) { }
            else if (skillManager.summon()) { }
            else if (ps.getCurrentHP() <= 25)
            {
                if (skillManager.heal()) { }
                else if (skillManager.mana()) { }
                else
                {
                    skillManager.basicAttack();
                    randomAttack();
                }
            }
            else if (ps.getCurrentMP() <= 10)
            {
                if (skillManager.mana()) { }
                else
                {
                    skillManager.basicAttack();
                    randomAttack();
                }
            }
            else
            {
                if (skillManager.strengthenSummon()) { }
                else if (skillManager.summon()) { }
                else if (skillManager.freeze())
                {
                    skillManager.attackOpponent();
                }
                else
                {
                    skillManager.basicAttack();
                    randomAttack();
                }
            }
        }
        else if (os.getCurrentHP() <= 20)
        {
            if (skillManager.strongAttack())
            {
                skillManager.attackOpponent();
            }
            else if (ps.getCurrentHP() <= 25)
            {
                if (skillManager.heal()) { }
                else if (skillManager.freeze())
                {
                    skillManager.attackOpponent();
                }
                else if (skillManager.mana()) { }
                else if (skillManager.summon()) { }
                else if (skillManager.strengthenSummon()) { }
                else
                {
                    skillManager.basicAttack();
                    randomAttack();
                }
            }
            else if (GameObject.FindGameObjectWithTag(str))
            {
                if (skillManager.areaAttack()) { }
                else if (skillManager.strengthenSummon()) { }
                else if (skillManager.summon()) { }
                else if (skillManager.mana()) { }
                else if (skillManager.freeze())
                {
                    skillManager.attackOpponent();
                }
                else
                {
                    skillManager.basicAttack();
                    randomAttack();
                }
            }
            else if (ps.getCurrentMP() <= 10)
            {
                if (skillManager.mana()) { }
                else if (skillManager.strengthenSummon()) { }
                else if (skillManager.summon()) { }
                else if (skillManager.freeze())
                {
                    randomAttack();
                }
                else
                {
                    skillManager.basicAttack();
                    randomAttack();
                }
            }
            else
            {
                if (skillManager.strengthenSummon()) { }
                else if (skillManager.summon()) { }
                else if (skillManager.freeze())
                {
                    skillManager.attackOpponent();
                }
                else
                {
                    skillManager.basicAttack();
                    randomAttack();
                }
            }
        }
        else if (ps.getCurrentHP() <= 40)
        {
            if (skillManager.heal()) { }
            else if (ps.getCurrentMP() <= 20)
            {
                if (skillManager.mana()) { }
                else if (skillManager.strengthenSummon()) { }
                else if (skillManager.summon()) { }
                else if (skillManager.strongAttack())
                {
                    randomAttack();
                }
                else if (skillManager.freeze())
                {
                    skillManager.attackOpponent();
                }
                else if (skillManager.areaAttack()) { }
                else
                {
                    skillManager.basicAttack();
                    randomAttack();
                }
            }
            else if (GameObject.FindGameObjectWithTag(str))
            {
                if (skillManager.areaAttack()) { }
                else if (skillManager.strongAttack())
                {
                    if (skillManager.attackMonster()) { }
                    else
                    {
                        skillManager.attackOpponent();
                    }
                }
                else if (skillManager.strengthenSummon()) { }
                else if (skillManager.summon()) { }
                else if (skillManager.freeze())
                {
                    randomAttack();
                }
                else
                {
                    skillManager.basicAttack();
                    randomAttack();
                }
            }
            else
            {
                if (skillManager.freeze())
                {
                    skillManager.attackOpponent();
                }
                else if (skillManager.strengthenSummon()) { }
                else if (skillManager.summon()) { }
                else if (skillManager.strongAttack())
                {
                    randomAttack();
                }
                else if (skillManager.areaAttack()) { }
                else
                {
                    skillManager.basicAttack();
                    randomAttack();
                }
            }
        }
        else if (ps.getCurrentMP() <= 20)
        {
            if (skillManager.mana()) { }
            else if (GameObject.FindGameObjectWithTag(str))
            {
                if (skillManager.areaAttack()) { }
                else if (skillManager.strongAttack())
                {
                    if (skillManager.attackMonster()) { }
                    else
                    {
                        skillManager.attackOpponent();
                    }
                }
                else if (skillManager.strengthenSummon()) { }
                else if (skillManager.summon()) { }
                else if (skillManager.freeze())
                {
                    randomAttack();
                }
                else
                {
                    skillManager.basicAttack();
                    randomAttack();
                }
            }
            else
            {
                if (skillManager.strengthenSummon()) { }
                else if (skillManager.summon()) { }
                else if (skillManager.freeze())
                {
                    skillManager.attackOpponent();
                }
                else if (skillManager.strongAttack())
                {
                    randomAttack();
                }
                else
                {
                    skillManager.basicAttack();
                    randomAttack();
                }
            }
        }
        else if (GameObject.FindGameObjectWithTag(str))
        {
            if (skillManager.areaAttack()) { }
            else if (skillManager.strengthenSummon()) { }
            else if (skillManager.summon()) { }
            else if (skillManager.strongAttack())
            {
                if (skillManager.attackMonster()) { }
                else
                {
                    skillManager.attackOpponent();
                }
            }
            else if (skillManager.freeze())
            {
                randomAttack();
            }
            else
            {
                skillManager.basicAttack();
                randomAttack();
            }
        }
        else
        {
            if (GameObject.FindGameObjectWithTag(str))
            {
                if (skillManager.areaAttack()) { }
                else if (skillManager.summon()) { }
                else if (skillManager.strengthenSummon()) { }
                else if (skillManager.strongAttack())
                {
                    opponentHP(25);
                }
                else if (skillManager.freeze())
                {
                    skillManager.attackOpponent();
                }
                else
                {
                    skillManager.basicAttack();
                    opponentHP(10);
                }
            }
            else
            {
                if (skillManager.summon()) { }
                else if (skillManager.strengthenSummon()) { }
                else if (skillManager.strongAttack())
                {
                    opponentHP(25);
                }
                else if (skillManager.areaAttack()) { }
                else if (skillManager.freeze())
                {
                    skillManager.attackOpponent();
                }
                else
                {
                    skillManager.basicAttack();
                    opponentHP(10);
                }
            }
        }
    }

    public void randomAttack()
    {
        int rnd = Random.Range(0, 2);
        if (rnd == 0)
        {
            skillManager.attackOpponent();
        }
        else
        {
            if (skillManager.attackMonster()) { }
            else
            {
                skillManager.attackOpponent();
            }
        }
    }

    public void opponentHP(float hp)
    {
        if (os.getCurrentHP() <= hp)
        {
            skillManager.attackOpponent();
        }
        else
        {
            randomAttack();
        }
    }

    private void Awake()
    {
        skillManager = gameObject.GetComponent<SkillManager>();
        ps = gameObject.GetComponent<PlayerStats>();
        os = opponentGO.GetComponent<PlayerStats>();
    }
}