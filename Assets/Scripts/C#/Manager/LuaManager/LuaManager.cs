using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XLua;

public class LuaManager : MonoBehaviour
{
    private static LuaManager instance;

    public static LuaManager Instance
    {
        get
        {
            if (instance != null)
            {
                return instance;
            }
            instance = FindObjectOfType<LuaManager>();
            if (instance == null)
            {
                var go = new GameObject("LuaManager");
                instance = go.AddComponent<LuaManager>();
            }
            return instance;
        }
    }

    public static LuaEnv luaEnv = new LuaEnv();


    public Action luaAwake;
    public Action luaStart;
    public Action luaUpdate;
    public Action luaOnDestroy;


    public TextAsset luaScript;
    private void Awake()
    {
        luaEnv.translator.debugDelegateBridgeRelease = true;
       
        luaEnv.DoString(luaScript.text);

        luaAwake = luaEnv.Global.Get<Action>("Awake");
        luaStart = luaEnv.Global.Get<Action>("Start");
        luaUpdate = luaEnv.Global.Get<Action>("Update");
        luaOnDestroy = luaEnv.Global.Get<Action>("OnDestroy");

        //…Ë÷√»»÷ÿ‘ÿº‡Ã˝
        LuaFileWathcer.InitLuaFileWather(luaEnv);

        if (luaAwake != null)
        {
            luaAwake();
        }
    }

    private void Start()
    {
        if (luaStart != null)
        {
            luaStart();
        }
    }

    private void Update()
    {
        if (luaUpdate != null)
        {
            luaUpdate();
        }
    }

    private void OnDestroy()
    {
        luaEnv.Dispose();
    }

    private void OnDisable()
    {
        LuaFileWathcer.LuaReload = null;

        luaAwake = null;

        luaStart = null;

        luaUpdate = null;

        if (luaOnDestroy != null)
        {
            luaOnDestroy();
        }
        luaOnDestroy = null;

    }

}
