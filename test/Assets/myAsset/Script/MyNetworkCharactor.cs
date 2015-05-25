using UnityEngine;

public class MyNetworkCharactor : Photon.MonoBehaviour {

    private Vector3 correctPlayerPos = Vector3.zero;
    private Quaternion correctPlayerRot = Quaternion.identity;
	
	// Update is called once per frame
	void Update () {
        if (!photonView.isMine)
        {
            transform.position = Vector3.Lerp(transform.position, this.correctPlayerPos, Time.deltaTime * 5);
            transform.rotation = Quaternion.Lerp(transform.rotation, this.correctPlayerRot, Time.deltaTime * 5);
        }
	}

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            //we own this player: send the others our data
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            
            //send animation state
            //myThirdPersonController myC = GetComponent<myThirdPersonController>();
            //stream.SendNext((int)myC._characterState);
            stream.SendNext((int)GetComponent<PlayerController>().animState);
            stream.SendNext(GetComponent<ObjectState>().HEALTH);
        }
        else
        {
            //network player, receive data
            this.correctPlayerPos = (Vector3)stream.ReceiveNext();
            this.correctPlayerRot = (Quaternion)stream.ReceiveNext();

            //receive animiation state
            //myThirdPersonController myC = GetComponent<myThirdPersonController>();
            //myC._characterState = (CharacterState)stream.ReceiveNext();
            GetComponent<PlayerController>().animState = (CharacterAnimState)stream.ReceiveNext();
            GetComponent<ObjectState>().HEALTH = (int)stream.ReceiveNext();
        }

    }


}
