using UnityEngine;

public class CameraManager : MonoBehaviour {
    public Transform followingTf;
    public CapsuleCollider2D followingBc;
    public Transform cameraTf;
    public Transform mapGroundTf;
    public BoxCollider2D mapGroundBc;
    public Camera mainCam;
    public Vector3 cameraOffset = new Vector3(0, 0, -10);
    float cameraHalfWidth;
    float cameraHalfHeight;

    void Start(){
        cameraHalfHeight = mainCam.orthographicSize;
        cameraHalfWidth = cameraHalfHeight / Screen.height * Screen.width;
    }

    void Update(){
        Vector3 finCameraPos = followingTf.position + cameraOffset;
        if(finCameraPos.x - cameraHalfWidth <= mapGroundTf.position.x - mapGroundBc.size.x / 2)
            finCameraPos.x = mapGroundTf.position.x - mapGroundBc.size.x / 2 + cameraHalfWidth;
        if(finCameraPos.x + cameraHalfWidth >= mapGroundTf.position.x + mapGroundBc.size.x / 2)
            finCameraPos.x = mapGroundTf.position.x + mapGroundBc.size.x / 2 - cameraHalfWidth;
        if(finCameraPos.y - cameraHalfHeight <= mapGroundTf.position.y + mapGroundBc.size.y / 2 + followingBc.size.y)
            finCameraPos.y = mapGroundTf.position.y + mapGroundBc.size.y / 2 + followingBc.size.y + cameraHalfHeight;
        cameraTf.position = finCameraPos;
    }
}
