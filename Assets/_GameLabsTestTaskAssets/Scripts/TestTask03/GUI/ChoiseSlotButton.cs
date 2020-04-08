using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Кнопка оружия. Кидаем на кнопку и выбераем за какое оружие она отвечает. 
/// После клика по ней, данные передаются в ГУИ менеджер, для их отображения игроку в ГУИ.
/// </summary>
public class ChoiseSlotButton : MonoBehaviour
{

    private Button m_Button;

    [Header("Модуль или оружие на кнопке")]
    [SerializeField]
    private WeaponDesingSO m_WeaponDesingSO;
    [SerializeField]
    private ModuleDesingSO m_ModuleDesingSO;

    [Header("Номер")]
    [SerializeField]
    private int number;


    private void Awake()
    {

        m_Button = gameObject.GetComponent<Button>();
        m_Button.onClick.AddListener(TaskOnClick);

    }

    public void TaskOnClick()
    {

        if (m_WeaponDesingSO != null) GameManager.Instance.m_GUIManager.ClickWeapon(number, m_WeaponDesingSO);
        else GameManager.Instance.m_GUIManager.ClickModule(number, m_ModuleDesingSO);

    }

}
