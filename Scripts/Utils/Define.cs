using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum WorldObject
    {
        Unknown,
        Player,
        Monster,
        Npc,
        Boss,
    }

	public enum State
	{
		Die,
		Moving,
		Idle,
		Skill,
        Attack,
	}

    public enum Layer
    {
        Player = 7,
        Monster = 8,
        Ground = 9,
        Block = 10,
        Npc = 11,
    }

    public enum Scene
    {
        Unknown,
        Login,
        Lobby,
        Game,
        Dungeon,
    }

    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,
    }

    public enum UIEvent
    {
        Click,
        Drag,
    }

    public enum MouseEvent
    {
        Press,
        PointerDown,
        PointerUp,
        Click,
        DoubleClick,
        Drag
    }

    public enum KeyEvent
    {
        Press,
        KeyDown,
        KeyUp,
        Click,
    }

    public enum CameraMode
    {
        QuarterView,
    }

    public enum ItemTemplate{
        None,
        Equipment,
        Consumable
    }
}
