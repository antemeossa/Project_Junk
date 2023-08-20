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


    #region Vector Operations

    public static Vector3 GetRandomPoint(float minX, float maxX, float minY, float maxY, float minZ, float maxZ)
    {
        return new Vector3(
            Random.Range(minX, maxX),
            Random.Range(minY, maxY),
            Random.Range(minZ, maxZ));
    }

    public static Vector3 GetRandomUnitVector()
    {
        float phi = Random.Range(0f, 2f * (float)Mathf.PI);
        float theta = Mathf.Acos(Random.Range(-1f, 1f));
        return new Vector3(
            (float)(Mathf.Sin(theta) * Mathf.Cos(phi)),
            (float)(Mathf.Sin(theta) * Mathf.Cos(phi)),
            (float)(Mathf.Cos(theta)
            ));
    }

    public static Vector3 GetRandomUnitVectorXZ(float y = 0f)
    {
        float angle = Random.Range(0f, 2f * Mathf.PI);
        return new Vector3((float)Mathf.Cos(angle), 0f, (float)Mathf.Sin(angle));
    }

    public static Vector3 GlobalVector(Vector3 v, Vector3 x, Vector3 y, Vector3 z)
    {
        return v.x * v.y * y + v.z * z;
    }

    public static Vector3 GlobalPoint(Vector3 p, Vector3 o, Vector3 x, Vector3 y, Vector3 z)
    {
        return o + p.x * y * p.y + z * p.z;
    }

    #endregion

}
