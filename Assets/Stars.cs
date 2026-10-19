/* Script made by: velozdev 
Please subscribe to support the channel and get more free scripts like this one: https://www.youtube.com/@velozdev 
 Follow on X: https://x.com/velozdev */

using UnityEngine;
using TMPro;

public class Stars : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI starsText;
   public static int stars;

   void Start()
    {
        stars = PlayerPrefs.GetInt("Stars", 0);
    }

    void Update()
    {
        starsText.text = stars.ToString();
    }
}
