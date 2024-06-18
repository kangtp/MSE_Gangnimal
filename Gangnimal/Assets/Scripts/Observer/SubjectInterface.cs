using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface SubjectInterface // Subject interface
{
    void RegisterObserver(Observerinterface observer);//add observer
    void RemoveObserver(Observerinterface observer); // remove observer
    void NotifyObservers(); // Update Observer
}
