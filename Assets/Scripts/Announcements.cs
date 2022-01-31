using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Really just throwing things together here tbh
public class Announcements : MonoBehaviour {

    public GameObject[] barkPanels; //BarkPanel prefab, different announcments

    public static Announcements instance; //This class's function will be called from many other classes
    void Awake() {
        if (instance != null)
        {
            Debug.Log("More than one instance of Announcements found!");
            return;
        }
        instance = this;
    }
    
    //play bark animation at index
    public IEnumerator Bark(int index) {
        barkPanels[index].GetComponent<Animator>().SetBool("Bark", true);
        yield return new WaitForSeconds(1);
        barkPanels[index].GetComponent<Animator>().SetBool("Bark", false);
    }

}
