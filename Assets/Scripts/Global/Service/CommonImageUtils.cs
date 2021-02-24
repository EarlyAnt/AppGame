using AppGame.Config;
using System.Collections.Generic;
using UnityEngine;

namespace AppGame.Global
{
    class CommonImageUtils : ICommonImageUtils
    {
        [Inject]
        public IModuleConfig ModuleConfig { get; set; }
        private bool initialized = false;
        private Dictionary<string, Sprite> icons = new Dictionary<string, Sprite>();

        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize()
        {
            if (!this.initialized)
            {//每次程序启动时只加载一次公共图片
                this.initialized = true;
                SpriteHelper.Instance.LoadModuleImages(ModuleViews.Common);
            }
        }
        /// <summary>
        /// 加载所有公共图片
        /// </summary>
        public void LoadCommonImages()
        {
            if (this.initialized)
            {
                this.LoadAvatar();
            }
        }
        /// <summary>
        /// 加载头像
        /// </summary>
        private void LoadAvatar()
        {
            this.icons = new Dictionary<string, Sprite>();
            this.icons.Add("0", SpriteHelper.Instance.LoadSpriteFromBuffer(ModuleViews.Common, this.ModuleConfig.GetImagePath(ModuleViews.Common, "app-icon-avatar-a006")));
            this.icons.Add("1", SpriteHelper.Instance.LoadSpriteFromBuffer(ModuleViews.Common, this.ModuleConfig.GetImagePath(ModuleViews.Common, "app-icon-avatar-a010")));
            this.icons.Add("2", SpriteHelper.Instance.LoadSpriteFromBuffer(ModuleViews.Common, this.ModuleConfig.GetImagePath(ModuleViews.Common, "app-icon-avatar-a004")));
            this.icons.Add("3", SpriteHelper.Instance.LoadSpriteFromBuffer(ModuleViews.Common, this.ModuleConfig.GetImagePath(ModuleViews.Common, "app-icon-avatar-a009")));
            this.icons.Add("4", SpriteHelper.Instance.LoadSpriteFromBuffer(ModuleViews.Common, this.ModuleConfig.GetImagePath(ModuleViews.Common, "app-icon-avatar-a005")));
            this.icons.Add("5", SpriteHelper.Instance.LoadSpriteFromBuffer(ModuleViews.Common, this.ModuleConfig.GetImagePath(ModuleViews.Common, "app-icon-avatar-a003")));
            this.icons.Add("6", SpriteHelper.Instance.LoadSpriteFromBuffer(ModuleViews.Common, this.ModuleConfig.GetImagePath(ModuleViews.Common, "app-icon-avatar-p0003")));
            this.icons.Add("7", SpriteHelper.Instance.LoadSpriteFromBuffer(ModuleViews.Common, this.ModuleConfig.GetImagePath(ModuleViews.Common, "app-icon-avatar-p0002")));
            this.icons.Add("8", SpriteHelper.Instance.LoadSpriteFromBuffer(ModuleViews.Common, this.ModuleConfig.GetImagePath(ModuleViews.Common, "app-icon-avatar-p0001")));
            this.icons.Add("9", SpriteHelper.Instance.LoadSpriteFromBuffer(ModuleViews.Common, this.ModuleConfig.GetImagePath(ModuleViews.Common, "app-icon-avatar-p0003")));
            //this.icons.Add("10", SpriteHelper.Instance.LoadSpriteFromBuffer(ModuleViews.Common, this.ModuleConfig.GetImagePath(ModuleViews.Common, "avatar_default")));
            this.icons.Add("11", SpriteHelper.Instance.LoadSpriteFromBuffer(ModuleViews.Common, this.ModuleConfig.GetImagePath(ModuleViews.Common, "app-icon-avatar-p0005")));
            this.icons.Add("12", SpriteHelper.Instance.LoadSpriteFromBuffer(ModuleViews.Common, this.ModuleConfig.GetImagePath(ModuleViews.Common, "app-icon-avatar-p0006")));
            this.icons.Add("13", SpriteHelper.Instance.LoadSpriteFromBuffer(ModuleViews.Common, this.ModuleConfig.GetImagePath(ModuleViews.Common, "app-icon-avatar-a001")));
            this.icons.Add("14", SpriteHelper.Instance.LoadSpriteFromBuffer(ModuleViews.Common, this.ModuleConfig.GetImagePath(ModuleViews.Common, "app-icon-avatar-a002")));
            this.icons.Add("15", SpriteHelper.Instance.LoadSpriteFromBuffer(ModuleViews.Common, this.ModuleConfig.GetImagePath(ModuleViews.Common, "app-icon-avatar-a007")));
            this.icons.Add("16", SpriteHelper.Instance.LoadSpriteFromBuffer(ModuleViews.Common, this.ModuleConfig.GetImagePath(ModuleViews.Common, "app-icon-avatar-a008")));
            this.icons.Add("17", SpriteHelper.Instance.LoadSpriteFromBuffer(ModuleViews.Common, this.ModuleConfig.GetImagePath(ModuleViews.Common, "app-icon-avatar-a011")));
            this.icons.Add("18", SpriteHelper.Instance.LoadSpriteFromBuffer(ModuleViews.Common, this.ModuleConfig.GetImagePath(ModuleViews.Common, "app-icon-avatar-a012")));
            this.icons.Add("19", SpriteHelper.Instance.LoadSpriteFromBuffer(ModuleViews.Common, this.ModuleConfig.GetImagePath(ModuleViews.Common, "app-icon-avatar-a013")));
            this.icons.Add("20", SpriteHelper.Instance.LoadSpriteFromBuffer(ModuleViews.Common, this.ModuleConfig.GetImagePath(ModuleViews.Common, "app-icon-avatar-a014")));
            this.icons.Add("21", SpriteHelper.Instance.LoadSpriteFromBuffer(ModuleViews.Common, this.ModuleConfig.GetImagePath(ModuleViews.Common, "app-icon-avatar-p0007")));
            this.icons.Add("22", SpriteHelper.Instance.LoadSpriteFromBuffer(ModuleViews.Common, this.ModuleConfig.GetImagePath(ModuleViews.Common, "app-icon-avatar-p0008")));
            this.icons.Add("23", SpriteHelper.Instance.LoadSpriteFromBuffer(ModuleViews.Common, this.ModuleConfig.GetImagePath(ModuleViews.Common, "app-icon-avatar-p0009")));
            this.icons.Add("24", SpriteHelper.Instance.LoadSpriteFromBuffer(ModuleViews.Common, this.ModuleConfig.GetImagePath(ModuleViews.Common, "app-icon-avatar-p0010")));
        }
        /// <summary>
        /// 获取指定名字的头像
        /// </summary>
        /// <param name="avatarName">头像名字</param>
        /// <returns></returns>
        public Sprite GetAvatar(string avatarName)
        {
            if (this.icons != null && this.icons.ContainsKey(avatarName))
                return this.icons[avatarName];
            else
                return null;
        }
    }
}