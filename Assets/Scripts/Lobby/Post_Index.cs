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

    public void SetPostIndex(string name, string title, string date,UnityEvent action)
    {
        postname.text = name;
        posttitle.text = title;
        postdate.text = date;
        postbutton.onClick.AddListener(() => action.Invoke());

    }
}
