using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavBtn : MonoBehaviour
{
    public Animator settingAnimataor;

    private bool slideIn = false;

    public void SettingBtnMethod()
    {
        if (slideIn == false)
        {
            slideIn = true;
            settingAnimataor.SetBool("SlideIn", slideIn);
        }
        else if (slideIn == true)
        {
            slideIn = false;
            settingAnimataor.SetBool("SlideIn", slideIn);
        }
    }
}
