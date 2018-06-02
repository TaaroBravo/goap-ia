using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class NPC_B : NPC
{
    public GameObject target;

    public float life;

    public float speed = 4f;
    public float rotationSpeed = 5f;
    public float distanceToShoot;
    public float fireRate;
    public float timerRecalculateEnemy;

    private float _distanceToTarget;
    private float _timerFireRate;
    private float _yPos;

    private float _coeShoot;
    private float _coePursuit;
    private float _coeFlee;

    float coeDistanceToTarget = 5;
    float coeLife = 20f;
    float coeShoot = 50f;

    int decisionIndex;
    int oldDecisionIndex;

    public float _timerToChange;

    Node oldNode;
    float timerNode;

    StateMachine _sm;

    public virtual void Start()
    {
        target = GameObject.Find("NPC_A");
        _sm = new StateMachine();
        _sm.AddState(new B_PursuitState(_sm, this));
        _sm.AddState(new B_ShootState(_sm, this));
        _sm.AddState(new B_FleeState(_sm, this));
        _timerFireRate = 1.5f;
        life = 20;
        fireRate = 1;
        decisionIndex = -1;
        distanceToShoot = 7;
        _yPos = transform.position.y;
        if(target)
            nodeNearTarget = CalculateTargetNode(target.transform.position);
    }

    public override void Update()
    {
        base.Update();
        if (!target)
            target = GameObject.Find("NPC_A");
        else
            _distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

        timerRecalculateEnemy += Time.deltaTime;
        if (timerRecalculateEnemy > 2)
        {
            target = GameObject.Find("NPC_A");
            timerRecalculateEnemy = 0;
        }

        StartCoroutine(DecisionIndexCorutine());

        if (_timerFireRate < fireRate)
            _timerFireRate += Time.deltaTime;

        _timerToChange += Time.deltaTime;
        if (_timerToChange > 3f)
        {
            _sm.SetState<B_PursuitState>();
            _sm.Update();
            _timerToChange = 0;
        }

        if (life <= 0)
            Destroy(gameObject);

        foreach (var bullet in Physics.OverlapSphere(transform.position, 0.5f))
        {
            if (bullet.gameObject.tag == "BulletA")
            {
                Destroy(bullet.gameObject);
                life -= 7;
            }
        }

        timerNode += Time.deltaTime;
        if (timerNode > 3)
        {
            Node actualNode = CalculateTargetNode(transform.position);
            if (oldNode == actualNode && target)
            {
                MoveTo(target.transform.position);
            }
            oldNode = actualNode;
            timerNode = 0;
        }

        transform.position = new Vector3(transform.position.x, _yPos, transform.position.z);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
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
                    _sm.SetState<B_PursuitState>();
                    break;
                case 1:
                    _sm.SetState<B_ShootState>();
                    break;
                case 2:
                    _sm.SetState<B_FleeState>();
                    break;
            }
            if(target)
            {
                if (nodeNearTarget != CalculateTargetNode(target.transform.position) || oldDecisionIndex != decisionIndex || _timerToChange > 1.5f)
                {
                    _timerToChange = 0;
                    oldDecisionIndex = decisionIndex;
                    nodeNearTarget = CalculateTargetNode(target.transform.position);
                    if (decisionIndex != 1)
                        _sm.Update();
                    else
                    {
                        if (_timerFireRate >= fireRate)
                        {
                            _sm.Update();
                            _timerFireRate = 0;
                        }
                    }
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }

    private void FindAnotherNPC(params object[] parameters)
    {
        if (GameObject.Find("NPC_A") != null)
            MoveTo(GameObject.Find("NPC_A").transform.position);
    }


    #region Coeficientes
    public List<float> CoefValues()
    {
        List<float> list = new List<float>();

        _coePursuit = _distanceToTarget * coeDistanceToTarget;

        _coeFlee = (1 / life) * coeLife;

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

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.gameObject.tag == "BulletA")
            life -= 5f;
    }

}
