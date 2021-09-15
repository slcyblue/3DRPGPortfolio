using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    UI_GameScene _sceneUI;

    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Game;
        gameObject.GetOrAddComponent<CursorController>();
        _sceneUI = Managers.UI.ShowSceneUI<UI_GameScene>();
        GameObject player = Managers.Game.GetPlayer();
        if(player==null){
            player = Managers.Game.Spawn(Define.WorldObject.Player, 10);
        }else{
            player.GetComponent<PlayerController>().Init(10);
            player.transform.position = new Vector3(0,0,-30.0f);
        }
        Camera.main.gameObject.GetOrAddComponent<CameraController>().SetPlayer(player);
        Managers.Game.Spawn(Define.WorldObject.Npc, 500);
        Managers.Game.Spawn(Define.WorldObject.Npc, 502);
        GameObject slayer = Managers.Game.Spawn(Define.WorldObject.Monster, 410);
        slayer.transform.position = Vector3.zero;
        
        Managers.UI.ShowSceneUI<UI_SkillPlate>();
        Managers.UI.ShowSceneUI<UI_BuffPlate>();
        Managers.UI.ShowSceneUI<UI_ExpBar>();
        Managers.UI.ShowSceneUI<UI_TopNavigation>();
    }

    public override void Clear()
    {
        
    }
}
