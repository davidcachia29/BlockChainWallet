using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nethereum.JsonRpc.UnityClient;
using Nethereum.Hex.HexTypes;
using Nethereum.Contracts;
using System;
using System.Numerics;


public class BalanceCheck : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void GetBalance(){
		StartCoroutine(getEthInAccount((balance) => {
			print("Balance in account is:" + balance);
		}));
	}

	public IEnumerator getEthInAccount(System.Action<decimal> callback){
		yield return new WaitForSeconds(3);//this delay is done so we give time for wallet to laod in memory.
	
		var request = new EthGetBalanceUnityRequest(LevelManager._url);
		yield return request.SendRequest(LevelManager.objAddress.Address,Nethereum.RPC.Eth.DTOs.BlockParameter.CreateLatest());
		if(request.Exception == null){
			var balance = request.Result.Value;
			callback (Nethereum.Util.UnitConversion.Convert.FromWei(balance,18));
		}
		else		
			print("Error occured when getting balance");
		
	}
}
