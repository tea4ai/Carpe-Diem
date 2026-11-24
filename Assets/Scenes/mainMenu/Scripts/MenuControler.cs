
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;
public class MenuControler : MonoBehaviour
{
    [Header("Volume Settings")]
    [SerializeField] private TMP_Text volumeInt = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private GameObject cfmPrompt = null;
    [SerializeField] private float defaultVolume = 0.4f;
    [Header("Gameplay Settings")]
    [SerializeField] private TMP_Text sensitivityInt = null;
    [SerializeField] private Slider sensitivitySlider = null;
    [SerializeField] private float defaultSensitivity = 8f;
    public float mainSensitivity = 8f;
    [Header("Graphics Settings")]
    [SerializeField] private Slider brightnessSlider = null;
    [SerializeField] private TMP_Text brightnessInt = null;
    [SerializeField] private float defaultBrightness = 0f;
    public Volume postProcessVolume;
    private ColorAdjustments colorAdjustments;
    private int qualityType;
    private bool isFS;
    private float brightnessLevel;
    [Header("Resolution Settings")]
    public TMP_Dropdown ResolutionDropdown = null;
    private Resolution[] resolutions;
    [Space(10)]
    [SerializeField] private TMP_Dropdown qualityDropdown = null;
    [SerializeField] private Toggle isFSToggle = null;
    [Header("Levels to load")]
    [SerializeField] private string startGame = null;
    [Header("Audio Clips")]
    [SerializeField] private AudioClip buttonClickClip;
    //[SerializeField] private string loadGame = null;
    public void startBtn()
    {
        PlayerPrefs.SetString("LastCheckpoint", "");
        PlayerPrefs.SetString("SavedGame", startGame + "#None!0");
        SceneManager.LoadScene(startGame);
    }
    public void ExitBtn()
    {
        Application.Quit();
        Debug.Log("Quiting");
    }
    public void ContinueGame()
    {
        if (PlayerPrefs.HasKey("SavedGame"))
        {
            string data = PlayerPrefs.GetString("SavedGame");
            if (!data.Contains("#") || !data.Contains("!"))
            {
                Debug.LogError("Save data is formatted incorrectly!");
                return;
            }

            string sceneName = data.Split('#')[0];

            int hashPos = data.IndexOf('#');
            int bangPos = data.IndexOf('!');

            string cpName = data.Substring(hashPos + 1, bangPos - hashPos - 1);
            if (cpName.Equals("None"))
            {
                return;
            }

            string enemyIndexStr = data.Substring(bangPos + 1);
            int enemyIndex = int.Parse(enemyIndexStr);

            PlayerPrefs.SetString("LastCheckpoint", cpName);
            PlayerPrefs.SetInt("StartingEnemyIndex", enemyIndex);

            SceneManager.LoadScene(sceneName);
        }
    }
    public void setVolume(float volume)
    {
        if (AudioMixerManager.instance != null)
        {
            AudioMixerManager.instance.setMasterVolume(volume);
        }
        AudioListener.volume = volume;
        volumeInt.text = (volume*100).ToString("0");
    }
    public void setSensitivity(float sensitivity)
    {
        mainSensitivity = Mathf.RoundToInt(sensitivity);
        sensitivityInt.text = sensitivity.ToString("0");
    }
    public void setResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void applyGameplay()
    {
        PlayerPrefs.SetFloat("sensitivity", mainSensitivity);
        StartCoroutine(cfmBox());
    }
    public void applyVolume()
    {
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
        StartCoroutine(cfmBox());
    }
    public void setBrightness(float brightness)
    {
        brightnessLevel = brightness;
        brightnessInt.text = (brightness).ToString();
        if (colorAdjustments != null)
        {
            colorAdjustments.postExposure.value = brightnessLevel;
        }
    }
    public void setFullScreen(bool fullScreen)
    {
        isFS = fullScreen;
        Screen.fullScreen = isFS;
    }
    public void setQuality(int quality)
    {
        qualityType = quality;
    }
    public void applyGraphics()
    {
        PlayerPrefs.SetFloat("masterBrightness", brightnessLevel);
        PlayerPrefs.SetInt("masterQuality", qualityType);
        QualitySettings.SetQualityLevel(qualityType);
        PlayerPrefs.SetInt("isFullScreen", (isFS ? 1 : 0));
        Screen.fullScreen = isFS;
        StartCoroutine(cfmBox());
    }
    public void setDefault(string menu)
    {
        if (menu == "Sound")
        {
            AudioListener.volume = defaultVolume;
            volumeSlider.value = defaultVolume;
            volumeInt.text = (defaultVolume*100).ToString("0");
            applyVolume();
        }
        if (menu == "Gameplay")
        {
            mainSensitivity = defaultSensitivity;
            sensitivitySlider.value = defaultSensitivity;
            sensitivityInt.text = defaultSensitivity.ToString("0");
            applyGameplay();
        }
        if (menu == "Graphics")
        {
            brightnessSlider.value = defaultBrightness;
            brightnessInt.text = defaultBrightness.ToString("0");
            qualityDropdown.value = 1;
            QualitySettings.SetQualityLevel(1);
            isFS = true;
            isFSToggle.isOn = true;
            Screen.fullScreen = true;
            Resolution curRes = Screen.currentResolution;
            Screen.SetResolution(curRes.width, curRes.height, Screen.fullScreen);
            ResolutionDropdown.value = resolutions.Length;
            applyGraphics();
        }
    }
    public IEnumerator cfmBox()
    {
        cfmPrompt.SetActive(true);
        yield return new WaitForSecondsRealtime(2);
        cfmPrompt.SetActive(false);
    }

    public void playSound()
    {
        if (SFXManager.Instance != null && buttonClickClip != null)
        {
            SFXManager.Instance.PlaySFX(buttonClickClip, 0.3f, transform);
        }
    }
    private void Awake()
    {
        if (postProcessVolume != null && postProcessVolume.profile != null)
        {
            if (postProcessVolume.profile.TryGet(out ColorAdjustments tmp))
            {
                colorAdjustments = tmp;
            }
        }
    }
    private void Start()
    {
        StartCoroutine(SetupResolutions()); 
    }

    private IEnumerator SetupResolutions()
    {
        yield return null;

        resolutions = Screen.resolutions;
        ResolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currRes = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currRes = i;
            }
        }

        ResolutionDropdown.AddOptions(options);
        ResolutionDropdown.value = currRes;
        ResolutionDropdown.RefreshShownValue();
    }
}