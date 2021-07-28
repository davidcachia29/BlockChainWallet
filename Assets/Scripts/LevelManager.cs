using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nethereum;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

	public InputField Password_Field;
	public static Wallet objAddress;
	public static string _url = "https://rinkeby.infura.io/v3/cf20ab8f01824d2db9d4981f70d33828";
	
	public string Password;
	// Use this for initialization
	void Start () {
	
	
		objAddress = new Wallet();
	
		
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator LoadWallet(){				
		yield return new WaitForFixedUpdate();

		//we try and fetch the address from the shared preferences
		string jsonFromSharedPrefs = PlayerPrefs.GetString("Wallet","");
		if(jsonFromSharedPrefs == "")
		{
			print("NO WALLET FOUND!!!!!!!");

			//create keys since they are not storred
		
			// CreateAccount (Password, (publicAddress,privateKey,privateKeyEncrypted) => {		
			// 	print("Public Address: "+ publicAddress);
			// 	print("Private Key:" + privateKey);	
			// 	//print("Json Code which can be used to restore account:" + encryptedJson); //this can be sent via email or other service
			// 	//store in shared prefs					
			// 	objAddress.Address = publicAddress;	
			// 	objAddress.EncryptedPrivateKey = privateKeyEncrypted;
							
			// 	PlayerPrefs.SetString("Wallet", Newtonsoft.Json.JsonConvert.SerializeObject (objAddress));
			// 	PlayerPrefs.Save(); //save only the encrypted private key. don't save the actual private key...
			// 	print("Wallet saved");
				
			// 	objAddress.PrivateKey = privateKey;

		
			// });
		}
		else
		{
			print("Loading key from shared prefs");
			objAddress = Newtonsoft.Json.JsonConvert.DeserializeObject<Wallet>(jsonFromSharedPrefs);
			objAddress.PrivateKey = Utilities.Decrypt(objAddress.EncryptedPrivateKey,true,Password);
			print("Public Address: "+ objAddress.Address);	
			print("Private Key Decrypted" + objAddress.PrivateKey);
		} 	
	}


	// private void CreateAccount(string password,  System.Action<string, string, string> callback)
    // {
    //    	// We use the Nethereum.Signer to generate a new secret key
	// 	var ecKey = Nethereum.Signer.EthECKey.GenerateKey();

	// 	// After creating the secret key, we can get the public address and the private key with
	// 	// ecKey.GetPublicAddress() and ecKey.GetPrivateKeyAsBytes()
	// 	// (so it return it as bytes to be encrypted)
	// 	var address = ecKey.GetPublicAddress();
	// 	//var privateKeyBytes = ecKey.GetPrivateKeyAsBytes();
	// 	var privateKeyString = ecKey.GetPrivateKey();
	// 	var privateKeyEncrypted = Utilities.Encrypt(privateKeyString,true,password);


	// 	// Then we define a new KeyStore service
	// 	//var keystoreservice =  new Nethereum.KeyStore.KeyStoreService();
		
	// 	// And we can proceed to define encryptedJson with EncryptAndGenerateDefaultKeyStoreAsJson(),
	// 	// and send it the password, the private key and the address to be encrypted.
	// 	//var encryptedJson = keystoreservice.EncryptAndGenerateDefaultKeyStoreAsJson (password, privateKeyBytes, address);
	// 	callback (address, privateKeyString,privateKeyEncrypted);
    // }

	public void WalletSettingsScene(){
		SceneManager.LoadScene("WalletSettings");
	}

	public void UnlockWallet(){
		Password = Password_Field.GetComponent<InputField>().text;
		print("password entered");

		StartCoroutine(LoadWallet());
	}
}
