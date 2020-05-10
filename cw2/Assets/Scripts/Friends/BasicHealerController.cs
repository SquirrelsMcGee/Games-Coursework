using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basic healing turret, heals the defense point
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class BasicHealerController : MonoBehaviour
{

    [Header("Behaviour Settings")]
    [Tooltip("How fast the turret heals the tower")]
    public float healingRate = 1.0f;
    [Tooltip("How far the turret can see")]
    public float range = 10.0f;
    [Tooltip("How much the turret heals each tick")]
    public int healAmount = 1;

    // Duration since last Timed update
    protected float deltaTime = 0.0f;

    // Line renderer for drawing the healing ray
    protected LineRenderer lineRenderer;

    // Start is called before the first frame update
    public virtual void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        Material whiteDiffuseMat = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.material = whiteDiffuseMat;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        TimedUpdate();
    }

    /// <summary>
    /// Runs when deltaTime >= fireRate
    /// </summary>
    protected virtual void TimedUpdate()
    {
        deltaTime += Time.deltaTime;

        if (deltaTime >= healingRate)
        {
            deltaTime = 0;
            StartCoroutine(DrawLine());
            TargetController.Instance.health += healAmount;
        }
    }

    /// <summary>
    /// Toggles the healing ray
    /// </summary>
    IEnumerator DrawLine()
    {
        lineRenderer.enabled = true;

        // Draws the line between the healing tower and the defense point
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, TargetController.Instance.transform.position);
        
        yield return new WaitForSeconds(0.3f);

        lineRenderer.enabled = false;
    }
}
