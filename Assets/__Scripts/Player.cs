using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum PlayerType {
	human,
	ai
}

[System.Serializable]
public class Player
{
	public PlayerType type = PlayerType.ai;
	public int playerNum;
	public SlotDef handSlotDef;
	public List<CardBartok> hand;

	public CardBartok AddCard(CardBartok eCB) {
		if (hand == null) hand = new List<CardBartok>();
		hand.Add(eCB);
		if(type == PlayerType.human) {
			CardBartok[] cards = hand.ToArray();
			cards = cards.OrderBy(cd => cd.rank).ToArray();
			hand = new List<CardBartok>(cards);
		}
		FanHand();
		return (eCB);
	}

	public CardBartok RemoveCard(CardBartok cb) {
		if (hand == null || !hand.Contains(cb)) return null;
		hand.Remove(cb);
		return (cb);
	}

	public void FanHand () {
		float startRot = 0;
		startRot = handSlotDef.rot;
		if (hand.Count > 1) {
			startRot += Bartok.S.handFanDegrees * (hand.Count - 1) / 2;
		}

		Vector3 pos;
		float rot;
		Quaternion rotQ;
		for (int i = 0; i < hand.Count; i++) {
			rot = startRot - Bartok.S.handFanDegrees * i;
			rotQ = Quaternion.Euler(0, 0, rot);
			pos = Vector3.up * CardBartok.CARD_HEIGHT / 2f;
			pos = rotQ * pos;

			pos += handSlotDef.pos;
			pos.z = -0.5f * i;

			hand[i].MoveTo(pos, rotQ);
			hand[i].state = CBState.toHand;
			/*
			hand[i].transform.localPosition = pos;
			hand[i].transform.rotation = rotQ;
			hand[i].state = CBState.hand;
			*/

			hand[i].faceUp = (type == PlayerType.human);
			hand[i].SetSortOrder(i * 4);
		}
	}
}
