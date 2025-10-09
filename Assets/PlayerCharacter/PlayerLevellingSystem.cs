using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevellingSystem
{
    public int level = 1;
    public int levelCap = 50;
    public float exp = 0;
    public float maxExp = 10;
    public LevelsDisplay displayRef;

    public float maxExpIncreaseMultiplier { get; private set; } = 0.25f;

    public void IncreaseExp(float amount)
    {
        if(level >= levelCap)
        {
            level = levelCap;
            return;
        }
        exp += amount;
        float expDifference = 0;
        if(exp > maxExp)
        {
            level += 1;
            expDifference = exp - maxExp;
            exp = 0;
            maxExp += Mathf.RoundToInt(maxExp * maxExpIncreaseMultiplier);
            IncreaseExp(expDifference);
        }
        displayRef.UpdateDisplay();
    }

}
