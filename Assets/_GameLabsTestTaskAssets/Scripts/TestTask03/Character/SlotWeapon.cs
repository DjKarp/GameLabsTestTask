using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotWeapon : Slot
{

    protected List<WeaponDesingSO> m_WeaponDesingSO = new List<WeaponDesingSO>();
    protected WeaponDesingSO currentWeapon;

    protected int weaponSOCount;

    public float currentReloarTime = 0.0f;
    public float timerReload = 0.0f;

    protected Transform enemyTransform;
    protected Pawn enemyPawn;

    public Transform shootPosition;

    protected GameObject shootPrefab;
    protected GameObject shootPrefabYellow;
    protected GameObject shootPrefabPurple;

    protected List<GameObject> shootPool = new List<GameObject>();
    protected List<Transform> shootPoolTR = new List<Transform>();
    protected List<Laser> shootPollLaser = new List<Laser>();
    protected List<GameObject> shootPoolYellow = new List<GameObject>();
    protected List<Transform> shootPoolYellowTR = new List<Transform>();
    protected List<Laser> shootPoolYellowLaser = new List<Laser>();
    protected List<GameObject> shootPoolPurple = new List<GameObject>();
    protected List<Transform> shootPoolPurpleTR = new List<Transform>();
    protected List<Laser> shootPollPurpleLaser = new List<Laser>();

    private GameObject shootParent;


    protected override void Awake()
    {

        base.Awake();

        shootParent = new GameObject("ShootParent");

        shootPrefab = Resources.Load("Prefabs/Laser") as GameObject;
        shootPrefabYellow = Resources.Load("Prefabs/LaserYellow") as GameObject;
        shootPrefabPurple = Resources.Load("Prefabs/LaserPurple") as GameObject;

        weaponSOCount = GameManager.Instance.weaponCountSO;

        for (int i = 1; i <= weaponSOCount; i++)
        {

            WeaponDesingSO tempDesingSO = Resources.Load("DesingSO/Weapons/Weapon_0" + i) as WeaponDesingSO;
            if (tempDesingSO != null) m_WeaponDesingSO.Add(tempDesingSO);

        }

        for (int i = 0; i < 4; i++)
        {

            shootPool.Add(Instantiate(shootPrefab, shootParent.transform));
            shootPoolTR.Add(shootPool[i].transform);
            shootPollLaser.Add(shootPool[i].GetComponent<Laser>());
            shootPool[i].SetActive(false);

            shootPoolYellow.Add(Instantiate(shootPrefabYellow, shootParent.transform));
            shootPoolYellowTR.Add(shootPoolYellow[i].transform);
            shootPoolYellowLaser.Add(shootPoolYellow[i].GetComponent<Laser>());
            shootPoolYellow[i].SetActive(false);

            shootPoolPurple.Add(Instantiate(shootPrefabPurple, shootParent.transform));
            shootPoolPurpleTR.Add(shootPoolPurple[i].transform);
            shootPollPurpleLaser.Add(shootPoolPurple[i].GetComponent<Laser>());
            shootPoolPurple[i].SetActive(false);

        }

    }

    protected void LateUpdate()
    {

        if (GameManager.Instance.CurrentGameMode == GameManager.GameMode.Fight)
        {

            if (timerReload < currentReloarTime) timerReload += Time.deltaTime;
            else Fire();

        }

    }

    private void Fire()
    {

        timerReload = 0.0f;

        switch (currentWeapon.CurrentRarityWeapon)
        {

            case WeaponDesingSO.RarityWeapon.standart:
                GetShhootFromPool(shootPool, shootPoolTR, shootPollLaser);
                SoundAndMusic.Instance.PlaySimpleLaserSound(shootPosition.gameObject);
                break;

            case WeaponDesingSO.RarityWeapon.yellow:
                GetShhootFromPool(shootPoolYellow, shootPoolYellowTR, shootPoolYellowLaser);
                SoundAndMusic.Instance.PlaySimpleLaserSound(shootPosition.gameObject);
                break;

            case WeaponDesingSO.RarityWeapon.purple:
                GetShhootFromPool(shootPoolPurple, shootPoolPurpleTR, shootPollPurpleLaser);
                SoundAndMusic.Instance.PlayStrangeLaserSound(shootPosition.gameObject);
                break;

        }

    }

    private void GetShhootFromPool(List<GameObject> listGO, List<Transform> listTR, List<Laser> listLS)
    {

        for (int i = 0; i < listGO.Count; i++)
        {

            if (!listGO[i].activeSelf)
            {

                listGO[i].SetActive(true);
                listTR[i].SetPositionAndRotation(shootPosition.position, Quaternion.LookRotation(enemyTransform.position - shootPosition.position));
                listLS[i].SetValue(currentWeapon.Damage, enemyPawn);

                break;

            }

        }

    }

    public override void ActivateSlot()
    {
        
        base.ActivateSlot();

        GameManager.Instance.choseNumberWeapon = Mathf.Clamp(GameManager.Instance.choseNumberWeapon, 0, m_WeaponDesingSO.Count);
        currentWeapon = m_WeaponDesingSO[GameManager.Instance.choseNumberWeapon];

        m_Pawn.AddWeapons(this);

        switch (currentWeapon.CurrentRarityWeapon)
        {

            case WeaponDesingSO.RarityWeapon.standart:
                m_MeshRenderer.material = blueMaterial;
                break;

            case WeaponDesingSO.RarityWeapon.yellow:
                m_MeshRenderer.material = yellowMaterial;
                break;

            case WeaponDesingSO.RarityWeapon.purple:
                m_MeshRenderer.material = purpleMaterial;
                break;

        }

        currentReloarTime = currentWeapon.FiringRate;

    }

    public WeaponDesingSO GetCurrentWeapon()
    {

        return currentWeapon;

    }

    public override void GreenPlaceWeapon(bool isOn)
    {

        transparentBoxPlace.SetActive(isOn);

    }

    public override void SetEnemy(Transform transform, Pawn pawn)
    {

        enemyPawn = pawn;
        enemyTransform = transform;

    }
    
}
