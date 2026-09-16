using UnityEngine;

public class HeroSelection : MonoBehaviour
{
    [SerializeField] private GameObject selectionRing;

    private bool isSelected = false;

    public bool IsSelected
    {
        get { return isSelected; }
    }

    void Start()
    {
        SetSelected(false);
    }

    public void SetSelected(bool selected)
    {
        isSelected = selected;

        if (selectionRing != null)
        {
            selectionRing.SetActive(selected);
        }
    }
}