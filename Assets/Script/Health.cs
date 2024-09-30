using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Unity.VisualScripting;

public class Health : MonoBehaviour
{
    public float _maxHealth = 100;
    private float _timer;


    [SerializeField] private float _fillSpeed;
    [Header("Health Bar Info")]
    [SerializeField]public float _currentHealth;
    [SerializeField] private Image _healthBarfill;
    
    [Header("Text Health  Info")]
    // [SerializeField] private TextMeshProUGUI _HealthText;
    [SerializeField] private Gradient _HealthcolorGradient;

    
    

    void Start()
    {
        _currentHealth = _maxHealth;
    }

    public void FixedUpdate()
    {
        
        _timer += Time.fixedDeltaTime;
        UpdateHealthBar();
    }

    public void UpdateHealth(float amount)
    {
        _currentHealth += amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        float targetFillAmount = _currentHealth / _maxHealth;
        _healthBarfill.DOFillAmount(targetFillAmount, _fillSpeed);
        _healthBarfill.DOColor(_HealthcolorGradient.Evaluate(targetFillAmount),_fillSpeed);
    }
    
}