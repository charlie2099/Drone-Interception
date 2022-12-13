﻿using UnityEngine;
using UnityEngine.EventSystems;

namespace Drone
{
    /// <summary>
    /// Responsible for outfitting drones with different components
    /// </summary>
    public class AttachmentDecorator : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
    {
        [SerializeField] private GameObject prefabToSpawn;
        private GameObject _objectInHand;
        private AttachmentPoint _attachmentPoint;

        public void OnPointerDown(PointerEventData eventData)
        {
            _objectInHand = Instantiate(prefabToSpawn, Input.mousePosition, Quaternion.identity);
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            if (_attachmentPoint != null && !_attachmentPoint.HasAttachment)
            {
                _attachmentPoint.AddAttachment(_objectInHand);
            }
            else
            {
                Destroy(_objectInHand);
            }
            _objectInHand = null;
        }

        private void Update()
        {
            if (_objectInHand != null)
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo))
                {
                    // Ignore the collider on the object in hand
                    if(hitInfo.collider == _objectInHand.GetComponent<Collider>()) { return; }
                    
                    Debug.Log("Collider: " + hitInfo.collider.name);
                    
                    _objectInHand.transform.position = hitInfo.point;
                    //_objectInHand.transform.rotation = hitInfo.transform.rotation;

                    if (hitInfo.transform.GetComponent<AttachmentPoint>() != null)
                    {
                        Debug.Log("ATTACHMENT COMPONENT");
                        _objectInHand.transform.position = hitInfo.transform.position;
                        //_objectInHand.transform.rotation = hitInfo.transform.rotation;
                        _attachmentPoint = hitInfo.transform.GetComponent<AttachmentPoint>();
                        //var drone = hitInfo.transform.GetComponentInParent<QuadcopterDrone>();
                    }
                    else 
                    {
                        _attachmentPoint = null;
                    }
                }
                else // if no colliders detected, lock z position of selected prefab
                {
                    //Debug.Log("NO ATTACHMENT");
                    var v3 = Input.mousePosition; 
                    v3.z = 4.0f; 
                    v3 = Camera.main.ScreenToWorldPoint(v3); 
                    _objectInHand.transform.position = v3;
                    
                    _attachmentPoint = null;
                }
            }
        }
    }
}