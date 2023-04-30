using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public enum PanelType
    {
        HudPanel,
        MainPanel,
        OptionsPanel,
        MainMenuPanel,
        QuitGamePanel,
        DeathPanel
    }

    [SerializeField] private GameObject _hudPanel;
    [SerializeField] private GameObject _mainPanel;
    [SerializeField] private GameObject _optionsPanel;
    [SerializeField] private GameObject _mainMenuPanel;
    [SerializeField] private GameObject _quitGamePanel;
    [SerializeField] private GameObject _deathPanel;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private Slider _powerSlider;
    [SerializeField] private TextMeshProUGUI _goldValue;
    [SerializeField] private TextMeshProUGUI _killValue;
    [SerializeField] private TextMeshProUGUI _waveValue;

    public PanelType ActivePanel { get; private set; }

    public void OpenMainMenuScene()
    {
        // this needs to be changed later when we have a proper scene management system
        // there is also no way to return to the title menu once we are in this scene... future work
        PlayerPrefs.SetInt("LoadTitleScreen", 1);
        OpenScene(0);
    }

    public void ResetCurrentScene()
    {
        OpenScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OpenScene(int scene)
    {
        // opens a scene by ID - 0 is the title menu and 1 is dev scene currently
        SceneManager.LoadScene(scene);
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        CloseAllPanels();
        _mainPanel.transform.Find("VersionText").GetComponent<TextMeshProUGUI>().text = Application.version;
    }

    private void Update()
    {
        switch (ActivePanel)
        {
            case PanelType.HudPanel:
                if (Input.GetButtonDown("Cancel"))
                {
                    OpenMainPanel();
                    break;
                }

                UpdateHUDUIComponents();

                break;
            case PanelType.MainPanel:
                if (Input.GetButtonDown("Cancel"))
                {
                    CloseAllPanels();
                    break;
                }
                break;
            case PanelType.OptionsPanel:
            case PanelType.MainMenuPanel:
            case PanelType.QuitGamePanel:
                if (Input.GetButtonDown("Cancel"))
                {
                    OpenMainPanel();
                }
                break;
        }
    }

    #region HUD FUNCTIONS

    private void UpdateHUDUIComponents()
    {
        _healthSlider.value = _playerController.Health / _playerController.MaxHealth;
        _powerSlider.value = _playerController.Power / _playerController.MaxPower;
        _goldValue.text = _playerController.Gold.ToString();
        _waveValue.text = _gameManager.CurrentWave.ToString() + " / " + _gameManager.FinalWave.ToString();
        _killValue.text = _gameManager.KillCount.ToString();
    }

    #endregion

    #region NAVIGATION FUCNTIONS

    public void OpenOptionsPanel()
    {
        ActivePanel = PanelType.OptionsPanel;
        SetPanelVisible();
    }

    public void OpenMainPanel()
    {
        GameManager.GamePaused = true;
        ActivePanel = PanelType.MainPanel;
        SetPanelVisible();
    }

    public void QuitApplication()
    {
        Application.Quit(0);

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Exit Unity editor, if previous line does not termine the application
        #endif
    }

    public void OpenMainMenuPanel()
    {
        ActivePanel = PanelType.MainMenuPanel;
        SetPanelVisible();
    }

    public void OpenQuitGamePanel()
    {
        ActivePanel = PanelType.QuitGamePanel;
        SetPanelVisible();
    }

    public void OpenDeathPanel()
    {
        GameManager.GamePaused = true;
        ActivePanel = PanelType.DeathPanel;
        SetPanelVisible();
    }

    public void CloseAllPanels()
    {
        GameManager.GamePaused = false;
        ActivePanel = PanelType.HudPanel;
        SetPanelVisible();
    }


    private void SetPanelVisible()
    {
        _deathPanel.SetActive(false);
        _mainPanel.SetActive(false);
        _optionsPanel.SetActive(false);
        _mainMenuPanel.SetActive(false);
        _quitGamePanel.SetActive(false);
        _hudPanel.SetActive(false);

        switch (ActivePanel)
        {
            case PanelType.MainPanel:
                _mainPanel.SetActive(true);
                break;
            case PanelType.OptionsPanel:
                _optionsPanel.SetActive(true);
                break;
            case PanelType.MainMenuPanel:
                _mainMenuPanel.SetActive(true);
                break;
            case PanelType.QuitGamePanel:
                _quitGamePanel.SetActive(true);
                break;
            case PanelType.HudPanel:
                _hudPanel.SetActive(true);
                break;
            case PanelType.DeathPanel:
                _deathPanel.SetActive(true);
                break;
        }

        SelectUIObject();
    }

    private void SelectUIObject()
    {
        GameObject[] uiFirstSelectable = GameObject.FindGameObjectsWithTag("UI_Navigation");
        foreach (GameObject uiObject in uiFirstSelectable)
        {
            if (uiObject.activeSelf)
            {
                EventSystem.current.SetSelectedGameObject(uiObject);
                break;
            }
        }
    }

    #endregion
}
