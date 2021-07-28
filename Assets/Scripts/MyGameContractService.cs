using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.ABI.Encoders;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Nethereum.JsonRpc.UnityClient;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.Eth.Transactions;
using Nethereum.Signer;
using UnityEngine;

public class MyGameContractService  {

	public string contractAddress = "0x0b4E8e53dd53ef278D8415E43e79eEC3BC045985"; //Get from Remix IDE
	
	public static string ABI=@"[
	{
		""constant"": false,
		""inputs"": [],
		""name"": ""buyManaAndReturnChange"",
		""outputs"": [],
		""payable"": true,
		""stateMutability"": ""payable"",
		""type"": ""function""
	},
	{
		""constant"": false,
		""inputs"": [],
		""name"": ""createInventoryForPlayer"",
		""outputs"": [],
		""payable"": false,
		""stateMutability"": ""nonpayable"",
		""type"": ""function""
	},
	{
		""constant"": false,
		""inputs"": [
			{
				""internalType"": ""address"",
				""name"": ""_addressTo"",
				""type"": ""address""
			},
			{
				""internalType"": ""uint256"",
				""name"": ""goldAmmount"",
				""type"": ""uint256""
			}
		],
		""name"": ""transferGold"",
		""outputs"": [],
		""payable"": false,
		""stateMutability"": ""nonpayable"",
		""type"": ""function""
	},
	{
		""constant"": true,
		""inputs"": [],
		""name"": ""countInventories"",
		""outputs"": [
			{
				""internalType"": ""uint256"",
				""name"": """",
				""type"": ""uint256""
			}
		],
		""payable"": false,
		""stateMutability"": ""view"",
		""type"": ""function""
	},
	{
		""constant"": true,
		""inputs"": [
			{
				""internalType"": ""address"",
				""name"": ""addrs"",
				""type"": ""address""
			}
		],
		""name"": ""getGoldForAddress"",
		""outputs"": [
			{
				""internalType"": ""uint256"",
				""name"": """",
				""type"": ""uint256""
			}
		],
		""payable"": false,
		""stateMutability"": ""view"",
		""type"": ""function""
	},
	{
		""constant"": true,
		""inputs"": [
			{
				""internalType"": ""address"",
				""name"": ""addrs"",
				""type"": ""address""
			}
		],
		""name"": ""getManaForAddress"",
		""outputs"": [
			{
				""internalType"": ""uint256"",
				""name"": """",
				""type"": ""uint256""
			}
		],
		""payable"": false,
		""stateMutability"": ""view"",
		""type"": ""function""
	},
	{
		""constant"": true,
		""inputs"": [],
		""name"": ""getPlayersAccts"",
		""outputs"": [
			{
				""internalType"": ""address[]"",
				""name"": """",
				""type"": ""address[]""
			}
		],
		""payable"": false,
		""stateMutability"": ""view"",
		""type"": ""function""
	}
]";


	private Contract contract;

	public MyGameContractService(){
		this.contract = new Contract(null,ABI,contractAddress);
	}

    //---------------------------------------------------- Create Inventory For Player

    public Function GetFunctionCreateInventoryForPlayer(){
        return contract.GetFunction("createInventoryForPlayer");
    }
    public TransactionInput CreateInventoryForPlayer (HexBigInteger gas = null, HexBigInteger valueAmount = null ) {
    	var  function = GetFunctionCreateInventoryForPlayer ();
        return function.CreateTransactionInput (LevelManager.objAddress.Address,gas,valueAmount);
    }

	//---------------------------------------------------- BUY MANA

	public Function GetFunctionBuyMana(){
		return contract.GetFunction("buyManaAndReturnChange");
	}

	public TransactionInput BuyManaTransactionInput (HexBigInteger gas = null, HexBigInteger valueAmount = null ) {
    	var  function = GetFunctionBuyMana ();
        return function.CreateTransactionInput (LevelManager.objAddress.Address,gas,valueAmount);
    }

	//---------------------------------------------------- GET MANA

	public Function GetFunctionGetManaForAddress(){
		return contract.GetFunction("getManaForAddress");
	}

	public int DecodeManaValue(string result){
		var function = GetFunctionGetManaForAddress();
		return function.DecodeSimpleTypeOutput<int>(result);
	}

	public CallInput CreateManaValueScoreInput(){
		var function = GetFunctionGetManaForAddress();
		return function.CreateCallInput(LevelManager.objAddress.Address);
	}

    //---------------------------------------------------- GET GOLD
	public Function GetFunctionGetGoldForAddress(){
		return contract.GetFunction("getGoldForAddress");
	}

    public int DecodeGoldValue(string result){
		var function = GetFunctionGetGoldForAddress();
		return function.DecodeSimpleTypeOutput<int>(result);
	}

    public CallInput CreateGoldValueInput(){
		var function = GetFunctionGetGoldForAddress();
		return function.CreateCallInput(LevelManager.objAddress.Address);
	}

    //---------------------------------------------------- Transfer Gold

    public Function GetFunctionTransferGold(){
        return contract.GetFunction("transferGold");
    }
    public TransactionInput TransferGold (HexBigInteger gas = null, HexBigInteger valueAmount = null,string _addressTo =null, int goldAmmount = 0) {
    	var  function = GetFunctionTransferGold ();
        return function.CreateTransactionInput (LevelManager.objAddress.Address,gas,valueAmount,_addressTo,goldAmmount);
    }


}
