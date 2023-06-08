using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections;

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

    public PlayerController PlayerController;
    [SerializeField] private GameObject _hudPanel;
    [SerializeField] private GameObject _mainPanel;
    [SerializeField] private GameObject _optionsPanel;
    [SerializeField] private GameObject _mainMenuPanel;
    [SerializeField] private GameObject _quitGamePanel;
    [SerializeField] private GameObject _deathPanel;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private Slider _powerSlider;
    [SerializeField] private TextMeshProUGUI _goldValue;
    [SerializeField] private TextMeshProUGUI _killValue;
    [SerializeField] private TextMeshProUGUI _eggValue;
    [SerializeField] private TextMeshProUGUI _waveValue;
    [SerializeField] private TextMeshProUGUI _gameInstructionText;
    [SerializeField] private TextMeshProUGUI _gameInstructionTitle;
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
                    if (GameManager.Victory)
                    {
                        OpenMainMenuScene();
                        break;
                    }

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
        _healthSlider.value = PlayerController.Health / PlayerController.MaxHealth;
        _powerSlider.value = PlayerController.Power / PlayerController.MaxPower;
        _goldValue.text = PlayerController.Gold.ToString();
        _waveValue.text = _gameManager.CurrentWave.ToString() + " / " + _gameManager.FinalWave.ToString();
        _killValue.text = _gameManager.KillCount.ToString();
        _eggValue.text = PlayerController.Eggs.ToString();
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

    public void OpenShopPanel()
    {
        
    }

    public void SetGameInstruction(string title, string text, int duration, bool isVictoryInstruction = false)
    {
        _gameInstructionText.text = text;
        _gameInstructionTitle.text = title;
        if (duration > 0)
        {
            StartCoroutine(GameInstructionFade(text, duration, isVictoryInstruction));
        }
    }

    IEnumerator GameInstructionFade(string text, int duration, bool isVictoryInstruction = false)
    {
        int gameInstructionTimeCounter = 0;
        if (isVictoryInstruction)
        {
            while (gameInstructionTimeCounter < duration)
            {
                _gameInstructionText.text = text.Replace("|", (duration - gameInstructionTimeCounter).ToString());
                gameInstructionTimeCounter++;
                yield return new WaitForSeconds(1f);
            }
        }
        else
        {
            yield return new WaitForSeconds((float)duration);
        }
        
        _gameInstructionText.text = string.Empty;
        _gameInstructionTitle.text = string.Empty;

        if (isVictoryInstruction)
        {
            OpenMainMenuScene();
        }
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
