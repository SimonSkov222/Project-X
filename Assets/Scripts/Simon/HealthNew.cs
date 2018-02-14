using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface HealthNew {
    int HealthMax { get; set; }
    int Health { get; set; }

    void OnDeath();

}
