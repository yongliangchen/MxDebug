using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using System.IO;

namespace Mx.Res
{
    
    /// <summary>
    /// 删除所有的 AssetsBundle 文件
    /// </summary>
    public class DeleteAssetsBundle
    {
        
        [MenuItem("MXFramework/AssetBundle/DeleteAssetsBundle",false, 6)]
        private static void DelAssetsBundle()
        {

            string strNeedDeleteDIR = string.Empty;

            strNeedDeleteDIR = AssetDefine.GetBuildAssetOutPath();

            if(!string.IsNullOrEmpty(strNeedDeleteDIR))
            {
               
                if(Directory.Exists(strNeedDeleteDIR))
                {
                    Directory.Delete(strNeedDeleteDIR, true);

                    if(File.Exists(strNeedDeleteDIR + ".meta"))
                    {
                        File.Delete(strNeedDeleteDIR + ".meta");
                    }
                   
                    AssetDatabase.Refresh();
                }

               Debug.Log("删除所有 AssetsBundle 完成！");
            }

        }

    }


}
