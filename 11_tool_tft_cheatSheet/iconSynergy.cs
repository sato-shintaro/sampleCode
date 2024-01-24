using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class iconSynergy : iconBase
{
    [SerializeField] protected GameObject mTextPre;

    protected GameObject mHelpText;
    protected GameObject mActivationHelpText;


    public void MakeSynergyHelp( string helpText )
    {
        mHelpText = Instantiate(mTextPre, Vector2.zero, Quaternion.identity, this.transform);
        mHelpText.name = "help_" + mNameJP;

        float _posX = this.GetComponent<RectTransform>().sizeDelta.x + 3.0f;
        float _posY = this.GetComponent<RectTransform>().anchoredPosition.y;
        mHelpText.GetComponent<RectTransform>().anchoredPosition = new Vector2(_posX, _posY);

        mHelpText.GetComponent<Text>().text = "<color=#FFC628>" + mNameJP + "｜</color>" + "<color=#BFBFBF>" + helpText + "</color>";
    }


    public void MakeSynergyConditionsHelp( string helpText )
    {
        mActivationHelpText = Instantiate(mTextPre, Vector2.zero, Quaternion.identity, this.transform);
        mActivationHelpText.name = "activeHelp_" + mNameJP;

        float _posX = this.GetComponent<RectTransform>().sizeDelta.x;
        float _posY = (this.GetComponent<RectTransform>().sizeDelta.y + 1.0f ) * -1;
        mActivationHelpText.GetComponent<RectTransform>().anchoredPosition = new Vector2(_posX, _posY);

        mActivationHelpText.GetComponent<Text>().text = "<color=#00CFBD>" + helpText + "</color>";
    }

}
