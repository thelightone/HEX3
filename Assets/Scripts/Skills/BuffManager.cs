using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public List<KeyValuePair<SkillParent, int>> activeBuffs;

    public static BuffManager Instance;

    private void Start()
    {
        Instance = this;
    }

    public void NewRound()
    {
        foreach (var buff in activeBuffs)
        {
            activeBuffs.Remove(buff);
            if (buff.Value > 1)
            {
                AddBuff(buff.Key, buff.Value - 1);

                if (buff.Key.durationType == SkillParent.DurationType.everyTurn)
                {
                    buff.Key.OnActivate();
                }
            }
            else
            {
                buff.Key.OnDeactivate();
            }
        }
    }

    public void AddBuff(SkillParent buff, int duration)
    {
        activeBuffs.Add(new KeyValuePair<SkillParent, int>(buff, duration));
    }

    public void RemoveBuff(SkillParent buff)
    {

    }
}
