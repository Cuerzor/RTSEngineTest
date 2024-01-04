using System.Collections.Generic;
using RTSEngine;
using RTSEngine.EntityComponent;
using RTSEngine.ResourceExtension;
using RTSEngine.UI;
using UnityEngine;

/// <summary>
/// һ�����ڽ�ʵ��������ʵ�����ʾ����
/// ���뽫�丽����Ŀ��ʵ����������ϡ�
/// </summary>
public class EntitySeller : EntityComponentBase
{
    /// <summary>
    /// ʵ�屻��ʼ��ʱ��
    /// </summary>
    protected override void OnInit()
    {
        if (!Entity.IsFactionEntity())
        {
            // ���ǵ�λ�����Ҳ��ǽ�����
            Debug.LogError($"[EntitySeller] This component can only be attached to unit or building entities!", gameObject);
            return;
        }
        resourceMgr = gameMgr.GetService<IResourceManager>();
    }
    /// <summary>
    /// ����ˢ��UI�Ļص�
    /// </summary>
    /// <param name="taskUIAttributesCache">���������UI�����б�</param>
    /// <param name="disabledTaskCodesCache">�����õ�����ID�б�</param>
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
    /// ���UI�Ļص���
    /// </summary>
    /// <param name="taskAttributes">���������UI���ԡ�</param>
    /// <returns>�Ƿ���Ч��</returns>
    public override bool OnTaskUIClick(EntityComponentTaskUIAttributes taskAttributes)
    {
        /// UI��Ч�����ҵ���ľ��Ǹ�UI��
        if (taskUI.IsValid() && taskAttributes.data.code == taskUI.Key)
        {
            // ����ʵ��
            resourceMgr.UpdateResource(Entity.FactionID, sellResources, add: true);
            Entity.Health.Destroy(upgrade: false, null);
            return true;
        }

        return false;
    }

    // UI
    [SerializeField]
    private EntityComponentTaskUIAsset taskUI = null;

    // ������Դ���
    [SerializeField]
    private ResourceInput[] sellResources = new ResourceInput[0];
    protected IResourceManager resourceMgr { private set; get; }
}
