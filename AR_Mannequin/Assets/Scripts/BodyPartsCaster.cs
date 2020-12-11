using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
/// <summary>
/// Created by Silver Xu 2020
/// Running a sphere cast to detect what part of the manikin's body the user is close to,
/// and calling vitals manager to display proper UI
/// </summary>
public class BodyPartsCaster : MonoBehaviour
{
    private Vector3 origin;
    private Vector3 direction;
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private float radius;
    [SerializeField]
    private float maxDistance;
    [SerializeField]
    private LayerMask layerMask;

    private TriggerVitalUI currentTriggerVital;

    private void FixedUpdate()
    {
        origin = mainCamera.transform.position;
        direction = mainCamera.transform.forward;
        RaycastHit[] hits = Physics.SphereCastAll(origin, radius, direction, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal);
        if (hits != null && hits.Count() > 0)
        {
            //get the nearset raycast hit
            RaycastHit nearestHit = hits.OrderBy(hit => hit.distance).First();

            VitalsManager.Instance.VitalsUIControlBasedOnUserPosition(ParseColliderName(nearestHit.transform.tag));
            
        }
        else
        {
            VitalsManager.Instance.VitalsUIControlBasedOnUserPosition(UserPosition.none);
        }
        
    }

    // Parse Body parts position based on its tag
    private UserPosition ParseColliderName(string colliderTagName)
    {
        UserPosition userPosition;
        string positionName = colliderTagName.Split('_')[1];
        
        if(System.Enum.TryParse<UserPosition>( positionName, true, out userPosition))
        {
            return userPosition;
        }
        return UserPosition.none;
    }
}
