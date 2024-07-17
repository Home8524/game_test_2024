using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VolumeControll : MonoBehaviour
{
    public Text VolumeText;
    [SerializeField] private AudioSource Lisner;
    private GameObject Me;
    public GameObject Canvas1;
    public GameObject Canvas2;
    private void Start()
    {
        Me = GameObject.Find("Main Camera");
        Lisner = Me.GetComponent<AudioSource>();
        Lisner.volume = 0.5f;
    }

   public void VolumeDown()
    {
        if(Lisner.volume>0)
        {
            Lisner.volume -= 0.1f;

            if (Lisner.volume < 0)
                Lisner.volume = 0;

            VolumeText.text = string.Format("{0:0.#}", Lisner.volume*10.0f);
        }
    }
   public void VolumeUp()
    {
        if (Lisner.volume < 1)
        {
            Lisner.volume += 0.1f;
            if (Lisner.volume > 1)
                Lisner.volume = 1;
            VolumeText.text = string.Format("{0:0.#}", Lisner.volume*10.0f);
        }
    }
    public void Back()
    {
        Canvas2.SetActive(true);
        Canvas1.SetActive(false);
    }
}
