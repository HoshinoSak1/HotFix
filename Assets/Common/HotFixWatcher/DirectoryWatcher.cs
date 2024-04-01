using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// �ļ������ݱ������
/// </summary>
public class DirectoryWatcher 
{
    /// <summary>
    /// DirectoryWatcher���캯��
    /// </summary>
    /// <param name="dirPath">��Ҫ��ص��ļ���</param>
    /// <param name="changeHandler">ִ�м�����.net������</param>
    /// 

    
    public DirectoryWatcher(string dirPath,FileSystemEventHandler changeHandler)
    {
        Debug.Log("���ڼ��� �� " +  dirPath);
        CreateWatch(dirPath, changeHandler);
    }

    /// <summary>
    /// ����������
    /// </summary>
    /// <param name="dirPath">�ļ�·��</param>
    /// <param name="changeHandler"></param>
    /// <param name="renameHandler"></param>
    /// <param name="createHandler"></param>
    /// <param name="deleteHandler"></param>
    void CreateWatch(string dirPath, FileSystemEventHandler changeHandler)
    {
        if(!Directory.Exists(dirPath))
        {
            Debug.LogError("�����ļ��в�����!");
            return;
        }


        var watcher = new FileSystemWatcher(dirPath);

        watcher.NotifyFilter = NotifyFilters.Attributes
                             | NotifyFilters.CreationTime
                             | NotifyFilters.DirectoryName
                             | NotifyFilters.FileName
                             | NotifyFilters.LastAccess
                             | NotifyFilters.LastWrite
                             | NotifyFilters.Security
                             | NotifyFilters.Size;

        watcher.Changed += changeHandler;
        watcher.Created += changeHandler;
        watcher.Deleted += changeHandler;

        //������Ŀ¼
        watcher.IncludeSubdirectories = true;
        
        //ֻ����.lua�ļ������˿��ܵ�.meta
        watcher.Filter = "*.lua";

        //����watcher
        watcher.EnableRaisingEvents = true;

        //���û�����
        watcher.InternalBufferSize = 4096;
    }

    
}
