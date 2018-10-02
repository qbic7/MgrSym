using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action {

    private string skill, target;
    private float summaryReward;
    private bool isUsed;

    public Action(string sk, string tr)
    {
        skill = sk;
        target = tr;
        summaryReward = 0;
        isUsed = true;
    }

    public string getSkill()
    {
        return skill;
    }

    public void setSkill(string value)
    {
        skill = value;
    }

    public string getTarget()
    {
        return target;
    }

    public void setTarget(string value)
    {
        target = value;
    }

    public float getSummaryReward()
    {
        return summaryReward;
    }

    public void setSummaryReward(float value)
    {
        summaryReward += value;
    }

    public bool getIsUsed()
    {
        return isUsed;
    }

    public void setIsUsed(bool value)
    {
        isUsed = value;
    }
}
