using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SwitchButton : MonoBehaviour
{
    [SerializeField] public Sprite image1;
    [SerializeField] public Sprite image2;
    [SerializeField] public Image image;
    
    private Button button;
    public bool isOn = true;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(button == null) button = GetComponent<Button>();
        if(image == null) image = GetComponentInChildren<Image>();
        image.sprite = image1;
        button.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        isOn = !isOn;
        if(isOn) image.sprite = image1;
        else image.sprite = image2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
