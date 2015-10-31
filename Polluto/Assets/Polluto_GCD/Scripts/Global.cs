using UnityEngine;
using System.Collections;

public static class Global{

public static Vector2 ToV2(this Vector3 v){
Vector2 temp;
    temp.x = v.x;
    temp.y = v.y;
    return temp;

}

    

}
