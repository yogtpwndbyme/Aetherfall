using UnityEngine;

public class UnitInfoData : MonoBehaviour
{
    [Header("Unit Info")]
    [SerializeField] private string displayName = "Unit";
    [SerializeField] private string title = "";
    [SerializeField] private Sprite portrait;

    public string DisplayName
    {
        get { return displayName; }
    }

    public string Title
    {
        get { return title; }
    }

    public Sprite Portrait
    {
        get { return portrait; }
    }
}