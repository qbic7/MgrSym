using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill {

    private float cooldown, mana, cdLeft;

    public Skill(float cd, float m)
    {
        cooldown = cd + 1;
        mana = m;
        cdLeft = 0;
    }

    public float getCooldown()
    {
        return cooldown;
    }

    public float getMana()
    {
        return mana;
    }

    public float getCdLeft()
    {
        return cdLeft;
    }

    public void setCdLeft(float cdl)
    {
        cdLeft = cdl;
    }

    public void countdown()
    {
        if (cdLeft > 0)
        {
            cdLeft--;
        }
    }

    public void setCooldown()
    {
        cdLeft = cooldown;
    }
}
