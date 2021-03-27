using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView : MonoBehaviour
{
 
    GameObject Target = null;
    public float PerceiveRadius;//��֪��Χ
    public float LightIntensity;//�ƹ�����
    public float ViewAngle;//��Ұ�Ƕ�
 
    bool isInView = false;//�Ƿ�����Ұ��Χ��
    bool isRayCast = false;//���߼�������Ƿ����赲�
 
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
 
            //1���޽Ƕ�ת��
            if ((Target.transform.position.z - this.transform.position.z) > 0
               &&
            (Target.transform.position.x - this.transform.position.x) > 0
               )
            {
                TargetAngle = 90f - AtanAngle;
                //Debug.Log ("����1 "+TargetAngle);
            }
 
            //2���޽Ƕ�ת��
            if ((Target.transform.position.z - this.transform.position.z) <= 0
               &&
            (Target.transform.position.x - this.transform.position.x) > 0
               )
            {
                TargetAngle = 90f + -AtanAngle;
                //Debug.Log ("����2 "+TargetAngle);
            }
 
            //3���޽Ƕ�ת��
            if ((Target.transform.position.z - this.transform.position.z) <= 0
               &&
            (Target.transform.position.x - this.transform.position.x) <= 0
               )
            {
                TargetAngle = 90f - AtanAngle + 180f;
                //Debug.Log ("����3 "+TargetAngle);
            }
 
            //4���޽Ƕ�ת��
            if ((Target.transform.position.z - this.transform.position.z) > 0
               &&
            (Target.transform.position.x - this.transform.position.x) <= 0
               )
            {
                TargetAngle = 270f + -AtanAngle;
                //Debug.Log ("����4 "+TargetAngle);
            }
 
 
            //����TargetAngle
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
 
            //����ǶȲ�
            AngleDiff = Mathf.Abs(TargetAngle - this.transform.rotation.eulerAngles.y);
            //Debug.Log("�ǶȲ�:" + TargetAngle + "(" + OriginTargetAngle + ")-" + this.transform.rotation.eulerAngles.y + "=" + AngleDiff);
        }
    }
 
    void JudgeView()
    {

        //��Ұ��Χ�ж�
        if (Target != null)
        {
            if (AngleDiff * 2 < ViewAngle)
                isInView = true;
            else
                isInView = false;
            //Debug.Log ("�ǶȲ� "+AngleDiff);

            if (!isInView) return;

            //������ײ���
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
            //������ײ��
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

        //����ǶȲ�
        CalculateAngle();
        //��֪��Ұ�жϣ��ж�isRayCast��isInView)
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
            //��ǰ����ǶȲ�
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
