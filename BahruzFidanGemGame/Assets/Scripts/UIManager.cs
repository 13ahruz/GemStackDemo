using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("General Panel")]
    [SerializeField] GameObject GeneralPanel;
    [SerializeField] GameObject tapToStart;
    [SerializeField] TMP_Text ourMoney;



    [Header("Settings Panel")]
    [SerializeField] GameObject SettingsPanel;
    [SerializeField] GameObject SettingsBackground;
    public GameObject musicImage;
    public GameObject soundImage;





    [Header("Win Panel")]
    [SerializeField] GameObject WinPanel;
    [SerializeField] GameObject WinPanelBackground;
    [SerializeField] List<Transform> winRingBoxes;
    [SerializeField] private GameObject get2xButton;
    [SerializeField] private GameObject getCollectedMoneyButton;
    [SerializeField] private TMP_Text winText;
    [SerializeField] private GameObject levelCompletedText;

    [Header("Intro Panel")]
    [SerializeField] GameObject IntroPanel;
    [SerializeField] GameObject IntroLoading;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        //demo value for start
        // GoldAmount = 1670;
        // PlayerPrefs.SetInt("gold",GoldAmount);
        // GemAmount = 650;
        // PlayerPrefs.SetInt("gem",GemAmount);


        // Getting Values on the Beginning

    }


    private void Start()
    {
        StartCoroutine(StartScreen());
    }


    IEnumerator StartScreen()
    {

        PlayerController.instance.SpeedMultiplier = 0;
        OpenIntroPanel();
        DOTween.To(() => IntroLoading.GetComponent<Image>().fillAmount, x => IntroLoading.GetComponent<Image>().fillAmount = x, 1, 4f);
        yield return new WaitForSeconds(4f);
        CloseIntroPanel();
        OpenGeneralPanel();
    }

    //////// General Panel ///////
    public void OpenGeneralPanel()
    {
        GeneralPanel.SetActive(true);
        tapToStart.transform.DOScale(new Vector3(3.25f, 2.243f, 2.549f), 0.5f).SetLoops(-1, LoopType.Yoyo);
    }
    public void CloseGeneralPanel()
    {
        GeneralPanel.SetActive(false);
    }

    //////// Home Panel ///////
    public void OpenIntroPanel()
    {
        IntroPanel.SetActive(true);
        DOTween.To(() => IntroLoading.GetComponent<Image>().fillAmount, x => IntroLoading.GetComponent<Image>().fillAmount = x, 1, 4);
    }
    public void CloseIntroPanel()
    {
        IntroPanel.SetActive(false);
    }


    //////// Settings Panel ///////
    public void OpenSettingsPanel()
    {
        PlayerController.instance.SpeedMultiplier = 0;
        SettingsPanel.SetActive(true);
        SettingsBackground.transform.localScale = Vector3.zero;
        Image panelImg = SettingsPanel.GetComponent<Image>();
        panelImg.color = new Color(0, 0, 0, 0);
        DOTween.To(() => panelImg.color, x => panelImg.color = x, new Color32(255, 255, 255, 255), 0.3f);
        SettingsBackground.transform.DOScale(0.014f, 0.2f);
    }

    public void CloseSettingsPanel()
    {

        Image panelImg = SettingsPanel.GetComponent<Image>();
        DOTween.To(() => panelImg.color, x => panelImg.color = x, new Color32(0, 0, 0, 0), 0.3f);
        SettingsBackground.transform.DOScale(0f, 0.2f);
        SettingsPanel.SetActive(false);
        PlayerController.instance.SpeedMultiplier = 6;
    }


    //////// Win Panel ///////
    public void OpenWinPanel()
    {
        WinPanel.SetActive(true);
        Image panelImg = WinPanel.GetComponent<Image>();
        DOTween.To(() => panelImg.color, x => panelImg.color = x, new Color32(32, 32, 32, 0), 0.2f);
        WinPanelBackground.transform.DOScale(new Vector3(0.057f, 0.039f, 0.036f), 0.3f);
        DOTween.To(() => int.Parse(winText.text), x => winText.text = x.ToString(), MoneyManager.instance.moneyCount, 2);
        StartCoroutine(winRings());
        get2xButton.transform.DOScale(new Vector3(0.283f, 0.744f, 1.010f), 0.5f).SetLoops(-1, LoopType.Yoyo);
        levelCompletedText.transform.DOLocalMoveY(30.7f, 0.3f);
        levelCompletedText.transform.DORotate(new Vector3(-5.169f, -20.05f, -13.209f), 2f).SetLoops(-1, LoopType.Yoyo);

    }
    public void CloseWinPanel()
    {
        DOTween.To(() => int.Parse(winText.text), x => winText.text = x.ToString(), MoneyManager.instance.moneyCount, 2).OnComplete(() =>
        {
            Image panelImg = WinPanel.GetComponent<Image>();
            DOTween.To(() => panelImg.color, x => panelImg.color = x, new Color32(255, 255, 255, 0), 0.3f);
            WinPanelBackground.transform.DOScale(0, 0.3f);
            WinPanel.SetActive(false);
            GetMoney();
        });



    }

    IEnumerator winRings()
    {
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < MoneyManager.instance.moneyCount / 80; i++)
        {
            winRingBoxes[i].gameObject.SetActive(true);
            winRingBoxes[i].DOScale(new Vector3(0.750f, 1.105f, 1.735f), 0.3f).SetLoops(2, LoopType.Yoyo);
            yield return new WaitForSeconds(0.35f);
        }
    }

    // IEnumerator startGame()
    // {

    //     OpenIntroPanel();
    //     yield return new WaitForSeconds(4f);
    //     PlayerController.instance.SpeedMultiplier = 0;
    //     CloseIntroPanel();
    //     CloseSettingsPanel();
    //     CloseWinPanel();
    //     OpenGeneralPanel();
    // }

    public void SoundUp()
    {
        if (soundImage.GetComponent<Image>().fillAmount < 0.98f)
        {
            soundImage.GetComponent<Image>().fillAmount += 7 / 100f;
        }
    }

    public void SoundDown()
    {
        if (soundImage.GetComponent<Image>().fillAmount > 0.2f)
            soundImage.GetComponent<Image>().fillAmount -= 7 / 100f;
    }

    public void MusicUp()
    {
        if (musicImage.GetComponent<Image>().fillAmount < 0.98f)
        {
            musicImage.GetComponent<Image>().fillAmount += 7 / 100f;
        }
    }

    public void MusicDown()
    {
        if (musicImage.GetComponent<Image>().fillAmount > 0.2f)
            musicImage.GetComponent<Image>().fillAmount -= 7 / 100f;
    }

    public void GetMoney()
    {
        DOTween.To(() => int.Parse(ourMoney.text), x => MoneyManager.instance.moneyCount = x, int.Parse(winText.text), 2).OnComplete(() =>
        {
            SceneManager.LoadScene(0);
        });

    }

    public void TapToStart()
    {
        tapToStart.SetActive(false);
        PlayerController.instance.SpeedMultiplier = 6;
    }


}









