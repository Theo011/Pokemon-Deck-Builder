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
                StartCoroutine(SetupDeckbuilder());
            }
        }
        catch (System.Exception e)
        {
            text_mesh.text = "SOMETHING WENT WRONG, TAP THE SCREEN TO QUIT...";
            error = true;
            Debug.Log(e.Message);
        }
    }

    void Update()
    {
        if (Input.anyKeyDown && change_canvas == true && is_already_in_deck_builder == false)
        {
            loading.enabled = false;
            GetComponent<AudioSource>().Stop();
            deck_builder.enabled = true;
            is_already_in_deck_builder = true;
            deck_builder_audio_source.Play();
        }
        
        if (Input.anyKeyDown && error == true)
        {
            Application.Quit();
            Debug.Log("Quit");
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

        for (int i = 1; i < all_cards.Count; i++)
        {
            www = UnityWebRequestTexture.GetTexture(all_cards[i].ImageUrl);
            yield return www.SendWebRequest();
            myTexture = DownloadHandlerTexture.GetContent(www);
            GameObject new_image = Instantiate(image.gameObject, image.transform.position, image.transform.rotation);
            // set name of newly create image
            new_image.name = all_cards[i].Name;
            new_image.GetComponent<Image>().sprite = Sprite.Create(myTexture, new Rect(0, 0, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f));
            new_image.transform.SetParent(image.transform.parent);
            new_image.transform.localScale = image.transform.localScale;
            
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
        
        text_mesh.text = "TAP THE SCREEN TO CONTINUE...";
        change_canvas = true;
    }
}