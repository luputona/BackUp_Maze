using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.ThirdPerson;

public class CharacterSound : MonoBehaviour
{

    private string m_colltype;

    void OnCollisionEnter(Collision coll)
    {
        if(coll.gameObject.GetComponent<AudioColliderType>())
        {
            m_colltype = coll.gameObject.GetComponent<AudioColliderType>().GetTerrianType();
        }
    }

    public void PlayStepSound()
    {
        switch(m_colltype)
        {
            case "Block":
                SoundManager.getInstance.PlaySe("Walk_01");
                break;
            case "Water":
                break;
            case "Gress":
                break;
            case "Wood":
                break;
            case "Snow":
                break;
            case "Gravel":
                SoundManager.getInstance.PlaySe("gravel_footstep");
                break;
            case "Metal":
                break;
            case "Plastic":
                break;
        }        
    }

    

}
