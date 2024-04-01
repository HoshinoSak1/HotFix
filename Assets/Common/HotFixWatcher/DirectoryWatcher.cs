using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// 文件夹内容变更监听
/// </summary>
public class DirectoryWatcher 
{
    /// <summary>
    /// DirectoryWatcher构造函数
    /// </summary>
    /// <param name="dirPath">需要监控的文件夹</param>
    /// <param name="changeHandler">执行监听的.net监听器</param>
    /// 

    
    public DirectoryWatcher(string dirPath,FileSystemEventHandler changeHandler)
    {
        Debug.Log("正在监听 ： " +  dirPath);
        CreateWatch(dirPath, changeHandler);
    }

    /// <summary>
    /// 创建监视器
    /// </summary>
    /// <param name="dirPath">文件路径</param>
    /// <param name="changeHandler"></param>
    /// <param name="renameHandler"></param>
    /// <param name="createHandler"></param>
    /// <param name="deleteHandler"></param>
    void CreateWatch(string dirPath, FileSystemEventHandler changeHandler)
    {
        if(!Directory.Exists(dirPath))
        {
            Debug.LogError("监听文件夹不存在!");
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

        //包含子目录
        watcher.IncludeSubdirectories = true;
        
        //只处理.lua文件，过滤可能的.meta
        watcher.Filter = "*.lua";

        //开启watcher
        watcher.EnableRaisingEvents = true;

        //设置缓冲区
        watcher.InternalBufferSize = 4096;
    }

    
}
