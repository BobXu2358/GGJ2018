using UnityEngine;

public class CameraManager : MonoBehaviour {
    public Transform followingTf;
    public Transform cameraTf;
    public Transform groundTf;
    public Vector3 cameraOffset;

    void Update(){
        Vector3 finCameraPos = followingTf.position + cameraOffset;
        if(finCameraPos.x - 12.5 <= groundTf.position.x - groundTf.lossyScale.x / 2)
            finCameraPos.x = groundTf.position.x - groundTf.lossyScale.x / 2 + (float)12.5;
        if(finCameraPos.x + 12.5 >= groundTf.position.x + groundTf.lossyScale.x / 2)
            finCameraPos.x = groundTf.position.x + groundTf.lossyScale.x / 2 - (float)12.5;
        cameraTf.position = finCameraPos;
    }
}
