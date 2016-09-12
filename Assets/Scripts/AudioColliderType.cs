using UnityEngine;
using System.Collections;

public class AudioColliderType : MonoBehaviour {

    public enum Mode
    {
        E_BLOCK,
        E_WATER,
        E_GRESS,
        E_WOOD,
        E_SNOW,
        E_GRAVEL,
        E_METAL,
        E_PLASTIC
    }

    public Mode terrianType;
	
    public string GetTerrianType()
    {
        string typeString = "";

        switch(terrianType)
        {
            case Mode.E_BLOCK :
                typeString = "Block";
                break;
            case Mode.E_WATER:
                typeString = "Water";
                break;
            case Mode.E_GRESS:
                typeString = "Gress";
                break;
            case Mode.E_WOOD:
                typeString = "Wood";
                break;
            case Mode.E_SNOW:
                typeString = "Snow";
                break;
            case Mode.E_GRAVEL:
                typeString = "Gravel";
                break;
            case Mode.E_METAL:
                typeString = "Metal";
                break;
            case Mode.E_PLASTIC:
                typeString = "Plastic";
                break;                
        }

        return typeString;
    }
}
