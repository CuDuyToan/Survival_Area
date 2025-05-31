using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndexBar : MonoBehaviour
{
    public Image ImgHealthBar;

    public Text TxtHealth;

    private float min;

    public float Min
    {
        set
        {
            min = value;
        }
        get
        {
            return min;
        }
    }

    private float max;
    public float Max
    {
        set
        {
            max = value;
        }
        get
        {
            return max;
        }
    }

    private float currentValue;
    private float CurrentValue
    {
        set 
        {
            currentValue = value;
        }
        get 
        {
            return currentValue;
        }
    }

    private float currentPercent;

    private float CurrentPercent
    {
        set
        {
            currentPercent = value;
        }
        get 
        { 
            return currentPercent;
        }
    }

    

    public void SetValue(float value)
    {
        if (true)
        {
            if (Max - Min == 0)
            {
                CurrentValue = 0;
                CurrentPercent = 0;
            }
            else
            {
                CurrentValue = value;
                CurrentPercent = (float)CurrentValue / (float)(Max - Min);
            }

            TxtHealth.text = string.Format("{0} %", (int)(CurrentPercent * 100));

            ImgHealthBar.fillAmount = CurrentPercent;
        }
    }

    // Use this for initialization
    void Start()
    {

    }

}
