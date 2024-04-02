using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using XLua;
using static LuaFileWathcer;
using Random = UnityEngine.Random;

public class EmailTester : MonoBehaviour
{

    private static EmailTester instance;

    public Action<EmailInfo> EmailManagerLister;

    [CSharpCallLua]
    public delegate void EmailGetDelegate(EmailInfo item);
    public event EmailGetDelegate getNewItem;

    public static bool isGameOver = false;

    public static EmailTester Instance
    {
        get
        {
            if (instance == null&& !isGameOver)
            {
                var go = new GameObject("EmailTester");
                instance = go.AddComponent<EmailTester>();
            }
            return instance;
        }
    }

    public  List<EmailInfo> emailInfos = new List<EmailInfo>();

    public int count = 0;

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        
        if (Input.GetKeyUp(KeyCode.B))
        {
            CreateNewEmail();
        }
    }

    private void Init()
    {
        for (int i = 0; i < Random.Range(5, 5); i++)
        {
            count++;
            if (i % 3 == 0)
            {
                emailInfos.Add(new EmailInfo(i, 0, string.Format("《望月》预约及社媒关注奖励 {0}", i),
                    string.Format("EmailTester{0}", i), "只是测试而已", System.DateTime.Now.AddMinutes(Random.Range(-1000, 1000)), "2000000", "", EmailStatus.haveCheck));
            }
            else if (i % 3 == 1)
            {
                emailInfos.Add(new EmailInfo(i, 0, string.Format("邮件标题测试{0}", i),
                string.Format("EmailTester{0}", i), "邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题", System.DateTime.Now.AddMinutes(Random.Range(-1000, 1000)), "2000000", "1|2|3", EmailStatus.haveItem));
            }
            //else if (i % 3 == 1)
            //{
            //    emailInfos.Add(new EmailInfo(i, 0, string.Format("邮件标题测试{0}", i),
            //    string.Format("EmailTester{0}", i), "邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题邮件标题", System.DateTime.Now.AddMinutes(Random.Range(-1000, 1000)), "2000000", "", EmailStatus.haveCheck));
            //}
            else
            {
                emailInfos.Add(new EmailInfo(i, 0, string.Format("邮件标题测试{0}", i),
               string.Format("EmailTester{0}", i), "只是测试而已", System.DateTime.Now.AddMinutes(Random.Range(-1000, 1000)), "2000000", "", EmailStatus.Finish));
            }
        }
        DontDestroyOnLoad(this);

    }
    public void CreateNewEmail()
    {
        EmailStatus status = (EmailStatus)((count + Random.Range(0, 3)) % 3);
        EmailInfo newEmail = new EmailInfo();
        if (status == EmailStatus.haveItem)
        {
            newEmail = new EmailInfo(count++, 0, string.Format("邮件标题测试 道具附加邮件{0}", count),
            string.Format("EmailTester{0}", count), "只是测试而已", System.DateTime.Now, Random.Range(5, 100).ToString(), "1|2|3", status);
        }


        if (status == EmailStatus.haveCheck)
        {
            newEmail = new EmailInfo(count++, 0, string.Format("邮件标题测试 只读邮件{0}", count),
            string.Format("EmailTester{0}", count), "只是测试而已", System.DateTime.Now, Random.Range(5, 100).ToString(), "1|2|3", status);
        }

        if (status == EmailStatus.Finish)
        {
            newEmail = new EmailInfo(count++, 0, string.Format("邮件标题测试 已读邮件{0}", count),
            string.Format("EmailTester{0}", count), "只是测试而已", System.DateTime.Now, Random.Range(5, 100).ToString(), "1|2|3", status);
        }

        emailInfos.Add(newEmail);
        //EmailManagerLister?.Invoke(newEmail);
        getNewItem(newEmail);
    }
    private void OnDisable()
    {
        getNewItem = null;
        isGameOver = true;
    }
}
