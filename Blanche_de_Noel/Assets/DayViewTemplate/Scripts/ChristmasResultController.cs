﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChristmasResultController : MonoBehaviour
{
    DayController dayController;
    GameObject messageText;

    float p_move = 0.0f;
    float x = 0.0f;

    float fadeOpacity = 0.0f;
    float snowOpacity = 0.0f;
    public AudioClip presentSet;

    GameObject Present;
    GameObject Present_O;
    GameObject PresentContain;
    GameObject SnowEffect;
    GameObject Fader;

    // Start is called before the first frame update
    void Start()
    {
        dayController = GameObject.Find("DayController").GetComponent<DayController>();
        messageText = GameObject.Find("Canvas/MessageText");

        Present = GameObject.Find("PresentBox");
        Present_O = GameObject.Find("PresentBox_O");
        //PresentContain = GameObject.Find("Benz");

        Fader = GameObject.Find("Fader");
        SnowEffect = GameObject.Find("Snow");
        Material fade = Fader.GetComponent<Renderer>().material;
        Material snow = SnowEffect.GetComponent<Renderer>().material;

        fade.color = new Color(0.0f, 0.0f, 0.0f, fadeOpacity);
        snow.color = new Color(0.0f, 0.0f, 0.0f, snowOpacity);

        Present_O.SetActive(false);
        //PresentContain.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void EnableNextButton() => dayController.EnableNextButton();
    void DisableNextButton() => dayController.DisableNextButton();

    void SimpleMessage(string m) => dayController.SimpleMessage(m);

    void present_move()
    {
        Present.gameObject.transform.Translate(0.0f, p_move, 0.0f);
        Debug.Log("Presentのxは" + Present.transform.position.y);
        if (Present.transform.position.y > 0.0f)
        {
            x = x + 0.001f;
            p_move = p_move - x * x;
        }
        else if (Present.transform.position.y < 0.0f)
        {
            p_move = 0.0f;
            EnableNextButton();
        }
    }

    void present_open()
    {
        if (Present.transform.position.y < 2.0f)
        {
            Present.gameObject.transform.Translate(0.0f, 0.2f, 0.0f);
        }
        else if (Present.transform.position.y >= 2.0f)
        {
            Present.SetActive(false);
            Present_O.SetActive(true);
            if (Present_O.transform.position.y >= 0.0f)
            {
                Present_O.gameObject.transform.Translate(0.0f, -0.2f, 0.0f);
            }
            EnableNextButton();
        }
    }

    void ChristmasDayFadeIn()
    {
        messageText.SetActive(false);
        if (snowOpacity <= 1.0f)
        {
            fadeOpacity = 1.0f;
            SnowEffect.GetComponent<Renderer>().material.color = new Color(255, 255, 255, snowOpacity);
            Fader.GetComponent<Renderer>().material.color = new Color(0, 0, 0, fadeOpacity);
            snowOpacity = snowOpacity + 0.01f;
        }
        else if (snowOpacity >= 1.0f)
        {
            snowOpacity = 1.0f;
            EnableNextButton();
        }
    }

    void ChristmasDayFadeOut()
    {
        if ((snowOpacity >= 0.0f) && (fadeOpacity >= 0.0f))
        {
            SnowEffect.GetComponent<Renderer>().material.color = new Color(255, 255, 255, snowOpacity);
            Fader.GetComponent<Renderer>().material.color = new Color(0, 0, 0, fadeOpacity);
            snowOpacity = snowOpacity - 0.01f;
            fadeOpacity = fadeOpacity - 0.01f;
        }
        else if ((snowOpacity <= 0.0f) && (fadeOpacity <= 0.0f))
        {
            snowOpacity = 0.0f;
            fadeOpacity = 0.0f;
            EnableNextButton();
        }
    }

    public void ChristmasDayResult()
    {
        switch (dayController.GetTurn())
        {
            case 0:
                DisableNextButton();
                ChristmasDayFadeIn();
                break;
            case 1:
                DisableNextButton();
                ChristmasDayFadeOut();
                break;
            case 2:
                messageText.SetActive(true);
                SimpleMessage("お母さん「今日はクリスマスだね。」");
                break;
            case 3:
                if(GAMEMAIN.GetKoukando() > 0)
                {
                    SimpleMessage("お母さん「最近いい子にしてたから…」");
                } else
                {
                    SimpleMessage("お母さん「最近はあまりいい子じゃなかったけど…」");
                }
                break;
            case 4:
                SimpleMessage("お母さん「サンタさんからプレゼントが届いたわよ！」");
                break;
            case 5:
                DisableNextButton();
                present_move();
                break;
            case 6:
                DisableNextButton();
                present_open();
                break;
            case 7:
                // SimpleMessage("ベンツだ！！");
                // GameObject.Find("Gradation").GetComponent<GradationFadeController>().FadeScreenTo(50);
                SceneManager.LoadScene("ResultScene");
                break;
            default:
                SimpleMessage("【エラー】このターンに何も割り当てられていません");
                break;
        }
    }
}
