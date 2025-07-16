using System;
using Spirit604.DotsCity.Simulation.TrafficPublic;
using UnityEngine;

public interface IQueueParticipant 
{
    public event EventHandler OnActionStart;
    public event EventHandler OnActionEnd;
    public event EventHandler OnActionPointReached;

    public void StartAction();
    public void EndAction();

    // I need to include this function because I am using Location Reached trigger
    public void NotifyLocationReached();
    public void SetRightHandIKTarget(Transform target);

    

    public void Move();
    public void Stop();
}
