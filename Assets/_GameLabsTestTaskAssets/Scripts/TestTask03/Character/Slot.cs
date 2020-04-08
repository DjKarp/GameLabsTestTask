using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{

    //Точка установки предмета в слоте
    protected Transform positionPoint;

    [Header("Нормальный установленный объект в слоте")]
    [SerializeField]
    protected GameObject installedGOSlot;
    protected MeshRenderer[] m_MeshRenderers;

    [Header("Прозрачный бокс")]
    [SerializeField]
    public GameObject transparentBoxPlace;

    protected MeshRenderer m_MeshRenderer;

    protected Material blueMaterial;
    protected Material yellowMaterial;
    protected Material purpleMaterial;
          

    protected RarityModul m_RarityModul;
    public enum RarityModul
    {

        standart,
        yellow,
        purple

    }

    protected Pawn m_Pawn;


    protected virtual void Awake()
    {

        m_Pawn = gameObject.GetComponentInParent<Pawn>();

        blueMaterial = Resources.Load("Materials/StarSparrowBlue") as Material;        
        yellowMaterial = Resources.Load("Materials/StarSparrowYellow") as Material;
        purpleMaterial = Resources.Load("Materials/StarSparrowPurple") as Material;

        m_MeshRenderer = installedGOSlot.GetComponent<MeshRenderer>();
        positionPoint = installedGOSlot.transform;

        m_MeshRenderers = gameObject.GetComponentsInChildren<MeshRenderer>();

        installedGOSlot.SetActive(false);
        transparentBoxPlace.SetActive(false);
        
    }

    public virtual Vector3 GetPositionPoint()
    {

        return positionPoint.position;

    }

    public virtual Quaternion GetRotationSlot()
    {

        return positionPoint.rotation;

    }

    public virtual void ActivateSlot()
    {

        installedGOSlot.SetActive(true);
        transparentBoxPlace.SetActive(false);

    }

    public virtual void GreenPlaceWeapon(bool isOn)
    {



    }

    public virtual void GreenPlaceModule(bool isOn)
    {



    }

    public virtual void SetEnemy(Transform transform, Pawn pawn)
    {



    }

}
