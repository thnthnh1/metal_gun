using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class MasterInfo : Singleton<MasterInfo>
{
    private sealed class _GetData_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal float _elapsedTime___0;

        internal int _platform___0;

        internal string _url___0;

        internal MasterInfo _this;

        internal object _current;

        internal bool _disposing;

        internal int _PC;

        object IEnumerator<object>.Current
        {
            get
            {
                return this._current;
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return this._current;
            }
        }

        public _GetData_c__Iterator0()
        {
        }

        public bool MoveNext()
        {
            uint num = (uint)this._PC;
            this._PC = -1;
            switch (num)
            {
                case 0u:
                    this._elapsedTime___0 = 0f;
                    this._platform___0 = 0;
                    this._url___0 = string.Format("{0}?platform={1}&version={2}", this._this.webServiceUrl, this._platform___0, this._this.version);
//                    UnityEngine.Debug.Log(this._url___0);
                    this._this.www = UnityWebRequest.Get(this._url___0);
                    this._this.www.SetRequestHeader("Content-Type", "application/json");
                    this._this.www.Send();
                    this._this.isWaitingResponse = true;
                    this._this.timeStartFetchData = Time.realtimeSinceStartup;
                    break;
                case 1u:
                    break;
                default:
                    return false;
            }
            if (!this._this.www.isDone)
            {
                this._elapsedTime___0 += Time.deltaTime;
                if (this._elapsedTime___0 < 10f)
                {
                    this._current = null;
                    if (!this._disposing)
                    {
                        this._PC = 1;
                    }
                    return true;
                }
            }
            if (string.IsNullOrEmpty(this._this.www.error) && this._this.www.isDone)
            {
                try
                {
                    // UnityEngine.Debug.Log(this._this.www.downloadHandler.text);
                    MasterInfoResponse masterInfoResponse = JsonConvert.DeserializeObject<MasterInfoResponse>(this._this.www.downloadHandler.text);
                    if (this._this.response != null)
                    {
                        if (this._this.response.data.dateTime < masterInfoResponse.data.dateTime)
                        {
                            this._this.response = masterInfoResponse;
                            this._this.timeFetchedData = Time.realtimeSinceStartup;
                        }
                    }
                    else
                    {
                        this._this.response = masterInfoResponse;
                        this._this.timeFetchedData = Time.realtimeSinceStartup;
                    }
                    float num2 = this._this.timeFetchedData - this._this.timeStartFetchData;
                    this._this.response.data.dateTime.AddSeconds((double)num2);
                    this._this.IsDataFetched = true;
                    this._this.ProcessCallbacks(this._this.response);
                }
                catch (Exception var_3_23F)
                {
                    this._this.ProcessCallbacks(null);
                }
            }
            else
            {
                this._this.ProcessCallbacks(null);
            }
            this._this.isWaitingResponse = false;
            this._PC = -1;
            return false;
        }

        public void Dispose()
        {
            this._disposing = true;
            this._PC = -1;
        }

        public void Reset()
        {
            throw new NotSupportedException();
        }
    }

    [SerializeField]
    private int version = 1;

    [SerializeField]
    private string webServiceUrl;

    private MasterInfoResponse response;

    private UnityWebRequest www;

    private float timeFetchedData;

    private float timeStartFetchData;

    private bool isWaitingResponse;

    private bool _IsDataFetched_k__BackingField;

    private Stack<UnityAction<MasterInfoResponse>> waitingCallbacks = new Stack<UnityAction<MasterInfoResponse>>();

    public bool IsDataFetched
    {
        get;
        private set;
    }

    public int Version
    {
        get
        {
            return this.version;
        }
    }

    private void Awake()
    {
        this.StartGetData(true, delegate (MasterInfoResponse data)
        {
            TimeSpan tournamentTimeleft = this.GetTournamentTimeleft();
        });
        UnityEngine.Object.DontDestroyOnLoad(this);
    }

    public void StartGetData(bool forceRenew = false, UnityAction<MasterInfoResponse> callback = null)
    {
        if (callback != null)
        {
            this.waitingCallbacks.Push(callback);
        }
        if (!forceRenew && this.response != null)
        {
            this.ProcessCallbacks(this.response);
            return;
        }
        if (!this.isWaitingResponse)
        {
            base.StartCoroutine(this.GetData());
        }
    }

    public DateTime GetCurrentDateTime()
    {
        if (this.response == null)
        {
            return DateTime.Now;
        }
        DateTime dateTime = this.response.data.dateTime;
        dateTime.AddSeconds((double)(Time.realtimeSinceStartup - this.timeFetchedData));
        return dateTime;
    }

    public string GetWeekRangeString(DateTime date)
    {
        int num = (date.DayOfWeek != DayOfWeek.Sunday) ? (date.DayOfWeek - DayOfWeek.Monday) : 6;
        DateTime dateTime = date.AddDays((double)(-(double)num));
        DateTime dateTime2 = dateTime.AddDays(6.0);
        return string.Format("{0:00}{1:00}{2:00}{3:00}{4}", new object[]
        {
            dateTime.Day,
            dateTime.Month,
            dateTime2.Day,
            dateTime2.Month,
            dateTime2.Year
        });
    }

    public string GetCurrentWeekRangeString()
    {
        return this.GetWeekRangeString(this.GetCurrentDateTime());
    }

    public string GetPreviousWeekRangeString()
    {
        DateTime dateTime = this.GetCurrentDateTime().AddDays(-7.0);
        int num = (dateTime.DayOfWeek != DayOfWeek.Sunday) ? (dateTime.DayOfWeek - DayOfWeek.Monday) : 6;
        DateTime dateTime2 = dateTime.AddDays((double)(-(double)num));
        DateTime dateTime3 = dateTime2.AddDays(6.0);
        return string.Format("{0:00}{1:00}{2:00}{3:00}{4}", new object[]
        {
            dateTime2.Day,
            dateTime2.Month,
            dateTime3.Day,
            dateTime3.Month,
            dateTime3.Year
        });
    }

    public double GetTournamentTimeleftInSecond()
    {
        return this.GetTournamentTimeleft().TotalSeconds;
    }

    public TimeSpan GetTournamentTimeleft()
    {
        DateTime currentDateTime = this.GetCurrentDateTime();
        int num = (currentDateTime.DayOfWeek != DayOfWeek.Sunday) ? (currentDateTime.DayOfWeek - DayOfWeek.Monday) : 6;
        DateTime dateTime = currentDateTime.AddDays((double)(6 - num));
        dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 23, 59, 59);
        return TimeSpan.FromTicks(dateTime.Ticks - currentDateTime.Ticks);
    }

    public void CountDownTimer(TimeSpan t, out int days, out int hours, out int minutes, out int seconds)
    {
        days = t.Days;
        hours = t.Hours;
        minutes = t.Minutes;
        seconds = t.Seconds;
    }

    public void CountDownTimer(TimeSpan t, out int hours, out int minutes, out int seconds)
    {
        hours = t.Hours;
        minutes = t.Minutes;
        seconds = t.Seconds;
    }

    public void CountDownTimer(TimeSpan t, out int minutes, out int seconds)
    {
        minutes = t.Minutes;
        seconds = t.Seconds;
    }

    private IEnumerator GetData()
    {
        MasterInfo._GetData_c__Iterator0 _GetData_c__Iterator = new MasterInfo._GetData_c__Iterator0();
        _GetData_c__Iterator._this = this;
        return _GetData_c__Iterator;
    }

    private void ProcessCallbacks(MasterInfoResponse response)
    {
        while (this.waitingCallbacks.Count > 0)
        {
            UnityAction<MasterInfoResponse> unityAction = this.waitingCallbacks.Pop();
            unityAction(response);
        }
    }
}
