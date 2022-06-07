using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ResponsiveScaling : MonoBehaviour
{
    [SerializeField] private float _xPercentRelativeToCenter = 0;
    [SerializeField] private Vector3 _scale = Vector3.one;
    [SerializeField] private Vector2 _referenceScreenScale = new Vector2(1242, 2688);
    [SerializeField] private bool _placeX = true;
    [SerializeField] private bool _scaleX = true;
    [SerializeField] private bool _scaleY = true;
    [SerializeField] private bool _scaleZ = true;


    void Update()
    {
        Scale();
        if(_placeX)
            Place();
    }

    private void Place()
    {
        var middle = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, 0.5f, transform.position.z - Camera.main.transform.position.z)).x;
        var side = Camera.main.ScreenToWorldPoint(new Vector3(0, 0.5f, transform.position.z - Camera.main.transform.position.z)).x;
        var pos = transform.position;
        pos.x = _xPercentRelativeToCenter * Mathf.Abs(middle - side);
        transform.position = pos;
    }

    private void Scale()
    {
        var res = _scale * Screen.width / _referenceScreenScale.x;
        if (!_scaleX)
            res.x = _scale.x;
        if (!_scaleY)
            res.y = _scale.y;
        if (!_scaleZ)
            res.z = _scale.z;
        transform.localScale = res;
    }

}
