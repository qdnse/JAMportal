﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using RenderPipeline = UnityEngine.Experimental.Rendering.RenderPipeline;

public class Portal : MonoBehaviour
{
    public Camera portalCamera;
    public Transform pairPortal;
    private GameObject player;

    void OnTriggerEnter(Collider hit)
    {
        Debug.Log("Enter controller called.");
        if (hit.gameObject.name == "Player")
        {
            Debug.Log("Salut le player.");
            //            player.transform.position = pairPortal.transform.position + pairPortal.transform.forward * 2;
            hit.transform.rotation = pairPortal.transform.rotation * Quaternion.Euler(pairPortal.transform.rotation.x, hit.transform.rotation.y, pairPortal.transform.rotation.z);// Quaternion.Inverse(hit.transform.rotation);  //player.transform.rotation;
            hit.enabled = false;
            hit.transform.position = pairPortal.transform.position + pairPortal.transform.forward * 2;
            hit.enabled = true;
        }
        else
        {
            hit.gameObject.transform.position = pairPortal.transform.position + pairPortal.transform.forward * 2;
        }
    }

    private void OnEnable()
    {
        RenderPipeline.beginCameraRendering += UpdateCamera;
    }

    private void OnDisable()
    {
        RenderPipeline.beginCameraRendering -= UpdateCamera;
    }

    void UpdateCamera(Camera camera)
    {
        if ((camera.cameraType == CameraType.Game || camera.cameraType == CameraType.SceneView) &&
            camera.tag != "Portal Camera")
        {
            portalCamera.projectionMatrix = camera.projectionMatrix; // Match matrices

            var relativePosition = transform.InverseTransformPoint(camera.transform.position);
            relativePosition = Vector3.Scale(relativePosition, new Vector3(-1, 1, -1));
            portalCamera.transform.position = pairPortal.TransformPoint(relativePosition);

            var relativeRotation = transform.InverseTransformDirection(camera.transform.forward);
            relativeRotation = Vector3.Scale(relativeRotation, new Vector3(-1, 1, -1));
            portalCamera.transform.forward = pairPortal.TransformDirection(relativeRotation);
        }
    }
    
    // Calculates reflection matrix around the given plane
    private static void CalculateReflectionMatrix(ref Matrix4x4 reflectionMat, Vector4 plane)
    {
        reflectionMat.m00 = (1F - 2F * plane[0] * plane[0]);
        reflectionMat.m01 = (-2F * plane[0] * plane[1]);
        reflectionMat.m02 = (-2F * plane[0] * plane[2]);
        reflectionMat.m03 = (-2F * plane[3] * plane[0]);

        reflectionMat.m10 = (-2F * plane[1] * plane[0]);
        reflectionMat.m11 = (1F - 2F * plane[1] * plane[1]);
        reflectionMat.m12 = (-2F * plane[1] * plane[2]);
        reflectionMat.m13 = (-2F * plane[3] * plane[1]);

        reflectionMat.m20 = (-2F * plane[2] * plane[0]);
        reflectionMat.m21 = (-2F * plane[2] * plane[1]);
        reflectionMat.m22 = (1F - 2F * plane[2] * plane[2]);
        reflectionMat.m23 = (-2F * plane[3] * plane[2]);

        reflectionMat.m30 = 0F;
        reflectionMat.m31 = 0F;
        reflectionMat.m32 = 0F;
        reflectionMat.m33 = 1F;
    }
}