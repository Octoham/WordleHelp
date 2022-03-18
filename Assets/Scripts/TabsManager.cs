using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabsManager : MonoBehaviour
{

    public GameObject[] tabs;
    public GameObject[] buttons;
    public enum Tabs { Main, WordList, Settings, };
    public Tabs currentTab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject button in buttons)
        {
            if (button == buttons[(int)currentTab])
            {
                if (button.GetComponent<Image>().color != new Color(0.1953125f, 0.1953125f, 0.1953125f))
                {
                    button.GetComponent<Image>().color = new Color(0.1953125f, 0.1953125f, 0.1953125f);
                }
            }
            else
            {
                if (button.GetComponent<Image>().color != new Color(0f, 0f, 0f))
                {
                    button.GetComponent<Image>().color = new Color(0f, 0f, 0f);
                }
            }
        }
        foreach (GameObject tab in tabs)
        {
            if (tab == tabs[(int)currentTab])
            {
                if (!tab.activeInHierarchy)
                {
                    tab.SetActive(true);
                }
            }
            else
            {
                if (tab.activeInHierarchy)
                {
                    tab.SetActive(false);
                }
            }
        }
    }

    public void Click(int tabID)
    {
        currentTab = (Tabs)tabID;
    }
}
