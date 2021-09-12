using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Define.CameraMode _mode = Define.CameraMode.QuarterView;

    [SerializeField]
    Vector3 _delta = new Vector3(0.0f, 5.0f, -4.0f);

    [SerializeField]
    GameObject _player = null;
    private float currentZoom = 2.0f;
    private float minZoom = 0.6f;
    private float maxZoom = 2.0f;
    public void SetPlayer(GameObject player) { _player = player; }

    void Start()
    {
        
    }
    void Update() {
        // 마우스 휠로 줌 인아웃
        currentZoom -= Input.GetAxis("Mouse ScrollWheel");
        // 줌 최소 및 최대 설정 
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);    
    }

    void LateUpdate()
    { 
        if (_mode == Define.CameraMode.QuarterView)
        {
            if (_player.IsValid() == false)
            {
                return;
            }

            RaycastHit hit;
            if (Physics.Raycast(_player.transform.position, _delta, out hit, _delta.magnitude, 1 << (int)Define.Layer.Block))
            {
                float dist = (hit.point - _player.transform.position).magnitude * 0.8f;
                transform.position = _player.transform.position + _delta.normalized * dist * currentZoom;
            }
            else
            {
				transform.position = _player.transform.position + _delta * currentZoom;
				transform.LookAt(_player.transform);
			}
		}
    }

    public void SetQuarterView(Vector3 delta)
    {
        _mode = Define.CameraMode.QuarterView;
        _delta = delta;
    }

    public Vector3 CameraZoom(){
        if(Input.GetAxis("Mouse ScrollWheel")<0){
            //zoom in
            
        }
        if(Input.GetAxis("Mouse ScrollWheel")>0){
            //zoom out

        }
        return _delta;
    }
}
