using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeCount : MonoBehaviour
{
    public float m_totalTime = 180;
    private Text m_text;
    public GameObject global;
    // Start is called before the first frame update
    void Start()
    {
        m_text = this.gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        m_totalTime -= Time.deltaTime;
        int minute = (int)m_totalTime / 60;
        int second = (int)m_totalTime % 60;
        m_text.text = string.Format("Remaining Time: {0:00}:{1:00}", minute, second);
        if (m_totalTime <= 0)
        {
            global.GetComponent<MainGlobal>().ShowLossCanvas();
        }
    }

}
