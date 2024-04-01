using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using XLua;


[LuaCallCSharp]
public enum ReloadType
{
    Create,
    Change,
    Delete
}


/// <summary>
/// ����lua�ļ�
/// </summary>
public static class LuaFileWathcer
{
    

    //����������
    static DirectoryWatcher dirWatcher = new DirectoryWatcher(PathUtils.LuaPath,
                                                                LuaFileOnChanged);

    private static Dictionary<string,ReloadType> fileDir = new Dictionary<string,ReloadType>();


    [CSharpCallLua]
    public delegate void ReloadDelegate(string path, ReloadType reloadType);

    public static ReloadDelegate LuaReload;

    public static void DestroyLuaFileWathcer() {
        Debug.Log("        LuaFileWathcer.LuaReload == null");
        LuaReload = null;
    }


    public static void InitLuaFileWather(LuaEnv luaEnv)
    {
        LuaReload = luaEnv.Global.Get<ReloadDelegate>("LuaReload");

        //�ü�����������Editor����ʱʼ�ռ��
        EditorApplication.update -= Reload;
        EditorApplication.update += Reload;
    }

    private static void Reload()
    {
        if(EditorApplication.isPlaying==false) {
            return;
        }
        if(fileDir.Count == 0 )
        {
            return;
        }
        foreach(var file in fileDir)
        {
            //ͨ��ί����Lua�࿪ʼ����ģ���Reload
            LuaReload(PathUtils.GetRequirePath( file.Key), file.Value);    
        }
        fileDir.Clear();
    }

    /// <summary>
    /// ��������Change
    /// </summary>
    private static void LuaFileOnChanged(object sender, FileSystemEventArgs e)
    {
        switch(e.ChangeType)
        {
            case WatcherChangeTypes.Created:
                OnCreate(sender,e);
                break;
            case WatcherChangeTypes.Deleted:
                OnDelete(sender, e);
                break;
            case WatcherChangeTypes.Changed:
                OnChanged(sender, e);
                break;
        }
    }

    /// <summary>
    /// ���Lua�ļ�
    /// </summary>
    private static void OnCreate(object sender, FileSystemEventArgs e)
    {
        string luaPath = PathUtils.GetLuaPath(e.Name);
        fileDir[luaPath] = ReloadType.Create;
    }

    /// <summary>
    /// ����Lua�ļ�
    /// </summary>
    private static void OnChanged(object sender,FileSystemEventArgs e)
    {
        string luaPath = PathUtils.GetLuaPath(e.Name);
        fileDir[luaPath] = ReloadType.Change;
    }

    /// <summary>
    /// ɾ��Lua�ļ�
    /// </summary>
    private static void OnDelete(object sender, FileSystemEventArgs e)
    {
        string luaPath = PathUtils.GetLuaPath(e.Name);
        fileDir[luaPath] = ReloadType.Delete;
    }

    
}
