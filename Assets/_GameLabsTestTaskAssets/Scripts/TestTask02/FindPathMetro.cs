using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// Главный скрипт выполненого задания № 2
/// В моём задании я выбрал структуру данных в виде графа. Как наиболее удобную для выполнения задания.
/// 
/// Путь высчитывать можно как и при запуске сцены - введя в текстовые поля названия начальной и конечной точек маршрута. 
/// Так и в инспекторе проставить этого скрипта.
/// </summary>
namespace TestTask02
{

    public class FindPathMetro : MonoBehaviour
    {

        [Header("Буква ПЕРВОЙ станции (откуда)")]
        [SerializeField]
        private string firstSubwayStationName = "";

        [Header("Буква ВТОРОЙ станции (куда)")]
        [SerializeField]
        private string twoSubwayStationName = "";


        [Header("Создайте и заполните список смежности станций метро")]
        [SerializeField]
        private List<ListOfCloseness> m_ListOfCloseness;

        private List<SubwayStation> subwayStations = new List<SubwayStation>();
        private List<PathBetweenTwoStation> paths = new List<PathBetweenTwoStation>();

        private List<int> subwayPath = new List<int>();
                
        private GameObject sphere;
        private GameObject line;
        private Material temparyMaterial;
        private Material[] pathMaterials;
        private GameObject lineParent;
        private GameObject[] lineArrayGO;
        private LineRenderer[] lineRenderers;
        private GameObject pointParent;
        private GameObject[] sphereArrayGO;
        private Transform[] sphereArrayTR;
        private TextMeshPro[] sphereTMPRO;

        private GUIControllerTask2 m_GUIController;
        
        private SubwayStation startSubway;
        private SubwayStation finishSubway;

        private int ChangeLineMetroCount = 0;

        private SubwayStation tempSubwayStation;
        private PathBetweenTwoStation tempPathBetweenTwoStation;
        private PathBetweenTwoStation tempPathBetweenTwoStation2;
        private Color tempColor;
        private int tempInt;



        private void Awake()
        {

            InitializationGraphs();

            InitializeAndLoad();

            DrawSubwayStationAndPath();        
            
        }

        public void StartFindPath()
        {

            firstSubwayStationName = m_GUIController.GetStartName();
            twoSubwayStationName = m_GUIController.GetFinishName();


            foreach (SubwayStation sbs in subwayStations)
            {

                sbs.waveCount = 0;
                
                if (sbs.Name == firstSubwayStationName) startSubway = sbs;
                else if (sbs.Name == twoSubwayStationName) finishSubway = sbs;

            }

            SearhPath(Wave(startSubway, finishSubway));

        }

        private void InitializationGraphs()
        {

            for (int i = 0; i < m_ListOfCloseness.Count; i++)
            {

                tempSubwayStation = new SubwayStation(m_ListOfCloseness[i].name, i);
                subwayStations.Add(tempSubwayStation);
                subwayPath.Add(m_ListOfCloseness[i].paths.Length);

            }

            for (int i = 0; i < m_ListOfCloseness.Count; i++)
            {

                for (int j = 0; j < m_ListOfCloseness[i].paths.Length; j++)
                {

                    foreach (SubwayStation sbs in subwayStations)
                    {

                        if (sbs.Name == m_ListOfCloseness[i].paths[j])
                        {

                            tempSubwayStation = sbs;
                            tempColor = m_ListOfCloseness[i].pathsColor[j];

                        }

                        if (sbs.Name == firstSubwayStationName) startSubway = sbs;
                        else if (sbs.Name == twoSubwayStationName) finishSubway = sbs;

                    }

                    tempPathBetweenTwoStation = new PathBetweenTwoStation(subwayStations[i], tempSubwayStation, tempColor);
                    paths.Add(tempPathBetweenTwoStation);

                }

            }

            for (int i = 0; i < paths.Count; i++)
            {

                foreach (SubwayStation sbs in subwayStations)
                {

                    if (paths[i].From.Name == sbs.Name) sbs.Paths.Add(paths[i]);

                }

            }

        }

        private void InitializeAndLoad()
        {

            m_GUIController = FindObjectOfType<GUIControllerTask2>();
            string centrText = "";
            if (m_ListOfCloseness.Count <= 0) centrText += "-=Не заполнен массив с точками станций и их путями=-";
            if (firstSubwayStationName == "" | twoSubwayStationName == "") centrText += "Начальная или конечная точка не задана";
            if (centrText != "") m_GUIController.SetCentrText(centrText);

            sphere = Resources.Load("Prefabs/SubwayStationSphere") as GameObject;
            line = Resources.Load("Prefabs/LinePath") as GameObject;
            temparyMaterial = Resources.Load("Materials/PathBetweenTwoStationMaterial") as Material;

            lineParent = new GameObject("Line Parent");
            pointParent = new GameObject("Point Parent");

            sphereArrayGO = new GameObject[subwayStations.Count];
            sphereArrayTR = new Transform[subwayStations.Count];
            sphereTMPRO = new TextMeshPro[subwayStations.Count];
            for (int i = 0; i < subwayStations.Count; i++)
            {

                sphereArrayGO[i] = Instantiate(sphere);
                sphereArrayTR[i] = sphereArrayGO[i].transform;
                sphereArrayTR[i].parent = pointParent.transform;
                sphereTMPRO[i] = sphereArrayGO[i].GetComponentInChildren<TextMeshPro>();

            }

            pathMaterials = new Material[paths.Count];
            lineArrayGO = new GameObject[paths.Count];
            lineRenderers = new LineRenderer[paths.Count];

            for (int i = 0; i < pathMaterials.Length; i++)
            {

                pathMaterials[i] = new Material(temparyMaterial);
                pathMaterials[i].color = paths[i].SubwayLineColor;

                lineArrayGO[i] = Instantiate(line, lineParent.transform);
                lineRenderers[i] = lineArrayGO[i].GetComponent<LineRenderer>();
                lineRenderers[i].material = pathMaterials[i];

            }

        }

        private void DrawSubwayStationAndPath()
        {
                        
            sphereArrayTR[0].position = new Vector3(-2, 0, 0);
            subwayStations[0].isMoved = true;
            sphereTMPRO[0].text = subwayStations[0].Name;
            subwayStations[0].posX = -2;
            subwayStations[0].posY = 0;


            for (int i = 1; i < subwayStations.Count; i++)
            {

                if (subwayPath[i] > 1) sphereArrayTR[i].position = new Vector3(i * 2, UnityEngine.Random.insideUnitSphere.y * i , 0);
                else sphereArrayTR[i].position = new Vector3(i*2, -1, 0);

                sphereTMPRO[i].text = subwayStations[i].Name;

                subwayStations[i].posX = Mathf.RoundToInt(sphereArrayTR[i].position.x);
                subwayStations[i].posY = Mathf.RoundToInt(sphereArrayTR[i].position.y);

                subwayStations[i].isMoved = true;
                
            }

            for (int i = 0; i < paths.Count; i++)
            {

                lineRenderers[i].SetPosition(0, new Vector3(paths[i].From.posX, paths[i].From.posY, 0));
                lineRenderers[i].SetPosition(1, new Vector3(paths[i].To.posX, paths[i].To.posY, 0));
                
            }           
            
        }       
        
        public void SearhPath(List<SubwayStation> markerStations)
        {

            ChangeLineMetroCount = 0;

            List<SubwayStation> finalPath = new List<SubwayStation>();

            finalPath.Add(finishSubway);

            for (int i = 0; i < finalPath.Count; i++)
            {

                SubwayStation subwayS = finalPath[i];

                if (subwayS.Name != startSubway.Name)
                {

                    foreach (var s in GetSubwayList(subwayS))
                    {

                        if (s.waveCount == subwayS.waveCount - 1)
                        {

                            finalPath.Add(s);
                            if (finalPath.Count > 2) ChangeLineMetroCount += ChangeLineOrNot(finalPath[finalPath.Count - 3].Name, finalPath[finalPath.Count - 2].Name, finalPath[finalPath.Count - 1].Name);
                            break;

                        }

                    }

                }
            }

            string strPath = "";
            for (int i = finalPath.Count - 1; i >= 0; i--)
            {

                if (i > 0) strPath += finalPath[i].Name + " - ";
                else strPath += finalPath[i].Name;

            }

            m_GUIController.SetCentrText("Кратчайший путь от начала -" + firstSubwayStationName + "- до финиша -" + twoSubwayStationName + " =  " + strPath + " ! Пересадок = " + ChangeLineMetroCount);

        }

        public List<SubwayStation> Wave(SubwayStation start, SubwayStation finish)
        {

            List<SubwayStation> listTemp = new List<SubwayStation>
            {

                start

            };
            
            for (int i = 0; i < listTemp.Count; i++)
            {

                SubwayStation subwayS = listTemp[i];

                if (subwayS.Name != finish.Name)
                {

                    foreach (var s in GetSubwayList(subwayS))
                    {

                        if (s.waveCount == 0 & start.Name != s.Name)
                        {

                            s.waveCount = subwayS.waveCount + 1;
                            listTemp.Add(s);

                        }

                    }

                }
                else
                {

                    return listTemp;

                }

            }

            return listTemp;

        }

        public List<SubwayStation> GetSubwayList(SubwayStation subway)
        {

            var result = new List<SubwayStation>();

            foreach (var pth in paths)
            {

                if (pth.From.Name == subway.Name) result.Add(pth.To);

            }

            return result;

        }

        private void Update()
        {

            if (Input.GetKeyDown(KeyCode.Escape)) SceneManager.LoadScene(0);

        }

        /// <summary>
        /// Подсчёт количества пересадок между 2мя маршрутами, т.е. между 2мя линиями метро
        /// </summary>
        /// <param name="oneSBW">Предыдущая станция</param>
        /// <param name="twoSBW">Серединная</param>
        /// <param name="threeSBW">Конечная</param>
        /// <returns></returns>
        private int ChangeLineOrNot(string oneSBW, string twoSBW, string threeSBW)
        {
            
            foreach (var item in paths)
            {

                if ((item.From.Name == oneSBW && item.To.Name == twoSBW) | (item.To.Name == oneSBW && item.From.Name == twoSBW)) tempPathBetweenTwoStation = item;
                else if ((item.From.Name == twoSBW && item.To.Name == threeSBW) | (item.To.Name == twoSBW && item.From.Name == threeSBW)) tempPathBetweenTwoStation2 = item;

            }

            if (tempPathBetweenTwoStation.SubwayLineColor.Equals(tempPathBetweenTwoStation2.SubwayLineColor)) return 0;
            else return 1;

        }

    }

    [Serializable]
    public struct ListOfCloseness
    {

        public string name;
        public string[] paths;
        public Color[] pathsColor;

    }

}


