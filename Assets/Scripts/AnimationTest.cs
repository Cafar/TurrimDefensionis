using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AnimationTest : MonoBehaviour
{
    [Header("Configuration")]
    public TowerData data;
    public Animator animator;

    public SpriteRenderer sp;

    [Space(10)]

    [Header("Testing")]
    public bool isActive = false;
    public bool isDestroyed = false;

    [Header("SOUNDS")]
    private AudioSource audioSource;
    //[SerializeField]
    //private AudioSource failComplex;
    //[SerializeField]
    //private AudioSource switchSound;


    public void SetTowerData(TowerData newData)
    {
        data = newData;
        animator.runtimeAnimatorController = data.animator;
        if (data.mapImage != null)
        {
            sp.sprite = data.mapImage;
            sp.transform.localScale = Vector3.one * data.imageScaling;
        }
        // towerHealthbar.value = data.resistance;
    }

    private void Start()
    {
        SetTowerData(data);
        isActive = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isActive = !isActive;
            animator.SetBool("isActive", isActive);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            isDestroyed = !isDestroyed;
            animator.SetBool("isDestroyed", isDestroyed);
        }
        
    }

}
