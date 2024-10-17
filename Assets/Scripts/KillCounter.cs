using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class KillCounter : MonoBehaviour
{
    public Text counterText;
    public int kills;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ShowKills();
    }
    private void ShowKills()
    {
        counterText.text = kills.ToString();
    }


    public void Addkill()
    {
        kills++;
     //   KillCount.text = "Kills: " + kills;
    }
}
