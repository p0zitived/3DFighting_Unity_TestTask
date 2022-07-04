using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] PlayerMoveController moveCtrl;
    [SerializeField] Animator playerAnimator;
    [SerializeField] Transform itemSpawner;
    [SerializeField] Transform itemHandler;

    private void Start()
    {
        InventoryObject.OnRemoveItem += OnDropItem;
    }

    private void Update()
    {
        SetAnimatorDir();
    }

    // Changes the DirX and DirZ params based on player move
    private void SetAnimatorDir()
    {
        playerAnimator.SetFloat("DirX", moveCtrl.GetSmoothDirection().x);
        playerAnimator.SetFloat("DirZ", moveCtrl.GetSmoothDirection().z);
    }

    private IEnumerator WaitAndReset(float time)
    {
        yield return new WaitForSeconds(time);
        playerAnimator.SetBool("PickUp", false);
    }

    private void OnDropItem(ItemObject item)
    {
        GameObject obj = Instantiate(item.prefab);
        obj.transform.position = itemSpawner.position;
    }
}
