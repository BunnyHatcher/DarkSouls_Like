using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    [SerializeField] private CinemachineTargetGroup cineTargetGroup;
    
    private List<Target> targets = new List<Target>();

    public Target CurrentTarget { get; private set; }

   
    
    
    // Add Target to Target list, if Targeter collider overlaps with collider box of target
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Target>(out Target target))
        {
            targets.Add(target);
            
            target.OnDestroyed += RemoveTarget; // if target is destroyed: subscribe to RemoveTarget method 
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Target>(out Target target))
        {
            RemoveTarget(target); // if target is out of range: subscribe to RemoveTarget method as well
        }
    }

    public bool SelectTarget()
    {
        if (targets.Count == 0) { return false; }

        CurrentTarget = targets[0];
        cineTargetGroup.AddMember(CurrentTarget.transform, 1f, 2f);

        return true;
    }

    //method for cancelling targeting
    public void Cancel()
    {
        if (CurrentTarget == null) { return; }

        cineTargetGroup.RemoveMember(CurrentTarget.transform);
        CurrentTarget = null;
    }
    
    //method to remove current target from list of targets
    private void RemoveTarget(Target target)
    {
        if(CurrentTarget == target)
        {
            cineTargetGroup.RemoveMember(CurrentTarget.transform);
            CurrentTarget = null;
        }

        target.OnDestroyed -= RemoveTarget;
        targets.Remove(target);
    }


}
