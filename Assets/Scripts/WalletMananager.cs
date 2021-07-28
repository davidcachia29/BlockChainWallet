using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WalletMananager : MonoBehaviour
{
    public InputField PrivateKey_Field;
    public InputField Password_Field;
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveWallet(){
        StartCoroutine(CreateUpdateWallet());
        
    }

    IEnumerator CreateUpdateWallet(){
        yield return new WaitForSeconds(0f);
        print(PrivateKey_Field);
        string privateKey = PrivateKey_Field.GetComponent<InputField>().text;
        string password = Password_Field.GetComponent<InputField>().text;

        string publicKey = Nethereum.Signer.EthECKey.GetPublicAddress(privateKey); //get public key from private key
        var privateKeyEncrypted = Utilities.Encrypt(privateKey,true,password);
        
        Wallet	objAddress = new Wallet();
        objAddress.Address = publicKey;	
		objAddress.EncryptedPrivateKey = privateKeyEncrypted;

        PlayerPrefs.DeleteAll(); //delete any PREVIOSLY saved settings
        PlayerPrefs.SetString("Wallet", Newtonsoft.Json.JsonConvert.SerializeObject (objAddress));
		PlayerPrefs.Save(); //save only the encrypted private key. don't save the actual private key...
        print("Public Key:"+publicKey);
		print("Wallet saved");

    }

    public void MainScene(){
		SceneManager.LoadScene("Main");
	}
}
