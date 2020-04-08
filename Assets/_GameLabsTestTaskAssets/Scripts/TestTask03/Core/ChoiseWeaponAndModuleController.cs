using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Класс управления установки модулей и оружия в космические корабли. 
/// </summary>
public class ChoiseWeaponAndModuleController : MonoBehaviour
{
    /// <summary>
    /// Делегат опевещения об изменении выбранного элемента. 
    /// Но используется чтобы стирать старые описания уже установленного модуля или оружия.
    /// </summary>
    public delegate void ChangeStateWaMController();
    public event ChangeStateWaMController changeStateWamController;

    [Header("Корабль А")]
    [SerializeField]
    private GameObject shipA;
    public Pawn pawnShipA;
    public List<SlotWeapon> allSlotsWeapon_A = new List<SlotWeapon>();
    public List<SlotModule> allSlotsModule_A = new List<SlotModule>();
    public Animator animatorShipA;

    [Header("Корабль B")]
    [SerializeField]
    private GameObject shipB;
    public Pawn pawnShipB; 
    public List<SlotWeapon> allSlotsWeapon_B = new List<SlotWeapon>();
    private List<GameObject> allSlotWeaponGO_B = new List<GameObject>();
    public List<SlotModule> allSlotsModule_B = new List<SlotModule>();
    public Animator animatorShipB;

    /// <summary>
    /// Все пустые слоты со всех кораблей. Для отслеживания и более простому поиску \ доступу к ним.
    /// </summary>
    public List<SlotWeapon> allSlotsWeapon = new List<SlotWeapon>();
    public List<SlotModule> allSlotsModule = new List<SlotModule>();
    
    /// <summary>
    /// Полупрозрачные модели оружия и модулей, для предпоказа игроку после выбора в GUI
    /// </summary>
    private GameObject transparentModuleGO;
    private Transform transparentModuleTR;
    private Quaternion startQuanterionModule;

    private GameObject transpatentWeaponGO;
    private Transform transparentWeaponTR;
    private Quaternion startQuanterionWeapon;

    /// <summary>
    /// Стейты действий игрока и магазина
    /// </summary>
    public ChoiseState currentChoiseState;
    public enum ChoiseState
    {

        Nothing,        // Игрок ничего не щёлкнул или завершил установку. Магазин в режиме ожидания действий игрока
        WeaponClick,    // Игрок щёлкнул на иконке оружия. Надо включить отображения оружия на сцере (полупрозрачного) и показать описание.
        ModuleClick,    // То же, что и предыдущий стейт, только для модулей
        Activate        // Игрок щёлкнул по модулю / оружию в его пазе и тем самым установил его.

    }

    private Camera m_Camera;

    /// <summary>
    /// Считаю, что лучше не объявлять переменные во время выполнения, а заранее подготовить временные.
    /// </summary>
    private Vector3 tempVector3;
    private float tempDistance;
    private float minDistance;
    


    private void Awake()
    {
        
        pawnShipA = shipA.GetComponent<Pawn>();
        pawnShipB = shipB.GetComponent<Pawn>();

        animatorShipA = pawnShipA.gameObject.GetComponent<Animator>();
        animatorShipB = pawnShipB.gameObject.GetComponent<Animator>();
        
        allSlotsWeapon_A.AddRange(shipA.GetComponentsInChildren<SlotWeapon>());
        allSlotsModule_A.AddRange(shipA.GetComponentsInChildren<SlotModule>());

        // Ищем все существующие слоты под оружие и отключаем все, кроме того количества, что должны быть у корабля. 
        // Сделано для внесения разнообразия и работает только для корабля B
        allSlotsModule_B.AddRange(shipB.GetComponentsInChildren<SlotModule>());
        allSlotsWeapon_B.AddRange(shipB.GetComponentsInChildren<SlotWeapon>());
        foreach (SlotWeapon sv in allSlotsWeapon_B) allSlotWeaponGO_B.Add(sv.gameObject);
        for (int i = 0; i < allSlotsWeapon_B.Count; i++) if (i >= pawnShipB.GetShipsDesingSO().weaponsSlot) allSlotWeaponGO_B[i].gameObject.SetActive(false);

        foreach (var v in allSlotsModule_A) allSlotsModule.Add(v);
        foreach (var v in allSlotsModule_B) allSlotsModule.Add(v);
        foreach (var v in allSlotsWeapon_A) allSlotsWeapon.Add(v);
        foreach (var v in allSlotsWeapon_B) allSlotsWeapon.Add(v);

        transparentModuleGO = Instantiate(Resources.Load("Prefabs/Module_01") as GameObject);
        transparentModuleTR = transparentModuleGO.transform;
        startQuanterionModule = transparentModuleTR.rotation;

        transpatentWeaponGO = Instantiate(Resources.Load("Prefabs/Weapon_01") as GameObject);
        transparentWeaponTR = transpatentWeaponGO.transform;
        startQuanterionWeapon = transparentWeaponTR.rotation;

        m_Camera = Camera.main;

        ChangeStateWaMControl(ChoiseState.Nothing);

    }

    private void Update()
    {

        switch (currentChoiseState)
        {

            case ChoiseState.WeaponClick:
                SetTransparentPosition(transparentWeaponTR);
                break;

            case ChoiseState.ModuleClick:
                SetTransparentPosition(transparentModuleTR);
                break;

            case ChoiseState.Activate:

                break;

            case ChoiseState.Nothing:
                transpatentWeaponGO.SetActive(false);
                transparentModuleGO.SetActive(false);
                GreenPlaceModule(false);
                GreenPlaceWeapon(false);
                break;
        }

    }

    private void ChangeStateWaMControl(ChoiseState state)
    {

        currentChoiseState = state;

        changeStateWamController();

    }

    /// <summary>
    /// Позицируем полупрозрачный модуль \ оружие около кораблей и следящим за двидением курсора мышки. 
    /// 
    /// Если близко к пустому слоту приближаем наш объект, то он атоматически встаёт на место, тем самым отображает, как будет установлен.
    /// Если удаляем курсор от слота, то возвращаем обратно объект в курсору.
    /// 
    /// Можно было лучше метод сделать, но уже не было времени. Уже ещё 2 тестовых задания в очереди. Но согласен, что некрасиво сделано. 
    /// </summary>
    /// <param name="tr">Трансформ полупрозрачного модуля или оружия.</param>
    private void SetTransparentPosition(Transform tr)
    {
               
        tempVector3 = Vector3.zero;
        tempDistance = Mathf.Infinity;
        minDistance = 0.0f;

        tempVector3 = m_Camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 20.0f));
        tr.position = tempVector3;
        tr.rotation = startQuanterionWeapon;

        if (currentChoiseState == ChoiseState.WeaponClick)
        {

            foreach (SlotWeapon slot in allSlotsWeapon)
            {

                if (slot.gameObject.activeSelf)
                {

                    tempDistance = Vector2.Distance(new Vector2(tempVector3.x, tempVector3.y), new Vector2(slot.GetPositionPoint().x, slot.GetPositionPoint().y));

                    if (tempDistance < 2.0f)
                    {

                        tr.position = slot.GetPositionPoint();
                        tr.rotation = slot.GetRotationSlot();
                        minDistance = tempDistance;

                        slot.transparentBoxPlace.SetActive(false);

                        if (Input.GetMouseButtonUp(0))
                        {

                            slot.ActivateSlot();
                            SetEnemyForWeapon(slot);
                            ChangeStateWaMControl(ChoiseState.Nothing);
                            allSlotsWeapon.Remove(slot);
                            allSlotsWeapon_A.Remove(slot);
                            allSlotsWeapon_B.Remove(slot);
                            break;

                        }

                    }
                    else
                    {

                        slot.transparentBoxPlace.SetActive(true);

                    }

                }               

            }

            if (minDistance >= 2.0f) tr.rotation = startQuanterionWeapon;
            
        }  
        else
        {

            foreach (SlotModule slot in allSlotsModule)
            {

                if (slot.gameObject.activeSelf)
                {

                    tempDistance = Vector2.Distance(new Vector2(tempVector3.x, tempVector3.y), new Vector2(slot.GetPositionPoint().x, slot.GetPositionPoint().y));

                    if (tempDistance < 2.0f)
                    {

                        tr.position = slot.GetPositionPoint();
                        tr.rotation = slot.GetRotationSlot();
                        minDistance = tempDistance;

                        slot.transparentBoxPlace.SetActive(false);

                        if (Input.GetMouseButtonUp(0))
                        {

                            slot.ActivateSlot();
                            ChangeStateWaMControl(ChoiseState.Nothing);
                            allSlotsModule.Remove(slot);
                            allSlotsModule_A.Remove(slot);
                            allSlotsModule_B.Remove(slot);
                            break;

                        }                           

                    }
                    else slot.transparentBoxPlace.SetActive(true);

                }

            }

            if (minDistance >= 2.0f) tr.rotation = startQuanterionModule;

        }       
        
    }

    /// <summary>
    /// Если устанавливаем оружие, то сразу ему передаём павн врага и его трансформ. 
    /// </summary>
    /// <param name="slot">Активированый слот</param>
    private void SetEnemyForWeapon(Slot slot)
    {

        if (allSlotsWeapon_A.Contains(slot as SlotWeapon)) slot.SetEnemy(shipB.transform, pawnShipB);
        else slot.SetEnemy(shipA.transform, pawnShipA);

    }

    /// <summary>
    /// Если игрок нажал на любое оружие, то показываем все свободные места его установки - зелёным полупрозрачным паралепипедом.
    /// Также включаем само оружие - полупрозрачный макет его. И меняем стейт. 
    /// </summary>
    public void ChoiseWeaponON()
    {

        GreenPlaceWeapon(true);
        GreenPlaceModule(false);
        transpatentWeaponGO.SetActive(true);
        ChangeStateWaMControl(ChoiseState.WeaponClick);

    }
    // Тоже, что и предыдущий метод, но для клика по модулю.
    public void ChoiseModuleON()
    {

        GreenPlaceModule(true);
        GreenPlaceWeapon(false);
        transparentModuleGO.SetActive(true);
        ChangeStateWaMControl(ChoiseState.ModuleClick);

    }
    /// <summary>
    /// Включение \ отключение отображения свободных слотов под модули. 
    /// Отображаются зелёными полупрозрачными паралепипедами.
    /// </summary>
    /// <param name="isOn">Включить - выключить отображение.</param>
    public void GreenPlaceModule(bool isOn)
    {

        foreach (var v in allSlotsModule_A) v.transparentBoxPlace.SetActive(isOn);
        foreach (var v in allSlotsModule_B) v.transparentBoxPlace.SetActive(isOn);
        
    }

    public void GreenPlaceWeapon(bool isOn)
    {
                       
        foreach (var v in allSlotsWeapon_A) v.transparentBoxPlace.SetActive(isOn);
        foreach (var v in allSlotsWeapon_B) v.transparentBoxPlace.SetActive(isOn);

    }

}
