using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Характеристики оружия и их хранение
/// </summary>
[CreateAssetMenu(menuName = "DesingSO/Weapon")]
public class WeaponDesingSO : ScriptableObject
{

    [Header("Название орудия.")]
    [SerializeField]
    public string weaponName;

    [Header("Урон одного выстрела.")]
    [SerializeField]
    public float Damage;

    //Т.е. сколько промежуток между двумя выстрелами.
    [Header("Скорострельность в сек.")]
    [SerializeField]
    public float FiringRate;

    [Header("Редкость предмета")]
    [SerializeField]
    public RarityWeapon CurrentRarityWeapon;
    public enum RarityWeapon
    {

        standart,
        yellow,
        purple

    }

}
