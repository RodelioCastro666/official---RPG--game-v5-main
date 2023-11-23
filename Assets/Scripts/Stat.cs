using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stat : MonoBehaviour
{
    private Image content;

    [SerializeField]
    private float lerpSpeed;

    [SerializeField]
    private Text statValue;

    private float currentFill;

    private float overFlow;

    public float MyOverFlow 
    { 
        get
        {
            float tmp = overFlow;
            overFlow = 0;
            return tmp;
        }
    }

    public float MyMaxValue { get; set; }

    

    public float MyCurrentValue
    {

        get
        {
            return currentValue;   
        }
        set
        {
            if (value > MyMaxValue)
            {
                overFlow = value - MyMaxValue;

                currentValue = MyMaxValue;
            }
            else if (value < 0)
            {
                currentValue = 0;
            }
            else
            {
                currentValue = value;
            }

            currentFill = currentValue / MyMaxValue;

            if (statValue != null)
            {
                statValue.text = currentValue + " / " + MyMaxValue;
            }

           

        }

      
    }

    private float currentValue;
   
    public bool IsFUll 
    {
        get { return content.fillAmount == 1; }
    }

    

    void Start()
    {
      
        content = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {

        if (currentFill != content.fillAmount)
        {
            content.fillAmount = Mathf.MoveTowards(content.fillAmount, currentFill, Time.deltaTime * lerpSpeed);
        }
       
    }

    public void Initialize(float currentValue, float maxValue)
    {
        if (content == null)
        {
            content = GetComponent<Image>();
        }

        MyMaxValue = maxValue;
        MyCurrentValue = currentValue;

        content.fillAmount = MyCurrentValue / MyMaxValue;
    }

    public void Reset()
    {
        content.fillAmount = 0;
    }
}
