using AppGame.Config;
using AppGame.Global;
//using Spine.Unity;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace AppGame.Util
{
    class I18NUtils : II18NUtil
    {
        [Inject]
        public ILanConfig LanConfig { get; set; }//语言配置工具
        [Inject]
        public II18NConfig I18NConfig { get; set; }//多语言资源文件配置表
        [Inject]
        public IFontConfig FontConfig { get; set; }//字体配置工具

        ///// <summary>
        ///// 根据语言设置动画
        ///// </summary>
        ///// <param name="spine">Spine动画组件</param>
        ///// <param name="animationName">原来的动画名称</param>
        ///// <param name="loop">是否循环</param>
        //public void SetAnimation(SkeletonGraphic spine, string animationName, bool loop = true)
        //{
        //    this.SetAnimation(spine, animationName, animationName, loop);
        //}
        ///// <summary>
        ///// 根据语言设置动画
        ///// </summary>
        ///// <param name="spine">Spine动画组件</param>
        ///// <param name="animationName">原来的动画名称</param>
        ///// <param name="defaultAnimation">默认的动画名称(如果根据键值，也就是参数animationName找不到对应语言的动画，则播放默认名称的动画)</param>
        ///// <param name="loop">是否循环</param>
        //public void SetAnimation(SkeletonGraphic spine, string animationName, string defaultAnimation, bool loop = true)
        //{
        //    if (spine == null || string.IsNullOrEmpty(animationName))
        //    {
        //        Debug.LogErrorFormat("<><AnimationUtils.SetAnimation.Parameters>spine is null: {0}, animationName is null or empty: {1}",
        //                              spine == null, string.IsNullOrEmpty(animationName));
        //        return;
        //    }

        //    string newAnimationName = this.I18NConfig.GetAnimationName(animationName);
        //    Debug.LogFormat("<><AnimationUtils.SetAnimation>AnimationName: {0}, NewAnimationName: {1}", animationName, newAnimationName);
        //    if (spine.AnimationState.Data.SkeletonData.Animations.Exists(t => t.Name == newAnimationName))
        //    {
        //        Debug.LogFormat("<><AnimationUtils.SetAnimation>Play: {0}", newAnimationName);
        //        spine.AnimationState.SetAnimation(0, newAnimationName, loop);
        //    }
        //    else if (spine.AnimationState.Data.SkeletonData.Animations.Exists(t => t.Name == defaultAnimation))
        //    {
        //        Debug.LogFormat("<><AnimationUtils.SetAnimation>Play: {0}", defaultAnimation);
        //        spine.AnimationState.SetAnimation(0, defaultAnimation, loop);
        //    }
        //}
        /// <summary>
        /// 根据语言设置文本和字体
        /// </summary>
        /// <param name="textComponent">Text组件</param>
        /// <param name="text">要显示的文本的键</param>    
        /// <param name="setFont">是否设置字体</param>
        public void SetText(Text textComponent, string text, bool setFont = false)
        {
            if (textComponent == null || string.IsNullOrEmpty(text))
            {
                Debug.LogErrorFormat("<><AnimationUtils.SetText.Parameters>textComponent is null: {0}, text is null or empty: {1}",
                                      textComponent == null, string.IsNullOrEmpty(text));
                return;
            }

            textComponent.text = this.I18NConfig.GetText(text);
            if (setFont) this.SetFont(textComponent, text);
        }
        /// <summary>
        /// 根据语言设置文本和字体
        /// </summary>
        /// <param name="textComponent">Text组件</param>
        /// <param name="text">要显示的文本的键</param>
        /// <param name="setFont">是否设置字体</param>
        /// <param name="param">文本占位符对应的内容</param>
        public void SetText(Text textComponent, string text, bool setFont = false, params object[] param)
        {
            if (textComponent == null || string.IsNullOrEmpty(text) || param == null)
            {
                Debug.LogErrorFormat("<><AnimationUtils.SetText.Parameters>textComponent is null: {0}, text is null or empty: {1}, param is null: {2}",
                                      textComponent == null, string.IsNullOrEmpty(text), param == null);
                return;
            }


            try
            {
                string newText = this.I18NConfig.GetText(text);
                textComponent.text = string.Format(newText, param);
                if (setFont) this.SetFont(textComponent, text);
            }
            catch (Exception ex)
            {
                Debug.LogErrorFormat("<><AnimationUtils.SetText.Parameters>Error: {0}", ex.Message);
            }
        }
        /// <summary>
        /// 设置字体
        /// </summary>
        /// <param name="textComponent">Text组件</param>
        /// <param name="text">要显示的文本的键</param>
        public void SetFont(Text textComponent, string text)
        {
            if (textComponent == null || string.IsNullOrEmpty(text))
            {
                Debug.LogErrorFormat("<><AnimationUtils.SetText.SetFont>textComponent is null: {0}, text is null or empty: {1}",
                                      textComponent == null, string.IsNullOrEmpty(text));
                return;
            }

            string language = AppData.Language;
            TextElement textElement = this.I18NConfig.GetTextElement(text);
            if (this.FontConfig.IsValid(textElement.FontShortName))
            {//使用配置文件中的字体设置
                string fontFullName = this.FontConfig.GetFontFullName(textElement.FontShortName);
                Debug.LogFormat("<><AnimationUtils.SetText>Font: {0}, Language: {1}", fontFullName, language);
                textComponent.font = Resources.Load<Font>(string.Format("Font/{0}", fontFullName));
            }
            else
            {//使用默认字体设置
                string fontFullName = AppGame.Config.FontConfig.DEFAULT_FONT;
                Debug.LogFormat("<><AnimationUtils.SetText>Font: {0}, Language: {1}", fontFullName, language);
                textComponent.font = Font.CreateDynamicFontFromOSFont(fontFullName, textComponent.fontSize);
            }
            textComponent.lineSpacing = AppData.Language == AppGame.Config.LanConfig.Languages.Japanese ? 0.6f : 1f;
        }
    }
}