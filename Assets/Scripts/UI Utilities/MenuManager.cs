using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
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

    [SerializeField] private GameObject _titlePanel;
    [SerializeField] private GameObject _mainPanel;
    [SerializeField] private GameObject _optionsPanel;
    [SerializeField] private GameObject _playGamePanel;
    [SerializeField] private GameObject _itemsPanel;
    [SerializeField] private GameObject _quitGamePanel;
    private float _timeElapsed = 0f;

    public PanelType ActivePanel { get; private set; }

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

    private void Start()
    {
        OpenTitlePanel();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (PlayerPrefs.HasKey("LoadTitleScreen"))
        {
            if (PlayerPrefs.GetInt("LoadTitleScreen") == 1)
            {
                OpenMainPanel();
                PlayerPrefs.SetInt("LoadTitleScreen", 0);
            }
        }
        
        _mainPanel.transform.Find("VersionText").GetComponent<TextMeshProUGUI>().text = Application.version;
    }

    private void Update()
    {
        // Hide or show mouse
        if (Mouse.current.delta.ReadValue().magnitude > 0.1f)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            _timeElapsed = 0f;
        }
        else
        {
            if (_timeElapsed > 3f)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                _timeElapsed += Time.deltaTime;
            }
        }

        switch (ActivePanel)
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
                }

                if (Input.anyKeyDown && !EventSystem.current.currentSelectedGameObject)
                {
                    SelectUIObject();
                }
                break;
            case PanelType.OptionsPanel:
            case PanelType.PlayGamePanel:
            case PanelType.ItemsPanel:
            case PanelType.QuitGamePanel:
                if (Input.GetButtonDown("Cancel"))
                {
                    OpenMainPanel();
                }
                break;
        }
    }

    #region NAVIGATION FUCNTIONS

    public void OpenPlayGamePanel()
    {
        ActivePanel = PanelType.PlayGamePanel;
        SetPanelVisible();
    }

    public void OpenOptionsPanel()
    {
        ActivePanel = PanelType.OptionsPanel;
        SetPanelVisible();
    }

    public void OpenItemsPanel()
    {
        ActivePanel = PanelType.ItemsPanel;
        SetPanelVisible();
    }

    public void OpenMainPanel()
    {
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

    public void OpenQuitGamePanel()
    {
        ActivePanel = PanelType.QuitGamePanel;
        SetPanelVisible();
    }

    public void OpenTitlePanel()
    {
        ActivePanel = PanelType.TitlePanel;
        SetPanelVisible();
    }

    private void SetPanelVisible()
    {
        _titlePanel.SetActive(false);
        _mainPanel.SetActive(false);
        _optionsPanel.SetActive(false);
        _playGamePanel.SetActive(false);
        _itemsPanel.SetActive(false);
        _quitGamePanel.SetActive(false);

        switch (ActivePanel)
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
