using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Arrow : MonoBehaviour
{
    [SerializeField, Range(0, 1)] private float _value;
    [SerializeField] private float _maxSize = 3;
    [SerializeField] private Transform _base;
    [SerializeField] private Transform _head;
    [SerializeField] private SpriteRenderer _spriteBase;
    [SerializeField] private SpriteRenderer _spriteHead;
    [SerializeField] private Color _color = Color.white;

    public float Value { get => _value; set => _value = value; }
    public Color Color { get => _color; set => _color = value; }

    void Update()
    {
        if(_value == 0)
        {
            _base.gameObject.SetActive(false);
            _head.gameObject.SetActive(false);
            return;
        }
        _base.gameObject.SetActive(true);
        _head.gameObject.SetActive(true);
        _spriteBase.color = _color;
        _spriteHead.color = _color;
        var dist = _value * _maxSize;
        var baseScale = _base.localScale;
        baseScale.x = dist;
        _base.localScale = baseScale;
        _head.localPosition = _base.localPosition + new Vector3(dist, 0);
    }
}
