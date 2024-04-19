namespace Params
{
    public class EditParam
    {
        /// <summary>
        /// 編輯模式
        /// </summary>
        public enum EditMode
        {
            /// <summary>
            /// 未定
            /// </summary>
            NONE = 0,

            /// <summary>
            /// 新增
            /// </summary>
            INSERT = 1,

            /// <summary>
            /// 修改
            /// </summary>
            UPDATE = 2,

            /// <summary>
            /// 刪除
            /// </summary>
            DELETE = 3
        }

    }
}
