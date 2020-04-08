using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Менеджер графического пользовательского интерфейса. 
/// </summary>
public class GUIManager : MonoBehaviour
{

    [Header("Экран установки модулей и оружия")]
    [SerializeField]
    private GameObject screenShop;

    [Header("Игровое GUI")]
    [SerializeField]
    private GameObject screenGame;

    [Header("Экраны победы и поражения")]
    [SerializeField]
    private GameObject screenWinner;

    [Header("Текстовое поле описания модуля или оружия")]
    [SerializeField]
    private TextMeshProUGUI textDescription;
    private GameObject fonTextDescription;

    [Header("Текстовое поле описания коробля А")]
    [SerializeField]
    private TextMeshProUGUI textDescriptionShip_A;

    [Header("Текстовое поле описания коробля В")]
    [SerializeField]
    private TextMeshProUGUI textDescriptionShip_B;
    

    //Ship A
    [Header("Ship A")]
    [SerializeField]
    private Image sliderHP_ShipA;
    [SerializeField]
    private TextMeshProUGUI textHP_ShipA;
    [SerializeField]
    private Image sliderShield_ShipA;
    [SerializeField]
    private TextMeshProUGUI textShield_ShipA;
    [SerializeField]
    private Image sliderReloadWeapon1_ShipA;
    [SerializeField]
    private Image sliderReloadWeapon2_ShipA;

    //Ship B
    [Header("Ship B")]
    [SerializeField]
    private Image sliderHP_ShipB;
    [SerializeField]
    private TextMeshProUGUI textHP_ShipB;
    [SerializeField]
    private Image sliderShield_ShipB;
    [SerializeField]
    private TextMeshProUGUI textShield_ShipB;
    [SerializeField]
    private Image sliderReloadWeapon1_ShipB;
    [SerializeField]
    private Image sliderReloadWeapon2_ShipB;

    [SerializeField]
    private TextMeshProUGUI finalText;

    private Animator GUI_Animator;

    private ChoiseWeaponAndModuleController wAmController;




    private void Awake()
    {

        GameManager.Instance.changeGameModeEvent += OnGameModeChanged;

        screenGame.SetActive(false);
        screenWinner.SetActive(false);
        screenShop.SetActive(true);

        wAmController = FindObjectOfType<ChoiseWeaponAndModuleController>();
        wAmController.changeStateWamController += ChangeStateWaMControl;

        fonTextDescription = GameObject.Find("DescriptionModuleWeaponFon");
        fonTextDescription.SetActive(false);

        GUI_Animator = gameObject.GetComponent<Animator>();
        
    }

    private void Start()
    {

        wAmController.pawnShipA.addedSlotEvent += SetTextDescriptionShipA;
        wAmController.pawnShipB.addedSlotEvent += SetTextDescriptionShipB;

        SetTextDescriptionShipA();
        SetTextDescriptionShipB();

    }

    private void LateUpdate()
    {
        
        if (GameManager.Instance.CurrentGameMode == GameManager.GameMode.Fight)
        {

            PlayerShipChangeValue();
            EnemyShipChangeValue();

        }

    }

    private void OnGameModeChanged()
    {

        switch (GameManager.Instance.CurrentGameMode)
        {

            case GameManager.GameMode.Shop:
                screenGame.SetActive(false);
                screenShop.SetActive(true);
                break;

            case GameManager.GameMode.Game:
                screenGame.SetActive(true);
                wAmController.animatorShipA.SetTrigger("isGame");
                wAmController.animatorShipB.SetTrigger("isGame");
                GUI_Animator.SetTrigger("isGame");
                PlayerShipChangeValue();
                EnemyShipChangeValue();
                break;

            case GameManager.GameMode.Winner:
                screenGame.SetActive(false);
                screenWinner.SetActive(true);
                finalText.text = "Ship А win!";
                break;

            case GameManager.GameMode.Loose:
                screenGame.SetActive(false);
                screenWinner.SetActive(true);
                finalText.text = "Ship B win!";
                break;

        }

    }

    private void PlayerShipChangeValue()
    {

        sliderHP_ShipA.fillAmount = wAmController.pawnShipA.HP / wAmController.pawnShipA.maxHP;
        textHP_ShipA.text = wAmController.pawnShipA.HP + " / " + wAmController.pawnShipA.maxHP;

        sliderShield_ShipA.fillAmount = wAmController.pawnShipA.ShieldPoint / wAmController.pawnShipA.maxShieldPoint;
        textShield_ShipA.text = wAmController.pawnShipA.ShieldPoint + " / " + wAmController.pawnShipA.maxShieldPoint + " + 1 в " + wAmController.pawnShipA.shieldRegeneration + " сек.";

        sliderReloadWeapon1_ShipA.fillAmount = wAmController.pawnShipA.GetWeaponSlots()[0].timerReload/ wAmController.pawnShipA.GetWeaponSlots()[0].currentReloarTime;
        sliderReloadWeapon2_ShipA.fillAmount = wAmController.pawnShipA.GetWeaponSlots()[1].timerReload / wAmController.pawnShipA.GetWeaponSlots()[1].currentReloarTime;

    }

    private void EnemyShipChangeValue()
    {

        sliderHP_ShipB.fillAmount = wAmController.pawnShipB.HP / wAmController.pawnShipB.maxHP;
        textHP_ShipB.text = wAmController.pawnShipB.HP + " / " + wAmController.pawnShipB.maxHP;

        sliderShield_ShipB.fillAmount = wAmController.pawnShipB.ShieldPoint / wAmController.pawnShipB.maxShieldPoint;
        textShield_ShipB.text = wAmController.pawnShipB.ShieldPoint + " / " + wAmController.pawnShipB.maxShieldPoint + " + 1 в " + wAmController.pawnShipB.shieldRegeneration + " сек.";

        sliderReloadWeapon1_ShipB.fillAmount = wAmController.pawnShipB.GetWeaponSlots()[0].timerReload / wAmController.pawnShipB.GetWeaponSlots()[0].currentReloarTime;
        sliderReloadWeapon2_ShipB.fillAmount = wAmController.pawnShipB.GetWeaponSlots()[1].timerReload / wAmController.pawnShipB.GetWeaponSlots()[1].currentReloarTime;

    }

    public void ClickWeapon(int number, WeaponDesingSO weapon)
    {

        GameManager.Instance.choseNumberWeapon = number;
        wAmController.ChoiseWeaponON();

        SetDescriptionWeapon(weapon);

    }

    public void ClickModule(int number, ModuleDesingSO module)
    {

        GameManager.Instance.choiseNumberModule = number;
        wAmController.ChoiseModuleON();

        SetDescriptionModule(module);

    }

    public void SetTextDescriptionShipA()
    {

        SetTextDescriptionShips(textDescriptionShip_A, wAmController.pawnShipA);

    }
    public void SetTextDescriptionShipB()
    {

        SetTextDescriptionShips(textDescriptionShip_B, wAmController.pawnShipB);

    }

    public void SetTextDescriptionShips(TextMeshProUGUI text, Pawn pawn)
    {

        text.SetText("Название: " + pawn.GetShipsDesingSO().shipName + "\n \n" +
            "Жизнь: " + pawn.maxHP + "\n \n" +
            "Щит: " + pawn.maxShieldPoint + "\n \n" +
            "Скорость восстановления щита: " + pawn.shieldRegeneration.ToString() + "\n \n" +
            "Пустых слотов для оружия: " + (pawn.GetShipsDesingSO().weaponsSlot - pawn.GetWeaponSlots().Length) + "\n \n" +
            "Пустых слотов для модулей: " + (pawn.GetShipsDesingSO().moduleSlot - pawn.GetModuleSlots().Length));

    }

    public void SetDescriptionWeapon(WeaponDesingSO weapon)
    {

        textDescription.SetText("Название: " + weapon.weaponName + "\n \n" +
            "Урон: " + weapon.Damage + "\n \n" +
            "Скорострельность 1 выстрел в " + weapon.FiringRate + " сек." + "\n \n" +
            "Редкость: " + weapon.CurrentRarityWeapon + "\n \n");

        fonTextDescription.SetActive(true);

    }

    public void SetDescriptionModule(ModuleDesingSO module)
    {

        textDescription.SetText("Название: " + module.moduleName + "\n \n" +
            "Изменение HP: " + module.changeMaxHP + "\n \n" +
            "Изменение заряда щита: " + module.changeMaxShield + "\n \n" +
            "Изменение восстановления щита: " + module.changeRegenerationShield + " %" + "\n \n" +
            "Изменение перезарядки оружия: " + module.changeReloadTime + " %" + "\n \n" +
            "Редкость: " + module.CurrentRarityModul + "\n \n");

        fonTextDescription.SetActive(true);

    }

    public void ChangeStateWaMControl()
    {

        switch (wAmController.currentChoiseState)
        {

            case ChoiseWeaponAndModuleController.ChoiseState.Nothing:
                textDescription.SetText("");
                fonTextDescription.SetActive(false);
                break;

        }

    }

}
