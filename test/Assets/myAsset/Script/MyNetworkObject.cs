using UnityEngine;

/// <summary>
/// ポジションのみ
/// </summary>
public class MyNetworkObject : Photon.MonoBehaviour
{

    private Vector3 correctPlayerPos = Vector3.zero;
    //private Quaternion correctPlayerRot = Quaternion.identity;

    // Update is called once per frame
    void Update()
    {
        if (!photonView.isMine)
        {
            transform.position = Vector3.Lerp(transform.position, this.correctPlayerPos, Time.deltaTime * 5);
            //transform.rotation = Quaternion.Lerp(transform.rotation, this.correctPlayerRot, Time.deltaTime * 5);
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            //we own this player: send the others our data
            stream.SendNext(transform.position);
            //stream.SendNext(transform.rotation);
        }
        else
        {
            //network player, receive data
            this.correctPlayerPos = (Vector3)stream.ReceiveNext();
            //this.correctPlayerRot = (Quaternion)stream.ReceiveNext();

        }

    }


}
