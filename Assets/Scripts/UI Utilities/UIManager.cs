using UnityEngine;
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
        QuitGamePanel
    }

    public enum InputDirectionSelected
    {
        Left,
        Right,
        Up,
        Down,
        None
    }

    [SerializeField] private GameObject _hudPanel;
    [SerializeField] private GameObject _mainPanel;
    [SerializeField] private GameObject _optionsPanel;
    [SerializeField] private GameObject _mainMenuPanel;
    [SerializeField] private GameObject _quitGamePanel;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private Slider _powerSlider;
    [SerializeField] private TextMeshProUGUI _goldValue;
    [SerializeField] private TextMeshProUGUI _waveValue;
    private InputDirectionSelected _inputDirection;
    private bool _allowPanelNavigation = true;

    public PanelType ActivePanel { get; private set; }

    public void OpenMainMenuScene()
    {
        // this needs to be changed later when we have a proper scene management system
        // there is also no way to return to the title menu once we are in this scene... future work
        PlayerPrefs.SetInt("LoadTitleScreen", 1);
        OpenScene(0);
    }

    private void OpenScene(int scene)
    {
        // opens a scene by ID - 0 is the title menu and 1 is dev scene currently
        SceneManager.LoadScene(scene);
    }

    private void Start()
    {
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
                if (Input.GetButtonDown("Cancel") || InputUp())
                {
                    CloseAllPanels();
                    break;
                }
                if (InputLeft())
                {
                    OpenOptionsPanel();
                    break;
                }
                if (InputRight())
                {
                    OpenMainMenuPanel();
                    break;
                }
                if (InputDown())
                {
                    OpenQuitGamePanel();
                    break;
                }
                break;
            case PanelType.OptionsPanel:
                if (Input.GetButtonDown("Cancel") || InputRight())
                {
                    OpenMainPanel();
                    break;
                }
                break;
            case PanelType.MainMenuPanel:
                if (Input.GetButtonDown("Cancel") || InputLeft())
                {
                    OpenMainPanel();
                    break;
                }
                if (InputRight())
                {
                    OpenMainMenuScene();
                }
                break;
            case PanelType.QuitGamePanel:
                if (Input.GetButtonDown("Cancel") || InputLeft())
                {
                    OpenMainPanel();
                    break;
                }
                if (InputRight())
                {
                    QuitApplication();
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
    }

    #endregion

    #region OPEN PANEL FUCNTIONS

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

    public void CloseAllPanels()
    {
        GameManager.GamePaused = false;
        ActivePanel = PanelType.HudPanel;
        SetPanelVisible();
    }


    private void SetPanelVisible()
    {
        _allowPanelNavigation = false;
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
        }
    }

    #endregion

    #region INPUT DETECTION FUNCTIONS

    private InputDirectionSelected GetInputDirection()
    {
        float moveX = Input.GetAxis("Horizontal");

        moveX = moveX < 0.3f && moveX > 0f ? 0f : moveX;
        moveX = moveX > -0.3f && moveX < 0f ? 0f : moveX;

        float moveY = Input.GetAxis("Vertical");

        moveY = moveY < 0.3f && moveY > 0f ? 0f : moveY;
        moveY = moveY > -0.3f && moveY < 0f ? 0f : moveY;

        if (moveX > 0.3f)
        {
            return InputDirectionSelected.Right;
        }
        if (moveX < -0.3f)
        {
            return InputDirectionSelected.Left;
        }

        if (moveY > 0.3f)
        {
            return InputDirectionSelected.Up;
        }
        if (moveY < -0.3f)
        {
            return InputDirectionSelected.Down;
        }

        _allowPanelNavigation = true;
        return InputDirectionSelected.None;
    }

    private bool InputRight()
    {
        _inputDirection = GetInputDirection();
        bool isInputRight = _inputDirection == InputDirectionSelected.Right && _allowPanelNavigation;
        return isInputRight;
    }

    private bool InputLeft()
    {
        _inputDirection = GetInputDirection();
        bool isInputLeft = _inputDirection == InputDirectionSelected.Left && _allowPanelNavigation;
        return isInputLeft;
    }

    private bool InputUp()
    {
        _inputDirection = GetInputDirection();
        bool isInputUp = _inputDirection == InputDirectionSelected.Up && _allowPanelNavigation;
        return isInputUp;
    }

    private bool InputDown()
    {
        _inputDirection = GetInputDirection();
        bool isInputDown = _inputDirection == InputDirectionSelected.Down && _allowPanelNavigation;
        return isInputDown;
    }

    #endregion
}