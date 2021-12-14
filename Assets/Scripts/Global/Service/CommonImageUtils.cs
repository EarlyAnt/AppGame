using AppGame.Config;
using System.Collections.Generic;
using UnityEngine;

namespace AppGame.Global
{
    class CommonImageUtils : ICommonImageUtils
    {
        [Inject]
        public IModuleConfig ModuleConfig { get; set; }
        private Dictionary<string, string> icons = new Dictionary<string, string>();

        /// <summary>
        /// 加载所有公共图片
        /// </summary>
        public void LoadCommonImages()
        {
            this.LoadAvatar();
        }
        /// <summary>
        /// 加载头像
        /// </summary>
        private void LoadAvatar()
        {
            this.icons = new Dictionary<string, string>();
            this.icons.Add("0", "app-icon-avatar-a006");
            this.icons.Add("1", "app-icon-avatar-a010");
            this.icons.Add("2", "app-icon-avatar-a004");
            this.icons.Add("3", "app-icon-avatar-a009");
            this.icons.Add("4", "app-icon-avatar-a005");
            this.icons.Add("5", "app-icon-avatar-a003");
            this.icons.Add("6", "app-icon-avatar-p0003");
            this.icons.Add("7", "app-icon-avatar-p0002");
            this.icons.Add("8", "app-icon-avatar-p0001");
            this.icons.Add("9", "app-icon-avatar-p0004");
            //this.icons.Add("10", "avatar_default");
            this.icons.Add("11", "app-icon-avatar-p0005");
            this.icons.Add("12", "app-icon-avatar-p0006");
            this.icons.Add("13", "app-icon-avatar-a001");
            this.icons.Add("14", "app-icon-avatar-a002");
            this.icons.Add("15", "app-icon-avatar-a007");
            this.icons.Add("16", "app-icon-avatar-a008");
            this.icons.Add("17", "app-icon-avatar-a011");
            this.icons.Add("18", "app-icon-avatar-a012");
            this.icons.Add("19", "app-icon-avatar-a013");
            this.icons.Add("20", "app-icon-avatar-a014");
            this.icons.Add("21", "app-icon-avatar-p0007");
            this.icons.Add("22", "app-icon-avatar-p0008");
            this.icons.Add("23", "app-icon-avatar-p0009");
            this.icons.Add("24", "app-icon-avatar-p0010");
        }
        /// <summary>
        /// 获取指定名字的头像
        /// </summary>
        /// <param name="avatarName">头像名字</param>
        /// <returns></returns>
        public string GetAvatarFileName(string avatarName)
        {
            if (this.icons != null && this.icons.ContainsKey(avatarName))
                return this.icons[avatarName];
            else
                return null;
        }
    }
}