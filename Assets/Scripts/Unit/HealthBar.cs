using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private UnitFightController _fightController;

    private Slider _slider;
    private TMP_Text _damage;

    void Start()
    {
        _slider = GetComponent<Slider>();
        _damage = GetComponentInChildren<TMP_Text>();

        _slider.maxValue = _fightController.maxHealth;
        _slider.value = _fightController.maxHealth;

        _fightController.HealthChanged += UpdateValue;
    }

    private void UpdateValue(float newHealth)
    {
        _slider.value = newHealth;
    }
}


