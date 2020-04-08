using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Тест скроллинга текстуры
/// </summary>
public class ScrollTex : MonoBehaviour
{
    MeshRenderer m_MeshRenderer;

    public float scrollX = 0.5f;
    public float scrollY = 0.5f;

    private void Awake()
    {

        m_MeshRenderer = gameObject.GetComponent<MeshRenderer>();

    }

    private void LateUpdate()
    {

        float offsetX = Time.time * scrollX;
        float offsetY = Time.time * scrollY;

        m_MeshRenderer.material.mainTextureOffset = new Vector2(offsetX, offsetY);

    }

}
