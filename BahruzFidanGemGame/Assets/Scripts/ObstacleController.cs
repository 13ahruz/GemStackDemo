using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ObstacleController : MonoBehaviour
{
    [SerializeField]
    private List<Transform> startGemRedMakers;
    [SerializeField]
    private List<Transform> sawObstacles;
    [SerializeField]
    private List<Transform> finishMachineLeft;
    [SerializeField]
    private List<Transform> finishMachineRight;





    void Start()
    {
        var seq = DOTween.Sequence();
        foreach (Transform startGemRedMaker in startGemRedMakers)
        {
            startGemRedMaker.transform.DOLocalMoveY(0.6f, 0.12f).SetLoops(-1, LoopType.Yoyo);
        }
        foreach (Transform saw in sawObstacles)
        {
            saw.transform.DORotate(new Vector3(0, 0, 180), 1f).SetLoops(-1, LoopType.Restart);
        }
        foreach (Transform machine in finishMachineLeft)
        {
            machine.DOMoveX(-1.2f, 0.3f).SetLoops(-1, LoopType.Yoyo);
        }
        foreach (Transform machine in finishMachineRight)
        {
            machine.DOMoveX(1.2f, 0.3f).SetLoops(-1, LoopType.Yoyo);
        }



    }


    void Update()
    {

    }
}
