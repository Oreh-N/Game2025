using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Team
{
	public static Player Instance;
    public Wallet _wallet { get; private set; } = new Wallet(500);
	public Shop Shop { get; private set; } = new Shop();

	public IInteractable CurrInteractObject { get; protected set; }


	private void Awake()
	{
		if (Instance != null && Instance != this)
		{ Destroy(gameObject); }
		else
		{ Instance = this; }

	}

	public void SpawnObject(GameObject obj)
	{
		CurrInteractObject.Spawn(obj);
	}

	public void InteractWithObject()
	{
		CurrInteractObject.Interact();
	}

	public void ChangeInteractableObject(IInteractable obj)
	{
		CurrInteractObject = obj;
	}

	void Start()
    {
    }

    
    void Update()
	{
		UIManager.Instance.UpdateMoneyPanel(_wallet.Money);

	}
}
