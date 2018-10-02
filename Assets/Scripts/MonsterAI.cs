using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAI : MonoBehaviour {

    [SerializeField] private string playerStr, monsterStr;

    private GameObject monster, player;

    MonsterStats ms, msEnemy;
    PlayerStats ps;

    private int target;

    public void attack()
    {
        target = Random.Range(0, 2);
        if (target == 0)
        {
            ps.loseHP(ms.getAttackDamage());
        } else if (target == 1)
        {
            monster = GameObject.FindGameObjectWithTag(monsterStr);
            if(monster != null)
            {
                msEnemy = monster.GetComponent<MonsterStats>();
                msEnemy.loseHP(ms.getAttackDamage());
            }
            else
            {
                ps.loseHP(ms.getAttackDamage());
            }
        }
    }

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag(playerStr);
        ps = player.GetComponent<PlayerStats>();
        ms = gameObject.GetComponent<MonsterStats>();
	}
}
