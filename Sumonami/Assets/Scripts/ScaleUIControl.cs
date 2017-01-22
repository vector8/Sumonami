using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleUIControl : MonoBehaviour
{
    public GameObject dial;
    public void setWeight(float weightPercent)
    {
        Vector3 euler = dial.transform.rotation.eulerAngles;
        euler.z = Mathf.Lerp(88, -88, weightPercent);
        dial.transform.rotation = Quaternion.Euler(euler);
    }
}
