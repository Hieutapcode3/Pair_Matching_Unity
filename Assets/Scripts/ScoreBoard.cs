using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
public class ScoreBoard : MonoBehaviour
{
    public Text[] scoreTxt_10Pairs;
    public Text[] dateTxt_10Pairs;
    
    public Text[] scoreTxt_15Pairs;
    public Text[] dateTxt_15Pairs;
    
    public Text[] scoreTxt_20Pairs;
    public Text[] dateTxt_20Pairs;

    void Start()
    {
        UpdateScoreBoard();
    }

    public void UpdateScoreBoard()
    {
        Config.UpdateScoreList();

        DisplayPairScoreData(Config.ScoreTimeList10Pairs,Config.PairNumberList10Pairs,scoreTxt_10Pairs,dateTxt_10Pairs);
        DisplayPairScoreData(Config.ScoreTimeList15Pairs,Config.PairNumberList15Pairs,scoreTxt_15Pairs,dateTxt_15Pairs);
        DisplayPairScoreData(Config.ScoreTimeList20Pairs,Config.PairNumberList20Pairs,scoreTxt_20Pairs,dateTxt_20Pairs);
    }
    private void DisplayPairScoreData(float[] scoreTimeList, string[] pairNumberList, Text[] scoreTxt, Text[] dataTxt)
    {
        for(var index =0; index < 3;index++)
        {
            if (scoreTimeList[index] > 0)
            {
                var dataTime = Regex.Split(pairNumberList[index], "T");

                var minutes = Mathf.Floor(scoreTimeList[index] / 60);
                float second  = Mathf.Floor(scoreTimeList[index] % 60);

                scoreTxt[index].text = minutes.ToString("00") + ":" + second.ToString("00");
                dataTxt[index].text = dataTime[0] + " " + dataTime[1];
            }
            else
            {
                scoreTxt[index].text = " ";
                dataTxt[index].text = " ";
            }
        }
    }
}
