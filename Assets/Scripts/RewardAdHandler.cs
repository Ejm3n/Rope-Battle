using UnityEngine;
using YG;

public class RewardAdHandler : MonoBehaviour
{
    private void OnEnable()
    {
        // Подписываемся на события открытия и закрытия Rewarded Ad
        YandexGame.OpenVideoEvent += OnRewardedAdOpened;
        YandexGame.CloseVideoEvent += OnRewardedAdClosed;
    }

    private void OnDisable()
    {
        // Отписываемся от событий
        YandexGame.OpenVideoEvent -= OnRewardedAdOpened;
        YandexGame.CloseVideoEvent -= OnRewardedAdClosed;
    }
    public void ShowRewardAd()
    {
        Time.timeScale = 0;
        if (YandexGame.SDKEnabled) // Проверяем, что SDK готово
        {
            // Останавливаем игру
            YandexGame.RewVideoShow(0); // Показ рекламы с ID = 0

        }
        else
        {
            Debug.Log("SDK ещё не готово к показу рекламы.");
        }
    }

    public void ShowAd()
    {
        if (YandexGame.SDKEnabled) // Проверяем, что SDK готово
        {
            // Останавливаем игру
            YandexGame.FullscreenShow(); // Показ рекламы с ID = 0

        }
        else
        {
            Debug.Log("SDK ещё не готово к показу рекламы.");
        }
    }
    private void OnRewardedAdOpened()
    {
        // Остановка игры при показе рекламы
        Debug.Log("Rewarded Ad открыта");
        Time.timeScale = 0; // Остановка игрового времени
    }

    private void OnRewardedAdClosed()
    {
        // Возобновление игры после закрытия рекламы
        Debug.Log("Rewarded Ad закрыта");
    }
}
