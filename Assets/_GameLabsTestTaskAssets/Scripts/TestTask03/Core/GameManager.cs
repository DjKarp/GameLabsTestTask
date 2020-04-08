using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// главный скрипт игры, который следит за переключением стейтов игры и инициализацию.
/// Игры толком нет, поэтоиму функций мало у него, основное действие происходит в ChoiseWeaponAndModuleController.
/// Скрипт сделан по шаблону проектирования Singleton
/// </summary>
public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; } = null;

    public delegate void ChangeState();
    public event ChangeState changeGameModeEvent;


    public GameMode CurrentGameMode;
    public enum GameMode
    {

        Shop,
        Game,
        Fight,
        PauseGame,
        Winner,
        Loose

    }

    //Количество доступного оружия.
    public int weaponCountSO = 3;
    //Количество доступных модулей.
    public int moduleCountSO = 4;
    public int choseNumberWeapon;
    public int choiseNumberModule;

    public GUIManager m_GUIManager;

    public Camera m_Camera;
    public CameraControl m_CameraControl;

    private Pawn[] pawns;



    private void Awake()
    {

        SearchDestroyCopySingletonOrThisCreateInstance();

        LoadAndinitialize();

    }

    private void Start()
    {

        ChangeGameMode(GameMode.Shop);

    }

    private void Update()
    {

        CheckStartGame();

        KeyBoardHack();

    }

    public void ChangeGameMode(GameMode m_GameMode)
    {
        
        CurrentGameMode = m_GameMode;

        changeGameModeEvent();

        switch (m_GameMode)
        {

            case GameMode.Shop:

                break;

            case GameMode.Game:
                PlayerControllerOnOff(true);
                break;

            case GameMode.PauseGame:

                break;

            case GameMode.Loose:

                break;

            case GameMode.Winner:

                break;

        }

    }

    private void LoadAndinitialize()
    {

        Physics.gravity = Vector3.zero;

        m_GUIManager = FindObjectOfType<GUIManager>();
        pawns = FindObjectsOfType<Pawn>();

        m_Camera = Camera.main;
        m_CameraControl = m_Camera.GetComponent<CameraControl>();

        PlayerControllerOnOff(false);

    }

    private void SearchDestroyCopySingletonOrThisCreateInstance()
    {

        if (Instance)
        {

            DestroyImmediate(gameObject);
            return;

        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

    }

    /// <summary>
    /// Включение и отключение контроля камерой мышкой. 
    /// В выключеном состоянии, камера статична
    /// </summary>
    /// <param name="isOn">Включен контроль над камерой или нет.</param>
    private void PlayerControllerOnOff(bool isOn)
    {

        m_CameraControl.enabled = isOn;        

    }

    /// <summary>
    /// Проверяем заполнены ли все слоты кораблей в режиме установки оружия и слотов. 
    /// И если всё установлено, то запускаем режим перестрелки.
    /// </summary>
    private void CheckStartGame()
    {

        if (CurrentGameMode == GameMode.Shop)
        {

            int i = 0;

            foreach (var pawn in pawns)
            {

                if (pawn.GetWeaponSlots().Length == pawn.GetShipsDesingSO().weaponsSlot) i++;
                if (pawn.GetModuleSlots().Length == pawn.GetShipsDesingSO().moduleSlot) i++;

            }

            if (i == pawns.Length * 2) ChangeGameMode(GameMode.Game);

        }

    }

    private void KeyBoardHack()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Alpha1)) ChangeGameMode(GameMode.Game);
        if (Input.GetKeyDown(KeyCode.Alpha2)) ChangeGameMode(GameMode.Loose);
        if (Input.GetKeyDown(KeyCode.Alpha3)) ChangeGameMode(GameMode.Winner);
        */
        if (Input.GetKeyDown(KeyCode.Escape)) SceneManager.LoadScene(0);

    }

    public void RestartScene()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

}
