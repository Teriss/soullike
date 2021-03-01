using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public Itemcontroller ic;
    public InteractionManager im;
    public DiretorManager dm;

    // Start is called before the first frame update
    void Awake()
    {
        ic = GetComponent<Itemcontroller>();
        im = Bind<InteractionManager>(transform.Find("caster").gameObject);
        dm = Bind<DiretorManager>(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private T Bind<T>(GameObject obj) where T : ActorManagerInterface {
        T temp;
        temp = obj.GetComponent<T>();
        if (temp == null)
            temp = obj.AddComponent<T>();
        temp.itemm = this;
        return temp;
    }
}
