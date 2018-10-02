using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour {

    [SerializeField] private Slider sliderHP, sliderMP;
    [SerializeField] private Text textHP, textMP;
    [SerializeField] private float maxHP = 100, maxMP = 100;
    private bool isDead = false, isFreezed = false;

    public void textBarRefresh(Slider sl, Text txt, float value)
    {
        txt.text = sl.value + "/" + value;
    }

    public void restoreHP(float hp)
    {
        sliderHP.value = Mathf.MoveTowards(sliderHP.value, sliderHP.value + hp, maxHP);
        textBarRefresh(sliderHP, textHP, maxHP);
    }

    public void restoreMP(float mp)
    {
        sliderMP.value = Mathf.MoveTowards(sliderMP.value, sliderMP.value + mp, maxMP);
        textBarRefresh(sliderMP, textMP, maxMP);
    }

    public void loseHP(float hp)
    {
        sliderHP.value = Mathf.MoveTowards(sliderHP.value, sliderHP.value - hp, maxHP);
        textBarRefresh(sliderHP, textHP, maxHP);

        if (sliderHP.value <= 0 && isDead == false)
        {
            isDead = true;
        }
    }

    public void loseMP(float mp)
    {
        sliderMP.value = Mathf.MoveTowards(sliderMP.value, sliderMP.value - mp, maxMP);
        textBarRefresh(sliderMP, textMP, maxMP);
    }

    public float getCurrentHP()
    {
        return sliderHP.value;
    }

    public float getMaxHP()
    {
        return maxHP;
    }

    public float getCurrentMP()
    {
        return sliderMP.value;
    }

    public float getMaxMP()
    {
        return maxMP;
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

    public void defaultValues()
    {
        isDead = false;
        isFreezed = false;
        sliderHP.value = Mathf.MoveTowards(sliderHP.value, maxHP, maxHP);
        textBarRefresh(sliderHP, textHP, maxHP);
        sliderMP.value = Mathf.MoveTowards(sliderMP.value, maxMP, maxMP);
        textBarRefresh(sliderMP, textMP, maxMP);
    }

    private void Awake()
    {
        textBarRefresh(sliderHP, textHP, maxHP);
        textBarRefresh(sliderMP, textMP, maxMP);
    }
}
