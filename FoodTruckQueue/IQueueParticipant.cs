using System;
using Spirit604.DotsCity.Simulation.TrafficPublic;
using UnityEngine;

public interface IQueueParticipant 
{
    public event EventHandler OnActionStart;
    public event EventHandler OnActionEnd;

    public void StartAction();
    public void EndAction();

    public void Move();
    public void Stop();
}
