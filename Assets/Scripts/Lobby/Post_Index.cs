using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Post_Index : MonoBehaviour
{
    [SerializeField] private Image postprofile;
    [SerializeField] private Image itemprofile;
    [SerializeField] private TMP_Text postname;
    [SerializeField] private TMP_Text posttitle;
    [SerializeField] private TMP_Text postdate;
    [SerializeField] private Button postbutton;
    [SerializeField] private Button postexpendbutton;

    public void SetPostIndex(string name, string title, string date, UnityEvent action)
    {
        if (postname != null)
        {
            postname.text = name;
        }
    
        if (posttitle != null)
        {
            posttitle.text = title;
        }
    
        if (postdate != null)
        {
            postdate.text = date;
        }
    
        if (postexpendbutton != null && action != null)
        {
            postexpendbutton.onClick.AddListener(() => action.Invoke());
        }
    }
}
