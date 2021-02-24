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
    private Signal<string, ModuleViews, ModuleViews> topViewChangedSignal = new Signal<string, ModuleViews, ModuleViews>();
    #endregion
    /************************************************构  造   函  数***********************************************/
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
        if (!topViewList.Contains(view))
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
        if (topViewList.Count > 0 && topViewList.Contains(view))
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
        return this.topViewList != null && this.topViewList.Contains(view);
    }
    //是否有低电页以上层级的模块页面打开(层级是指ETopView枚举中的位置)
    public bool HasModulePageOpened()
    {
        return this.HasModulePageOpened(ModuleViews.None);
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
    //重置页面状态栈
    public void Reset()
    {
        this.topViewList.Clear();
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
        if (this.topViewList == null)
            return "";
        ;
        System.Text.StringBuilder strbContent = new System.Text.StringBuilder();
        for (int i = 0; i < this.topViewList.Count; i++)
        {
            strbContent.AppendFormat("{0}, ", this.topViewList[i]);
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
    Common = 1,
    GameStart = 2,
    Cycling = 3
}