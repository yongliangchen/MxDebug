using UnityEngine;

namespace Mx.UI
{
    public sealed class UIFormItem : MonoBehaviour
    {
        private UIParam currentUIParam = new UIParam();
        /// <summary>当前UI窗体类型</summary>
        public UIParam CurrentUIParam
        {
            get { return currentUIParam; }
            set { currentUIParam = value; }
        }

    }
}