using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PokemonTcgSdk;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoadingScript : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text_mesh;
    private bool change_canvas = false;
    private bool error = false;
    static public List<PokemonTcgSdk.Models.PokemonCard> all_cards = new List<PokemonTcgSdk.Models.PokemonCard>(); // keep it like this
    public Canvas deck_builder;
    public Canvas loading;
    public Image image;
    private float width_count = 0.0f;
    static private bool is_already_loaded = false;
    public AudioSource deck_builder_audio_source;
    private bool is_already_in_deck_builder = false;
    public AudioSource button_press;
    static private Dictionary<PokemonTcgSdk.Models.PokemonCard, Texture2D> deck_builder_cards = new Dictionary<PokemonTcgSdk.Models.PokemonCard, Texture2D>(); // keep it like this
    static private Dictionary<Texture2D, string> card_tex_id = new Dictionary<Texture2D, string>(); // keep it like this
    // static private SortedDictionary<string, string> sorted_by_type = new SortedDictionary<string, string>(); // keep it like this
    // static private SortedDictionary<string, string> sorted_by_hp = new SortedDictionary<string, string>(); // keep it like this
    // static private SortedDictionary<string, string> sorted_by_rarity = new SortedDictionary<string, string>(); // keep it like this
    static public string sort_type = "default";
    static private string last_sort_type = "default";
    private List<GameObject> game_objects = new List<GameObject>();

    void Start()
    {
        try
        {
            if (!is_already_loaded)
            {
                var cards = Card.Get<Pokemon>();
                cards.Cards.ForEach(card => { all_cards.Add(card); });
                StartCoroutine(SetupDeckbuilder());
                is_already_loaded = true;
            }
            else
            {
                if (deck_builder_cards == null)
                {
                    StartCoroutine(SetupDeckbuilder());
                }
                else
                {
                    load_cards();
                }
            }
        }
        catch (System.Exception e)
        {
            if (SystemInfo.deviceType == DeviceType.Handheld)
            {
                text_mesh.text = "SOMETHING WENT WRONG, TAP THE SCREEN TO QUIT...";
            }
            else
            {
                text_mesh.text = "SOMETHING WENT WRONG, PRESS ANYTHING TO QUIT...";
            }
            
            error = true;
            Debug.Log(e.Message);
        }
    }

    void Update()
    {
        if (Input.anyKeyDown && change_canvas == true && is_already_in_deck_builder == false)
        {
            button_press.Play();
            loading.enabled = false;
            GetComponent<AudioSource>().Stop();
            deck_builder.enabled = true;
            is_already_in_deck_builder = true;
            deck_builder_audio_source.Play();
        }
        
        if (Input.anyKeyDown && error == true)
        {
            button_press.Play();
            Application.Quit();
            Debug.Log("Quit");
        }

        if (sort_type != last_sort_type)
        {
            load_cards_ordered();
            last_sort_type = sort_type;
        }
    }

    IEnumerator SetupDeckbuilder()
    {
        text_mesh.text = "DOWNLOADING IMAGES...";
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(all_cards[0].ImageUrl);
        yield return www.SendWebRequest();
        Texture2D myTexture = DownloadHandlerTexture.GetContent(www);
        image.sprite = Sprite.Create(myTexture, new Rect(0, 0, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f));
        image.name = all_cards[0].Name;
        deck_builder_cards.Add(all_cards[0], myTexture);

        for (int i = 1; i < all_cards.Count; i++)
        {
            www = UnityWebRequestTexture.GetTexture(all_cards[i].ImageUrl);
            yield return www.SendWebRequest();
            myTexture = DownloadHandlerTexture.GetContent(www);
            GameObject new_image = Instantiate(image.gameObject, image.transform.position, image.transform.rotation);
            new_image.name = all_cards[i].Name;
            new_image.GetComponent<Image>().sprite = Sprite.Create(myTexture, new Rect(0, 0, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f));
            new_image.transform.SetParent(image.transform.parent);
            new_image.transform.localScale = image.transform.localScale;
            game_objects.Add(new_image);

            if (i % 2 == 0)
            {
                width_count += image.GetComponent<RectTransform>().rect.width + 30;
                new_image.transform.localPosition = image.transform.localPosition + new Vector3(width_count, 0, 0);
            }
            else
            {
                new_image.transform.localPosition = image.transform.localPosition + new Vector3(width_count, -image.rectTransform.rect.height - 30, 0);
            }

            deck_builder_cards.Add(all_cards[i], myTexture);
        }
        
        Sort();
        
        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            text_mesh.text = "TAP THE SCREEN TO CONTINUE...";
        }
        else
        {
            text_mesh.text = "PRESS ANYTHING TO CONTINUE...";
        }
        
        change_canvas = true;
    }

    void Sort()
    {
        foreach (var deck_card in deck_builder_cards)
        {
            card_tex_id.Add(deck_card.Value, deck_card.Key.Id);
            // sorted_by_type.Add(deck_card.Key.Types[0], deck_card.Key.Id);
            // sorted_by_hp.Add(deck_card.Key.Hp, deck_card.Key.Id);
            // sorted_by_rarity.Add(deck_card.Key.Rarity, deck_card.Key.Id);
        }
    }

    void load_cards()
    {
        CardScript.height_count = 0.0f;
        CardScript.card_count = 0;

        Loader();

        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            text_mesh.text = "TAP THE SCREEN TO CONTINUE...";
        }
        else
        {
            text_mesh.text = "PRESS ANYTHING TO CONTINUE...";
        }

        change_canvas = true;
    }

    void load_cards_ordered()
    {
        if (sort_type == "default")
        {
            all_cards.Reverse();

            Debug.Log("Default");

            foreach (var game_object in game_objects)
            {
                Destroy(game_object);
            }

            width_count = 0.0f;
            Loader();
        }

        if (sort_type == "reverse")
        {
            all_cards.Reverse();

            Debug.Log("Reverse");

            foreach (var game_object in game_objects)
            {
                Destroy(game_object);
            }

            width_count = 0.0f;
            Loader();
        }
    }

    void Loader()
    {
        image.sprite = Sprite.Create(deck_builder_cards[all_cards[0]], new Rect(0, 0, deck_builder_cards[all_cards[0]].width, deck_builder_cards[all_cards[0]].height), new Vector2(0.5f, 0.5f));
        image.name = all_cards[0].Name;

        for (int i = 1; i < all_cards.Count; i++)
        {
            GameObject new_image = Instantiate(image.gameObject, image.transform.position, image.transform.rotation);
            new_image.name = all_cards[i].Name;
            new_image.GetComponent<Image>().sprite = Sprite.Create(deck_builder_cards[all_cards[i]], new Rect(0, 0, deck_builder_cards[all_cards[i]].width, deck_builder_cards[all_cards[i]].height), new Vector2(0.5f, 0.5f));
            new_image.transform.SetParent(image.transform.parent);
            new_image.transform.localScale = image.transform.localScale;
            game_objects.Add(new_image);

            if (i % 2 == 0)
            {
                width_count += image.GetComponent<RectTransform>().rect.width + 30;
                new_image.transform.localPosition = image.transform.localPosition + new Vector3(width_count, 0, 0);
            }
            else
            {
                new_image.transform.localPosition = image.transform.localPosition + new Vector3(width_count, -image.rectTransform.rect.height - 30, 0);
            }
        }
    }
}