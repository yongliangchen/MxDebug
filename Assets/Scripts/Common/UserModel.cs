using UnityEngine;

/// <summary>用户数据</summary>
public class UserModel
{

    #region 选中的消息ID

    ///// <summary>选中的消息ID</summary>
    //public static string SelectionMsgId { get; set; }

    private static int selectionIndex = -1;
    /// <summary>选中的消息ID</summary>
    public static int SelectionIndex
    {
        get
        {
            return selectionIndex;
        }
        set
        {
            selectionIndex = value;
        }
    }

    #endregion

    #region 折叠状态

    /// <summary>折叠状态</summary>
    public static CollapseState CollapseState { get; set; }

    #endregion
}
