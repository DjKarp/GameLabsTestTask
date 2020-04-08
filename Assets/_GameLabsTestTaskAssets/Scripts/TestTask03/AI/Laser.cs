using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Скрипт на самом выстреле лазера - так сказать пуля.
/// Движем объект к врагу. При касании, наносим урон и отключаем объект, и то же делаем, если слишком далеко улетел от стартовой точки снаряд. 
/// 
/// Объект отключается, потому что все лазерные выстрелы беруться из пула заранее созданных объектов.
/// </summary>
public class Laser : MonoBehaviour
{

    private Transform m_Transform;

    private float speed = 14.0f;

    private float Damage = 0.0f;
    private Pawn enemyPawn;
    private Transform enemyTransform;

    private Vector3 startPosition;
    private float maxDistantion = 25.0f;


    private void Awake()
    {

        m_Transform = gameObject.transform;

    }

    private void LateUpdate()
    {

        m_Transform.position += (enemyTransform.position - m_Transform.position) * Time.deltaTime * speed;

        if (Vector3.Distance(startPosition, m_Transform.position) > maxDistantion) DamageEnemy();

    }

    private void OnCollisionEnter(Collision collision)
    {

        DamageEnemy();

    }

    private void DamageEnemy()
    {

        enemyPawn.TakeDamage(Damage);
        gameObject.SetActive(false);

    }

    /// <summary>
    /// В зависимости от того, из какого оружия будет вылетать снаряд, надо установить его показатели.
    /// </summary>
    /// <param name="damage">Количество урона</param>
    /// <param name="pawn">Павн врага.</param>
    public void SetValue(float damage, Pawn pawn)
    {

        Damage = damage;
        enemyPawn = pawn;
        enemyTransform = enemyPawn.gameObject.transform;

        startPosition = m_Transform.position;

    }

}
