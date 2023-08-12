using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public GameManager GM;
    public Transform camHolder;

    


    private currentModeType previousMode;

    public void showErrorMessage(string errorText)
    {

    }

    #region CameraOperations

    public void camShake(float strength, float duration)
    {
        camHolder.DOComplete();
        camHolder.DOShakePosition(duration, strength);
        camHolder.DOShakeRotation(duration, strength);
    }
    #endregion
    #region GameMode Operations
    public void changeGameMode(currentModeType type)
    {
        GM.currentMode = type;
    }

    
    public void switchGameModeBetween(currentModeType mode1, currentModeType mode2)
    {
        if(GM.currentMode == mode1)
        {
            GM.currentMode = mode2;
        }
        else if(GM.currentMode == mode2)
        {
            GM.currentMode = mode1;
        }        
    }

    

    #endregion


    #region General Operations
    public string enumToString<T>(T enumValue)
    {
        string enumString = enumValue.ToString();
        string formattedString = string.Empty;

        // Add spaces between words by checking for uppercase letters
        for (int i = 0; i < enumString.Length; i++)
        {
            if (i > 0 && char.IsUpper(enumString[i]))
            {
                formattedString += " ";
            }

            formattedString += enumString[i];
        }

        return formattedString;
    }

    #endregion


}
