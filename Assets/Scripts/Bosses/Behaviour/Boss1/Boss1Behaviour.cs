using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Behaviour : MonoBehaviour
{
    // Debugging
    [HideInInspector] public Renderer rend;

    // stages
    private BaseStage currentStage;
    public InitialStage initStage = new InitialStage();
    public MiddleStage middleStage = new MiddleStage();
    public FinalStage finalStage = new FinalStage();

    // stats
    public float health;
    public float baseDamage;

    void Awake() {
        rend = GetComponent<Renderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentStage = initStage;
        currentStage.EnterStage(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentStage.UpdateStage(this);
    }

    void FixedUpdate() 
    {
        // handoff fixedupdate to current state
        currentStage.FixedUpdateStage(this);
    }

    // handle state switching
    public void SwitchStage(BaseStage stage) {
        currentStage.ExitStage(this);
        currentStage = stage;
        currentStage.EnterStage(this);
    }
}
