using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickableArea : MonoBehaviour
{
    [SerializeField]
    private float alphaThreshhold;

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Image>().alphaHitTestMinimumThreshold = alphaThreshhold;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
