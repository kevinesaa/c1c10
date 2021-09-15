using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropShadow : MonoBehaviour {

    public GameObject player;
    public float playerOffset;
    public float offset;
    public float maxHeight;

    public LayerMask levelLayermask;

    private SpriteRenderer shadowSpriteRenderer;
    private Vector3 shadowOriginalSize;
    private ContactFilter2D shadowContactFilter;
    protected RaycastHit2D[] shadowContactCache = new RaycastHit2D[6];

    private void Awake()
    {
        shadowSpriteRenderer = GetComponent<SpriteRenderer>();
        shadowOriginalSize = transform.localScale;

        shadowContactFilter = new ContactFilter2D();
        shadowContactFilter.layerMask = levelLayermask;
        shadowContactFilter.useLayerMask = true;
    }
	
	// Update is called once per frame
	void LateUpdate () 
    {
        int count = Physics2D.Raycast((Vector2)player.transform.position + Vector2.up * playerOffset, Vector2.down, shadowContactFilter, shadowContactCache);

        if (count > 0)
        {
            shadowSpriteRenderer.enabled = true;
            transform.position = shadowContactCache[0].point + shadowContactCache[0].normal * offset;
            float height = Vector3.SqrMagnitude(player.transform.position - transform.position);
            float ratio = Mathf.Clamp(1.0f - height / (maxHeight * maxHeight), 0.0f, 1.0f);

            transform.localScale = shadowOriginalSize * ratio;
        }
        else
        {
            shadowSpriteRenderer.enabled = false;
        }
	}
}
