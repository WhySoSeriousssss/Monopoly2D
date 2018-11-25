using System.Collections;
using UnityEngine;

public class SpinWheel : MonoBehaviour {

    public Transform wheelTransform;
    float angularSpeed;
    float angularAcceleration;

    Player player;


    public void Initialize(Player callingPlayer)
    {
        player = callingPlayer;
        angularSpeed = 260 + Random.Range(-30f, 30f);
        angularAcceleration = 40f + Random.Range(-5f, 5f);
        wheelTransform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360f));

        StartCoroutine(SpinTheWheel());
    }


    IEnumerator SpinTheWheel()
    {
        yield return new WaitForSeconds(0.5f);

        while (angularSpeed >= 0)
        {
            wheelTransform.Rotate(Vector3.back * angularSpeed * Time.deltaTime);
            angularSpeed = angularSpeed - angularAcceleration * Time.deltaTime;
            yield return null;
        }

        float eulerZ = wheelTransform.rotation.eulerAngles.z;
        if (eulerZ < 0)
            eulerZ += 360;
        int itemIndex = (int)(16 * eulerZ / 360);
        SpinningWheelManager.instance.Execute(itemIndex, player);

        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
