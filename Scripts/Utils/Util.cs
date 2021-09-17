using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Util
{
    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
		if (component == null)
            component = go.AddComponent<T>();
        return component;
	}

    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform == null)
            return null;
        
        return transform.gameObject;
    }

    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
            return null;

        if (recursive == false)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
		}
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }

    public static GameObject FindGameSceneChild(string name, bool recursive){
        GameObject go = GameObject.Find($"@UI_Root/UI_GameScene/{name}");
        return go;
    }

    public static Vector3 FindGround(Vector3 position){
		Ray rayUp = new Ray(position, Vector3.up);
		Ray rayDown = new Ray(position, Vector3.down);
		RaycastHit[] hits = Physics.RaycastAll(rayUp, 100.0f);
		bool findGround = false;

		if(hits.Count()>0){
			foreach(var hit in hits){
				if(hit.collider.CompareTag("Ground")){
					position.y = hit.collider.transform.position.y;
					findGround = true;
				}
			}
			if(!findGround){
				hits = Physics.RaycastAll(rayDown, 100.0f);
				
				foreach(var hit in hits){
					if(hit.collider.CompareTag("Ground")){
						position.y = hit.collider.transform.position.y;
					}
				}
			}
		}else{
			hits = Physics.RaycastAll(rayDown, 100.0f);
			
			foreach(var hit in hits){
				if(hit.collider.CompareTag("Ground")){
					position.y = hit.collider.transform.position.y;
				}
			}
		}
		return position;
	}
}
