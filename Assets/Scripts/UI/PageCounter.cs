using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

/// <summary>
/// 页计数器
/// </summary>
public class PageCounter
{
    /************************************************属性与变量命名************************************************/
    public int ItemCount { get; private set; }//条目总数
    public int ItemIndex { get; private set; }//当前条目序号
    public int PageSize { get; private set; }//页大小
    public int PageCount { get; private set; }//页总数
    public int PageIndex { get; private set; }//当前页序号
    public int IndexOfPage
    {
        get
        {
            if (this.Verify())
                return this.ItemIndex - this.FirstOfPage;
            else
                return 0;
        }
    }//当前条目位于当前页的位置
    public int FirstOfPage
    {
        get
        {
            if (this.Verify())
                return this.PageIndex * this.PageSize;
            else
                return 0;
        }
    }//当前页的第一个位置
    public int LastOfPage
    {
        get
        {
            if (this.Verify())
                return this.IsLastPage() ? (this.ItemCount - 1) : (this.PageIndex * this.PageSize + this.PageSize - 1);
            else
                return 0;
        }
    }//当前页的最后一个位置
    public Action PageChanged { get; set; }//切换页的委托
    /************************************************Unity方法与事件***********************************************/
    public PageCounter()
    {
    }
    /************************************************自 定 义 方 法************************************************/
    //初始化
    private void Initialize(int itemCount, int pageSize)
    {
        this.ItemIndex = 0;
        this.ItemCount = itemCount;
        this.PageSize = pageSize;
        this.PageIndex = 0;
        if (this.Verify())
        {
            if (this.ItemCount <= this.PageSize)
            {
                this.PageCount = 1;
            }
            else
            {
                this.PageCount = this.ItemCount / this.PageSize;
                if (this.ItemCount % this.PageSize > 0) this.PageCount += 1;
            }
            //Debug.LogFormat("PageCount: {0}, ItemCount: {1}, PageSize: {2}, LastOfPage: {3}", this.PageCount, this.ItemCount, this.PageSize, this.LastOfPage);
        }
    }
    //重设条目总数和页大小
    public void Reset(int itemCount, int pageSize)
    {
        this.Initialize(itemCount, pageSize);
    }
    //前一个条目
    public int PreItem()
    {
        if (this.Verify())
        {
            this.ItemIndex = (this.ItemIndex - 1 + this.ItemCount) % this.ItemCount;
            int pageIndex = this.Seek(this.Locate(this.ItemIndex));
            if (pageIndex >= 0 && pageIndex < this.PageCount)
                return this.ItemIndex;
            else return 0;
        }
        else return 0;
    }
    //后一个条目
    public int NextItem()
    {
        if (this.Verify())
        {
            this.ItemIndex = (this.ItemIndex + 1) % this.ItemCount;
            int pageIndex = this.Seek(this.Locate(this.ItemIndex));
            if (pageIndex >= 0 && pageIndex < this.PageCount)
                return this.ItemIndex;
            else return 0;
        }
        else return 0;
    }
    //前一页
    public int PrePage()
    {
        if (this.Verify())
        {
            this.PageIndex = (this.PageIndex - 1 + this.PageCount) % this.PageCount;
            return this.PageIndex;
        }
        else return 0;
    }
    //后一页
    public int NextPage()
    {
        if (this.Verify())
        {
            this.PageIndex = (this.PageIndex + 1) % this.PageCount;
            return this.PageIndex;
        }
        else return 0;
    }
    //跳转到指定页
    public int Seek(int pageIndex)
    {
        if (pageIndex >= 0 && pageIndex < this.PageCount)
        {
            bool pageChanged = this.PageIndex != pageIndex;
            this.PageIndex = pageIndex;
            if (pageChanged) this.OnPageChanged();
            return this.PageIndex;
        }
        else
        {
            Debug.LogErrorFormat("页码[{0}]不能为负数，并且不能大于总页数[{1}]。", pageIndex, this.PageCount);
            return -1;
        }
    }
    //定位到条目所在的页
    public int Locate(int itemIndex)
    {
        if (!this.Verify()) return -1;
        if (itemIndex >= 0 && itemIndex < this.ItemCount)
        {
            this.ItemIndex = itemIndex;
            int pageIndex = this.PageCount - 1;
            while (pageIndex >= 0 && pageIndex * this.PageSize > itemIndex)
            {
                pageIndex -= 1;
            }
            bool pageChanged = this.PageIndex != pageIndex;
            this.PageIndex = pageIndex;
            if (pageChanged) this.OnPageChanged();
            return pageIndex;
        }
        else
        {
            Debug.LogError("定位某个元素在哪一页时发生错误，指定的ItemIndex为负数，或者大于元素总数ItemCount。");
            return -1;
        }
    }
    //验证数据
    private bool Verify()
    {
        if (this.ItemCount <= 0)
        {
            Debug.LogError("条目数量必须大于零。");
            return false;
        }
        if (this.PageSize <= 0)
        {
            Debug.LogError("页大小必须大于零。");
            return false;
        }
        return true;
    }
    //是否最后一页
    private bool IsLastPage()
    {
        if (this.Verify())
        {
            return this.PageIndex + 1 == this.PageCount;
        }
        else return true;
    }
    //当切换页时
    private void OnPageChanged()
    {
        if (this.PageChanged != null)
            this.PageChanged();
    }
}
