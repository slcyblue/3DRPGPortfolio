using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonScene : BaseScene
{
    UI_GameScene _sceneUI;

    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Dungeon;
        gameObject.GetOrAddComponent<CursorController>();
        _sceneUI = Managers.UI.ShowSceneUI<UI_GameScene>();
        GameObject player = Managers.Game.GetPlayer();
        player.GetComponent<PlayerController>().Init(10);
        player.transform.position = Vector3.zero;
        Camera.main.gameObject.GetOrAddComponent<CameraController>().SetPlayer(player);

        GameObject go = new GameObject { name = "SpawningPool" };
        SpawningPool pool = go.GetOrAddComponent<SpawningPool>();
        pool.SetKeepMonsterCount(5);
        Managers.Resource.Instantiate("Slayer", null).GetComponent<BossController>().Init(410);
        
        Managers.UI.ShowSceneUI<UI_SkillPlate>();
        Managers.UI.ShowSceneUI<UI_BuffPlate>();
        Managers.UI.ShowSceneUI<UI_ExpBar>();
        Managers.UI.ShowSceneUI<UI_TopNavigation>();
    }

    public override void Clear()
    {
        
    }
}
