using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class COriginBuilding : CGameObjBas
{
    public override EObjType ObjType() { return EObjType.eOriginBuilding; }

    // ==================== SerializeField ===========================================

    [SerializeField] protected StaticGlobalDel.EBrickColor m_MyBrickColor = StaticGlobalDel.EBrickColor.eBlue;
    public StaticGlobalDel.EBrickColor MyBrickColor => m_MyBrickColor;

    [SerializeField] protected GameObject m_MyCopyBrickObj = null;
    // ==================== SerializeField ===========================================

    protected Material m_MyMaterial = null;

    protected override void Awake()
    {
        base.Awake();

        Renderer lTempRenderer = this.GetComponent<Renderer>();
        m_MyMaterial = lTempRenderer.material;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == StaticGlobalDel.TagPlayerRoll)
        {
           // Debug.Log($"other.tag = {other.tag}");

            GameObject lTempGameObject = GameObject.Instantiate(m_MyCopyBrickObj);

            lTempGameObject.transform.position = this.transform.position;
            lTempGameObject.transform.localScale = this.transform.localScale;
            lTempGameObject.transform.parent = m_MyGameManager.AllBrickObj;

            Transform lTempchildTransform = null;
            Renderer lTempTransformRenderer = null;
            CBrickObj lTempBrickObj = null;
            Rigidbody lTempRigidbody = null;

            int lTempchildCount = lTempGameObject.transform.childCount;
            for (int i = 0; i < lTempchildCount; i++)
            {
                lTempchildTransform = lTempGameObject.transform.GetChild(i);
                lTempTransformRenderer = lTempchildTransform.GetComponent<Renderer>();
                lTempTransformRenderer.material = m_MyMaterial;
                lTempBrickObj = lTempTransformRenderer.gameObject.AddComponent<CBrickObj>();
                lTempBrickObj.MyBrickColor = MyBrickColor;

                lTempRigidbody = lTempchildTransform.GetComponent<Rigidbody>();
              //  lTempRigidbody.AddForce(Vector3.up * 500.0f);
                lTempRigidbody.AddExplosionForce(500.0f, other.transform.position, 1000.0f);
               // lTempTransformRenderer.gameObject.tag = StaticGlobalDel.TagBrickObj;
            }

            lTempGameObject.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}
