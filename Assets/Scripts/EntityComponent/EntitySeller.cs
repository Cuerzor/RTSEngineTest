using System.Collections.Generic;
using RTSEngine;
using RTSEngine.EntityComponent;
using RTSEngine.ResourceExtension;
using RTSEngine.UI;
using UnityEngine;

/// <summary>
/// 一个用于将实体卖掉的实体组件示例。
/// 必须将其附加在目标实体的子物体上。
/// </summary>
public class EntitySeller : EntityComponentBase
{
    /// <summary>
    /// 实体被初始化时。
    /// </summary>
    protected override void OnInit()
    {
        if (!Entity.IsFactionEntity())
        {
            // 不是单位，并且不是建筑物
            Debug.LogError($"[EntitySeller] This component can only be attached to unit or building entities!", gameObject);
            return;
        }
        resourceMgr = gameMgr.GetService<IResourceManager>();
    }
    /// <summary>
    /// 请求刷新UI的回调
    /// </summary>
    /// <param name="taskUIAttributesCache">点击的任务UI属性列表。</param>
    /// <param name="disabledTaskCodesCache">被禁用的任务ID列表。</param>
    /// <returns></returns>
    protected override bool OnTaskUICacheUpdate(List<EntityComponentTaskUIAttributes> taskUIAttributesCache, List<string> disabledTaskCodesCache)
    {
        return RTSHelper.OnSingleTaskUIRequest(
            this,
            taskUIAttributesCache,
            disabledTaskCodesCache,
            taskUI);
    }
    /// <summary>
    /// 点击UI的回调。
    /// </summary>
    /// <param name="taskAttributes">点击的任务UI属性。</param>
    /// <returns>是否有效？</returns>
    public override bool OnTaskUIClick(EntityComponentTaskUIAttributes taskAttributes)
    {
        /// UI有效，并且点击的就是该UI。
        if (taskUI.IsValid() && taskAttributes.data.code == taskUI.Key)
        {
            // 卖掉实体
            resourceMgr.UpdateResource(Entity.FactionID, sellResources, add: true);
            Entity.Health.Destroy(upgrade: false, null);
            return true;
        }

        return false;
    }

    // UI
    [SerializeField]
    private EntityComponentTaskUIAsset taskUI = null;

    // 卖出资源相关
    [SerializeField]
    private ResourceInput[] sellResources = new ResourceInput[0];
    protected IResourceManager resourceMgr { private set; get; }
}
