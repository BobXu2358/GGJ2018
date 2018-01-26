using UnityEngine;

public class CameraManager : MonoBehaviour {
    public Transform followingTf;
    public Transform cameraTf;
    public Transform mapGroundTf;
    public Camera camera;
    public Vector3 cameraOffset;
    float cameraHalfWidth;
    float cameraHalfHeight;

    void Start(){
        cameraHalfHeight = camera.orthographicSize;
        cameraHalfWidth = cameraHalfHeight / Screen.height * Screen.width;
    }

    void Update(){
        Vector3 finCameraPos = followingTf.position + cameraOffset;
        if(finCameraPos.x - cameraHalfWidth <= mapGroundTf.position.x - mapGroundTf.lossyScale.x / 2)
            finCameraPos.x = mapGroundTf.position.x - mapGroundTf.lossyScale.x / 2 + cameraHalfWidth;
        if(finCameraPos.x + cameraHalfWidth >= mapGroundTf.position.x + mapGroundTf.lossyScale.x / 2)
            finCameraPos.x = mapGroundTf.position.x + mapGroundTf.lossyScale.x / 2 - cameraHalfWidth;
        if(finCameraPos.y - cameraHalfHeight <= mapGroundTf.position.y + mapGroundTf.lossyScale.y / 2 + followingTf.lossyScale.y)
            finCameraPos.y = mapGroundTf.position.y + mapGroundTf.lossyScale.y / 2 + followingTf.lossyScale.y + cameraHalfHeight;
        cameraTf.position = finCameraPos;
    }
}
