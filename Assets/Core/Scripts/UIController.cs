using UnityEngine;
using UnityEngine.UI;
using AppsFlyerSDK;

public class UIController : MonoBehaviour
{
    [SerializeField] private Text _data;
    [SerializeField] private Text _scoreUI;
    [SerializeField] private GameObject[] _pages;
    private int _count;

    public void ChangeMenu(int index)
    {
        foreach (var page in _pages)
            page.SetActive(false);
        _pages[index].SetActive(true);
    }

    public void ChangeScore(int index)
    {
        _count += index;
        _scoreUI.text = $"Score: {_count}";
    }

    public void RefreshScore()
    {
        _count = 0;
        _scoreUI.text = $"Score: {_count}";
    }
}