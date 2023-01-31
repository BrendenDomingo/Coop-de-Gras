using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public enum PanelType
    {
        TitlePanel,
        MainPanel,
        OptionsPanel,
        PlayGamePanel,
        ItemsPanel,
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

    [SerializeField] private GameObject _titlePanel;
    [SerializeField] private GameObject _mainPanel;
    [SerializeField] private GameObject _optionsPanel;
    [SerializeField] private GameObject _playGamePanel;
    [SerializeField] private GameObject _itemsPanel;
    [SerializeField] private GameObject _quitGamePanel;
    private PanelType _activePanel;
    private InputDirectionSelected _inputDirection;
    private bool _allowPanelNavigation = true;

    public PanelType ActivePanel { get { return _activePanel; } }

    public void OpenDevScene()
    {
        // this needs to be changed later when we have a proper scene management system
        // there is also no way to return to the title menu once we are in this scene... future work
        OpenScene(1);
    }

    private void OpenScene(int scene)
    {
        // opens a scene by ID - 0 is the title menu and 1 is dev scene currently
        SceneManager.LoadScene(scene);
    }

    void Start()
    {
        OpenTitlePanel();
        _mainPanel.transform.Find("VersionText").GetComponent<TextMeshProUGUI>().text = Application.version;
    }

    void Update()
    {
        switch (_activePanel)
        {
            case PanelType.TitlePanel:
                if (Input.anyKeyDown)
                { 
                    OpenMainPanel();
                }
                break;
            case PanelType.MainPanel:
                if (Input.GetButtonDown("Cancel"))
                {
                    OpenQuitGamePanel();
                    break;
                }
                if (InputLeft())
                {
                    OpenOptionsPanel();
                    break;
                }
                if (InputRight())
                {
                    OpenItemsPanel();
                    break;
                }
                if (InputUp())
                {
                    OpenPlayGamePanel();
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
            case PanelType.PlayGamePanel:
                if (Input.GetButtonDown("Cancel") || InputDown())
                {
                    OpenMainPanel();
                    break;
                }
                if (Input.GetButtonDown("Submit") || InputUp())
                {
                    OpenDevScene();
                }
                break;
            case PanelType.ItemsPanel:
                if (Input.GetButtonDown("Cancel") || InputLeft())
                {
                    OpenMainPanel();
                    break;
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

    #region OPEN PANEL FUCNTIONS

    public void OpenPlayGamePanel()
    {
        _activePanel = PanelType.PlayGamePanel;
        SetPanelVisible();
    }

    public void OpenOptionsPanel()
    {
        _activePanel = PanelType.OptionsPanel;
        SetPanelVisible();
    }

    public void OpenItemsPanel()
    {
        _activePanel = PanelType.ItemsPanel;
        SetPanelVisible();
    }

    public void OpenMainPanel()
    {
        _activePanel = PanelType.MainPanel;
        SetPanelVisible();
    }

    public void QuitApplication()
    {
        Application.Quit(0);

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Exit Unity editor, if previous line does not termine the application
        #endif
    }

    public void OpenQuitGamePanel()
    {
        _activePanel = PanelType.QuitGamePanel;
        SetPanelVisible();
    }

    public void OpenTitlePanel()
    {
        _activePanel = PanelType.TitlePanel;
        SetPanelVisible();
    }

    private void SetPanelVisible()
    {
        _allowPanelNavigation = false;
        _titlePanel.SetActive(false);
        _mainPanel.SetActive(false);
        _optionsPanel.SetActive(false);
        _playGamePanel.SetActive(false);
        _itemsPanel.SetActive(false);
        _quitGamePanel.SetActive(false);

        switch (_activePanel)
        {
            case PanelType.TitlePanel:
                _titlePanel.SetActive(true);
                break;
            case PanelType.MainPanel:
                _mainPanel.SetActive(true);
                break;
            case PanelType.OptionsPanel:
                _optionsPanel.SetActive(true);
                break;
            case PanelType.PlayGamePanel:
                _playGamePanel.SetActive(true);
                break;
            case PanelType.ItemsPanel:
                _itemsPanel.SetActive(true);
                break;
            case PanelType.QuitGamePanel:
                _quitGamePanel.SetActive(true);
                break;
        }
    }

    #endregion

    #region INPUT DETECTION FUNCTIONS

    private InputDirectionSelected GetInputDirection()
    {
        float moveX = Input.GetAxis("Horizontal");
        if (moveX > 0.2f)
        {
            return InputDirectionSelected.Right;
        }
        if (moveX < -0.2f)
        {
            return InputDirectionSelected.Left;
        }

        float moveY = Input.GetAxis("Vertical");
        if (moveY > 0.2f)
        {
            return InputDirectionSelected.Up;
        }
        if (moveY < -0.2f)
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