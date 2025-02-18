using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class inGameSettings : MonoBehaviour
{
    [Header("other scripts")]
    public Audio_sampler_Final _audioSampler;
    

    [Header("Db Change")]
    public Text DbText;
    public InputField minDbField;
    public InputField maxDbField;

    [Header("refVal Scale")]
    public Slider refValSlider;
    public Text refValText;

    [Header("lowPass adjusters")]
    public Text lowPassText;
    public Slider lowPassSlider;
    public InputField moveSmotherField;

    [Header("dbIndicator")]
    public Image dbIndicator;

    private void OnEnable()
    {
        //minDbField.text = _ballonieManager.minDb.ToString();
        //maxDbField.text = _ballonieManager.maxDb.ToString();
        moveSmotherField.text = _audioSampler.lowPassFactor.ToString();
    }
    private void Update()
    {
        showDb();
        setDb45_90();
    }
    void showDb()
    {
        if (gameObject.activeSelf)
        {
            DbText.text = _audioSampler.dbVal.ToString();
        }
    }
    public void refValueChanger()
    {
        if (refValSlider.value > 0 && refValSlider.value < 0.1)
            _audioSampler.RefValue = 0.1f;
        if (refValSlider.value > 0.1 && refValSlider.value < 0.2)
            _audioSampler.RefValue = 0.05f;
        if (refValSlider.value > 0.2 && refValSlider.value < 0.3)
            _audioSampler.RefValue = 0.01f;
        if (refValSlider.value > 0.3 && refValSlider.value < 0.4)
            _audioSampler.RefValue = 0.005f;
        if (refValSlider.value > 0.4 && refValSlider.value < 0.5)
            _audioSampler.RefValue = 0.001f;
        if (refValSlider.value > 0.5 && refValSlider.value < 0.6)
            _audioSampler.RefValue = 0.0005f;
        if (refValSlider.value > 0.6 && refValSlider.value < 0.7)
            _audioSampler.RefValue = 0.0001f;
        if (refValSlider.value > 0.7 && refValSlider.value < 0.8)
            _audioSampler.RefValue = 0.00005f;
        if (refValSlider.value > 0.8 && refValSlider.value < 0.9)
            _audioSampler.RefValue = 0.00001f;
        if (refValSlider.value > 0.9 && refValSlider.value < 1)
            _audioSampler.RefValue = 0.000005f;
        refValText.text = "Ref: " + _audioSampler.RefValue.ToString();
    }

    public void movementSmoother()
    {
        if (lowPassSlider.value < 1)
            _audioSampler.lowPassFactor = lowPassSlider.value;
        lowPassText.text = "Mov Smoother: " + _audioSampler.lowPassFactor.ToString();
    }

    public void setMoveMentSmoother()
    {
        _audioSampler.lowPassFactor = float.Parse(moveSmotherField.text);
    }

    public void setDb()
    {
        //_ballonieManager.minDb = float.Parse( minDbField.text);
        //_ballonieManager.maxDb = float.Parse(maxDbField.text);
    }

    void setDb45_90()
    {
        if (!gameObject.activeSelf)
            return;

        float val = convertBetweenTwoScales(_audioSampler.dbVal, 20, 60, 0, 1);

        dbIndicator.fillAmount = val;

    }

    public float convertBetweenTwoScales(float oldVal, float firstScaleMin, float firstScaleMax, float secondScaleMin, float secondScaleMax)
    {
        float firstScaleLength = firstScaleMax - firstScaleMin;
        float secondScaleLength = secondScaleMax - secondScaleMin;

        //shift to origin
        float originShift = oldVal - firstScaleMin;
        //normalize
        float normalizedVal = originShift / firstScaleLength;
        //upscale 
        float upscaleVal = normalizedVal * secondScaleLength;

        //shft form origin
        float newVal = upscaleVal + secondScaleMin;

        return newVal;


    }
}
