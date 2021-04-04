using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    private Slider slider;
    private StateManager state;
    private float tempValue;
    public bool isPlayer;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        if (isPlayer)
            state = GameObject.FindGameObjectWithTag("Player").GetComponent<StateManager>();
        else
            state = transform.parent.parent.gameObject.GetComponent<StateManager>();
        slider.maxValue = slider.value = state.maxHP;
        tempValue = slider.value;
    }

    // Update is called once per frame
    void Update()
    {
        tempValue = Mathf.Lerp(tempValue, state.HP, 0.1f);
        slider.value = tempValue;

        if(!isPlayer)
            transform.rotation = Camera.main.transform.rotation;
    }
}