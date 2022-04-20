using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleSlider : MonoBehaviour
{
	//게이지 파트
	public bool b=true;
	public Image image;

	float speed;
	
	public Text progress;

  
    void Update()
    {
		if(b)
		{
			//time+=Time.deltaTime*speed;
			speed = Fire.instance.speed;
			image.fillAmount= speed/15;
			if(progress)
			{
				progress.text = (int)(image.fillAmount*100)+"%";
			}
			
			if(speed>20f)
			{
							
				speed=20f;
			}
    	}
	}
	
	
}