using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
/// <summary>
/// Простеньгий контроллер 2х текстовых полей, в которое нужно вводить начальную и конечную остановки.
/// Кнопки, высчитывающая путь.
/// и текста
/// </summary>
public class GUIControllerTask2 : MonoBehaviour
{
    [Header("Текст по центру")]
    [SerializeField]
    private TextMeshProUGUI centrTextMesh;

    [Header("Текстовое поле старта")]
    [SerializeField]
    private TMP_InputField inputFieldStart;

    [Header("Текстовое поле финиша")]
    [SerializeField]
    private TMP_InputField inputFieldFinish;


    public void SetCentrText(string newText)
    {

        centrTextMesh.text = newText;

    }

    public string GetStartName()
    {

        return inputFieldStart.text;

    }

    public string GetFinishName()
    {

        return inputFieldFinish.text;

    }

}
