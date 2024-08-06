using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PowerupUI : MonoBehaviour
{
    public Transform powerupIcon;
    public Transform powerupPivot;
    public Transform powerupLocationTransform;
    public string powerupName;

    public void Rotate(Vector3 rotateTo, bool takeNearestPath, bool clockwise)
    {
        float angle = Vector3.SignedAngle((powerupLocationTransform.position - transform.position), (rotateTo - transform.position), Vector3.forward);

        if (angle == 0)
        {
            return;
        }

        if (!takeNearestPath)
        {
            if (angle < 0 && clockwise)
            {
                angle = angle + 360;
            }
            else if (angle > 0 && !clockwise)
            {
                angle = angle - 360;
            }
        }

        // Debug.Log(angle);
        // Debug.Log(clockwise);
        powerupPivot.DORotate(new Vector3(0, 0, angle), 1f, RotateMode.LocalAxisAdd).OnUpdate(delegate { powerupIcon.position = powerupLocationTransform.position; });
    }

    public void ScaleWhenSelected(bool isSelected)
    {
        powerupIcon.DOScale((isSelected) ? (Vector3.one * 1.2f) : (Vector3.one / 1.2f), 0.3f);
    }
}
