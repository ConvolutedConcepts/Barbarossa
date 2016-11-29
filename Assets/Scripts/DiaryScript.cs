using UnityEngine;
using System.Collections;

public class DiaryScript : MonoBehaviour {
    int page_num;
    public GameObject diaryPage0;
    public GameObject diaryPage1;
    public GameObject diaryPage2;
    public GameObject diaryPage3;
    public GameObject diaryPage4;
    public GameObject diaryPage5;

    // Use this for initialization
    void Start () {
        page_num = 0;
	}
	
    public void nextPage()
    {
        if (page_num == 0)
            loadPage1();
        else if (page_num == 1)
            loadPage2();
        else if (page_num == 2)
            loadPage3();
        else if (page_num == 3)
            loadPage4();
        else if (page_num == 4)
            loadPage5();
        else
            GameStatus.GoToLevel_1();
    }

    void loadPage1()
    {
        page_num = 1;
        diaryPage0.SetActive(false);
        diaryPage1.SetActive(true);
    }

    void loadPage2()
    {
        page_num = 2;
        diaryPage1.SetActive(false);
        diaryPage2.SetActive(true);
    }

    void loadPage3()
    {
        page_num = 3;
        diaryPage2.SetActive(false);
        diaryPage3.SetActive(true);
    }

    void loadPage4()
    {
        page_num = 4;
        diaryPage3.SetActive(false);
        diaryPage4.SetActive(true);
    }

    void loadPage5()
    {
        page_num = 5;
        diaryPage4.SetActive(false);
        diaryPage5.SetActive(true);
    }
}
