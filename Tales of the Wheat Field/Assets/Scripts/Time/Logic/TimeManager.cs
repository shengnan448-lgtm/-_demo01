using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    int gameSecond, gameMinute, gameHour, gameDay, gameMonth, gameYear;
    Season gameSeason = Season.春天;
    int monthInSeason = 3;
    [Header("是否控制时间暂停")] 
    public bool gameClockPause;//控制是否开关
    float tikTime;
    private void Awake()
    {
        NewGameInit();
    }
    private void Start()
    {
        EventHandler.CallGameMinuteEvent(gameMinute, gameHour);
        EventHandler.CallGameDateEvent(gameHour, gameDay, gameMonth, gameYear, gameSeason);
    }
    private void Update()
    {
        tikTime += Time.deltaTime;
        if(tikTime>Settings.secondThreshold)
        {
            tikTime -= Settings.secondThreshold;
            UpdateGameTime();
        }
        //时间控制作弊器
        if(Input.GetKeyDown(KeyCode.T))
        {
            for (int i = 0; i < 60; i++)
            {
                UpdateGameTime();
            }
        }
    }
    void NewGameInit()
    {
        gameSecond = 0;
        gameMinute =0;
        gameHour = 7;
        gameDay = 1;
        gameMonth = 1;
        gameYear = 2026;
    }
    private void UpdateGameTime()
    {
        gameSecond++;
        if(gameSecond>Settings.secondThreshold)
        {
            gameSecond = 0;
            gameMinute++;
            if(gameMinute>Settings.minuteHold)
            {
                gameMinute = 0;
                gameHour++;
                if(gameHour>Settings.hourHold)
                {
                    gameDay++;
                    gameHour = 0;
                    if(gameDay>Settings.dayHold)
                    {
                        gameDay = 1;
                        gameMonth++;
                        if (gameMonth > 12)
                            gameMonth = 1;
                        monthInSeason--;
                        if(monthInSeason==0)
                        {
                            monthInSeason = 3;
                            int seasonNumber = (int)gameSeason;
                            seasonNumber++;
                            if(seasonNumber>3)
                            {
                                gameYear++;
                                seasonNumber = 0;
                            }
                            gameSeason = (Season)seasonNumber;
                            if(gameYear>9999)
                            {
                                gameYear = 2026;
                            }
                        }
                    }
                }
                    EventHandler.CallGameDateEvent(gameHour, gameDay, gameMonth, gameYear, gameSeason);
            }
            EventHandler.CallGameMinuteEvent(gameMinute, gameHour);
        } 
    }
}
