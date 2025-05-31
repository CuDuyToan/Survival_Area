using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Purdah : MonoBehaviour
{
    private Image purdahImage;
    [SerializeField, Min(1)] private float defaultSpeed = 10;

    public static event Action<bool> inPurdah;
    public float DefaultSpeed
    {
        set
        {
            defaultSpeed = value;
        }
        get
        {
            if (defaultSpeed >= 100 * 0.75f) defaultSpeed = 100 * 0.75f;
            return defaultSpeed;
        }
    }

    private void Awake()
    {
        purdahImage = GetComponent<Image>();
    }

    private void Start()
    {
        Invoke(nameof(InvokeClosePurdah), 1);
    }

    private void InvokeClosePurdah()
    {
        OnClosePurdah(DefaultSpeed);
    }

    public void OnOpenPurdah(float amount)
    {
        StopAllCoroutines();
        StartCoroutine(OpenPurdah(amount, true));
    }

    private IEnumerator OpenPurdah(float amount, bool closeNow)
    {
        inPurdah?.Invoke(true);

        Color color = purdahImage.color;

        while (purdahImage.color.a < 1)
        {
            color.a += amount * 0.01f;

            purdahImage.color = color;

            yield return null;
        }

        if(closeNow)
        {
            StartCoroutine(ClosePurdah(amount));
        }
    }

    public void OnClosePurdah(float amount)
    {
        StopAllCoroutines();
        StartCoroutine(ClosePurdah(amount));
    }

    private IEnumerator ClosePurdah(float amount)
    {
        inPurdah?.Invoke(false);
        Color color = purdahImage.color;

        while (purdahImage.color.a > 0)
        {
            color.a -= amount * 0.01f;

            purdahImage.color = color;

            yield return null;
        }
    }
}
