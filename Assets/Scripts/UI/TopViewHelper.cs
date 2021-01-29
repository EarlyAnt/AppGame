using AppGame.Util;
using strange.extensions.signal.impl;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TopViewHelper : Singleton<TopViewHelper>
{
    /************************************************属性与变量命名************************************************/
    #region 变量与属性命名
    public ModuleViews TopView
    {
        get
        {
            if (this.topViewList == null || this.topViewList.Count == 0)
                return ModuleViews.None;
            return this.topViewList[this.topViewList.Count - 1];
        }
    }
    public const string ADD_VIEW = "AddView";
    public const string REMOVE_VIEW = "RemoveView";
    public const string RESET_VIEW = "ResetView";
    private List<ModuleViews> topViewList = new List<ModuleViews>();//普通页面集合
    private Dictionary<ModuleViews, bool> silenceViewList = new Dictionary<ModuleViews, bool>();//静默页面集合
    private Signal<string, ModuleViews, ModuleViews> topViewChangedSignal = new Signal<string, ModuleViews, ModuleViews>();
    #endregion
    /************************************************构  造   函  数***********************************************/
    public TopViewHelper()
    {
        this.silenceViewList.Add(ModuleViews.LowBattary, false);
        this.silenceViewList.Add(ModuleViews.InCharge, false);
        this.silenceViewList.Add(ModuleViews.GuideTip, false);
        this.silenceViewList.Add(ModuleViews.Navigator, false);
    }
    /************************************************自 定 义 方 法************************************************/
    //添加页面栈变化监听
    public void AddListener(Action<string, ModuleViews, ModuleViews> action)
    {
        this.topViewChangedSignal.AddListener(action);
    }
    //移除页面栈变化监听
    public void RemoveListener(Action<string, ModuleViews, ModuleViews> action)
    {
        this.topViewChangedSignal.RemoveListener(action);
    }
    //页面入栈
    public void AddView(ModuleViews view)
    {
        if (this.silenceViewList.ContainsKey(view))
        {//静默页面状态置为打开
            this.silenceViewList[view] = true;
        }
        else if (!topViewList.Contains(view))
        {//记录普通页面
            topViewList.Add(view);
            this.OnViewChanged(ADD_VIEW, view);
        }
        Debug.Log(this.ToString());
    }
    //页面出栈(已弃用)
    public void PopTopView()
    {
        throw new Exception("此方法已作废");
    }
    //页面出栈
    public void RemoveView(ModuleViews view)
    {
        if (this.silenceViewList.ContainsKey(view))
        {//静默页面状态置为关闭
            this.silenceViewList[view] = false;
        }
        else if (topViewList.Count > 0 && this.TopView != ModuleViews.MainView && topViewList.Contains(view))
        {//移除普通页面
            topViewList.Remove(view);
            this.OnViewChanged(REMOVE_VIEW, view);
        }
        Debug.Log(this.ToString());
    }
    //指定页面是否为最上层页面(时间上的后打开页面，而不是视觉上的前后层)
    public bool IsTopView(ModuleViews view)
    {
        if (topViewList.Count > 0)
        {
            return view == topViewList[topViewList.Count - 1];
        }
        return false;
    }
    //指定页面是否已打开
    public bool IsOpenedView(ModuleViews view)
    {
        //普通页面在列表中，或者静默页面为打开状态，则视为已打开
        return ((this.topViewList != null && this.topViewList.Contains(view)) ||
                (this.silenceViewList != null && this.silenceViewList.ContainsKey(view) && this.silenceViewList[view]));
    }
    //是否已返回主页面
    public bool IsBackToMainView()
    {
        List<ModuleViews> moduleViews = new List<ModuleViews>() { ModuleViews.Collection, ModuleViews.AnimalPlayer,
                                                                  ModuleViews.MessageInbox, ModuleViews.PetPage,
                                                                  ModuleViews.Expression, ModuleViews.SystemMenu,
                                                                  ModuleViews.Achievement, ModuleViews.DrinkWaterRemind };
        if (this.topViewList == null || this.topViewList.Count == 0 ||
            !this.topViewList.Exists(t => moduleViews.Contains(t)))
            return true;
        else
            return false;
    }
    //是否在引导环节
    public bool IsInGuideView()
    {
        return (TopViewHelper.Instance.IsOpenedView(ModuleViews.NoviceGuide)
                || TopViewHelper.Instance.IsOpenedView(ModuleViews.SystemMenuGuide));
    }
    //是否有低电页以上层级的模块页面打开(层级是指ETopView枚举中的位置)
    public bool HasModulePageOpened()
    {
        return this.HasModulePageOpened(ModuleViews.InCharge);
    }
    //是否有指定页以上层级的模块页面打开(层级是指ETopView枚举中的位置)
    public bool HasModulePageOpened(ModuleViews obstructViewLevel)
    {
        if (this.topViewList != null)
        {
            int index = this.topViewList.FindIndex(t => (int)t > (int)obstructViewLevel);
            if (index >= 0)
            {
                //Debug.LogFormat("<><TopViewHelper.HasModulePageOpened>ModulePage: {0}, {1}", index, this.topViewList[index]);
                return true;
            }
            else return false;
        }
        else return false;
    }
    //是否有指定页以上层级的模块页面打开(层级是指ETopView枚举中的位置)
    public bool HasSilenceViewOpened(ModuleViews silenceView)
    {
        if (this.topViewList != null)
        {
            if (this.silenceViewList.ContainsKey(silenceView) && this.silenceViewList[silenceView])
            {
                //Debug.LogFormat("<><TopViewHelper.HasModulePageOpened>SilenceView: {0}, {1}", silenceView, this.silenceViewList[silenceView]);
                return true;
            }
            else return false;
        }
        else return false;
    }
    //重置页面状态栈
    public void Reset()
    {
        this.topViewList.Clear();
        this.topViewList.Add(ModuleViews.MainView);
        List<ModuleViews> keys = this.silenceViewList.Keys.ToList();
        for (int i = 0; i < keys.Count; i++)
            this.silenceViewList[keys[i]] = false;
        this.OnViewChanged(RESET_VIEW, this.TopView);
    }
    //输出页面栈内容
    public override string ToString()
    {
        System.Text.StringBuilder strbContent = new System.Text.StringBuilder("<><TopViewHelper.ToString>TopViewList: ");
        if (this.topViewList != null && this.topViewList.Count > 0)
        {
            for (int i = 0; i < this.topViewList.Count; i++)
            {
                strbContent.AppendFormat("[{0}, {1}], ", i + 1, this.topViewList[i]);
            }
            strbContent = strbContent.Remove(strbContent.Length - 2, 2);
            return strbContent.ToString();
        }
        else return "";
    }
    //输出所有已打开的页面
    public string GetAllOpenedView()
    {
        if (this.topViewList == null || this.silenceViewList == null)
            return "";
        ;
        System.Text.StringBuilder strbContent = new System.Text.StringBuilder();
        for (int i = 0; i < this.topViewList.Count; i++)
        {
            strbContent.AppendFormat("{0}, ", this.topViewList[i]);
        }
        strbContent.Append("+++++,");//分割线
        foreach (KeyValuePair<ModuleViews, bool> kvp in this.silenceViewList)
        {
            if (kvp.Value) strbContent.AppendFormat("{0}, ", kvp.Key);
        }
        return strbContent.ToString();
    }
    //当页面栈变化时
    private void OnViewChanged(string operate, ModuleViews operateView)
    {
        Debug.LogFormat("<><TopViewHelper.OnViewChanged>operate: {0}, operateView: {1}, topView: {2}", operate, operateView, this.TopView);
        this.topViewChangedSignal.Dispatch(operate, operateView, this.TopView);
    }
}

//模块页面
public enum ModuleViews
{
    None = 0,
    MainView = 1,
    Home = 2,
    MissionBrower = 3,
    MainMenu = 4,
    Navigator = 5,
    LowBattary = 11,//以LowBattery为分界线，数值小的为默认页面，数值大的为各功能页面
    InCharge = 12,
    LanguageSetting = 21,
    NoviceGuide = 22,
    SystemMenuGuide = 23,
    SystemMenu = 24,
    CupPair = 25,
    Collection = 26,
    AnimalPlayer = 27,
    PetPage = 28,
    CommingSoon = 29,
    Tmall = 30,
    NewsEvent = 31,
    AlarmClock = 32,
    AlertBoard = 33,
    Achievement = 34,
    Friend = 35,
    DownloadPage = 36,
    DownloadPage2 = 37,
    PetFeed = 38,
    WorldMap = 39,
    GuideTip = 40,
    PetDress = 41,
    Expression = 42,
    ExpressionInbox = 43,
    MessageInbox = 44,
    Garden = 45,
    DrinkWaterRemind = 46
}