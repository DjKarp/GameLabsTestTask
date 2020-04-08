using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/// <summary>
/// Скриптик закгрузки нужной сцены по номеру BuildIndex
/// </summary>
public class LoadTaskSceneButton : MonoBehaviour
{

    [Header("Кнопка. Оставте пустым, если скрипт на кнопке.")]
    [SerializeField]
    private Button m_Button;

    [Header("Загружаемая сцена")]
    [SerializeField]
    private SceneNumber LoadScene;
    public enum SceneNumber
    {

        One,
        Two,
        Three

    }

    private void Awake()
    {

        if (m_Button == null) m_Button = gameObject.GetComponent<Button>();
        m_Button.onClick.AddListener(TaskOnClick);

    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();

    }

    void TaskOnClick()
    {

        switch (LoadScene)
        {

            case SceneNumber.One:
                SceneManager.LoadScene(1);
                break;

            case SceneNumber.Two:
                SceneManager.LoadScene(2);
                break;

            case SceneNumber.Three:
                SceneManager.LoadScene(3);
                break;

        }

    }

}
