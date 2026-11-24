using UnityEngine;

public class EntityAnimsEvents : MonoBehaviour
{
    private Entity entity;
    private void Awake()
    {
        if (entity == null)
        {
            entity = GetComponentInParent<Entity>();
           
        }
    }
    public void DamageTargets() => entity.DamageTargets();
    private void DisableMovementAndJump()
    {
        entity.EnableMovement(false);
    }
    private void EnableMovementAndJump()
    {
        entity.EnableMovement(true);
    }

    private void DestroyAfterAnimation()
    {
        Destroy(entity.gameObject);
    }
    private void AddSFXToAnim(string keyForSFX)
    {
        if (keyForSFX != null && entity.isVisible)
        {
            SFXManager.Instance.PlaySFX(entity.entityClipsDictionary[keyForSFX], 0.6f, entity.transform);
        }
    }
}
