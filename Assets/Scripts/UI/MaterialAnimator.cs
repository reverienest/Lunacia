using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialAnimator : MonoBehaviour
{
    public AnimationCurve valueSpectrum = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));

    public Material material;

    public string propertyName;
    //private Color baseColor;

    private float timeSince;
    private float endTime;
    private bool isAnimating;
    private bool isAnimatingReverse;

    void Start()
    {
        if (material == null)
        {
            if (GetComponent<Renderer>() == null)
                Debug.LogError("Material and Renderer are not set on object");
            else
                material = GetComponent<Renderer>().material;
        }
        endTime = valueSpectrum.keys[1].time;
        //baseColor = GetComponent<Renderer>().material.GetColor("_TintColor");
    }

    void Update()
    {
        if (isAnimating)
        {
            timeSince += Time.deltaTime;
            material.SetFloat(propertyName, valueSpectrum.Evaluate(timeSince));
            //baseColor.a = valueSpectrum.Evaluate(timeSince);
            //GetComponent<Renderer>().material.SetColor("_TintColor", baseColor);

            if (timeSince >= endTime)
                isAnimating = false;
        }
        if (isAnimatingReverse)
        {
            timeSince -= Time.deltaTime;
            material.SetFloat(propertyName, valueSpectrum.Evaluate(timeSince));

            if (timeSince <= 0)
                isAnimatingReverse = false;
        }
    }

    public void StartAnimatingMaterial()
    {
        isAnimating = true;
        timeSince = 0;
        material.SetFloat(propertyName, valueSpectrum.Evaluate(timeSince));
    }

    public void StopAnimatingMaterial()
    {
        isAnimating = false;
        timeSince = endTime;
        material.SetFloat(propertyName, valueSpectrum.Evaluate(timeSince));
    }

    public void StartAnimatingMaterialReverse()
    {
        isAnimatingReverse = true;
        timeSince = endTime;
        material.SetFloat(propertyName, valueSpectrum.Evaluate(timeSince));
    }

    public void StopAnimatingMaterialReverse()
    {
        isAnimatingReverse = false;
        timeSince = 0;
        material.SetFloat(propertyName, valueSpectrum.Evaluate(timeSince));
    }
}
