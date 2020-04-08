using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Главный скрипт выполнения первого тестового задания. 
/// 
/// С шейдерами опыта мало, поэтому реализацию на GPU волны не успел.
/// </summary>
public class MeshFlagGenerator : MonoBehaviour
{

    [Header("Количество вершин в высоту флага")]
    [SerializeField]
    private int heightFlag = 6;

    [Header("Количество вершин в ширину")]
    [SerializeField]
    private int widthFlag = 10;

    [Header("Амплитуда колебаний")]
    [SerializeField]
    private float waveAmplitude = 1.0f;

    private Transform m_Transform;

    private CameraControl m_CameraControl;
    private GUIController m_GUIController;

    private GameObject flagGO;
    private Transform flagTransform;

    private MeshFilter m_MeshFilter;
    private MeshRenderer m_MeshRenderer;

    private Material defaultMaterial;
    private Material scrollShaderMaterial;
    private Material wavesShaderMaterial;
    private GameObject sphereGO;

    private Mesh m_Mesh;

    private List<Vector3> allVertices = new List<Vector3>();
    private List<GameObject> allVertecsGO = new List<GameObject>();
    private List<Transform> allVertecsTransform = new List<Transform>();
    private List<int> allVertecsTriangles = new List<int>();
    private List<Vector2> allVerticesUV = new List<Vector2>();

    private Vector2 offset;
    
    private bool isWave = false;
        
    private Vector3[] waveVerticles;
    private int verticlesCount;

    public bool isScrollTexture = false;
    public float scrollHorizontalSpeed = 1.0f;
    private float scrollTimer = 0.0f;
    private float scrollOffset = 0.0f;

    private bool isShaderWaveOn = false;
    private bool isShaderScrollOn = false;



    private void Awake()
    {

        InitializeAndLoadResources();

        CreateMesh();

    }

    private void LateUpdate()
    {

        if (isWave) WaveGenerator();
        if (isScrollTexture) ScrollingTexture();

        if (Input.GetKeyDown(KeyCode.Escape)) SceneManager.LoadScene(0);

    }

    private void InitializeAndLoadResources()
    {

        m_Transform = gameObject.transform;
        offset = new Vector2(m_Transform.position.x, m_Transform.position.y);

        m_CameraControl = FindObjectOfType<CameraControl>();
        m_GUIController = FindObjectOfType<GUIController>();

        flagGO = new GameObject("Flag");
        flagTransform = flagGO.transform;

        m_CameraControl.target = flagTransform;

        m_MeshFilter = flagGO.AddComponent<MeshFilter>();
        m_MeshRenderer = flagGO.AddComponent<MeshRenderer>();

        m_Mesh = new Mesh();
        m_MeshFilter.mesh = m_Mesh;

        defaultMaterial = Resources.Load("Materials/FlagDefault") as Material;
        m_MeshRenderer.material = defaultMaterial;
        m_MeshRenderer.sharedMaterial.SetTextureScale("_MainTex", new Vector2(1 / (float)widthFlag, 1 / (float)heightFlag));

        sphereGO = Resources.Load("Prefabs/VertecsSphere") as GameObject;

        scrollShaderMaterial = Resources.Load("Materials/FlagShaderScrolling") as Material;
        scrollShaderMaterial.SetTextureScale("_MainTex", new Vector2(1 / (float)widthFlag, 1 / (float)heightFlag));

        wavesShaderMaterial = Resources.Load("Materials/FlagShaderWaves") as Material;
        wavesShaderMaterial.SetTextureScale("_MainTex", new Vector2(1 / (float)widthFlag, 1 / (float)heightFlag));

    }

    private void CreateMesh()
    {


        for (int i = 0; i <= heightFlag; i++)
        {

            for (int j = 0; j <= widthFlag; j++)
            {

                allVertices.Add(new Vector3(j + offset.x, i + offset.y, 0));
                allVerticesUV.Add(new Vector2(j, i));

                allVertecsGO.Add(Instantiate(sphereGO, allVertices[allVertices.Count - 1], Quaternion.identity));
                allVertecsTransform.Add(allVertecsGO[allVertecsGO.Count - 1].transform);
                allVertecsTransform[allVertecsTransform.Count - 1].parent = flagTransform;

            }

        }

        for (int count = 0, i = 0; i < heightFlag; i++)
        {

            for (int j = 0; j < widthFlag; j++)
            {

                allVertecsTriangles.Add(count);
                allVertecsTriangles.Add(count + widthFlag + 1);
                allVertecsTriangles.Add(count + 1);
                allVertecsTriangles.Add(count + 1);
                allVertecsTriangles.Add(count + widthFlag + 1);
                allVertecsTriangles.Add(count + widthFlag + 2);

                count++;

            }

            count++;

        }

        ResetWaves();

    }

    private void OnDestroy()
    {

        defaultMaterial.SetTextureScale("_MainTex", new Vector2(1, 1));
        defaultMaterial.mainTextureOffset = new Vector2(0.0f, 0.0f);

    }

    private void WaveGenerator()
    {

        waveVerticles = m_Mesh.vertices;

        verticlesCount = 0;

        for (int i = 0; i <= heightFlag; i++)
        {

            for (int j = 0; j <= widthFlag; j++)
            {

                waveVerticles[verticlesCount].z = Mathf.Sin((Time.time + waveVerticles[verticlesCount].x) * waveAmplitude);
                allVertecsTransform[verticlesCount].position = waveVerticles[verticlesCount];
                verticlesCount++;

            }

        }

        m_Mesh.vertices = waveVerticles;
        m_Mesh.RecalculateBounds();
        m_Mesh.RecalculateNormals();
        
    }  

    public void ToggleOnOffWavesOnFlag()
    {

        if (!isWave) isWave = true;
        else isWave = false;

        m_GUIController.SetToggleNameOnButtonToggleWaves(isWave);

    }

    public void ResetWaves()
    {

        if (isWave) ToggleOnOffWavesOnFlag();

        for (int i = 0; i < allVertecsTransform.Count; i++) allVertecsTransform[i].position = allVertices[i];

        m_Mesh.Clear();
        m_Mesh.vertices = allVertices.ToArray();
        m_Mesh.uv = allVerticesUV.ToArray();
        m_Mesh.triangles = allVertecsTriangles.ToArray();
        m_Mesh.Optimize();
        m_Mesh.RecalculateNormals();

    }

    private void ScrollingTexture()
    {

        scrollTimer += Time.deltaTime;
        scrollOffset = -scrollTimer * (scrollHorizontalSpeed / 100);

        defaultMaterial.mainTextureOffset = new Vector2(scrollOffset, 0.0f);
        
    }

    public void ToggleOnOffScrollingTexture()
    {

        if (!isScrollTexture)
        {

            isScrollTexture = true;
            scrollTimer = 0.0f;

        }
        else isScrollTexture = false;        

        m_GUIController.SetToggleNameOnButtonToggleScroll(isScrollTexture);

    }

    public void ResetScrollingTexture()
    {

        if (isScrollTexture) ToggleOnOffScrollingTexture();

        defaultMaterial.mainTextureOffset = new Vector2(0.0f, 0.0f);        

    }

    public void GPU_ToggleOnOffScrollingTexture()
    {

        if (!isShaderScrollOn)
        {

            isShaderScrollOn = true;
            m_MeshRenderer.material = scrollShaderMaterial;

        }
        else
        {

            isShaderScrollOn = false;
            m_MeshRenderer.material = defaultMaterial;

        }

        m_GUIController.SetToggleNameOnButtonToggleScrollGPU(isShaderScrollOn);

    }

    public void GPU_ToggleOnOffWaves()
    {

        if (!isShaderWaveOn)
        {

            isShaderWaveOn = true;
            m_MeshRenderer.material = wavesShaderMaterial;

        }
        else
        {

            isShaderWaveOn = false;
            m_MeshRenderer.material = defaultMaterial;

        }

        m_GUIController.SetToggleNameOnButtonToggleWavesGPU(isShaderWaveOn);

    }

}
