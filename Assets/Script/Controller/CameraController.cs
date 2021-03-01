using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour {
    public PlayerInput pi;

    public float horizontalSpeed = 100.0f;
    public float verticalSpeed = 80.0f;

    private GameObject playerHandler;
    private GameObject cameraHandler;
    private GameObject model;
    private LockTarget lockTarget;
    private float tempEulerX;
    public Image lockDot;
    public bool lockState;
    public bool isAI;

    private void Awake() {
        cameraHandler = transform.gameObject;
        playerHandler = cameraHandler.transform.parent.gameObject;
        ActorController ac = playerHandler.GetComponent<ActorController>();
        model = ac.model;
        //pi = ac.pi;
        tempEulerX = 14.1f;

        if (!isAI) {
            lockDot.enabled = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

    }

    private void Update() {
        if (lockTarget != null) {
            if (!isAI) {
                lockDot.rectTransform.position = Camera.main.WorldToScreenPoint(lockTarget.obj.transform.position + new Vector3(0, lockTarget.halfHeight,0));

            }
            if (Vector3.Distance(model.transform.position, lockTarget.obj.transform.position) > 10.0f) {
                LockProcess(null, false, false, isAI);
            }
            if (lockTarget.am != null && lockTarget.am.sm.IsDie) {
                LockProcess(null, false, false, isAI);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate() {
        pi = playerHandler.GetComponent<ActorController>().pi;
        Vector3 tempModelEuler = model.transform.eulerAngles;

        if (lockTarget == null) {
            playerHandler.transform.Rotate(Vector3.up, pi.Vright * horizontalSpeed * Time.fixedDeltaTime);

            tempEulerX -= pi.Vup * verticalSpeed * Time.fixedDeltaTime;
            tempEulerX = Mathf.Clamp(tempEulerX, -40, 25);
            cameraHandler.transform.localEulerAngles = new Vector3(tempEulerX, 0, 0);

            model.transform.eulerAngles = tempModelEuler;
        }
        else {
            Vector3 tempForward = lockTarget.obj.transform.position - playerHandler.transform.position;
            tempForward.y = 0;
            playerHandler.transform.forward = tempForward;
            cameraHandler.transform.LookAt(lockTarget.obj.transform.position + Vector3.up * 0.5f * lockTarget.halfHeight);
        }
    }

    public void Lockon() {
        Vector3 modelOrigin1 = model.transform.position;
        Vector3 modelOrigin2 = modelOrigin1 + new Vector3(0, 1, 0);
        Vector3 boxCenter = modelOrigin2 + cameraHandler.transform.forward * 5.0f;
        Collider[] cols = Physics.OverlapBox(boxCenter, new Vector3(0.5f, 0.5f, 5f), model.transform.rotation, LayerMask.GetMask(isAI ? "player":"enemy"));

        if (cols.Length == 0 ) {
            LockProcess(null, false, false, isAI);
        }
        else {
            if (lockTarget!= null && lockTarget.obj == cols[0].gameObject) {
                LockProcess(null, false, false, isAI);
            }
            else {
                lockTarget = new LockTarget(cols[0].gameObject,cols[0].bounds.extents.y);
                LockProcess(lockTarget, true, true, isAI);
            }
        }
    }

    private void LockProcess(LockTarget _target,bool _lockDotEnable,bool _lockState,bool _isAI) {
        lockTarget = _target;
        if (!_isAI) {
            lockDot.enabled = _lockDotEnable;
        }
        lockState = _lockState;
    }

    private class LockTarget{
        public GameObject obj;
        public float halfHeight;
        public ActorManager am;
        
        public LockTarget(GameObject obj,float halfHeight) {
            this.obj = obj;
            this.halfHeight = halfHeight;
            am = obj.GetComponent<ActorManager>();
        }
    }


}
