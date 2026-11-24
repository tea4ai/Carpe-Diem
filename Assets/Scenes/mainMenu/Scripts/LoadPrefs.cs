using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LoadPrefs : MonoBehaviour
{
    [Header("General settings")]
    [SerializeField] private bool usable = false;
    [SerializeField] private MenuControler menuControler = null;

    [Header("Volume settings")]
    [SerializeField] private TMP_Text volumeInt = null;
    [SerializeField] private Slider volumeSlider = null;

    [Header("Brightness settings")]
    [SerializeField] private Slider brightnessSlider = null;
    [SerializeField] private TMP_Text brightnessInt = null;
    
    [Header("Quality settings")]
    [SerializeField] private TMP_Dropdown qualityDropdown = null;

    [Header("Fullscreen settings")]
    [SerializeField] private Toggle isFSToggle = null;

    [Header("Sensitivity settings")]
    [SerializeField] private TMP_Text sensitivityInt = null;
    [SerializeField] private Slider sensitivitySlider = null;
    
    [Header("Levels to load")]
    private string loadGame = null;
    [SerializeField] private Button cButton = null;

    private void Awake()
    {
        if (usable)
        {
            if (PlayerPrefs.HasKey("masterVolume"))
            {
                float localVolume = PlayerPrefs.GetFloat("masterVolume");
                volumeInt.text = localVolume.ToString();
                volumeSlider.value = localVolume;
                AudioListener.volume = localVolume;
            }
            else
                menuControler.setDefault("Sound");
            if (PlayerPrefs.HasKey("masterQuality"))
            {
                int localQuality = PlayerPrefs.GetInt("masterQuality");
                qualityDropdown.value = localQuality;
                QualitySettings.SetQualityLevel(localQuality);
            }
            if (PlayerPrefs.HasKey("isFullScreen"))
            {
                int localFS = PlayerPrefs.GetInt("isFullScreen");
                if (localFS == 1)
                {
                    Screen.fullScreen = true;
                    isFSToggle.isOn = true;
                }
                else
                {
                    Screen.fullScreen = false;
                    isFSToggle.isOn = false;
                }
            }
            if (PlayerPrefs.HasKey("masterBrightness"))
            {
                float localBrightness = PlayerPrefs.GetFloat("masterBrightness");
                brightnessInt.text = localBrightness.ToString();
                brightnessSlider.value = localBrightness;
            }
            if (PlayerPrefs.HasKey("sensitivity"))
            {
                float localSen = PlayerPrefs.GetFloat("sensitivity");
                sensitivityInt.text = localSen.ToString();
                sensitivitySlider.value = localSen;
                menuControler.mainSensitivity = localSen;
            }
        }
    }
    private void Start()
    {
        if (PlayerPrefs.HasKey("SavedGame"))
        {
            loadGame = PlayerPrefs.GetString("SavedGame");
            if (loadGame.Equals("TutorialLVL#None!0")) cButton.interactable = false;
            else cButton.interactable = true;
        }
        else
            cButton.interactable = false;
    }
}