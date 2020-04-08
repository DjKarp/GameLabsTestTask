using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Структура данных графа для линии метро между станциями
/// </summary>
namespace TestTask02
{

    public class PathBetweenTwoStation : MonoBehaviour
    {

        public SubwayStation From { get; set; }

        public SubwayStation To { get; set; }

        public Color SubwayLineColor { get; set; }

        public PathBetweenTwoStation (SubwayStation from, SubwayStation to, Color color)
        {

            From = from;

            To = to;

            SubwayLineColor = color;

        }

    }

}
