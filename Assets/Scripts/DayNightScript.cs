using UnityEngine;

public class DayNightScript : MonoBehaviour
{
    private float dayDuration = 1000.0f;
    private float hour;
    private float dawnTime = 4.0f;
    private Material skybox;
    private Light sun;
    private Light moon;

    void Start() {
        hour = 12.0f;
        skybox = RenderSettings.skybox;
        sun = transform.Find("Sun").GetComponent<Light>();
        moon = transform.Find("Moon").GetComponent<Light>();
    }

    void Update() {
        float dTime = Time.deltaTime / dayDuration;
        hour += 24.0f * dTime;
        if(hour >= 24.0f) {
            hour -= 24.0f;
        }
        GameState.gameTimeHour = hour;

        float coef;
        if(hour >= dawnTime && hour < 24.0f - dawnTime) { // дневная фаза
            coef = Mathf.Sin((hour - dawnTime) * Mathf.PI / (24.0f - 2.0f * dawnTime));
            sun.intensity = coef;
            if(RenderSettings.sun != sun) { // переход - перша зміна на день
                RenderSettings.sun = sun;
                moon.intensity = 0;
            }
        }
        else { // ночная фаза
            float arg = hour < dawnTime ? hour : hour - 24.0f;
            coef = 0.3f * Mathf.Sin((arg - (-dawnTime)) * Mathf.PI / (2.0f * dawnTime));
            moon.intensity = coef;
            if(RenderSettings.sun != moon) {
                RenderSettings.sun = moon;
                sun.intensity = 0;
            }
        }
        RenderSettings.ambientIntensity = coef + 0.1f;
        skybox.SetFloat("_Exposure", coef);
        
        this.transform.Rotate(0, 0, -360.0f * dTime);
    }
}
