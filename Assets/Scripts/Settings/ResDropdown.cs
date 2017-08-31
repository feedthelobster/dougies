using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ResDropdown : MonoBehaviour {

	   public Dropdown drop;

     	public void ChangeRes(){
     		
     		switch(drop.value){
     			
     			case 0:
     			  Screen.SetResolution(1920, 1080, true);
     				break;
     			case 1:
     			  Screen.SetResolution(1366, 768, true);
     				break;
     			case 2:
     			  Screen.SetResolution(1280, 720, true);
     				break;
     			case 3:
     			  Screen.SetResolution(720, 480, true);
     				break;
     			case 4:
     			  Screen.SetResolution(640, 480, true);
     				break;
     			case 5:
     			  Screen.SetResolution(480, 272, true);
     				break;

     		}
     	}

}
