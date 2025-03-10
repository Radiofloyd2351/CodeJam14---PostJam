using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarObject
{
    [SerializeField] private GameObject _bar;
    [SerializeField] private GameObject _container;
    private float _value;
    private float _maxValue;
    private int _id;
    private float _regenCooldown;
    private float _regenAmount;
    private float _barSize;
    private Entity _ctx;

    public BarObject(Entity ctx, int id, Color32 color, GameObject bar, GameObject container, float maxValue = 15f, float regen = 0.1f, float regenCooldown = 2f) {
        _bar = bar;
        _container = container;
        _maxValue = maxValue;
        _value = maxValue;
        _id = id;
        _barSize = _bar.transform.localScale.x;
        _regenAmount = regen;
        _regenCooldown = regenCooldown;
        _ctx = ctx;
        _bar.GetComponent<SpriteRenderer>().color = color;

    }

    public bool PayValue(float cost) {
        if (_value < cost) {
            return false;
        }
        _container.SetActive(true);
        _value-= cost;
        _bar.transform.localPosition = new Vector3(((_value/ _maxValue) - 1) * _barSize / 2f, 0f, 0f);
        _bar.transform.localScale = new Vector3(_barSize * _value/ _maxValue, _bar.transform.localScale.y, 0f);
        CoroutineManager.instance.RunCoroutine(ReloadValue(), _id + _ctx.id);
        return true;
    }
    public IEnumerator ReloadValue() {
        yield return new WaitForSeconds(_regenCooldown);
        while (_value< _maxValue) {
            yield return new WaitForSeconds(0.01f);
            _value+= _regenAmount;
            if (_value> _maxValue) {
                _value= _maxValue;
            }
            _bar.transform.localPosition = new Vector3(((_value/ _maxValue) - 1) * _barSize / 2f, 0f, 0f);
            _bar.transform.localScale = new Vector3(_barSize * _value/ _maxValue, _bar.transform.localScale.y, 0f);
        }
        yield return new WaitForSeconds(1f);
        _container.SetActive(false);
    }


    public float GetValue() {
        return _value;
    }

}
