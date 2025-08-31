using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public Text levelText;
    public Transform hpWhite;
    public Transform hpRed;
    public Text hpText;
    public Transform expBar;
    public GameObject LevelUpBox;
    public BuffTemplate selectedBuff;
    public GameObject BuffIconPrefab;
    public Transform buffFrame;
    public GameObject buffPadding;
    public GameObject messagePrefab;
    public Image black;
    public string parent;
    private int n = 0;
    [System.Serializable]
    public class EffectFrame
    {
        public Image icon;
        public Text title;
        public Text description;
        public GameObject select;
    }
    public List<EffectFrame> effectFrames = new List<EffectFrame>();
    public List<BuffTemplate> currentEffects = new List<BuffTemplate>();
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        LevelUpBox.SetActive(false);
        black.DOFade(0, 0);
        black.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetHpBar(int hp,int total)
    {
        //use DOTween to set the scale of hpRed and hpWhite
        float ratio = (float)hp / total;
        hpRed.DOScaleX(ratio, 0.2f);
        hpWhite.DOScaleX(ratio, 0.5f);
        hpText.text = hp + " / " + total;
    }
    public void SetExpBar(int exp, int total)
    {
        float ratio = (float)exp / total;
        expBar.DOScaleX(ratio, 0.2f);
    }
    public void SetLevel(int level)
    {
        levelText.text = "PLAYER - LV - " + level;
    }
    public void LevelUp()
    {
        FillEffect();
        LevelUpBox.SetActive(true);
        Invoke("Pause", 0.08f);
    }
    private void Pause()
    {
        Time.timeScale = 0;
    }
    private void FillEffect()
    {
        currentEffects.Clear();
        for(int i = 0; i < 3; i++)
        {
            currentEffects.Add(BattleData.instance.buffs[Random.Range(0, BattleData.instance.buffs.Count)]);
        }
        int index = 0;
        foreach(BuffTemplate buffT in currentEffects)
        {
            effectFrames[index].icon.sprite = buffT.icon;
            effectFrames[index].title.text = buffT.name;
            effectFrames[index].description.text = buffT.description;
            index++;
        }
        SetEffect(0);
    }
    public void SetEffect(int x)
    {
        selectedBuff = currentEffects[x];
        foreach(EffectFrame ef in effectFrames)
        {
            ef.select.SetActive(false);
        }
        effectFrames[x].select.SetActive(true);
    }
    public void SelectedEffect()
    {
        Time.timeScale = 1;
        buffPadding.SetActive(false);
        LevelUpBox.SetActive(false);
        EffectController.instance.ApplyEffect(selectedBuff);
        if (selectedBuff.buff.once) return;
        if (n == 14) return;
        GameObject icon = Instantiate(BuffIconPrefab, buffFrame);
        icon.GetComponent<Image>().sprite = selectedBuff.icon;
        n++;
    }
    public void ShowMessage(Msg msg)
    {
        Debug.Log(msg.content);
        GameObject message = Instantiate(messagePrefab, transform);
        message.GetComponent<Message>().msg = msg;
    }
    public void End()
    {
        Invoke("FadeIn", 0.8f*2+3.5f);
    }
    public void FadeIn()
    {
        black.gameObject.SetActive(true);
        black.DOFade(1, 0.9f).OnComplete(() =>
        {
            SceneManager.LoadScene(parent);
        });
    }
}
