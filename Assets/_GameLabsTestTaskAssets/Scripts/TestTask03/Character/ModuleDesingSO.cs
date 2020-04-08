using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Характеристики модулей
/// </summary>
[CreateAssetMenu(menuName = "DesingSO/Modul")]
public class ModuleDesingSO : ScriptableObject
{
    [Header("Название модуля.")]
    [SerializeField]
    public string moduleName;

    [Header("Изменение максимального количества очков здоровья (HP).")]
    [SerializeField]
    public float changeMaxHP;

    [Header("Изменение прочности щита (Shield).")]
    [SerializeField]
    public float changeMaxShield;

    [Header("Изменение перезарядки всего оружия.(%)")]
    [SerializeField]
    public float changeReloadTime;

    [Header("Изменение значения восстановления щита.(%)")]
    [SerializeField]
    public float changeRegenerationShield;

    [Header("Редкость предмета")]
    [SerializeField]
    public RarityModul CurrentRarityModul;
    public enum RarityModul
    {

        standart,
        yellow,
        purple

    }

}
