using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // 需要引入

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager instance;
    public GameObject bulletImagePrefab;
    public GameObject bulletFrame;
    private List<GameObject> bulletImages = new List<GameObject>();
    public Text bulletNumText;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }
    public void SetupUI()
    {
        WeaponController.Weapon weapon = WeaponController.instance.GetWeapon();
        bulletImages.Clear();
        foreach (Transform child in bulletFrame.transform)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < weapon.bulletCount; i++)
        {
            GameObject g = Instantiate(bulletImagePrefab, bulletFrame.transform);
            bulletImages.Add(g);
        }
        bulletNumText.text = $"{weapon.bulletCount}/{weapon.bulletCount}";
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateBulletUI(int count,int total)
    {
        // 先全部禁用
        foreach (var imgObj in bulletImages)
        {
            var img = imgObj.GetComponent<Image>();
            if (img != null) img.enabled = false;
        }
        // 启用前count个
        for (int i = 0; i < count && i < bulletImages.Count; i++)
        {
            var img = bulletImages[i].GetComponent<Image>();
            if (img != null) img.enabled = true;
        }
        bulletNumText.text = $"{count}/{total}";
    }
}
