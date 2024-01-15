using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class Cogalacak :MonoBehaviour 
{
    public List<Sorular> SorularList;
    [HideInInspector]
    public Sorular SuankiSoru;
    public Text SoruText;
    public TMP_Text PuanText;
    public TMP_Text ToplamPuanText;
    public GameObject SlotObje;

    private AudioSource audioSource;
    public AudioClip[] audioClips;

    public TMP_InputField input;

    int RandomSoru;
    int SimdikiSoruCevapPuan = 0;
    int toplamPuan = 0;
    void Awake()
    {
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        SoruVer();

    }

    void SoruVer() {

        foreach (Transform obje in this.transform)
        {
            Destroy(obje.gameObject);
        }
        RandomSoru = Random.Range(0, SorularList.Count);
        SuankiSoru.Soru = SorularList[RandomSoru].Soru;
        SuankiSoru.Cevap = SorularList[RandomSoru].Cevap;
        Debug.Log(RandomSoru);
        audioSource.clip = audioClips[RandomSoru];
        audioSource.Play();
        for (int i = 0; i < SuankiSoru.Cevap.Length; i++)
        {
            GameObject Cogalan = Instantiate(SlotObje, transform);
            Cogalan.transform.Find("SoruText").GetComponent<Text>().text = SuankiSoru.Cevap[i].ToString();
            SorularList[RandomSoru].Acilmayanlar.Add(Cogalan.transform.Find("SoruText").GetComponent<Text>());
            SoruText.text = SuankiSoru.Soru;
            Cogalan.transform.Find("SoruText").gameObject.SetActive(false);
        }
        SuankiSoru.Acilmayanlar = SorularList[RandomSoru].Acilmayanlar;
        SimdikiSoruCevapPuan = SuankiSoru.Cevap.Length * 100;
        PuanText.text = SimdikiSoruCevapPuan.ToString();


    }

    public void HarfAl() {

        if (SuankiSoru.Acilmayanlar.Count > 0) 
        {

            int RandomHarf = Random.Range(0, SuankiSoru.Acilmayanlar.Count);
            SuankiSoru.Acilmayanlar[RandomHarf].gameObject.SetActive(true);
            SuankiSoru.Acilmayanlar.RemoveAt(RandomHarf);
        }
        else {
            SorularList.RemoveAt(RandomSoru);
            Debug.Log("Kazandýnýz.");
            toplamPuan += SimdikiSoruCevapPuan;
            ToplamPuanText.text = "Toplam Puan: " + toplamPuan.ToString();
            Invoke("SoruVer", 1.15f);
        }
        SimdikiSoruCevapPuan -= 100;
        PuanText.text = SimdikiSoruCevapPuan.ToString();
    }
    public void DirekTahmin() {
        if (input.text == SuankiSoru.Cevap || input.text.ToLower() == SuankiSoru.Cevap.ToLower())
        {
            Debug.Log("Kazandýnýz.");
            toplamPuan += SimdikiSoruCevapPuan;
            ToplamPuanText.text = "Toplam Puan: " + toplamPuan.ToString();

            SorularList.RemoveAt(RandomSoru);
            foreach (Text textler in SuankiSoru.Acilmayanlar)
            {
                textler.gameObject.SetActive(true);
            }
            Invoke("SoruVer", 1.15f);
        }
        else{
            Debug.Log("Yanlýþ Tahmin.");
        }

    }

}
[System.Serializable]
public class Sorular{
    public string Soru;
    public string Cevap;
    
    public List<Text> Acilmayanlar;

    public Sorular(string soru, string cevap, List<Text> acilmayanlar)
    {
        Soru = soru;
        Cevap = cevap;
        Acilmayanlar = acilmayanlar;
    }
}
