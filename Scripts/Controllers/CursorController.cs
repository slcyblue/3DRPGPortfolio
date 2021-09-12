using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
	int _mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster) | (1 << (int)Define.Layer.Npc);

	Texture2D _attackIcon;
	Texture2D _handIcon;
	Texture2D _lootIcon;

	enum CursorType
	{
		None,
		Attack,
		Hand,
		Loot,
	}

	CursorType _cursorType = CursorType.None;

	void Start()
    {
		_attackIcon = Managers.Resource.Load<Texture2D>("Textures/Cursor/Attack");
		_handIcon = Managers.Resource.Load<Texture2D>("Textures/Cursor/Hand");
		_lootIcon = Managers.Resource.Load<Texture2D>("Textures/Cursor/Loot");
	}

    void Update()
    {
		if (Input.GetMouseButton(0))
			return;

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 100.0f, _mask))
		{
			if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
			{
				if (_cursorType != CursorType.Attack)
				{
					Cursor.SetCursor(_attackIcon, new Vector2(_attackIcon.width / 5, 0), CursorMode.Auto);
					_cursorType = CursorType.Attack;
				}
			}else if(hit.collider.gameObject.layer == (int)Define.Layer.Npc){
				if(_cursorType != CursorType.Loot){
					Cursor.SetCursor(_lootIcon, new Vector2(_attackIcon.width / 5, 0), CursorMode.Auto);
					_cursorType = CursorType.Loot;
				}
			}else
			{
				if (_cursorType != CursorType.Hand)
				{
					Cursor.SetCursor(_handIcon, new Vector2(_handIcon.width / 3, 0), CursorMode.Auto);
					_cursorType = CursorType.Hand;
				}
			}
		}
	}
}
