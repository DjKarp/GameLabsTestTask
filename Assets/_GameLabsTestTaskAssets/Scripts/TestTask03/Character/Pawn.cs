using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Основа для всех персонажей. Общие переменные и методы.
/// </summary>
public class Pawn : MonoBehaviour
{
    /// <summary>
    /// Эвент установки в корабль оружия или модуля. 
    /// Это вдёт к пересчёту показателей характеристик корабля.
    /// В ГУИ надо будет переотобразить новые данные.
    /// </summary>
    public delegate void AddedSlot();
    public event AddedSlot addedSlotEvent;

    //Трансформ самого Pawn
    protected Transform m_Transform;

    //Текущий и максимальный уровни здоровья
    public float HP;
    public float maxHP;

    //Текущий и максимальный заряд щита
    public float ShieldPoint;
    public float maxShieldPoint;

    //Регенерация щита
    public float shieldRegeneration;

    //Слоты оружия
    protected List<SlotWeapon> weaponsSlot = new List<SlotWeapon>();


    //Слоты модулей
    protected List<SlotModule> moduleSlot = new List<SlotModule>();   


    [Header("Scriptable Object ShipsDesing для данного космического корабля")]
    [SerializeField]
    protected ShipsDesingSO m_ShipsDesingSO;

    //Модульное изменениея
    protected float moduleHP;
    protected float moduleShieldPoint;
    protected float moduleReload;
    protected float moduleShieldRegeneration;

    private float regenerationTimer;



    protected virtual void Awake()
    {

        if (m_ShipsDesingSO != null)
        {

            maxHP = m_ShipsDesingSO.maxHP;
            maxShieldPoint = m_ShipsDesingSO.maxShieldPoint;
            shieldRegeneration = m_ShipsDesingSO.shieldRegeneration;

        }
        else Debug.LogWarning("No choise ShipDesingSO in Pawn");        

        m_Transform = gameObject.transform;

        GameManager.Instance.changeGameModeEvent += OnGameModeChanged;

    }

    private void Start()
    {

        Initialize();

    }

    protected virtual void Update()
    {

        ShieldRegeneration();

    }

    public virtual void TakeDamage(float damage)
    {

        if (ShieldPoint > 0 && ShieldPoint > damage) ShieldPoint -= damage;
        else if (ShieldPoint > 0 && ShieldPoint <= damage)
        {

            damage -= ShieldPoint;
            HP -= damage;

        }
        else HP -= damage;
        
        CheckDie();

    }

    protected virtual void CheckDie()
    {

        if (IsDie())
        {

            Die();

        }

    }

    public virtual bool IsDie()
    {

        if (HP <= 0.0f) return true;
        else return false;

    }

    protected virtual void Die()
    {
        
        if (gameObject.name == "StarShip_A")
        {

            GameManager.Instance.ChangeGameMode(GameManager.GameMode.Loose);

        }
        else if (gameObject.name == "StarShip_B")
        {

            GameManager.Instance.ChangeGameMode(GameManager.GameMode.Winner);

        }   
        
    }

    protected virtual void OnGameModeChanged()
    {

        switch (GameManager.Instance.CurrentGameMode)
        {

            case GameManager.GameMode.Game:
                Initialize();
                break;

        }

    }
    /// <summary>
    /// Инициализация и пересчёт характеристик корабля в зависимости от установленного в него модуля.
    /// </summary>
    protected virtual void Initialize()
    {

        moduleHP = 0;
        moduleShieldPoint = 0;
        shieldRegeneration = 0;
        moduleShieldRegeneration = 0;
        moduleReload = 0;
        foreach (var modul in moduleSlot)
        {

            if (modul != null)
            {

                moduleHP += modul.GetCurrentModule().changeMaxHP;
                moduleShieldPoint += modul.GetCurrentModule().changeMaxShield;
                moduleShieldRegeneration += (m_ShipsDesingSO.shieldRegeneration * modul.GetCurrentModule().changeRegenerationShield)/ 100;
                moduleReload += modul.GetCurrentModule().changeReloadTime;

            }

        }

        maxHP = m_ShipsDesingSO.maxHP + moduleHP;
        HP = maxHP;

        maxShieldPoint = m_ShipsDesingSO.maxShieldPoint + moduleShieldPoint;
        ShieldPoint = maxShieldPoint;

        shieldRegeneration = m_ShipsDesingSO.shieldRegeneration + moduleShieldRegeneration;
        
    }

    public void AddWeapons(SlotWeapon weapon)
    {

        weaponsSlot.Add(weapon);        
        Initialize();
        addedSlotEvent();

    }

    public void AddModule(SlotModule module)
    {

        moduleSlot.Add(module);
        Initialize();
        addedSlotEvent();       

    }

    private void ShieldRegeneration()
    {

        if (ShieldPoint < maxShieldPoint)
        {

            if (regenerationTimer < shieldRegeneration) regenerationTimer += Time.deltaTime;
            else
            {

                regenerationTimer = 0.0f;
                ShieldPoint++;

            }

        }

    }

    public ShipsDesingSO GetShipsDesingSO()
    {

        return m_ShipsDesingSO;

    }

    public SlotWeapon[] GetWeaponSlots()
    {

        return weaponsSlot.ToArray();

    }

    public SlotModule[] GetModuleSlots()
    {

        return moduleSlot.ToArray();

    }

    public float getModuleReloadChange()
    {

        return moduleReload;

    }

}
