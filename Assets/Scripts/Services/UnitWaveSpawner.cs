using UnityEngine;
using RTSEngine.Game;
using RTSEngine.Determinism;
using RTSEngine.UnitExtension;
using RTSEngine;
using RTSEngine.Entities;

/// <summary>
/// 一个游戏的Service，用于每隔一段时间生成一个单位。
/// 必须将该组件的GameObject放在GameManager的子物体中。
/// </summary>
public class UnitWaveSpawner : MonoBehaviour, IPostRunGameService
{
    /// <summary>
    /// 初始化。被IPostRunGameService需求。
    /// </summary>
    /// <param name="gameMgr">游戏管理器。</param>
    public void Init(IGameManager gameMgr)
    {
        if (!spawnTransform.IsValid() || !gotoTransform.IsValid() || !unitPrefabObj.IsValid())
        {
            Debug.LogError("[UnitWaveSpawner] All inspector fields must be assigned!");
            return;
        }
        timer = new TimeModifiedTimer(spawnPeriod);
        unitPrefab = unitPrefabObj.GetComponent<IUnit>();
        unitMgr = gameMgr.GetService<IUnitManager>();
    }
    private void Update()
    {
        if (timer.ModifiedDecrease()) // RTS的计时器会根据游戏速率改变，所以使用这个方法
        {
            timer.Reload(); // 重设计时器。

            // 创建单位。
            for (int i = 0; i < spawnAmount; i++)
            {
                unitMgr.CreateUnit(unitPrefab, spawnTransform.position, Quaternion.identity, new InitUnitParameters
                {
                    free = true,

                    useGotoPosition = true,
                    gotoPosition = gotoTransform.position,
                });
            }
        }
    }
    // 计时器相关。
    private TimeModifiedTimer timer;
    [SerializeField]
    private float spawnPeriod = 5.0f;

    // 创建单位相关。
    protected IUnitManager unitMgr { private set; get; }
    private IUnit unitPrefab;
    [SerializeField, EnforceType(typeof(IUnit), prefabOnly: true)]
    private GameObject unitPrefabObj = null;
    [SerializeField]
    private int spawnAmount = 3;
    [SerializeField]
    private Transform spawnTransform = null;
    [SerializeField]
    private Transform gotoTransform = null;

}