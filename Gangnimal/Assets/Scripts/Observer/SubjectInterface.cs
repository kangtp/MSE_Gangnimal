using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface SubjectInterface
{
    void RegisterObserver(Observerinterface observer);
    void RemoveObserver(Observerinterface observer);
    void NotifyObservers();
}
