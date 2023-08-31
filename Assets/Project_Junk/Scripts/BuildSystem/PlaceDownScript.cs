using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlaceDownScript : MonoBehaviour
{
    [SerializeField]
    private float dropHeight, dropTime, shakeStrength, shakeDuration;

    [SerializeField]
    private GameObject LOD0;

    [SerializeField]
    private Material dissolveMat;
    private Renderer objRenderer;

    [SerializeField]
    private Material defaultMaterial;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    public float buildTime = 1;

    [SerializeField]
    private List<Material> defaultMaterials = new List<Material>();

    [SerializeField]
    private AudioClip dropBoom, assembleSound;
    private AudioSource audioSource;

    private void Awake()
    {
        objRenderer = LOD0.GetComponent<Renderer>();
    }
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = assembleSound;
        audioSource.Play();
        objRenderer = LOD0.GetComponent<Renderer>();
        //dissolve(buildTime);





    }
    public void dropDown(GameObject obj)
    {
        float y = transform.position.y;

        audioSource.clip = dropBoom;

        obj.transform.DOMoveY(y - dropHeight, dropTime, false).OnComplete(() =>
        {
            GetComponent<BuildingScript>().playPlacementVFX();
            objRenderer.SetMaterials(defaultMaterials);
            StartCoroutine(changeColor());
            GameManager.Instance.Utils.camShake(shakeDuration, shakeStrength);
            GetComponent<BuildingScript>().placedDown = true;
            audioSource.clip = dropBoom;
            audioSource.Play();

        });

    }

    public void dissolve(float t)
    {

        List<Material> tmpList = new List<Material>();


        for (int i = 0; i < defaultMaterials.Count; i++)
        {
            tmpList.Add(dissolveMat);
        }

        objRenderer.SetMaterials(tmpList);


        StartCoroutine(dissolveAction(t));
    }

    IEnumerator dissolveAction(float speed)
    {
        float currentT = 0;
        dissolveMat.SetFloat("_DissolveAmount", 1);
        while (currentT <= 1)
        {
            currentT += Time.deltaTime * speed;
            dissolveMat.SetFloat("_DissolveAmount", 1 - currentT);

            yield return null;
        }

        dropDown(transform.gameObject);


        yield return null;
    }

    IEnumerator changeColor()
    {
        float currentT = 0;

        while (currentT <= 2)
        {
            LOD0.GetComponent<Renderer>().material.color = Color.Lerp(dissolveMat.GetColor("_BaseColor"), Color.white, currentT);
            currentT += Time.deltaTime;
            yield return null;
        }
    }


}


