using UnityEngine;
using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine.UI;

public class NtpDate : MonoBehaviour {

    private DateTime ntpDate;   // NTP同期時刻
    private float rcvAppDate;   // NTP通信時のアプリ時刻

    private IPEndPoint ipAny;
    private UdpClient sock;
    private Thread thread;
    private volatile bool threadRunning = false;
    private byte[] rcvData;

    public Text m_timeText;
    public int count;

    private DateTime startTime;
    private DateTime endTime;
    int timesec;
    int timecheck;

    bool inputcheck = false;
    // 初期化
    void Start()
    {
        
        endTime = Date;

        // リクエスト実行
        SyncDate();
        // 時刻表示(デバッグ用)
        StartCoroutine(ShowSyncDate());
    }

    // 同期時刻の表示
    private IEnumerator ShowSyncDate()
    {
        while (true)
        {
            yield return Yielders.Get(1.0f);

            if (Date == null)
            {
                Debug.Log("Time is not received.");
            }
            else
            {
                //Debug.Log("Receive date : " + Date.ToString());
                m_timeText.text = string.Format("{0}", Date.ToString());

                if (inputcheck)
                {
                    timesec--;
                }
                if (timesec == 0)
                {
                    count++;
                }
                Debug.Log("count : "+ count);
                
                Debug.Log("starTime : " + startTime);
                Debug.Log("EndTime : " + endTime);
            }
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            check();
            inputcheck = true;
        }
        
        Debug.Log("addSec : " + timesec);
    }

    void check()
    {
        startTime = Date;
        endTime = Date.AddSeconds(80);
        //endTime.AddSeconds(10);
        TimeSpan timecal = endTime - startTime;

        timesec = timecal.Seconds;
        
    }

    // アプリケーション終了時処理
    void OnApplicationQuit()
    {
        if (thread != null)
        {
            thread.Abort();
        }
        if (sock != null)
        {
            sock.Close();
        }
    }

    // 時刻同期を行う
    public void SyncDate()
    {
        // リクエスト実行
        threadRunning = true;
        thread = new Thread(new ThreadStart(Request));
        thread.Start();

        // リクエスト待機コルーチン実行
        StartCoroutine(WaitForRequest());

        Debug.Log("Thread is started.");

        
    }

    // NTPサーバに対してリクエストを実行する
    private void Request()
    {
        // ソケットを開く
        ipAny = new IPEndPoint(IPAddress.Any, 123);
        sock = new UdpClient(ipAny);

        // リクエスト送信
        byte[] sndData = new byte[48];
        sndData[0] = 0xB;
        sock.Send(sndData, sndData.Length, "ntp.jst.mfeed.ad.jp", 123);

        // データ受信
        rcvData = sock.Receive(ref ipAny);

        // 実行中フラグクリア
        threadRunning = false;
    }

    // リクエスト待機コルーチン
    private IEnumerator WaitForRequest()
    {
        // リクエスト終了まで待機
        while (threadRunning)
        {
            yield return 0;
        }

        // アプリ時刻保存
        rcvAppDate = Time.realtimeSinceStartup;

        // 受信したバイナリデータをDateTime型に変換
        ntpDate = new DateTime(1900, 1, 1);
        var high = (double)BitConverter.ToUInt32(new byte[] { rcvData[43], rcvData[42], rcvData[41], rcvData[40] }, 0);
        var low = (double)BitConverter.ToUInt32(new byte[] { rcvData[47], rcvData[46], rcvData[45], rcvData[44] }, 0);
        ntpDate = ntpDate.AddSeconds(high + low / UInt32.MaxValue);

        // UTC→ローカル日時に変換
        ntpDate = ntpDate.ToLocalTime();
    }

    // NTP同期時刻
    public DateTime Date
    {
        get
        {
            return ntpDate.AddSeconds(Time.realtimeSinceStartup - rcvAppDate);
        }
    }
}
