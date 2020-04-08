using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

/// <summary>
/// Синглтон, отвечающий за всё музыкальное сопровождение. Музыку и звуки задаёт автоматически в соответствии со списком уровней и привязанных
/// к ним звуков. Именно синглтон нужен, так как FMOD умеет не прерывать музыку между загрузками сцен, а одиночка как раз и следит за тем, 
/// надо ли переключать композициию.
/// 
/// Доступ из любого скрипта проекта -> SoundAndMusic.Instance. <-
/// 
/// </summary> 
public class SoundAndMusic : MonoBehaviour
{

    private static SoundAndMusic _instance = null;
    
    private Camera m_Camera;
    private GameObject m_CameraGO;
    
    /// <summary>
    /// /////////////////////////// = Sound And Music path in FMOD = /////////////////////////////////////
    /// </summary>

    //Game - Variable

    /// <summary>
    /// Ниже идёт список эвентов FMOD. А так же индивидуальные эвенты.
    /// </summary>
    //Melody
    private string menuMusic = "event:/Music";
    
    //FX
    private string simpleSound = "event:/SimpleShot";
    private string strangeSound = "event:/StrengeShoot";
    
    //FMOD Events
    private EventInstance musicEvent;

    private EventDescription myEvDes;
    private string tempPath;
    

    public static SoundAndMusic Instance
    {

        get
        {
            if (_instance == null)
            {

                _instance = FindObjectOfType<SoundAndMusic>();

                if (_instance == null)
                {

                    GameObject go = new GameObject();
                    go.name = "SingletonController";
                    _instance = go.AddComponent<SoundAndMusic>();
                    DontDestroyOnLoad(go);

                }

            }

            return _instance;

        }

    }

    /// <summary>
    /// /////////////////////////// = Initializations = /////////////////////////////////////
    /// </summary>
    void Awake()
    {

        if (_instance == null)
        {

            _instance = this;
            DontDestroyOnLoad(this.gameObject);
                        
        }
        else
        {

            Destroy(gameObject);

        }

    }
    void Start() { InitializeManager(); }
    private void InitializeManager()
    {
        
        CheckCamera();
                
        ChangePlayingMusic();
                
    }

    /// <summary>
    /// Так как в каждой сцене своя камера, то нужно следить, чтобы на неё всегда была ссылка. 
    /// Из камеры должны звучать все звуки и мелодии связанные с интерфейсом и т.п.
    /// </summary>
    public void CheckCamera()
    {

        if (m_Camera == null) m_Camera = Camera.main;
        if (m_CameraGO == null && m_Camera != null && m_Camera.gameObject != null) m_CameraGO = m_Camera.gameObject;
        if (m_CameraGO == null) m_CameraGO = gameObject;

    }

    private void ChangePlayingMusic()
    {

        if (GetMusicPath() == null)
        {

            musicEvent = RuntimeManager.CreateInstance(menuMusic);
            musicEvent.start();

        }
        
    }

    public void StopMusic()
    {

        musicEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

    }

    public string GetMusicPath()
    {

        musicEvent.getDescription(out myEvDes);
        myEvDes.getPath(out tempPath);
        return tempPath;

    }

    /// <summary>
    /// /////////////////////////// = Sound And Music Methods = /////////////////////////////////////
    /// 
    /// Общедоступные методы для вызова их через синглтон. 
    /// Здесь и надо создавать новые методы для новых звуков в игре.
    /// Заделены по зонам ответственности для более лёгкой навигаии.
    /// 
    /// </summary>

    ///////////// = GAME = ///////////////
    //////// = WEAPON = //////////

    public void PlaySimpleLaserSound(GameObject forFMOD)
    {

        RuntimeManager.PlayOneShotAttached(simpleSound, forFMOD);

    }

    public void PlayStrangeLaserSound(GameObject forFMOD)
    {

        RuntimeManager.PlayOneShotAttached(strangeSound, forFMOD);

    }

    //////// = OTHER = //////////

}
