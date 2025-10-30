using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    [Header("References")]
    public Slider zoomSlider;
    public ObjectRotateZoom_Mobile zoomTarget; // script điều khiển zoom

    void Start()
    {
        if(zoomSlider != null && zoomTarget != null)
        {
            zoomSlider.minValue = zoomTarget.minDistance;
            zoomSlider.maxValue = zoomTarget.maxDistance;

            // đảo ngược: giá trị slider ban đầu = khoảng cách hiện tại
            float normalized = (zoomTarget.CurrentDistance - zoomTarget.minDistance) / (zoomTarget.maxDistance - zoomTarget.minDistance);
            zoomSlider.value = zoomTarget.maxDistance - normalized * (zoomTarget.maxDistance - zoomTarget.minDistance);

            zoomSlider.onValueChanged.AddListener(OnSliderChanged);
        }
    }

    private void OnSliderChanged(float value)
    {
        if(zoomTarget != null)
        {
            zoomTarget.SetZoom(value);
        }
    }
}