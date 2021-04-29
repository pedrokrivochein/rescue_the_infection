using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using Cinemachine;

public class MenuGame : MonoBehaviour {

    public AudioMixer audioMixer;
    public Dropdown resolutionDropdown;
    Resolution[] resolutions;
    public GameObject menu;
    public bool inMenu = true;
    public GameObject options;
    public GameObject credits;
    public GameObject panel;
    public GameObject loadingScreen;
    public Vector3 scale;

	// Use this for initialization
	void Start () {
        //GetComponent<SaveGame>().Load();
        resolutions = Screen.resolutions;

        //resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++) {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            //options.Add(option);

            //if (resolutions[i].width == Screen.currentResolution.width &&
            //resolutions[i].height == Screen.currentResolution.height);
        }

        //resolutionDropdown.AddOptions(options);
        //resolutionDropdown.value = currentResolutionIndex;
        //resolutionDropdown.RefreshShownValue();
        menu = GameObject.Find("Menu");
        inMenu = true;
        //Time.timeScale = 0.0F;
        ////Debug.Log("Time set to 0");
        Invoke("disableDisclaimer", 10.0F);
        //scale = GetComponent<RectTransform>().localScale;
        //loadingScreen.SetActive(false);
	}

	// Update is called once per frame
	void Update () {
        //if(Input.GetKeyDown(KeyCode.K)){
            //Application.LoadLevel(0);
        //}
        /*if(!inMenu){
            if(options.active){
                Cursor.lockState = CursorLockMode.None;
            }else{
                Cursor.lockState = CursorLockMode.Locked;
            }
        }*/
	}

	public void PlayGame (){
        menu.SetActive(false);
        inMenu = false;
        Time.timeScale = 1;
        panel.SetActive(true);
        //Cursor.lockState = CursorLockMode.Locked;
    }

    public void QuitGame ()
    {
        Application.Quit();
    }

    public void SetResolution (int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetVolume (float volume)
    {
        GameObject.Find("CM_ThirdCamera").GetComponent<CinemachineFreeLook>().m_XAxis.m_MaxSpeed = volume;
    }

    public void SetSensitivity (float sens)
    {
        GameObject.Find("Camera").GetComponent<CameraScript>().sensitivity = sens;
    }

    public void Options (bool open)
    {
        if(!open){
            options.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }else{
            options.SetActive(false);
            //Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void Credits (bool open)
    {
        if(!open){
            credits.SetActive(true);
        }else{
            credits.SetActive(false);
        }
    }

    public void SetQuality (int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void OnMouseDown(){
        if(gameObject.name == "Play"){
            StartCoroutine(BeginLoad(1));
        }else if(gameObject.name == "Options"){
        }else if(gameObject.name == "Credits"){
        }else if(gameObject.name == "Quit"){
            Application.Quit();
        }
    }
    public void OnMouseOver(){
        //GetComponent<RectTransform>().localScale = scale + new Vector3(1.0F, 1.0F, 1.0F);
    }

    public void OnMouseExit(){
        //GetComponent<RectTransform>().localScale = scale;
    }

    private IEnumerator BeginLoad(int i)
	{
        loadingScreen.gameObject.SetActive(true);
		AsyncOperation operation = SceneManager.LoadSceneAsync(i);

		while (!operation.isDone)
		{
			/*if(loadingScreen.transform.GetChild(0).GetComponent<Image>().color.a > 0.0F){
                Color alpha = new Color();
                alpha.a = loadingScreen.transform.GetChild(0).GetComponent<Image>().color.a - Time.deltaTime;
                loadingScreen.transform.GetChild(0).GetComponent<Image>().color = alpha;
            }else{
                Color alpha = new Color();
                alpha.a = loadingScreen.transform.GetChild(0).GetComponent<Image>().color.a + Time.deltaTime;
                loadingScreen.transform.GetChild(0).GetComponent<Image>().color = alpha;
            }*/
			yield return null;
		}

		operation = null;
		loadingScreen.gameObject.SetActive(false);
	}

    public void disableDisclaimer(){
        GameObject.Find("Disclaimer").SetActive(false);
    }
}
