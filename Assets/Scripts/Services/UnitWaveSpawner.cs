using UnityEngine;
using RTSEngine.Game;
using RTSEngine.Determinism;
using RTSEngine.UnitExtension;
using RTSEngine;
using RTSEngine.Entities;

/// <summary>
/// һ����Ϸ��Service������ÿ��һ��ʱ������һ����λ��
/// ���뽫�������GameObject����GameManager���������С�
/// </summary>
public class UnitWaveSpawner : MonoBehaviour, IPostRunGameService
{
    /// <summary>
    /// ��ʼ������IPostRunGameService����
    /// </summary>
    /// <param name="gameMgr">��Ϸ��������</param>
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
        if (timer.ModifiedDecrease()) // RTS�ļ�ʱ���������Ϸ���ʸı䣬����ʹ���������
        {
            timer.Reload(); // �����ʱ����

            // ������λ��
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
    // ��ʱ����ء�
    private TimeModifiedTimer timer;
    [SerializeField]
    private float spawnPeriod = 5.0f;

    // ������λ��ء�
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