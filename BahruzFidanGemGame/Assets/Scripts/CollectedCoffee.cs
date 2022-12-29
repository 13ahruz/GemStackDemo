using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using RayFire;

public class CollectedCoffee : MonoBehaviour
{
    [SerializeField]
    private Transform player;
    [SerializeField]
    private Material diamondMat;
    [SerializeField]
    CollectedCoffee collectedCoffeeScript;
    [SerializeField]
    Material randomColor;
    [SerializeField]
    private int price = 1;
    private float pitch = 1;






    private void Start()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectable"))
        {
            CollectedCoffeeData.instance.pitch += 0.1f;
            CollectedCoffeeData.instance.nextPitchTime = Time.time + 1;
            SoundManager.instance.Play("Collect");
            other.tag = "GemStart";
            CollectedCoffeeData.instance.CoffeeList.Add(other.transform);
            CollectedCoffee collectedCoffeeScript = other.gameObject.AddComponent<CollectedCoffee>();
            collectedCoffeeScript.diamondMat = CollectedCoffeeData.instance.diamondMaterial;
            collectedCoffeeScript.player = CollectedCoffeeData.instance.player;

            other.gameObject.AddComponent<Rigidbody>().isKinematic = true;


            var seq = DOTween.Sequence();
            for (int i = CollectedCoffeeData.instance.CoffeeList.Count - 1; i >= 0; i--)
            {
                seq.Join(CollectedCoffeeData.instance.CoffeeList[i].DOScale(1.5f, 0.06f));
                seq.AppendInterval(0.02f);
                seq.Join(CollectedCoffeeData.instance.CoffeeList[i].DOScale(1f, 0.06f));
            }
        }

        if (other.CompareTag("GemStartRedMaker") && transform.tag == "GemStart")
        {
            CollectedCoffeeData.instance.nextPitchTime = Time.time + 1;
            CollectedCoffeeData.instance.pitch += 0.1f;
            SoundManager.instance.Play("Crash");
            price = 3;
            for (int i = 0; i < 25; i++)
            {
                GameObject piece = Instantiate(CollectedCoffeeData.instance.pieces[i], transform.position, Quaternion.identity);
                piece.GetComponent<Rigidbody>().AddExplosionForce(5, transform.position, 5, 3);
                Destroy(piece, 10);
            }
            transform.GetChild(1).gameObject.SetActive(true);
            Destroy(transform.GetChild(0).gameObject);

            transform.tag = "GemStartRed";

        }

        if (other.CompareTag("GemDiamondRedMaker") && transform.tag == "GemStartRed")
        {
            SoundManager.instance.PlayNormal("RedCrash");
            price = 5;
            for (int i = 0; i < 20; i++)
            {
                GameObject piecesRed = Instantiate(CollectedCoffeeData.instance.piecesRed[i], transform.position, Quaternion.identity);
                piecesRed.GetComponent<Rigidbody>().AddExplosionForce(5, transform.position, 5, 3);
                Destroy(piecesRed, 10);
            }
            transform.GetChild(1).gameObject.SetActive(true);
            Destroy(transform.GetChild(0).gameObject);
            transform.tag = "GemDiamondRed";
        }
        if (other.CompareTag("GemRandomColoredMaker") && transform.tag == "GemDiamondRed")
        {
            CollectedCoffeeData.instance.pitch += 0.2f;
            CollectedCoffeeData.instance.nextPitchTime = Time.time + 1;
            SoundManager.instance.Play("Spray");
            price = 8;
            if (Time.time > CollectedCoffeeData.instance.nextColorChangeTime)
            {
                CollectedCoffeeData.instance.randomColor = CollectedCoffeeData.instance.randomColors[Random.Range(0, 3)];
                CollectedCoffeeData.instance.nextColorChangeTime = Time.time + CollectedCoffeeData.instance.colorChangeSpeed / CollectedCoffeeData.instance.colorChangeRate;
            }
            transform.DOScale(1.5f, 0.1f).SetLoops(2, LoopType.Yoyo);
            transform.GetChild(0).GetComponent<Renderer>().material = CollectedCoffeeData.instance.randomColor;
            // transform.GetChild(3).DOScale(140f, 0.1f).SetLoops(2, LoopType.Yoyo);
            transform.tag = "GemRandomColored";
        }
        if (other.CompareTag("GemRandomColoredMaker") && transform.tag == "GemRandomColored")
        {
            CollectedCoffeeData.instance.pitch += 0.2f;
            CollectedCoffeeData.instance.nextPitchTime = Time.time + 1;
            SoundManager.instance.Play("Spray");
            price = 8;
            if (Time.time > CollectedCoffeeData.instance.nextColorChangeTime)
            {
                CollectedCoffeeData.instance.randomColor = CollectedCoffeeData.instance.randomColors[Random.Range(0, 3)];
                CollectedCoffeeData.instance.nextColorChangeTime = Time.time + CollectedCoffeeData.instance.colorChangeSpeed / CollectedCoffeeData.instance.colorChangeRate;
            }
            transform.DOScale(1.5f, 0.1f).SetLoops(2, LoopType.Yoyo);
            transform.GetChild(0).GetComponent<Renderer>().material = CollectedCoffeeData.instance.randomColor;
            // transform.GetChild(3).DOScale(140f, 0.1f).SetLoops(2, LoopType.Yoyo);
        }


        if (other.CompareTag("RingCircle") && transform.tag == "GemDiamondRed")
        {
            SoundManager.instance.PlayNormal("RingReady");
            price = 20;
            transform.GetChild(0).transform.DOScale(0.23f, 0.1f);
            transform.GetChild(0).DOLocalMove(new Vector3(0, 0.145f, 0), 0.2f);
            transform.GetChild(0).transform.SetParent(transform.GetChild(1));
            transform.GetChild(0).gameObject.SetActive(true);
            other.transform.DOMoveY(0.380f, 0.12f).SetLoops(2, LoopType.Yoyo);
            // transform.GetChild(4).DOScale(140f, 0.1f).SetLoops(2, LoopType.Yoyo);
            other.transform.DOScale(2f, 0.1f).SetLoops(2, LoopType.Yoyo);
            transform.tag = "RingReady";
        }
        if (other.CompareTag("RingCircle") && transform.tag == "GemRandomColored")
        {
            SoundManager.instance.PlayNormal("RingReady");
            price = 20;
            transform.GetChild(0).transform.DOScale(0.25f, 0.1f);
            transform.GetChild(0).DOLocalMove(new Vector3(0, 0.1528f, 0), 0.2f);
            transform.GetChild(0).transform.SetParent(transform.GetChild(1));
            transform.GetChild(0).gameObject.SetActive(true);
            other.transform.DOMoveY(0.380f, 0.12f).SetLoops(2, LoopType.Yoyo);
            // transform.GetChild(4).DOScale(140f, 0.1f).SetLoops(2, LoopType.Yoyo);145
            other.transform.DOScale(1.5f, 0.1f).SetLoops(2, LoopType.Yoyo);
            transform.tag = "RingReady";
        }
        if (other.CompareTag("Seller") && (transform.gameObject.layer != 7))
        {
            SoundManager.instance.PlayNormal("SellMiddle");
            MoneyManager.instance.MoneyUp(price);
            transform.SetParent(null);
            transform.tag = "Sold";
            CollectedCoffeeData.instance.CoffeeList.Remove(transform);
            transform.DOMove(other.transform.GetChild(0).position, 1f).OnComplete(() => { Destroy(gameObject); });
        }
        if (other.transform.CompareTag("Obstacle"))
        {
            if (player != null && transform.gameObject.layer != 7)
            {
                if (Time.time > CollectedCoffeeData.instance.nextDamageTime)
                {
                    SoundManager.instance.PlayNormal("Damage");
                    player.transform.DOLocalJump(new Vector3(player.transform.position.x, 0, player.transform.position.z - 4), 0.7f, 1, 0.2f);
                    CollectedCoffeeData.instance.CoffeeList.Remove(transform);
                    Destroy(gameObject);
                    CollectedCoffeeData.instance.nextDamageTime = Time.time + CollectedCoffeeData.instance.damageRate / CollectedCoffeeData.instance.damageSpeed;
                }

            }
        }

        if (other.CompareTag("PoolStart"))
        {
            CollectedCoffeeData.instance.water.Play();
            CollectedCoffeeData.instance.pitch += 0.2f;
            CollectedCoffeeData.instance.nextPitchTime = Time.time + 1;
            SoundManager.instance.Play("Water");
            var seq1 = DOTween.Sequence();
            seq1.Append(transform.DOMoveY(-3.44f, 0.3f))
            .OnComplete(() => { transform.DOMoveZ(170f, 0.5f); })
            .OnComplete(() =>
            {
                if (transform.gameObject.layer != 7)
                {
                    transform.DOMoveY(0.55f, 0.3f);
                }
                else
                {
                    transform.DOLocalMoveY(0f, 0.3f);
                }


            });
        }

        if (other.CompareTag("PoolEnd") && (transform.tag == "GemRandomColored"))
        {
            price = 10;
            transform.GetChild(0).GetComponent<MeshRenderer>().material = CollectedCoffeeData.instance.diamondMaterial;
            ParticleSystem diamondParticle = transform.GetComponentInChildren<ParticleSystem>();
            diamondParticle.Play();
        }
        if (other.CompareTag("PoolEnd") && (transform.tag == "GemDiamondRed"))
        {
            price = 10;
            transform.GetChild(0).GetComponent<MeshRenderer>().material = CollectedCoffeeData.instance.diamondMaterial;
            ParticleSystem diamondParticle = transform.GetComponentInChildren<ParticleSystem>();
            diamondParticle.Play();
        }
        if (other.CompareTag("SellArea") && transform.tag == "RingReady")
        {
            MoneyManager.instance.MoneyUp(price);
            transform.parent = null;
            CollectedCoffeeData.instance.CoffeeList.Remove(transform);
            transform.DOMove(CollectedCoffeeData.instance.finishPoses[CollectedCoffeeData.instance.finishCount].position, 0.5f);
            transform.Rotate(0, 90, 0);
            CollectedCoffeeData.instance.finishCount += 1;
        }

        if (other.CompareTag("SellArea") && (transform.tag != "RingReady"))
        {
            transform.parent = null;
            CollectedCoffeeData.instance.CoffeeList.Remove(transform);
            transform.DOMove(CollectedCoffeeData.instance.finishPosUncompleted.position, 0.5f);
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("PoolInside"))
        {
            CollectedCoffeeData.instance.inPool = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PoolInside"))
        {
            CollectedCoffeeData.instance.inPool = false;
        }
    }

    /* private void DisableAllMeshes()
     {
         /// GemStart 
         transform.GetChild(0).gameObject.SetActive(false);
         /// GemStartRed
         transform.GetChild(1).gameObject.SetActive(false);
         /// GemDiamondRed
         transform.GetChild(2).gameObject.SetActive(false);
         /// GemDiamondGreen
         transform.GetChild(3).gameObject.SetActive(false);
         /// RingReady
         if (transform.childCount >= 5)
             transform.GetChild(4).gameObject.SetActive(false);


     }*/

}
