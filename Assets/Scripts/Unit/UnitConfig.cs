using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUnitConfig", menuName = "Data/UnitConfig", order = 51)]
public class UnitConfig : ScriptableObject
{
    public int health;
    public int damageMelee;
    public int damageRange;
    public int armor;
    public int goRange;

    public float missAbil;
    public float critChance;
    public float critModif;
    public int vamp;
    public bool melee;

    public int cost;

    public SkillParent skill;

    public Race race;
    public GameObject model;

    public enum Race
    {
        race1,
        race2
    }
}
