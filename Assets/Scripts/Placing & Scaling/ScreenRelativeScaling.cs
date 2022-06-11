using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// this script is meant for more precise and specific adjustments than ResponsiveScaling
// if ResponsiveScaling can be used to have things scale relatively to the screen's aspect ratio,
// this script can help with keeping some part of real world scaling (to keep real world placing
// & proportions in mind)
[ExecuteInEditMode]
public class ScreenRelativeScaling : MonoBehaviour
{
    [SerializeField] private float _leftMostPosition;
    [SerializeField] private float _rightMostPosition;
    [SerializeField] private bool _keepLeft = false;
    [SerializeField] private bool _keepRight = false;
    [SerializeField] private bool _placeX = true;
    [SerializeField] private bool _scaleX = true;
    [SerializeField] private bool _square = false;

    private Vector3 originalPosition;
    private Vector3 originalLossyScale;
    
    void Awake()
    {
        originalPosition = transform.position;
        originalLossyScale = transform.lossyScale;
    }


    void Update()
    {
        Place();
    }

    private void Place()
    {
        var left = Camera.main.ScreenToWorldPoint(new Vector3(_leftMostPosition * Screen.width, 0.5f, transform.position.z - Camera.main.transform.position.z)).x;
        var right = Camera.main.ScreenToWorldPoint(new Vector3(_rightMostPosition * Screen.width, 0.5f, transform.position.z - Camera.main.transform.position.z)).x;

        if (_keepLeft)
            left = originalPosition.x - originalLossyScale.x / 2f;
        if (_keepRight)
            right = originalPosition.x + originalLossyScale.x / 2f;
        // could have avoided the first calculations in the first place, but would be harder to get what I'm going for


        var middle = Camera.main.ScreenToWorldPoint(new Vector3((_leftMostPosition+_rightMostPosition) /2f * Screen.width, 0.5f, transform.position.z - Camera.main.transform.position.z)).x;
        var size = new Vector3(Mathf.Abs(left - right), 0, 0);
        size.y = transform.localScale.y;
        size.z = transform.localScale.z;
        var pos = transform.position;
        pos.x = middle;
        if(_placeX)
            transform.position = pos;
        Vector3 lossyScale = Vector3.one;
        if (transform.parent != null)
            lossyScale = transform.parent.lossyScale;
        if (!_scaleX)
            size.x = transform.localScale.x;
        else if (!_keepLeft && !_keepRight) // want to avoid ending up with a deformation, as accurate positionning takes priority over accurate sizing
            size.x = size.x / lossyScale.x;
        if (_square)
        {
            size.y = size.z = size.x;
        }
        transform.localScale = size;
    }

}
