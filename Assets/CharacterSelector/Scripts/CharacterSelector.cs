using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kosmos.Utility;

namespace CharacterSelector
{
    public class CharacterSelector : MonoBehaviour
    {
        private Selection[] selectionArr;
        private float angle = 0;
        private float rotationTime = 1;
        private Quaternion refRotation;
        private int selectedIndex = 0;

        private void Start()
        {
            selectionArr = GetComponentsInChildren<Selection>();

            if (selectionArr.Length > 1)
            {
                Vector3 dir = selectionArr[0].transform.position - transform.position;
                refRotation = selectionArr[0].transform.rotation;
                angle = 360.0f / selectionArr.Length;
                
                for(int i = 1; i < selectionArr.Length; i++)
                {
                    selectionArr[i].transform.position = Quaternion.Euler(0, angle, 0) * dir;
                    dir = selectionArr[i].transform.position - transform.position;
                }
            }
        }

        public Selection GetSelection()
        {
            return selectionArr[selectedIndex];
        }

        public void Next()
        {
            selectedIndex++;

            if (selectedIndex >= selectionArr.Length)
                selectedIndex = 0;

            RotateCharacterSelector(angle);
        }

        public void Prev()
        {
            selectedIndex--;

            if (selectedIndex < 0)
                selectedIndex = selectionArr.Length - 1;

            RotateCharacterSelector(-angle);
        }

        private void RotateCharacterSelector(float angle)
        {
            Quaternion sRot = transform.rotation;
            Quaternion fRot = transform.rotation * Quaternion.Euler(0, -angle, 0);

            Transform selected = selectionArr[selectedIndex].transform;

            Quaternion sRotSelected = selected.rotation;
            Quaternion fRotSelected = refRotation;

            TimerController.Singleton.AddTimer(new TimerController.CustomTimer(rotationTime,
            delegate
            {
                transform.rotation = fRot;
                selected.rotation = fRotSelected;
            },
            delegate (float time)
            {
                float t = Easings.Interpolate(time / rotationTime, Easings.Functions.CubicEaseOut);

                transform.rotation = Quaternion.Lerp(sRot, fRot, t);
                selected.rotation = Quaternion.Lerp(sRotSelected, fRotSelected, t);
            }));
        }
    }
}

