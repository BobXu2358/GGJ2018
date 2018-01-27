using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface EnemyBaseState {
    // Update is called once per frame
    void Update();
    void HandleInput();
}
