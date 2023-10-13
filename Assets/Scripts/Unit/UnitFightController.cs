using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using static UnitConfig;
using DissolveExample;
using UnityEngine.VFX;

public class UnitFightController : MonoBehaviour
{
    public UnitMoveController moveController;
    public UnitConfig unitConfig;
    public UnitFightController enemy;
    public DissolveChilds effectDeath;
    public ParticleSystem sparks;

    public Arrow arrow;
    public SwordEffect swordEffect;

    public delegate void HealthChangedDelegate(float newHealth);
    public event HealthChangedDelegate HealthChanged;

    public float maxHealth;
    private float _health;
    public float health
    {
        get { return _health; }
        set
        {
            if (value < 0)
                _health = 0;
            else if (value >= maxHealth)
                _health = maxHealth;
            else
                _health = value;
        }
    }
    public float damageMelee;
    public float damageRange;
    public float armor;
    public float goRange;

    public float missAbil;
    public float critChance;
    public float critModif;
    public float vamp;
    public bool melee;
    private float _hitDamage;
    public float hitDamage
    {
        get { return _hitDamage; }
        set
        {
            if (value < 0)
                _hitDamage = 0;
            else
                _hitDamage = value;
        }
    }
    public int iterations;
    public int limIter;
    public bool dodged;

    public int cost;

    public SkillParent skill;

    public GameObject model;

    public Race race;

    public HexTile shootTarget;


    private void Awake()
    {
        moveController = GetComponent<UnitMoveController>();

        maxHealth = unitConfig.health;
        health = maxHealth;
        damageMelee = unitConfig.damageMelee;
        damageRange = unitConfig.damageRange;
        armor = unitConfig.armor;
        goRange = unitConfig.goRange;
        vamp = unitConfig.vamp;
        melee = unitConfig.melee;
        cost = unitConfig.cost;
        skill = unitConfig.skill;
        race = unitConfig.race;
        missAbil = unitConfig.missAbil;
        critChance = unitConfig.critChance;
        critModif = unitConfig.critModif;

        dodged = false;

        model = Instantiate(unitConfig.model, transform.position - new Vector3(0, 0.95f, 0), transform.rotation, gameObject.transform);
        model.transform.SetAsFirstSibling();

        effectDeath = GetComponentInChildren<DissolveChilds>();
        sparks = GetComponentInChildren<ParticleSystem>();

        arrow = GetComponentInChildren<Arrow>();
        swordEffect = GetComponentInChildren<SwordEffect>();
    }

    public void AttackMove(HexTile destTile, HexTile target)
    {
        InitializeEnemy(target, true);
        moveController.Move(destTile, target);
    }

    public void InitializeEnemy(HexTile target, bool melee)
    {
        enemy = target.unitOn.fightController;
        enemy.enemy = this;
        dodged = enemy.dodged = false;
        enemy.CalculateDamage(CheckCritDamage(melee));
    }

    public void StartFight(HexTile target)
    {
        float option = (float)System.Math.Round(Random.value);
        moveController.animator.SetFloat("AttackBlend", option);
        moveController.animator.SetTrigger("Attack");

        enemy.moveController.TurnToEnemy(moveController.currentTile, false, false);

        limIter = Random.Range(1, 3);
        enemy.limIter = limIter + 10;
        iterations = 0;
    }

    public void PreStartShoot(HexTile target)
    {
        InitializeEnemy(target, false);
        shootTarget = target;
        moveController.TurnToEnemy(target, false, true);
    }

    public void StartShoot()
    {
        moveController.animator.SetTrigger("Shoot");
    }

    public void Shoot()
    {
        arrow.FlyToEnemy(shootTarget);
        moveController.FinishMove();
        shootTarget = null;
    }

    public void Hit()
    {
        swordEffect.PlayEffects();

        if (iterations > limIter && !enemy.dodged)
        {
            enemy.ReceiveDamage(true);
        }
    }

    public void PreHit()
    {
        iterations++;
        if (iterations <= limIter)
        {
            float option = (float)System.Math.Round(Random.value);
            enemy.moveController.animator.SetFloat("DefBlend", option);
            enemy.moveController.animator.SetTrigger("Block");

            Debug.Log(iterations);
        }
    }

    public void PostBlock()
    {
        if (dodged)
        {
            moveController.animator.SetTrigger("Stop");
            return;
        }
        float option = (float)System.Math.Round(Random.value);
        moveController.animator.SetFloat("AttackBlend", option);
        moveController.animator.SetTrigger("Attack");
    }

    public void CalculateDamage(float calcHitDamage)
    {
        if (Dodge())
        {
            dodged = true;
            return;
        }
        var dam = Random.Range(0.8f * calcHitDamage, 1.2f * calcHitDamage);
        hitDamage = dam - (dam * (armor / 100));
    }

    public void ReceiveDamage(bool isNeedHitDamage)
    {
        if (isNeedHitDamage)
        {
            var prevHealth = health;
            health -= hitDamage;
            enemy.Vampirism(prevHealth - health);
        }

        HealthChanged?.Invoke(health);

        if (health <= 0)
        {
            StartCoroutine("Death");
        }
        else
        {
            moveController.animator.SetTrigger("Damaged");
        }
    }

    public IEnumerator Death()
    {
        moveController.animator.SetTrigger("Death");
        moveController.currentTile.MakeFree();
        yield return new WaitForSeconds(1.5f);
        effectDeath.ResetAndStart();
        yield return new WaitForSeconds(6);
        gameObject.SetActive(false);
        moveController.player.units.Remove(moveController);
    }

    public bool Dodge()
    {
        return Random.Range(0, 100) < missAbil ? true : false;
    }

    public float CheckCritDamage(bool melee)
    {
        var damage = melee ? damageMelee : damageRange;
        return Random.Range(0, 100) < critChance ? damage * critModif / 100 : damage;
    }

    public void Vampirism(float calcHitDamage)
    {
        health += calcHitDamage * vamp / 100;
        HealthChanged?.Invoke(health);
    }

    public void ActivateSkill()
    {
        skill.Init(this);
    }

}

