using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Структура данных графа для станций метро.
/// </summary>
namespace TestTask02
{

    public class SubwayStation : MonoBehaviour
    {
        
        public string Name { get; set; }

        public int Number { get; set; }

        public int waveCount { get; set; }
        
        public bool isMoved { get; set; }


        public int posX;
        public int posY;

        public List<PathBetweenTwoStation> Paths = new List<PathBetweenTwoStation>();
        
        public SubwayStation (string nameSubwayStation, int countNumber)
        {

            Name = nameSubwayStation;

            Number = countNumber;

            waveCount = 0;

            isMoved = false;

        }
        
    }

}
