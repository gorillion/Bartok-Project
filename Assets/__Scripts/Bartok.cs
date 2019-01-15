using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//:o)

public class Bartok : MonoBehaviour
{
	static public Bartok S;

	[Header("Set In Inspectour")]
	public TextAsset deckXML;
	public TextAsset layoutXML;
	public Vector3 layoutCenter = Vector3.zero;
	public float handFanDegrees = 10f;

	[Header("Set Dynamically")]
	public Deck deck;
	public List<CardBartok> drawPile;
	public List<CardBartok> discardPile;
	public List<Player> players;
	public CardBartok targetCard;

	private BartokLayout layout;
	private Transform layoutAnchor;

    void Awake()
    {
		S = this;
    }


    void Start()
    {
		deck = GetComponent<Deck>();
		deck.InitDeck(deckXML.text);
		Deck.Shuffle(ref deck.cards);

		layout = GetComponent<BartokLayout>();
		layout.ReadLayout(layoutXML.text);

		drawPile = UpgradeCardsList(deck.cards);
		LayoutGame();
    }

	public void ArrangeDrawPile () {
		CardBartok tCB;

		for (int i = 0; i < drawPile.Count; i++) {
			tCB = drawPile[i];
			tCB.transform.SetParent(layoutAnchor);
			tCB.transform.localPosition = layout.drawPile.pos;
			tCB.faceUp = false;
			tCB.SetSortingLayerName(layout.drawPile.layerName);
			tCB.SetSortOrder(-i * 4);
			tCB.state = CBState.drawpile;
		}
	}

	void LayoutGame () {
		if(layoutAnchor == null) {
			GameObject tGO = new GameObject("__LayoutAnchor");
			layoutAnchor = tGO.transform;
			layoutAnchor.transform.position = layoutCenter;
		}

		ArrangeDrawPile();

		Player p1;
		players = new List<Player>();
		foreach (SlotDef tSD in layout.slotDefs) {
			p1 = new Player();
			p1.handSlotDef = tSD;
			players.Add(p1);
			p1.playerNum = tSD.player;
		}
		players[0].type = PlayerType.human;
	}

	public CardBartok Draw () {
		CardBartok cd = drawPile[0];
		drawPile.RemoveAt(0);
		return (cd);
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			players[0].AddCard(Draw());
		}
		if (Input.GetKeyDown(KeyCode.Alpha2)) {
			players[1].AddCard(Draw());
		}
		if (Input.GetKeyDown(KeyCode.Alpha3)) {
			players[2].AddCard(Draw());
		}
		if (Input.GetKeyDown(KeyCode.Alpha4)) {
			players[3].AddCard(Draw());
		}
	}

	List<CardBartok> UpgradeCardsList(List<Card> CD) {
		List<CardBartok> CB = new List<CardBartok>();
		foreach(Card tCD in CD) {
			CB.Add(tCD as CardBartok);
		}
		return (CB);
	}
}
