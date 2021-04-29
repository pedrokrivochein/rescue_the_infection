using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour {

    public Transform alvo;
    RaycastHit hit = new RaycastHit();
    public float distCam;
    public float ajusteCamera;
    public LayerMask canCollide;
    public float distCamDefault;
    public float aimDist;
    public float x;
    public float y;
    public float compassDist;
    public bool inCutscene = false;
    public float speed = 10.0F;
    public GameObject player;
    public GameObject CMCamera;

        void Awake(){
            GetComponent<Camera>().farClipPlane = 3000.0F;
            player = GameObject.Find("Ana");
            CMCamera = GameObject.Find("CM_ThirdCamera");
        }
        // Use this for initialization
        void Start () {
            inCutscene = false;
            speed = 10.0F;
        }
        
        // Update is called once per frame
        void Update () {
            Debug.DrawLine(alvo.position, transform.position);
    	    if (Physics.Linecast(alvo.position, transform.position, out hit, canCollide)){
            	transform.position = hit.point + transform.forward * ajusteCamera;
                ////Debug.Log(hit.transform.gameObject.name);
       		}
            if(player != null)
            if(!player.GetComponent<CoverSystem>().inCover){
                transform.localPosition = new Vector3(x, y, -distCam);
            }else{
                if(!player.GetComponent<CoverSystem>().right){
                    transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(-x, y, -distCam), speed * Time.deltaTime);
                }else{
                    transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(x, y, -distCam), speed * Time.deltaTime);
                }
            }
            if(player != null)
            if(player.GetComponent<PlayerC>().aiming){
                ////Debug.Log("Get To 1");
                if(CMCamera.GetComponent<Cinemachine.CinemachineFreeLook>().m_Lens.FieldOfView > 30.0F){
                    CMCamera.GetComponent<Cinemachine.CinemachineFreeLook>().m_Lens.FieldOfView -= (speed * 8) * Time.deltaTime;
                    y += 2.0F * Time.deltaTime;
                }
                return;
            }
            if(player != null)
            if(player.GetComponent<PlayerC>().compass){
                if(distCam > compassDist){
                    distCam -= speed * Time.deltaTime;
                    y += 2.0F * Time.deltaTime;
                }
                return;
            }
            if(player != null)
            if(!player.GetComponent<PlayerC>().compass && !inCutscene && !player.GetComponent<PlayerC>().aiming){
                if(distCam < distCamDefault){
                    distCam += speed * Time.deltaTime;
                    y -= 2.0F * Time.deltaTime;
                }
                if(CMCamera.GetComponent<Cinemachine.CinemachineFreeLook>().m_Lens.FieldOfView < 40.0F){
                    CMCamera.GetComponent<Cinemachine.CinemachineFreeLook>().m_Lens.FieldOfView += (speed * 8) * Time.deltaTime;
                }
                //return;
            }
            y = 0;
    }

    public void ShakeOnce(float duration, float magnitude){
        StartCoroutine(Shake(duration, magnitude));
    }
    
    public IEnumerator Shake(float duration, float magnitude){
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0.0F;
        while(elapsed < duration){
            float x = UnityEngine.Random.Range(-1.0F, 1.0F) * magnitude;
            float y = UnityEngine.Random.Range(-1.0F, 1.0F) * magnitude;
            transform.localPosition = new Vector3(x, y, originalPos.z);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = originalPos;
    }

    void OnEnable(){
        transform.localPosition = new Vector3(0.0F, 0.0F, 0.0F);
    }
}
