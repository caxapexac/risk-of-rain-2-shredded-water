using Moonstorm.Starstorm2;
using RoR2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSelector : MonoBehaviour
{
    private GameObject _selection;

    public float UpdateInterval = 0.5f;
    private float _updateDelay = 0.0f;

    public float Radius = 100.0f;
    public Ray Ray;
    public LayerMask LayerMask = Physics.AllLayers;
    public QueryTriggerInteraction QueryTriggerInteraction = QueryTriggerInteraction.UseGlobal;

    public CharacterBody Selected;
    public TeamMask TeamMask;

    private void Awake()
    {
        _selection = GameObject.CreatePrimitive(PrimitiveType.Cube);
        _selection.transform.SetParent(transform, false);
        _selection.transform.localPosition = Vector3.zero;
        foreach (Collider component in _selection.GetComponents<Collider>())
        {
            Destroy(component);
        }
        foreach (Rigidbody component in _selection.GetComponents<Rigidbody>())
        {
            Destroy(component);
        }
    }

    private void OnDestroy()
    {
        Destroy(_selection);
    }

    private void Update()
    {
        if (Selected != null)
        {
            _selection.transform.position = Selected.transform.position;
        }
        else
        {
            _selection.transform.position = transform.position;
        }
    }

    private void FixedUpdate()
    {
        _updateDelay -= Time.fixedDeltaTime;
        if (_updateDelay > 0)
            return;
        _updateDelay = UpdateInterval;
        Selected = Select();

        SS2Log.Warning($"{nameof(TargetSelector)}.{nameof(FixedUpdate)} {(Selected == null ? "none" : Selected.name)}");
    }

    private CharacterBody Select()
    {
        Collider[] array = Physics.OverlapSphere(Ray.origin, Radius, LayerMask, QueryTriggerInteraction);
        if (array.Length == 0)
            return null;

        for(int index = 0; index < array.Length; index++)
        {
            CharacterBody bestBody = Validate(array[index]);
            if (bestBody == null)
                continue;

            float bestSerchScore = Estimate(bestBody);

            for (; index < array.Length; index++)
            {
                CharacterBody body = Validate(array[index]);
                if (body == null)
                    continue;

                float serchScore = Estimate(body);
                if (serchScore < bestSerchScore)
                    continue;
                bestBody = body;
                bestSerchScore = serchScore;
            }

            return bestBody;
        }

        return null;
    }

    private CharacterBody Validate(Collider collider)
    {
        CharacterBody body = collider.GetComponentInParent<CharacterBody>();
        if(body == null)
            return null;

        Vector3 localPosition = collider.transform.position - Ray.origin;
        float distance = Vector3.Dot(Ray.direction, localPosition);
        if (distance < 0)
            return null;

        if (!TeamMask.HasTeam(body.teamComponent.teamIndex))
            return null;

        return body;
    }

    private float Estimate(CharacterBody body)
    {
        Vector3 localPosition = body.transform.position - Ray.origin;
        float distance = Vector3.Dot(Ray.direction, localPosition);
        Vector3 projection = Ray.GetPoint(distance);
        return distance + (localPosition - projection).sqrMagnitude / (distance);
    }
}
