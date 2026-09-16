using UnityEngine;

public class PlaceholderUnitGenerator : MonoBehaviour
{
    [ContextMenu("Generate Placeholder Units")]
    public void GenerateUnits()
    {
        CreateUnit("Worker", new Vector3(-6f, 0f, 0f), 0.85f);
        CreateUnit("Melee", new Vector3(-3f, 0f, 0f), 1.0f);
        CreateUnit("Ranged", new Vector3(0f, 0f, 0f), 0.95f);
        CreateUnit("Mage", new Vector3(3f, 0f, 0f), 1.0f);
        CreateUnit("Tharokh_Heavy", new Vector3(6f, 0f, 0f), 1.35f);
    }

    private void CreateUnit(string unitName, Vector3 position, float scale)
    {
        GameObject root = new GameObject(unitName);
        root.transform.position = position;

        // Body
        CreatePart(
            PrimitiveType.Capsule,
            "Body",
            root.transform,
            new Vector3(0f, 1.15f, 0f),
            new Vector3(0.75f, 1.15f, 0.55f) * scale
        );

        // Head
        CreatePart(
            PrimitiveType.Sphere,
            "Head",
            root.transform,
            new Vector3(0f, 2.25f * scale, 0f),
            Vector3.one * 0.55f * scale
        );

        // Left arm
        CreatePart(
            PrimitiveType.Cube,
            "Arm_L",
            root.transform,
            new Vector3(-0.65f * scale, 1.25f * scale, 0f),
            new Vector3(0.35f, 1.25f, 0.4f) * scale
        );

        // Right arm
        CreatePart(
            PrimitiveType.Cube,
            "Arm_R",
            root.transform,
            new Vector3(0.65f * scale, 1.25f * scale, 0f),
            new Vector3(0.35f, 1.25f, 0.4f) * scale
        );

        // Left leg
        CreatePart(
            PrimitiveType.Cube,
            "Leg_L",
            root.transform,
            new Vector3(-0.25f * scale, 0.35f * scale, 0f),
            new Vector3(0.4f, 0.9f, 0.5f) * scale
        );

        // Right leg
        CreatePart(
            PrimitiveType.Cube,
            "Leg_R",
            root.transform,
            new Vector3(0.25f * scale, 0.35f * scale, 0f),
            new Vector3(0.4f, 0.9f, 0.5f) * scale
        );

        // Extra visual identifiers
        if (unitName == "Melee")
            CreateSword(root.transform, scale);

        if (unitName == "Ranged")
            CreateBow(root.transform, scale);

        if (unitName == "Mage")
            CreateStaff(root.transform, scale);

        if (unitName == "Tharokh_Heavy")
            CreateHeavyShoulders(root.transform, scale);
    }

    private GameObject CreatePart(
        PrimitiveType type,
        string partName,
        Transform parent,
        Vector3 localPosition,
        Vector3 localScale)
    {
        GameObject part = GameObject.CreatePrimitive(type);

        part.name = partName;
        part.transform.SetParent(parent);
        part.transform.localPosition = localPosition;
        part.transform.localScale = localScale;

        return part;
    }

    private void CreateSword(Transform parent, float scale)
    {
        CreatePart(
            PrimitiveType.Cube,
            "Sword",
            parent,
            new Vector3(0.95f * scale, 1.1f * scale, 0f),
            new Vector3(0.12f, 1.5f, 0.12f) * scale
        );
    }

    private void CreateBow(Transform parent, float scale)
    {
        GameObject bow = CreatePart(
            PrimitiveType.Cube,
            "Bow",
            parent,
            new Vector3(-0.95f * scale, 1.2f * scale, 0f),
            new Vector3(0.1f, 1.5f, 0.1f) * scale
        );

        bow.transform.localRotation = Quaternion.Euler(0f, 0f, -15f);
    }

    private void CreateStaff(Transform parent, float scale)
    {
        CreatePart(
            PrimitiveType.Cylinder,
            "Staff",
            parent,
            new Vector3(0.9f * scale, 1.1f * scale, 0f),
            new Vector3(0.12f, 1.1f, 0.12f) * scale
        );
    }

    private void CreateHeavyShoulders(Transform parent, float scale)
    {
        CreatePart(
            PrimitiveType.Sphere,
            "Shoulder_L",
            parent,
            new Vector3(-0.8f * scale, 1.75f * scale, 0f),
            new Vector3(0.8f, 0.6f, 0.7f) * scale
        );

        CreatePart(
            PrimitiveType.Sphere,
            "Shoulder_R",
            parent,
            new Vector3(0.8f * scale, 1.75f * scale, 0f),
            new Vector3(0.8f, 0.6f, 0.7f) * scale
        );
    }
}