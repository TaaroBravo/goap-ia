using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class A_Lider : NPC_A
{
    public float distanceToShoot;
    public float fireRate;

    private float _distanceToTarget;
    private float _timerFireRate;
    private float _timerToChange;

    private float _coeShoot;
    private float _coePursuit;
    private float _coeFlee;

    float coeDistanceToTarget = 1;
    float coeLife = 20f;
    float coeShoot = 60f;
    float totalNPCSLife;

    int decisionIndex;
    int oldDecisionIndex;

    Node oldNode;
    float timerNode;

    StateMachine _sm;

    public override void Start()
    {
        base.Start();
        _sm = new StateMachine();
        _sm.AddState(new A_PursuitState(_sm, this));
        _sm.AddState(new A_ShootState(_sm, this));
        _sm.AddState(new A_FleeState(_sm, this));
        _timerFireRate = 1.5f;
        fireRate = 1;
        decisionIndex = -1;
        distanceToShoot = 7;
        A_ArrayNPC.Add(this);
        if(target)
            nodeNearTarget = CalculateTargetNode(target.transform.position);
        timerReCalculatePathFinding = -1;
    }

    public override void Update()
    {
        base.Update();

        if (!oldNode)
            oldNode = CalculateTargetNode(transform.position);

        if (target)
            _distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
        StartCoroutine(DecisionIndexCorutine());

        if (_timerFireRate < fireRate)
            _timerFireRate += Time.deltaTime;

        _timerToChange += Time.deltaTime;
        timerNode += Time.deltaTime;
        if (_timerToChange > 3f)
        {
            _sm.SetState<A_PursuitState>();
            _sm.Update();
            _timerToChange = 0;
        }

        foreach (var bullet in Physics.OverlapSphere(transform.position, 0.5f))
        {
            if (bullet.gameObject.tag == "BulletB")
            {
                Destroy(bullet.gameObject);
                life -= 7;
            }
        }
        if(timerNode > 3)
        {
            Node actualNode = CalculateTargetNode(transform.position);
            if (oldNode == actualNode && target)
            {
                MoveTo(target.transform.position);
            }
            oldNode = actualNode;
            timerNode = 0;
        }
        
    }

    IEnumerator DecisionIndexCorutine()
    {
        while (true)
        {
            List<float> coefValues = CoefValues();
            decisionIndex = RouletteWheelSelection(coefValues);
            switch (decisionIndex)
            {
                case 0:
                    _sm.SetState<A_PursuitState>();
                    attack = false;
                    break;
                case 1:
                    _sm.SetState<A_ShootState>();
                    attack = true;
                    break;
                case 2:
                    _sm.SetState<A_FleeState>();
                    attack = false;
                    break;
            }
            if(target)
            {
                if (nodeNearTarget != CalculateTargetNode(target.transform.position) || oldDecisionIndex != decisionIndex || _timerToChange > 3f)
                {
                    _timerToChange = 0;
                    oldDecisionIndex = decisionIndex;
                    nodeNearTarget = CalculateTargetNode(target.transform.position);
                    if (decisionIndex != 1)
                        _sm.Update();
                    else
                    {
                        if (_timerFireRate > fireRate)
                        {
                            _sm.Update();
                            _timerFireRate = 0;
                        }
                    }
                }
            }
            yield return new WaitForSeconds(3f);
        }
    }

    private void FindAnotherNPC(params object[] parameters)
    {
        if(GameObject.Find("NPC_B") != null)
            MoveTo(GameObject.Find("NPC_B").transform.position);
    }

    #region Coeficientes
    public List<float> CoefValues()
    {
        List<float> list = new List<float>();

        _coePursuit = _distanceToTarget * coeDistanceToTarget;

        foreach (var npc in A_ArrayNPC)
            totalNPCSLife += npc.life;
        _coeFlee = (1 / (totalNPCSLife / A_ArrayNPC.Count)) * coeLife;

        if (_distanceToTarget <= distanceToShoot)
            _coeShoot = _distanceToTarget * coeShoot;
        else
            _coeShoot = 0;

        list.Add(_coePursuit);
        list.Add(_coeShoot);
        list.Add(_coeFlee);

        return list;
    }
    #endregion

    #region RWS
    public static int RouletteWheelSelection(List<float> values)
    {
        float sum = 0;

        sum = values.Sum();

        List<float> coefList = new List<float>();

        foreach (var coef in values)
        {
            coefList.Add(coef / sum);
        }

        int random = Random.Range(0, 10);
        float selectedNumber = random / 10f;

        float sumCoef = 0;
        for (int i = 0; i < values.Count; i++)
        {
            sumCoef += coefList[i];

            if (sumCoef > selectedNumber)
                return i;
        }

        return -1;
    }
    #endregion


}


