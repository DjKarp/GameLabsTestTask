using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Простенький скрипт кнопки переключения в разные режимы игры. 
/// Выбираем кнопку (или вешаем компонент на неё), выбираем режим.
/// Также можно поставить галку на отключение кнопки после срабатывания.
/// </summary>
[RequireComponent(typeof(Button))]
public class StateButton : MonoBehaviour
{

    [Header("Кнопка. Оставте пустым, если скрипт на кнопке.")]
    [SerializeField] 
    private Button m_Button;

    [Header("В какой игровой режим переводит кнопка.")]
    [SerializeField] 
    private GameManager.GameMode m_GameMode;

    [Header("Выключать кнопку после нажатия?")]
    [SerializeField]
    private bool isTurnOffButton;

    private Transform m_Transform;

    private Image m_Image;
    private TextMeshProUGUI m_TextMeshPro;



    private void Awake()
    {

        if (m_Button == null) m_Button = gameObject.GetComponent<Button>();
        m_Image = gameObject.GetComponent<Image>();
        m_TextMeshPro = gameObject.GetComponentInChildren<TextMeshProUGUI>();

        m_Transform = gameObject.transform;

        m_Button.onClick.AddListener(TaskOnClick);

    }

    private void Reset()
    {

        m_Button = gameObject.GetComponent<Button>();
        if (m_Button == null) m_Button = gameObject.AddComponent<Button>();

    }

    void TaskOnClick()
    {

        GameManager.Instance.ChangeGameMode(m_GameMode);

        if (isTurnOffButton)
        {

            m_Button.enabled = false;
            m_TextMeshPro.enabled = false;
            m_Image.enabled = false;

        }

    }

}
