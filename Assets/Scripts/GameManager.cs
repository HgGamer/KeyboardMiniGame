using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject KeyCapPrefab;
    public Transform KeyCapHolder;
    public GameObject SwitchHolder;
    public Transform SpawnHolders;
    public TMP_Text timeText;

    public TMP_Text endText;
    public GameObject endScreen;

    public List<Keycap> playercaps = new List<Keycap>();
    float timer = 30;


    public void RestartGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Start()
    {
        foreach (Transform sw in SwitchHolder.transform)
        {
            var instance = Instantiate(KeyCapPrefab);
            instance.transform.SetParent(sw.transform, true);
            instance.GetComponent<Keycap>().label = sw.GetComponent<Switch>().label;
            instance.GetComponent<Keycap>().labelBL = sw.GetComponent<Switch>().labelBL;
            instance.GetComponent<Keycap>().labelBR = sw.GetComponent<Switch>().labelBR;
            instance.GetComponent<Keycap>().labelTL = sw.GetComponent<Switch>().labelTL;
            instance.GetComponent<Keycap>().labelTR = sw.GetComponent<Switch>().labelTR;
            instance.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        }
        Switch[] switches = FindObjectsOfType<Switch>();
        List<int> usedids = new List<int>();
        int spawncount = 5;
        while (spawncount > 0)
        {
            int index = Random.Range(0, switches.Length - 1);
            if (usedids.Contains(index) || switches[index].transform.childCount == 0)
            {
                continue;
            }
            else
            {
                usedids.Add(index);
                var keycap = switches[index].transform.GetChild(0).transform;
                keycap.position = SpawnHolders.GetChild(spawncount).transform.position;
                keycap.SetParent(keycap.parent.parent, true);
                keycap.localEulerAngles += new Vector3(0, 0, Random.Range(-20.0f, 20.0f));
                playercaps.Add(keycap.GetComponent<Keycap>());
                spawncount--;
            }
        }

        foreach (var sw in switches)
        {
            if (sw.transform.childCount > 0)
            {
                if (Random.Range(0, 100) > 20)
                {
                    Destroy(sw.transform.GetChild(0).gameObject);
                    //sw.transform.GetChild(0).gameObject.SetActive(false);
                }
                else
                {
                    sw.transform.GetChild(0).GetComponent<Keycap>().fixedbutton = true;
                    sw.transform.GetChild(0).GetComponent<Image>().color = new Color(1.0f, 0.8818358f, 0.5801887f);
                }
            }
        }
    }

    void EndScreen(){
        endScreen.SetActive(true);
    }
    bool gameEnd = false;
    void Update()
    {
        timeText.text=""+(int)timer;
        if (!gameEnd)
        {
            timer -= Time.deltaTime;
            if(timer<0){
                gameEnd = true;
                endText.text="Time's Up!";
                EndScreen();
                return;
            }
            //check key locations
            foreach (var key in playercaps)
            {
                if (key.label != key.transform.parent.GetComponent<Switch>()?.label)
                {
                    return;
                }
            }
            gameEnd = true;
            endText.text="Good Job!";
            EndScreen();
            Debug.Log("end");
        }else{
            Vector2 position = endScreen.GetComponent<RectTransform>().anchoredPosition;
            endScreen.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(position,Vector2.zero,Time.deltaTime);
        } 
        
    }
}
