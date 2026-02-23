using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRideable
{
    void Attach(GameObject attachObject);
    void Detach(GameObject detachObject);
}