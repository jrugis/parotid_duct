using UnityEngine;
public class make_discs : MonoBehaviour
{
   public GameObject disc;
     
   void Start()
   {
       for (int i=0; i<5; i++)
       {
           GameObject obj = Instantiate(disc, this.transform);
           //GameObject obj = Instantiate(disc, new Vector3(x,y,0), Quaternion.identity);
           //obj.transform.SetParent(this.transform);
       }       
   }
}

/*
    foreach(Transform child in transform)
    {
        Something(child.gameObject);
    }
position, scale, orientation
vertex colors???
cast shadows OFF
recieve shadows OFF
*/