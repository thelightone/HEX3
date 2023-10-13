using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillButton : MonoBehaviour
{
    public void InvokeSkill()
    {
        TileManager.Instance.activeUnit.fightController.ActivateSkill();
    }
}
