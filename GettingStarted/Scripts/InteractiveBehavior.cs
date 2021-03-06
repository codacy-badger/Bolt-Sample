﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InteractiveBehavior : Bolt.EntityBehaviour<IInteractiveState>
{
	[Range(2, 10)]
	public int interactiveRadius;

	private Color activeColor;
	private Color normalColor;

	private void Awake()
	{
		activeColor = Color.blue;
		normalColor = GetComponent<Renderer>().material.color;
	}

	public override void Attached()
	{
		state.SetTransforms(state.Transform, transform);

		if (entity.isOwner)
		{
			state.Color = normalColor;
		}

		state.AddCallback("Color", () =>
		{
			GetComponent<MeshRenderer>().material.color = state.Color;
		});
	}

	private void Update()
	{
		if (entity.isAttached && entity.isOwner)
		{
			bool anyNearbyPlayer = (from player in GameObject.FindGameObjectsWithTag("Player")
									where Vector3.Distance(transform.position, player.transform.position) < interactiveRadius
									select player).Any();

			if (anyNearbyPlayer)
			{
				state.Color = activeColor;
			}
			else
			{
				state.Color = normalColor;
			}
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, interactiveRadius);
	}
}
