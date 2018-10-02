using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterStats : MonoBehaviour {

    [SerializeField] private float attackDamage, maxHP;
    [SerializeField] private Slider sliderHP;
    private bool isDead = false, isFreezed = false;

    public void loseHP(float hp)
    {
        sliderHP.value = Mathf.MoveTowards(sliderHP.value, sliderHP.value - hp, maxHP);

        if(sliderHP.value <= 0 && !isDead)
        {
            isDead = true;
            Destroy(gameObject);
        }
    }

    public void setStats(float atkDmg, float hp)
    {
        attackDamage += atkDmg;
        maxHP += hp;
        sliderHP.maxValue += hp;
        sliderHP.value = Mathf.MoveTowards(sliderHP.value, maxHP, maxHP);
    }

    public float getHP()
    {
        return sliderHP.value;
    }

    public float getAttackDamage()
    {
        return attackDamage;
    }

    public bool getIsDead()
    {
        return isDead;
    }

    public bool getIsFreezed()
    {
        return isFreezed;
    }

    public void setIsFreezed(bool value)
    {
        isFreezed = value;
    }

    public void destroyMonster()
    {
        Destroy(gameObject);
    }
}
