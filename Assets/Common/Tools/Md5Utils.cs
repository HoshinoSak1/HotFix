using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

/// <summary>
/// Md5工具
/// </summary>
public static class Md5Utils
{
    /// <summary>
    /// 根据文件流得到文件的MD5
    /// </summary>
    /// <param name="stream">文件流</param>
    /// <returns></returns>
    public static string GetMD5(FileStream stream)
    {
        if (stream == null) { return null; }
        MD5 md5 = MD5.Create();
        md5 = new MD5CryptoServiceProvider();
        byte[] targetData = md5.ComputeHash(stream);
        StringBuilder stringBuilder = new StringBuilder();
        for(int i = 0;i<targetData.Length; i++)
        {
            stringBuilder.AppendFormat("{0:x2}", targetData[i]);
        }
        return stringBuilder.ToString();
    }

    /// <summary>
    /// 文件是否产生修改
    /// </summary>
    /// <param name="curMD5">当前MD5值</param>
    /// <param name="preMD5">原本MD5值</param>
    /// <returns></returns>
    public static bool isModify(string curMD5,string preMD5)
    {
        StringComparer comparer = StringComparer.OrdinalIgnoreCase;
        return comparer.Compare(curMD5, preMD5) != 0;
    }
}
