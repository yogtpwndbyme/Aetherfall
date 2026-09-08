using UnityEngine;
using UnityEngine.InputSystem;

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
        selectionRing.SetActive(false);
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            HandleSelection();
        }
    }

    private void HandleSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.transform == transform)
            {
                SelectHero();
            }
            else if (hit.transform.CompareTag("Enemy"))
            {
                // Behåll Hero markerad när vi klickar på en fiende.
                return;
            }
            else
            {
                DeselectHero();
            }
        }
    }

    private void SelectHero()
    {
        isSelected = true;
        selectionRing.SetActive(true);
    }

    private void DeselectHero()
    {
        isSelected = false;
        selectionRing.SetActive(false);
    }
}