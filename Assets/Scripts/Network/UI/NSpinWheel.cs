using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NSpinWheel : MonoBehaviour {

    public Transform wheelTransform;

    NPlayer player;


    public void Spin(float angularSpeed, float angularAcceleration)
    {
        //wheelTransform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360f));

        StartCoroutine(SpinTheWheel(angularSpeed, angularAcceleration));
    }


    IEnumerator SpinTheWheel(float angularSpeed, float angularAcceleration)
    {
        yield return new WaitForSeconds(0.5f);

        while (angularSpeed >= 0)
        {
            wheelTransform.Rotate(Vector3.back * angularSpeed * Time.deltaTime);
            angularSpeed = angularSpeed - angularAcceleration * Time.deltaTime;
            yield return null;
        }

        if (PhotonNetwork.isMasterClient)
        {
            float eulerZ = wheelTransform.rotation.eulerAngles.z;
            if (eulerZ < 0)
                eulerZ += 360;
            int itemIndex = (int)(16 * eulerZ / 360);
            NSpinWheelManager.instance.Execute(itemIndex);
        }

        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
