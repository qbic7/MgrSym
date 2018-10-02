using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour {

    [SerializeField] private GameObject opponentGO;
    [SerializeField] private GameObject monsterPrefab;
    [SerializeField] private string monsterPlayer, monsterOpponent;
    [SerializeField] private float basicAtkDmg, strAtkDmg, areaAtkDmg, healValue, manaValue, strSummonAtk, strSummonHP;

    private GameObject monsterGO;

    Skill[] skills;
    PlayerStats player, opponent;
    MonsterStats ms;

    private float hp, mp;
    private int skillNumber = -1;

    public void attackOpponent()
    {
        if (skillNumber == 7)
        {
            opponent.setIsFreezed(true);
        }
        else
        {
            opponent.loseHP(hp);
        }
        player.loseMP(mp);
        skills[skillNumber].setCooldown();
    }

    public bool attackMonster()
    {
        monsterGO = GameObject.FindGameObjectWithTag(monsterOpponent);
        if (monsterGO != null)
        {
            ms = monsterGO.GetComponent<MonsterStats>();
            if (skillNumber == 7)
            {
                ms.setIsFreezed(true);
            }
            else
            {
                ms.loseHP(hp);
            }
            player.loseMP(mp);
            skills[skillNumber].setCooldown();
            return true;
        }
        return false;
    }

    public bool basicAttack()
    {
        if (skills[0].getCdLeft() == 0 && manaCheck(skills[0]))
        {
            hp = basicAtkDmg;
            mp = skills[0].getMana();
            skillNumber = 0;
            return true;
        }
        return false;
    }

    public bool strongAttack()
    {
        if (skills[1].getCdLeft() == 0 && manaCheck(skills[1]))
        {
            hp = strAtkDmg;
            mp = skills[1].getMana();
            skillNumber = 1;
            return true;
        }
        return false;
    }

    public bool areaAttack()
    {
        if (skills[2].getCdLeft() == 0 && manaCheck(skills[2]))
        {
            opponent.loseHP(areaAtkDmg);
            monsterGO = GameObject.FindGameObjectWithTag(monsterOpponent);
            if (monsterGO != null)
            {
                ms = monsterGO.GetComponent<MonsterStats>();
                ms.loseHP(areaAtkDmg);
            }
            player.loseMP(skills[2].getMana());
            skills[2].setCooldown();
            return true;
        }
        return false;
    }

    public bool heal()
    {
        if (skills[3].getCdLeft() == 0 && manaCheck(skills[3]))
        {
            player.restoreHP(healValue);
            player.loseMP(skills[3].getMana());
            skills[3].setCooldown();
            return true;
        }
        return false;
    }

    public bool mana()
    {
        if (skills[4].getCdLeft() == 0 && manaCheck(skills[4]))
        {
            player.restoreMP(manaValue);
            skills[4].setCooldown();
            return true;
        }
        return false;
    }

    public bool summon()
    {
        monsterGO = GameObject.FindGameObjectWithTag(monsterPlayer);
        if (skills[5].getCdLeft() == 0 && manaCheck(skills[5]) && monsterGO == null)
        {
            Instantiate(monsterPrefab);
            player.loseMP(skills[5].getMana());
            skills[5].setCooldown();
            return true;
        }
        return false;
    }

    public bool strengthenSummon()
    {
        monsterGO = GameObject.FindGameObjectWithTag(monsterPlayer);
        if (skills[6].getCdLeft() == 0 && manaCheck(skills[6]) && monsterGO != null)
        {
            ms = monsterGO.GetComponent<MonsterStats>();
            ms.setStats(strSummonAtk, strSummonHP);
            player.loseMP(skills[6].getMana());
            skills[6].setCooldown();
            return true;
        }
        return false;
    }

    public bool freeze()
    {
        if (skills[7].getCdLeft() == 0 && manaCheck(skills[7]))
        {
            mp = skills[7].getMana();
            skillNumber = 7;
            return true;
        }
        return false;
    }

    public bool manaCheck(Skill sk)
    {
        if(player.getCurrentMP() >= sk.getMana())
        {
            return true;
        }
        return false;
    }

    public float getCdLeft(int value)
    {
        return skills[value].getCdLeft();
    }

    public void countdown()
    {
        for(int i = 0; i < skills.Length; i++)
        {
            skills[i].countdown();
        }
    }

    public int getSkillsLength()
    {
        return skills.Length;
    }

    public void defaultValues()
    {
        for(int i = 0; i < skills.Length; i++)
        {
            skills[i].setCdLeft(0);
        }
        hp = 0;
        mp = 0;
        skillNumber = -1;
    }

    private void Awake()
    {
        opponent = opponentGO.GetComponent<PlayerStats>();
        player = gameObject.GetComponent<PlayerStats>();

        skills = new Skill[8];

        skills[0] = new Skill(0, 0);
        skills[1] = new Skill(2, 10);
        skills[2] = new Skill(1, 5);
        skills[3] = new Skill(4, 15);
        skills[4] = new Skill(8, 0);
        skills[5] = new Skill(5, 10);
        skills[6] = new Skill(2, 5);
        skills[7] = new Skill(3, 10);
    }
}
