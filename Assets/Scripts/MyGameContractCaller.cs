using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Nethereum.JsonRpc.UnityClient;
using Nethereum.Hex.HexTypes;
using Nethereum.Contracts;
using System;
using System.Numerics;

public class MyGameContractCaller : MonoBehaviour {

	private DeployContractTransactionBuilder contractTransactionBuilder = new DeployContractTransactionBuilder();

	private MyGameContractService myGameContractService = new MyGameContractService();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void createInventoryForPlayer(){
        StartCoroutine(createInventoryForPlayerRequest());
    }

    public IEnumerator createInventoryForPlayerRequest(){ //Connect with Ethereum blockchain and register this address to the contract       
		var transactionInput = myGameContractService.CreateInventoryForPlayer (new HexBigInteger (3000000));
		//transactionInput.Nonce = new HexBigInteger(23); NONCE HAS TO BE SET WHEN YOU HAVE MULTIPLE TRANSACACTION IN THE SAME BLOCK https://github.com/MetaMask/metamask-extension/issues/2732; https://github.com/Nethereum/Nethereum/blob/master/src/Nethereum.Unity/TransactionSignedUnityRequest.cs
		var transactionSignedRequest = new TransactionSignedUnityRequest(LevelManager._url, LevelManager.objAddress.PrivateKey , LevelManager.objAddress.Address);
		
		//send and wait (run smart contract)
        yield return transactionSignedRequest.SignAndSendTransaction (transactionInput);
		if (transactionSignedRequest.Exception == null) {
			print("Transaction Receipt:" + transactionSignedRequest.Result); //the transaction address
			
			while(true){
				yield return new WaitForSeconds(3);
				EthGetTransactionReceiptUnityRequest tranReceiptStatus = new EthGetTransactionReceiptUnityRequest(LevelManager._url);
				yield return tranReceiptStatus.SendRequest(transactionSignedRequest.Result);
				
				if(tranReceiptStatus.Result != null){
					if(tranReceiptStatus.Result.Status != null){
						if(tranReceiptStatus.Result.Status.Value == 1){ //smart contract was successfully executed
							print("Transaction Completed Successfully - Inventory Created");
						}
						else if(tranReceiptStatus.Result.Status.Value == 0){ //not successfully executed; example: low input ammount; or not enough gas
							print("Error when running smart contract");
						}
						break;
					}							
				}	
			}
		} else {
			Debug.Log("Error buying mana: " + transactionSignedRequest.Exception.Message);
		}
    }


	public void buyMana(){
		StartCoroutine(buyManaRequest());
	}

	public IEnumerator buyManaRequest(){	
		var transactionInput = myGameContractService.BuyManaTransactionInput (new HexBigInteger (3000000), new HexBigInteger(1000));
		//transactionInput.Nonce = new HexBigInteger(23); NONCE HAS TO BE SET WHEN YOU HAVE MULTIPLE TRANSACACTION IN THE SAME BLOCK https://github.com/MetaMask/metamask-extension/issues/2732; https://github.com/Nethereum/Nethereum/blob/master/src/Nethereum.Unity/TransactionSignedUnityRequest.cs
		var transactionSignedRequest = new TransactionSignedUnityRequest(LevelManager._url, LevelManager.objAddress.PrivateKey , LevelManager.objAddress.Address);
		
		//send and wait (run smart contract)
        yield return transactionSignedRequest.SignAndSendTransaction (transactionInput);
		if (transactionSignedRequest.Exception == null) {
			print("Transaction Receipt:" + transactionSignedRequest.Result); //the transaction address
			
			while(true){
				yield return new WaitForSeconds(3);
				EthGetTransactionReceiptUnityRequest tranReceiptStatus = new EthGetTransactionReceiptUnityRequest(LevelManager._url);
				yield return tranReceiptStatus.SendRequest(transactionSignedRequest.Result);
				
				if(tranReceiptStatus.Result != null){
					if(tranReceiptStatus.Result.Status != null){
						if(tranReceiptStatus.Result.Status.Value == 1){ //smart contract was successfully executed
							print("Transaction Completed Successfully");
						}
						else if(tranReceiptStatus.Result.Status.Value == 0){ //not successfully executed; example: low input ammount; or not enough gas
							print("Error when running smart contract");
						}
						break;
					}							
				}	
			}
		} else {
			Debug.Log("Error buying mana: " + transactionSignedRequest.Exception.Message);
		}

	}

	public void getManaForAddress(){
		StartCoroutine(getManaRequestForAddress());
	}

	public IEnumerator getManaRequestForAddress(){		
		var getManaRequest = new EthCallUnityRequest(LevelManager._url);
		var getManaInput = myGameContractService.CreateManaValueScoreInput();
		print("Loading Mana for account:" + LevelManager.objAddress.Address);
		yield return getManaRequest.SendRequest(getManaInput, Nethereum.RPC.Eth.DTOs.BlockParameter.CreateLatest());
		if (getManaRequest.Exception == null) {
			int mana= myGameContractService.DecodeManaValue(getManaRequest.Result);
			print("Mana:" +  mana);			
		} else {
			Debug.Log("Error getting mana: " + getManaRequest.Exception.Message);
		}
	}
    public void getGoldForAddress(){
		StartCoroutine(getGoldRequestForAddress());
	}

    public IEnumerator getGoldRequestForAddress(){	
		var getGoldRequest = new EthCallUnityRequest(LevelManager._url);
		var getGoldInput = myGameContractService.CreateGoldValueInput();
		print("Loading Gold for account:" + LevelManager.objAddress.Address);
		yield return getGoldRequest.SendRequest(getGoldInput, Nethereum.RPC.Eth.DTOs.BlockParameter.CreateLatest());
		if (getGoldRequest.Exception == null) {
			int gold= myGameContractService.DecodeGoldValue(getGoldRequest.Result);
			print("Gold:" +  gold);			
		} else {
			Debug.Log("Error getting gold: " + getGoldRequest.Exception.Message);
		}
	}

    public void TransferGold(){
        string _addressTo = ((InputField)GameObject.Find("Canvas").transform.Find("InputAddressPlayer2").GetComponent<InputField>()).text;
        int goldAmmount = System.Convert.ToInt32(((InputField)GameObject.Find("Canvas").transform.Find("InputGoldAmmount").GetComponent<InputField>()).text);

        print("_addressTo:"+_addressTo);
        print("gold ammount:"+goldAmmount);

        StartCoroutine(TransferGoldRequest(_addressTo,goldAmmount));

    }

    public IEnumerator TransferGoldRequest(string _addressTo, int goldAmmount){	
		var transactionInput = myGameContractService.TransferGold (new HexBigInteger (3000000),null,_addressTo,goldAmmount);
		//transactionInput.Nonce = new HexBigInteger(23); NONCE HAS TO BE SET WHEN YOU HAVE MULTIPLE TRANSACACTION IN THE SAME BLOCK https://github.com/MetaMask/metamask-extension/issues/2732; https://github.com/Nethereum/Nethereum/blob/master/src/Nethereum.Unity/TransactionSignedUnityRequest.cs
		var transactionSignedRequest = new TransactionSignedUnityRequest(LevelManager._url, LevelManager.objAddress.PrivateKey , LevelManager.objAddress.Address);
		
		//send and wait (run smart contract)
        yield return transactionSignedRequest.SignAndSendTransaction (transactionInput);
		if (transactionSignedRequest.Exception == null) {
			print("Transaction Receipt:" + transactionSignedRequest.Result); //the transaction address
			
			while(true){
				yield return new WaitForSeconds(3);
				EthGetTransactionReceiptUnityRequest tranReceiptStatus = new EthGetTransactionReceiptUnityRequest(LevelManager._url);
				yield return tranReceiptStatus.SendRequest(transactionSignedRequest.Result);
				
				if(tranReceiptStatus.Result != null){
					if(tranReceiptStatus.Result.Status != null){
						if(tranReceiptStatus.Result.Status.Value == 1){ //smart contract was successfully executed
							print("Transaction Completed Successfully:gold transferred");
						}
						else if(tranReceiptStatus.Result.Status.Value == 0){ //not successfully executed; example: low input ammount; or not enough gas
							print("Error when running smart contract");
						}
						break;
					}							
				}	
			}
		} else {
			Debug.Log("Error transferring gold: " + transactionSignedRequest.Exception.Message);
		}

	}
    

    public void buyManaTwice(){
		StartCoroutine(buyManaTwiceRequest());
	}

	public IEnumerator buyManaTwiceRequest()
    {	
            //THESE TWO TRANSACTIONS PROBABLY ARE GOING TO BE IN THE SAME BLOCK, Will trigger error: Replacement transaction underpriced if nonce is not given
		
            //Get nonce
            var request = new EthGetTransactionCountUnityRequest(LevelManager._url);
            yield return request.SendRequest(LevelManager.objAddress.Address,Nethereum.RPC.Eth.DTOs.BlockParameter.CreateLatest());
		    if(request.Exception == null){
			    var nonce =request.Result.Value;
                print("Nonce"+nonce);
                
                //send first transaction

                var transactionInput1 = myGameContractService.BuyManaTransactionInput (new HexBigInteger (3000000), new HexBigInteger(1000));            
                transactionInput1.Nonce = new HexBigInteger(nonce);// NONCE HAS TO BE SET WHEN YOU HAVE MULTIPLE TRANSACACTION IN THE SAME BLOCK https://github.com/MetaMask/metamask-extension/issues/2732; https://github.com/Nethereum/Nethereum/blob/master/src/Nethereum.Unity/TransactionSignedUnityRequest.cs
                var transactionSignedRequest1 = new TransactionSignedUnityRequest(LevelManager._url, LevelManager.objAddress.PrivateKey , LevelManager.objAddress.Address);
            
                yield return transactionSignedRequest1.SignAndSendTransaction (transactionInput1);
                if (transactionSignedRequest1.Exception == null) {
                    print("Transaction Receipt:" + transactionSignedRequest1.Result); //the transaction address
                } else {
                    Debug.Log("Error buying mana: " + transactionSignedRequest1.Exception.Message);
                }

                //send second transaction

                var transactionInput2 = myGameContractService.BuyManaTransactionInput (new HexBigInteger (3000000), new HexBigInteger(2000));            
                transactionInput2.Nonce = new HexBigInteger(nonce+1);
                var transactionSignedRequest2 = new TransactionSignedUnityRequest(LevelManager._url, LevelManager.objAddress.PrivateKey , LevelManager.objAddress.Address);
            
                yield return transactionSignedRequest2.SignAndSendTransaction (transactionInput2);
                if (transactionSignedRequest2.Exception == null) {
                    print("Transaction Receipt:" + transactionSignedRequest2.Result); //the transaction address
                } else {
                    Debug.Log("Error buying mana: " + transactionSignedRequest2.Exception.Message);
                }
            }

            


	}




    //	






}
