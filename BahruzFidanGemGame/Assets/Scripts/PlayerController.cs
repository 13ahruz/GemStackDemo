using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using TMPro;
using CASP.CameraManager;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    [SerializeField] float Horizontal;
    public float VerticalSpeed;
    public float SpeedMultiplier;
    public List<Transform> Coffees;
    [SerializeField] float OffsetZ = 1;
    [SerializeField] float LerpSpeed = 1;
    [SerializeField] Material diamondMat;
    //[SerializeField] FloatingJoystick joystick;
    [SerializeField] Transform finishMoney;
    private bool moveTest = false;
    [SerializeField]
    private CinemachineVirtualCamera vCam;
    [SerializeField]
    private List<Transform> ringBoxesLeft;
    [SerializeField]
    private List<Transform> ringBoxesRight;
    private bool gameFinished = false;
    private Touch touch;
    [SerializeField]
    private TMP_Text finishCount;




    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {

        // Coffees.Add(transform.GetChild(0));
        CollectedCoffeeData.instance.CoffeeList.Add(transform.GetChild(0));


    }

    void Update()
    {

        moveMoney();
        // if (!CollectedCoffeeData.instance.inPool)
        // {
        //     Horizontal = Input.GetAxis("Horizontal");
        // }
        //Horizontal = joystick.Horizontal;
        transform.position += Vector3.forward * SpeedMultiplier * Time.deltaTime;
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                //float clampLimit = Mathf.Clamp(transform.position.x, -1.66f, 1.66f);
                transform.position = new Vector3(transform.position.x + touch.deltaPosition.x * 0.004f, transform.position.y, transform.position.z);
                transform.position = new Vector3(Mathf.Clamp(transform.position.x, -1.66f, 1.66f), transform.position.y, transform.position.z);
            }
        }
        if (CollectedCoffeeData.instance.CoffeeList.Count > 1)
        {
            CoffeeFollow();
        }




    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectable"))
        {
            CollectedCoffeeData.instance.pitch += 0.2f;
            CollectedCoffeeData.instance.nextPitchTime = Time.time + 1;
            SoundManager.instance.Play("Collect");
            other.tag = "GemStart";
            CollectedCoffeeData.instance.CoffeeList.Add(other.transform);
            other.gameObject.AddComponent<CollectedCoffee>();
            other.gameObject.AddComponent<Rigidbody>().isKinematic = true;

            // var seq = DOTween.Sequence();
            var seq = DOTween.Sequence();
            for (int i = CollectedCoffeeData.instance.CoffeeList.Count - 1; i >= 0; i--)
            {
                seq.Join(CollectedCoffeeData.instance.CoffeeList[i].DOScale(1.5f, 0.06f));
                seq.AppendInterval(0.02f);
                seq.Join(CollectedCoffeeData.instance.CoffeeList[i].DOScale(1f, 0.06f));
            }
        }
        if (other.CompareTag("PoolStart"))
        {
            CollectedCoffeeData.instance.pitch += 0.2f;
            CollectedCoffeeData.instance.nextPitchTime = Time.time + 1;
            SoundManager.instance.Play("Water");
            var seq1 = DOTween.Sequence();
            seq1.Append(transform.DOMoveY(-3.44f, 0.3f))
            .OnComplete(() => { transform.DOMoveZ(170f, 0.5f); })
            .OnComplete(() => { transform.DOMoveY(0.5f, 0.3f); });
        }
        if (other.CompareTag("SellArea"))
        {
            SpeedMultiplier = 0;
        }
        if (other.CompareTag("RingOpen"))
        {
            foreach (Transform box in ringBoxesLeft)
            {
                box.DORotate(new Vector3(box.transform.rotation.x, box.transform.rotation.y, 10), 1f);
            }
            foreach (Transform box in ringBoxesRight)
            {
                box.DORotate(new Vector3(box.transform.rotation.x, box.transform.rotation.y, -10), 1f);
            }
        }

    }

    void CoffeeFollow()
    {
        for (int i = 1; i < CollectedCoffeeData.instance.CoffeeList.Count; i++)
        {
            Vector3 PrePos = CollectedCoffeeData.instance.CoffeeList[i - 1].transform.position + Vector3.forward * OffsetZ;
            Vector3 CurPos = CollectedCoffeeData.instance.CoffeeList[i].position;
            CollectedCoffeeData.instance.CoffeeList[i].transform.position = Vector3.Lerp(CurPos, (new Vector3(PrePos.x, CurPos.y, PrePos.z)), LerpSpeed * Time.deltaTime);
        }
    }

    void moveMoney()
    {
        if ((transform.childCount == 0) && !moveTest)
        {

            moveTest = true;
            foreach (Transform box in ringBoxesLeft)
            {
                box.DORotate(new Vector3(box.transform.rotation.x, box.transform.rotation.y, -90), 1.5f);
            }
            foreach (Transform box in ringBoxesRight)
            {
                box.DORotate(new Vector3(box.transform.rotation.x, box.transform.rotation.y, 90), 1.5f);
            }


            SoundManager.instance.PlayNormal("Bravo");

            var seq = DOTween.Sequence();

            seq.Append(transform.DORotate(new Vector3(0, 0, 180), 2f)).Join(transform.DOMove(new Vector3(-0.035f, 0.507f, 286.431f), 2f))

           .OnComplete(() =>
           {
               SpeedMultiplier = 0;
               seq.Kill();
               var seq1 = DOTween.Sequence();
               CameraManager.instance.OpenCamera("Finish", 1.5f, CameraEaseStates.EaseInOut);
               finishMoney.SetParent(transform);
               DOTween.To(() => int.Parse(finishCount.text), x => finishCount.text = x.ToString(), MoneyManager.instance.moneyCount, 2);
               seq1.Append(transform.DOMoveY(((float)(0.02 * MoneyManager.instance.moneyCount)), 2f))
                .OnComplete(() =>
                  {
                      UIManager.instance.OpenWinPanel();
                  });
           });


        }
    }
}



