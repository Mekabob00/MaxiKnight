using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShopHPUI : MonoBehaviour
{
    public Slider _HPSlider;
    public Gradient _Gradient;
    public Image _Fill;
    public float value;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.GetComponent<Slider>().value = (float)DataManager.Instance._CastleHP / (float)10;
        _Fill.color = _Gradient.Evaluate(_HPSlider.normalizedValue);
    }
}
