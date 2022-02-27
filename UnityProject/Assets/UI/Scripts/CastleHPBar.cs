using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastleHPBar : MonoBehaviour
{
    public Slider _HPSlider;
    public Gradient _Gradient;
    public Image _Fill;

    private void Start()
    {
        _HPSlider.maxValue = DataManager.Instance._CastleMaxHP;
        _HPSlider.value = DataManager.Instance._CastleHP;
    }

    private void Update()
    {
        _Fill.color = _Gradient.Evaluate(_HPSlider.normalizedValue);
    }

    public void SetMaxHealth(int health_)
    {
        _HPSlider.maxValue = health_;
        _HPSlider.value = health_;

        _Fill.color = _Gradient.Evaluate(1f); //0~1 ”ÍˆÍ

    }

    public void SetCurrentHealth(int health_)
    {
        _HPSlider.value = health_;
    }

    public void Button_RecoveryHP()
    {
        _HPSlider.value = DataManager.Instance._CastleHP;
    }
}