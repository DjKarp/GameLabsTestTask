using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Хранение данных о свойствах и характеристиках корабля.
/// Создание вынесено в своё меню.
/// </summary>
[CreateAssetMenu(menuName = "DesingSO/Ships")]
public class ShipsDesingSO : ScriptableObject
{

    [Header("Имя корабля.")]
    [SerializeField]
    public string shipName;

    [Header("Максимальное количество здоровья.")]
    [SerializeField]
    public float maxHP;

    [Header("Максимальнsq заряд щита")]
    [SerializeField]
    public float maxShieldPoint;

    [Header("Скорость регенерации щита")]
    [SerializeField]
    public float shieldRegeneration;

    [Header("Количество слотов под оружие")]
    [SerializeField]
    [Range(0, 5)]
    public int weaponsSlot;

    [Header("Количество слотов для модулей")]
    [SerializeField]
    [Range(0,5)]
    public int moduleSlot;

}
