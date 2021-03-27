using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView : MonoBehaviour
{
 
    GameObject Target = null;
    public float PerceiveRadius;//感知范围
    public float LightIntensity;//灯光亮度
    public float ViewAngle;//视野角度
 
    bool isInView = false;//是否在视野范围内
    bool isRayCast = false;//射线检测结果（是否有阻挡物）
 
    float TargetAngle = 0;
    float AngleDiff = 0;
 
    public Light m_Light;
    //public GameObject m_Bullet;
    //public GameObject m_Launcher;


    // Use this for initialization
    void Start() {
        this.GetComponent<SphereCollider>().radius = PerceiveRadius;
        m_Light = GetComponent<Light>();
    }


    void CalculateAngle()
    {
        if (Target != null)
        {
            float AtanAngle = (Mathf.Atan((Target.transform.position.z - this.transform.position.z) /
            (Target.transform.position.x - this.transform.position.x))
            * 180.0f / 3.14159f);
            //Debug.Log (this.transform.rotation.eulerAngles+"   "+AtanAngle);
 
            //1象限角度转换
            if ((Target.transform.position.z - this.transform.position.z) > 0
               &&
            (Target.transform.position.x - this.transform.position.x) > 0
               )
            {
                TargetAngle = 90f - AtanAngle;
                //Debug.Log ("象限1 "+TargetAngle);
            }
 
            //2象限角度转换
            if ((Target.transform.position.z - this.transform.position.z) <= 0
               &&
            (Target.transform.position.x - this.transform.position.x) > 0
               )
            {
                TargetAngle = 90f + -AtanAngle;
                //Debug.Log ("象限2 "+TargetAngle);
            }
 
            //3象限角度转换
            if ((Target.transform.position.z - this.transform.position.z) <= 0
               &&
            (Target.transform.position.x - this.transform.position.x) <= 0
               )
            {
                TargetAngle = 90f - AtanAngle + 180f;
                //Debug.Log ("象限3 "+TargetAngle);
            }
 
            //4象限角度转换
            if ((Target.transform.position.z - this.transform.position.z) > 0
               &&
            (Target.transform.position.x - this.transform.position.x) <= 0
               )
            {
                TargetAngle = 270f + -AtanAngle;
                //Debug.Log ("象限4 "+TargetAngle);
            }
 
 
            //调整TargetAngle
            float OriginTargetAngle = TargetAngle;
            if (Mathf.Abs(TargetAngle + 360 - this.transform.rotation.eulerAngles.y)
               <
            Mathf.Abs(TargetAngle - this.transform.rotation.eulerAngles.y)
               )
            {
                TargetAngle += 360f;
            }
            if (Mathf.Abs(TargetAngle - 360 - this.transform.rotation.eulerAngles.y)
               <
            Mathf.Abs(TargetAngle - this.transform.rotation.eulerAngles.y)
               )
            {
                TargetAngle -= 360f;
            }
 
            //输出角度差
            AngleDiff = Mathf.Abs(TargetAngle - this.transform.rotation.eulerAngles.y);
            //Debug.Log("角度差:" + TargetAngle + "(" + OriginTargetAngle + ")-" + this.transform.rotation.eulerAngles.y + "=" + AngleDiff);
        }
    }
 
    void JudgeView()
    {

        //视野范围判断
        if (Target != null)
        {
            if (AngleDiff * 2 < ViewAngle)
                isInView = true;
            else
                isInView = false;
            //Debug.Log ("角度差 "+AngleDiff);

            if (!isInView) return;

            //射线碰撞检测
            Vector3 vec = new Vector3(Target.transform.position.x - this.transform.position.x,
                                    0f,
                                    Target.transform.position.z - this.transform.position.z);
 
            RaycastHit hitInfo;
            if (Physics.Raycast(this.transform.position + Vector3.up, vec, out
                               hitInfo, 20))
            {
                GameObject gameObj = hitInfo.collider.gameObject;
                //Debug.Log("Object name is " + gameObj.name);
                if (gameObj.tag == "Player")
                    isRayCast = true;
                else
                    isRayCast = false;
            }
            //画出碰撞线
            Debug.DrawLine(this.transform.position + 0.5f * Vector3.up, hitInfo.point, Color.red, 0.01f);
 
        }
 
    }
 

 
    // Update is called once per frame
 
    void Update()
    {
        m_Light.range = PerceiveRadius;
        m_Light.color = new Color(1f, 0, 0);
        m_Light.intensity = LightIntensity;
        m_Light.spotAngle = ViewAngle;
        this.GetComponent<SphereCollider>().radius = PerceiveRadius;

        //计算角度差
        CalculateAngle();
        //感知视野判断（判断isRayCast与isInView)
        JudgeView();

        if (isInView && isRayCast) {
            GetComponentInParent<EnemyAI>().FoundTarget(Target);
        }


    }
 
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Target = other.gameObject;
            //提前计算角度差
            CalculateAngle();
            
        }
    }
 
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (Target == null)
            {
                Target = other.gameObject;
            }
        }
    }
 
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Target = null;
            isInView = false;
            isRayCast = false;
        }
    }
}
